using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
public class PlayerController : MonoBehaviour
{
    [Header("Flip Settings")]
    [SerializeField] private float flipDistance = 1f;
    [SerializeField] private float durationMove = 1f;

    [Header("Sound Settings")] 
    [SerializeField] private AudioClip[] flipAudioClips;
    [SerializeField] private AudioClip dangerAudioClips;
    
    [Header("Game Settings")]
    [SerializeField] private GameObject objLevel;
    [SerializeField] private GameObject objScan;
    [SerializeField] private GameObject objCamera;
    [SerializeField] private GameObject folderElements;
    [SerializeField] private AnimationCurve curveMove;
    [SerializeField] private AnimationCurve curveJump;
    
    private float 
        flipHeight = 0.5f,
        flipDelayMultiplier = 0.25f,
        flipJumpMultiplier = 2f;
    private List<GameObject> _listPlayerGlassful = new List<GameObject>();
    private Quaternion _startRotation, _endRotation;
    private TouchControlls _touchControls;
    private bool _isDanger, _isHole, _isExtraMove, _isFlipping, _isMenu;
    private GameObject _freeGlassful;
    private Vector3 _moveDestination;
    private AudioSource _flipSound;
    
    private void Awake()
    {
        _touchControls = new TouchControlls();
        _flipSound = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        _touchControls.Player.Enable();
        EventManager.OpenMenu += (() => {_isMenu = true;});
        EventManager.CloseMenu += (() => {_isMenu = false;});
    }
    private void Start()
    {
        _touchControls.Player.Flip.canceled += _ =>
        {
            if (!_isFlipping && EventSystem.current.currentSelectedGameObject == null && !_isMenu)
            {
                flipJumpMultiplier = 1f;
                flipDelayMultiplier = 0.25f;
                _moveDestination = Vector3.right * flipDistance;
                NextFlipPoint();
            }
        };
        _touchControls.Player.Flip.performed += _ =>
        {
            if (!_isFlipping && EventSystem.current.currentSelectedGameObject == null && !_isMenu)
            {
                flipJumpMultiplier = 1.5f;
                flipDelayMultiplier = 0.15f;
                _moveDestination = Vector3.right * flipDistance * 2;
                NextFlipPoint();
            }
        };

        foreach (Transform child in transform)
            _listPlayerGlassful.Add(child.gameObject);
    }
    private void NextFlipPoint()
    {
        _isExtraMove = false;
        Vector3 groundCheckPosition = objScan.transform.position + _moveDestination;
        Collider[] groundColliders = new Collider[10];
        Physics.OverlapSphereNonAlloc(groundCheckPosition, 0.3f, groundColliders, 1 << 3);
        if (groundColliders[0] == null)
        {
            groundCheckPosition += Vector3.up * 0.9f;
            Physics.OverlapSphereNonAlloc(groundCheckPosition, 0.3f, groundColliders, 1 << 3);
            if (groundColliders[0] == null)
            {
                groundCheckPosition += Vector3.down * 1.8f;
                Physics.OverlapSphereNonAlloc(groundCheckPosition, 0.3f, groundColliders, 1 << 3);
                if (groundColliders[0] != null)
                    _isExtraMove = true;
            }
        }

        if (groundColliders[0] != null)
        {
            _isFlipping = true;
            _isDanger = false; 
            _freeGlassful = null;
            _isHole = groundColliders[0].gameObject.CompareTag("Hole");
            
            Collider[] itemColliders = new Collider[10];
            Physics.OverlapSphereNonAlloc(groundCheckPosition, 0.3f, itemColliders, 1 << 7);
            if (itemColliders[0] != null)
                switch (itemColliders[0].gameObject.tag)
                {
                    case "FreeGlass":
                        _freeGlassful = itemColliders[0].gameObject;
                        break;
                    case "Danger":
                        _isDanger = true;
                        break;
                }
            _flipSound.PlayOneShot(flipAudioClips[0], 0.3f);
            for (int i = 0; i < _listPlayerGlassful.Count; i++)
                StartCoroutine(GlassfulMove(_listPlayerGlassful[i], i, _freeGlassful != null, groundCheckPosition));
            StartCoroutine(CameraMove(groundCheckPosition));
            objScan.transform.position = Vector3.up * groundCheckPosition.y;
        }
    }
    private IEnumerator GlassfulMove(GameObject glassful, int indexOfGlassful, bool isFreeGlassful, Vector3 moveDestination)
    {
        yield return new WaitForSeconds(indexOfGlassful * flipDelayMultiplier);
        
        if (isFreeGlassful)
            indexOfGlassful++;
        Vector3 startPosition = glassful.transform.position;
        Vector3 endPosition = moveDestination + (Vector3.up * (0.2f * indexOfGlassful));
        
        float elapsedTime = 0;
        float moveComplete = 0;
        float currebtDurationMove = durationMove;
        if (_listPlayerGlassful.Count > 5)
            currebtDurationMove = durationMove * 0.7f;
        float jumpExtraHeight = 0;
        while (moveComplete < 1)
        {
            elapsedTime += Time.deltaTime;
            moveComplete = elapsedTime / currebtDurationMove;
            if(_isExtraMove)
                jumpExtraHeight = curveJump.Evaluate(moveComplete) * (_listPlayerGlassful.Count - indexOfGlassful) * 0.05f; 
            Vector3 endRelCenter = endPosition +
                                   (Vector3.up * (curveJump.Evaluate(moveComplete) * flipHeight * flipJumpMultiplier))
                                   + (Vector3.up * jumpExtraHeight);

            glassful.transform.position = Vector3.Slerp(startPosition, endRelCenter, curveMove.Evaluate(moveComplete));
            GlassfulRotation(glassful, moveComplete);
            yield return new WaitForFixedUpdate();
        }
        
        if (_isHole)
        {
            _touchControls.Player.Disable();
            EventManager.OnGameOver();
            moveComplete = 0;
            while (glassful.transform.position.y > -4)
            {
                glassful.transform.position += Vector3.down * Time.deltaTime;
                
                GlassfulRotation(glassful, moveComplete += Time.deltaTime * 0.1f);
                yield return new WaitForFixedUpdate();
            }
        }
        else
        {
            _flipSound.PlayOneShot(flipAudioClips[indexOfGlassful+1], 0.5f);
            if(_isDanger && indexOfGlassful == 0)
                _flipSound.PlayOneShot(dangerAudioClips,1);
            EventManager.OnPlayerScore(1);
           
        }
    }
    private void GlassfulRotation(GameObject glassful, float moveComplete)
    {
        float rotationComplete;
        if (moveComplete < 0.5f)
        {
            rotationComplete = moveComplete * 2;
            _startRotation = Quaternion.Euler(0, 0, 0);
            _endRotation = Quaternion.Euler(0, 0, 180);
        }
        else
        {
            rotationComplete = (moveComplete * 2) - 1;
            _startRotation = Quaternion.Euler(0, 0, -180);
            _endRotation = Quaternion.Euler(0, 0, 0);    
        }
        glassful.transform.rotation = Quaternion.Slerp(_startRotation, _endRotation, rotationComplete);
    }
    private IEnumerator CameraMove(Vector3 moveDestination)
    {
        float elapsedTime = 0;
        float moveComplete = 0;
        float duration = durationMove + (_listPlayerGlassful.Count - 1) * flipDelayMultiplier;
        Vector3 startPosition = objCamera.transform.position;
        Vector3 endPosition = moveDestination;
        while (moveComplete < 1)
        {
            elapsedTime += Time.deltaTime;
            moveComplete = elapsedTime / duration;
            objCamera.transform.position = Vector3.Lerp(startPosition, endPosition, curveMove.Evaluate(moveComplete));
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSecondsRealtime(0.2f);
        LevelMove();
    }
    private void LevelMove()
    {
        if (_freeGlassful != null)
            AddNewGlassful(_freeGlassful);
        
        _listPlayerGlassful = _listPlayerGlassful.OrderByDescending(glassful => glassful.transform.position.y).ToList();
        
        if (_isDanger)
            RemoveLastGlassful();

        if(_listPlayerGlassful.Count == 0)
            EventManager.OnGameOver();
        else
        {
            float distance = _listPlayerGlassful[^1].transform.position.x;
            objLevel.transform.position += Vector3.left * (flipDistance * distance);
            EventManager.OnPlayerScore(distance);
            EventManager.OnLevelMoveDistance(distance); 
            EventManager.OnLevelMove();

            if(!_isHole)
                _isFlipping = false;    
        }
    }
    private void AddNewGlassful(GameObject glassful)
    {
        glassful.transform.SetParent(transform);
        glassful.layer = 6;
        glassful.tag = "Player";
        _listPlayerGlassful.Add(glassful);
    }
    private void RemoveLastGlassful()
    {
        GameObject lastGlassful = _listPlayerGlassful[^1].gameObject;
        lastGlassful.transform.SetParent(folderElements.transform);
        lastGlassful.layer = 3;
        lastGlassful.tag = "Untagged";
        _listPlayerGlassful.Remove(lastGlassful);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    [Header("Level Prefabs")] 
    [SerializeField] private GameObject[] elements;
    [SerializeField] private GameObject[] glassfuls;
    [SerializeField] private GameObject[] decorates;
    [SerializeField] private GameObject folderFreeGlassfuls;
    [SerializeField] private GameObject folderElements;
    
    private int _prevElementIndex, _prevGlassfulIndex;
    private Vector3 _prevSpawnPosition;
    private List<GameObject> 
        _spawnSmallDecorateObjects = new List<GameObject>(),
        _spawnDecorateObjects = new List<GameObject>(),
        _spawnGlassfulObjects = new List<GameObject>(),
        _spawnElementObjects = new List<GameObject>();
    private float[] _levelHeight = new float[] {0.9f, 0f, -0.9f};
    private float _elementRotation = 90;

    private void OnEnable()
    {
        EventManager.LevelMove += DestroyObject;
        EventManager.LevelMoveDistance += StartLevelGenerator;
    }
    private void OnDestroy()
    {
        EventManager.LevelMove -= DestroyObject;
        EventManager.LevelMoveDistance -= StartLevelGenerator;
    }
    private void Start()
    {
        SpawnDecorate(Vector3.zero, 0);
        SpawnGround(Vector3.right,  0);
        StartLevelGenerator(15);
    }
    private void StartLevelGenerator(float distance)
    {
        StartCoroutine(ElementGenerator(distance));
    }
    private IEnumerator ElementGenerator(float distance)
    {
        for (int i = 0; i < distance; i++)
        {
            while (true)
            {
                float nextHeight = _levelHeight[Random.Range(0, _levelHeight.Length)];
                Vector3 prevPosition = _spawnElementObjects[^1].transform.position;
                Vector3 spawnPosition = prevPosition + (Vector3.right) + (Vector3.up * nextHeight);
                if (spawnPosition.y > 0)
                {
                    int indexElement = 0;
                    if(_prevElementIndex == 0 && prevPosition.y == spawnPosition.y)
                        indexElement = Random.Range(0, elements.Length);
                    
                    SpawnGround(spawnPosition, indexElement);
                    
                    if (indexElement == 0 && _spawnGlassfulObjects.Count < 9)
                        if (Random.value > 0.5f)
                        {
                            int indexGlassful = Random.Range(0, glassfuls.Length);
                            SpawnGlassful(spawnPosition + (Vector3.up * 0.1f), indexGlassful);
                        }

                    if (_spawnDecorateObjects[^1].transform.position.x + 8 < spawnPosition.x)
                    {
                        int shiftZ = Random.Range(3, 6);
                        int shiftY = Random.Range(2, 6);
                        SpawnDecorate(spawnPosition + (Vector3.down * shiftY) + (Vector3.forward * shiftZ), Random.Range(1, decorates.Length));
                    }
                    break;
                }
            }
            yield return new WaitForFixedUpdate();
        }
    }
    private void SpawnGround(Vector3 spawnPosition, int index)
    {
        _prevElementIndex = index;
        _elementRotation = _elementRotation == 0 ? 180 : 0;
        GameObject levelElement = Instantiate(elements[index], spawnPosition, Quaternion.Euler(0, _elementRotation, 0));
        levelElement.transform.SetParent(folderElements.transform);
        _spawnElementObjects.Add(levelElement);
    } 
    private void SpawnDecorate(Vector3 spawnPosition, int index)
    {
        GameObject decorate = Instantiate(decorates[index], spawnPosition, Quaternion.identity);
        decorate.transform.SetParent(folderElements.transform);
        _spawnDecorateObjects.Add(decorate);
    }
    private void SpawnGlassful(Vector3 spawnPosition, int index)
    {
        _prevGlassfulIndex = index;
        GameObject freeGlassful = Instantiate(glassfuls[index], spawnPosition, Quaternion.Euler(0, 0, 0));
        freeGlassful.transform.SetParent(folderFreeGlassfuls.transform);
        freeGlassful.layer = 7;
        freeGlassful.tag = "FreeGlass";
        _spawnGlassfulObjects.Add(freeGlassful);
    }
    private void DestroyObject()
    {
        for (var i = _spawnElementObjects.Count - 1; i >= 0; i--)
        {
            GameObject spawnGameObject = _spawnElementObjects[i];
            if (spawnGameObject.transform.position.x < -5)
            {
                _spawnElementObjects.Remove(spawnGameObject);
                Destroy(spawnGameObject);
            }
        }
        for (var i = _spawnGlassfulObjects.Count - 1; i >= 0; i--)
        {
            GameObject spawnGameObject = _spawnGlassfulObjects[i];
            if (spawnGameObject.transform.position.x < -5)
            {
                _spawnGlassfulObjects.Remove(spawnGameObject);
                Destroy(spawnGameObject);
            }
        }
        for (var i = _spawnDecorateObjects.Count - 1; i >= 0; i--)
        {
            GameObject spawnGameObject = _spawnDecorateObjects[i];
            if (spawnGameObject.transform.position.x < -5)
            {
                _spawnDecorateObjects.Remove(spawnGameObject);
                Destroy(spawnGameObject);
            }
        }
        for (var i = _spawnSmallDecorateObjects.Count - 1; i >= 0; i--)
        {
            GameObject spawnGameObject = _spawnSmallDecorateObjects[i];
            if (spawnGameObject.transform.position.x < -5)
            {
                _spawnSmallDecorateObjects.Remove(spawnGameObject);
                Destroy(spawnGameObject);
            }
        }
    }
}

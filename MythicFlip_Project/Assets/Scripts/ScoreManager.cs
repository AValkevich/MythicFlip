using System;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private RectTransform score;
    private int _playerScore;

    private void OnEnable()
    {
        EventManager.PlayerScore += PlayerScore;
        EventManager.GamerOver += GameOver;
        EventManager.OpenMenu += OpenMenu;
        EventManager.CloseMenu += Start;
    }
    private void OnDestroy()
    {
        EventManager.PlayerScore -= PlayerScore;
        EventManager.GamerOver -= GameOver;
        EventManager.OpenMenu -= OpenMenu;
        EventManager.CloseMenu -= Start;
    }
    private void Start()
    {
        score.localPosition = new Vector3(-150, 550, 0);
    }
    private void PlayerScore(float score)
    {
        _playerScore += (int) score;
        scoreText.text = _playerScore.ToString();
    }
    private void GameOver()
    {
        score.localPosition = Vector3.zero;
    }
    private void OpenMenu()
    {
        score.localPosition = new Vector3(0, 200, 0);
    }
}

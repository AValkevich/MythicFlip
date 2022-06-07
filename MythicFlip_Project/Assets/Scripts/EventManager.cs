using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void Distance(float distance);
    public static event Distance LevelMoveDistance;
    public static event Distance PlayerScore;

    public static event Action LevelMove;
    public static event Action GamerOver;
    public static event Action OpenMenu;
    public static event Action CloseMenu;

    public static void OnLevelMove()
    {
        LevelMove?.Invoke();
    }
    public static void OnLevelMoveDistance(float distance)
    {
        LevelMoveDistance?.Invoke(distance);
    }
    public static void OnPlayerScore(float score)
    {
        PlayerScore?.Invoke(score);
    }
    public static void OnGameOver()
    {
        GamerOver?.Invoke();
    }
    public static void OnOpenMenu()
    {
        OpenMenu?.Invoke();
    }    
    public static void OnCloseMenu()
    {
        CloseMenu?.Invoke();
    }

}

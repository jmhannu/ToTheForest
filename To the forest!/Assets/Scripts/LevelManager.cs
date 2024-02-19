using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class LevelManager
{
    public static bool Running;
    public static bool Last; 
    public static float CurrentSpeed;
    public static float MinZ;
    public static float MaxZ;
    public static int FinishDistance; 
    public static int Counter;

    public static GameState State; 

    public enum GameState
    {
        Playing,
        Won,
        Failed
    }

    public static void InitGame(int _sectionLength, int _finishDistance)
    {
        Running = false;
        MaxZ = (_sectionLength / 2);
        MinZ = MaxZ * -1;
        Counter = 0;
        State = GameState.Playing;
        FinishDistance = _finishDistance;
    }

    public static void EndGame(bool _won)
    {
        Running = false;
        CurrentSpeed = 0;

        if (_won) State = GameState.Won;
        else State = GameState.Failed;
    }

    public static void SetSpeed(float _speed)
    {
        CurrentSpeed = _speed;
    }

    public static List<GameObject> LoadObjects(string _path)
    {
        var tempList = Resources.LoadAll(_path, typeof(GameObject)).OfType<GameObject>().ToList();
        return tempList;
    }

    public static void AddedSection(int _sectionLength)
    {
        MaxZ += _sectionLength;
    }

    public static void RemovedSection(int _sectionLength)
    {
        MinZ += _sectionLength;
    }

    public static void Counting(int _count)
    {
        Counter = _count;

        if (Counter >= FinishDistance) EndGame(true);
    }
}

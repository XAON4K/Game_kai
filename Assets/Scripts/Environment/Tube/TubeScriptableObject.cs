using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;

[CreateAssetMenu(fileName = "S_Falling", menuName = "Falling settings")]
public class TubeScriptableObject : ScriptableObject
{
    [Header("Висота труби")]
    public int tubeHeight = 100;

    [Space(5)]

    [Header("Список перешкод (Тип перешкоди, висота, тип цього типу перешкоди)")]
    public List<Obstacle> obstacles;

    [Space(5)]

    [Header("Куди в кінці переміститься гравець")]
    public SceneTransition.Scenes scene;
}

[System.Serializable]
public class Obstacle
{
    public TubeSpawner.ObstacleType Type;
    public int Height;
    public int Index;
}
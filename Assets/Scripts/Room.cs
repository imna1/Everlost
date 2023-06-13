using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
public class Room : MonoBehaviour
{
    public int ID;
    public int X;
    public int Y;
    public bool[] IsDoorsExisted; // 0 - UL, 5 - L
    public GameObject[] Doors;
    public GameObject[] FakeWalls;
    public List<GameObject> EnemiesPrefabs;
    public List<Transform> EnemiesSpots;
    public List<WayPoint> Waypoints;
    public Transform EnemyContainer;
    [HideInInspector] public MiniMapRoom MiniMapRoom;
    [HideInInspector] public bool IsCleared;
    [HideInInspector] public int Distance = 99999;
    [HideInInspector] public bool VisitedWhileGeneration = false;
    [HideInInspector] public bool IsImpasse = false;
    public void RemoveWall(int index)
    {
        IsDoorsExisted[index] = true;
        Doors[index].SetActive(true);
        FakeWalls[index].SetActive(false);
    }
}

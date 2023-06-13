using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private RoomBuilder _RoomContainer;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private Transform _playerTransform;
    public List<Transform> Enemies;
    public List<Enemy> EnemiesComponents;
    public Room CurrentRoom;
    public Room[,] Rooms;

    private PathFinder pathFinder;

    private void Awake()
    {
        instance = this;
    }
    public void ChangeRoom(int xBias, int yBias, float xMoveBias, float yMoveBias)
    {
        CurrentRoom.gameObject.SetActive(false);
        Rooms[CurrentRoom.Y + yBias, CurrentRoom.X + xBias].gameObject.SetActive(true);
        CurrentRoom = Rooms[CurrentRoom.Y + yBias, CurrentRoom.X + xBias];
        _playerTransform.position += new Vector3(xMoveBias, yMoveBias, 0);
        _RoomContainer.SetMiniMapImages(CurrentRoom, xBias, yBias);
        if (!CurrentRoom.IsCleared)
        {
            for (int i = 0; i < CurrentRoom.EnemyContainer.childCount; i++)
            {
                Enemies.Add(CurrentRoom.EnemyContainer.GetChild(i));
                EnemiesComponents.Add(CurrentRoom.EnemyContainer.GetChild(i).GetComponent<Enemy>());
                CurrentRoom.Waypoints.Add(EnemiesComponents[i].WayPoint);
            }
            if (Enemies.Count <= 0)
                SwitchRoomToClear();
            else
                pathFinder.SetAllNeighbours(CurrentRoom.Waypoints);
        }
    }
    private void Start()
    {
        pathFinder = PathFinder.instance;
        ChangeRoom(0, 0, 0, 0);
    }
    public void Pause()
    {
        Time.timeScale = 0;
    }
    public void UnPause()
    {
        Time.timeScale = 1;
    }
    public void OnEnemyDied(Transform enemy)
    {
        Enemies.Remove(enemy);
        var goComponent = enemy.GetComponent<Enemy>();
        EnemiesComponents.Remove(goComponent);
        pathFinder.Waypoints.Remove(goComponent.WayPoint);
        if (Enemies.Count <= 0)
        {
            SwitchRoomToClear();
        }
    }
    public void SetRoomChild(Transform transform)
    {
        transform.SetParent(CurrentRoom.transform);
    }
    private void SwitchRoomToClear()
    {
        CurrentRoom.IsCleared = true;
        for (int i = 0; i < CurrentRoom.IsDoorsExisted.Length; i++)
        {
            if (CurrentRoom.IsDoorsExisted[i])
            {
                CurrentRoom.Doors[i].transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }
    private void Update() //временно
    {
        for (int i = 0; i < Enemies.Count; i++)
        {
            if (!EnemiesComponents[i].IsActive && Vector2.Distance(Enemies[i].position, _playerTransform.position) < EnemiesComponents[i].RadiusToSpotPlayer)
            {
                EnemiesComponents[i].SwitchToActive();
                var colliders = Physics2D.OverlapCircleAll(Enemies[i].position, EnemiesComponents[i].ScreamDistance, _enemyLayer);
                foreach (var enemy in colliders)
                {
                    var goComponent = enemy.GetComponent<Enemy>();
                    if (!goComponent.IsActive)
                        goComponent.SwitchToActive();
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBuilder : MonoBehaviour
{
    [SerializeField] private GameObject[] _roomPrefabs;
    [SerializeField] private GameObject _startRoomPrefab;
    [SerializeField] private GameObject _miniMapRoomPrefab;
    [SerializeField] private Transform _minimap;
    [SerializeField] private Transform _player;
    [SerializeField] private Sprite[] _doors;
    [SerializeField] private Sprite[] _walls;
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private int _minAmountOfRooms;
    [SerializeField] private int _maxAmountOfRooms;
    [Range(1, 6)][SerializeField] private int _branchesFromStart;
    [Range(0, 1)][SerializeField] private float _chanceSaveDirection;
    [Range(0, 6)][SerializeField] private int _minExtraDoors;
    [Range(0, 6)][SerializeField] private int _maxExtraDoors;
    [Range(0, 1)][SerializeField] private float _chanceToExtraDoors;

    private float ROOMXBIAS = 20;
    private float ROOMYBIAS = -15;
    private float ROOMXRFOMYBIAS = -10;
    private float MINIMAPROOMXBIAS = 100;
    private float MINIMAPROOMYBIAS = -70;
    private float MINIMAPROOMXFROMYBIAS = -50;
    private Room _current;
    private Room _chosen;
    private Room _startRoom;
    private Room[,] _rooms;
    private Dictionary<int, Bias> _biasValues = new Dictionary<int, Bias>()
    {
        [0] = new Bias(-1, -1),
        [1] = new Bias(0, -1),
        [2] = new Bias(1, 0),
        [3] = new Bias(1, 1),
        [4] = new Bias(0, 1),
        [5] = new Bias(-1, 0)
    };
    private Dictionary<Bias, int> _indexValues = new Dictionary<Bias, int>()
    {
        [new Bias(-1, -1)] = 0,
        [new Bias(0, -1)] = 1,
        [new Bias(1, 0)] = 2,
        [new Bias(1, 1)] = 3,
        [new Bias(0, 1)] = 4,
        [new Bias(-1, 0)] = 5
    };

    private void Start()
    {
        MINIMAPROOMXBIAS *= _miniMapRoomPrefab.transform.localScale.x;
        MINIMAPROOMYBIAS *= _miniMapRoomPrefab.transform.localScale.x;
        MINIMAPROOMXFROMYBIAS *= _miniMapRoomPrefab.transform.localScale.x;
        ROOMXBIAS *= _startRoomPrefab.transform.localScale.x;
        ROOMYBIAS *= _startRoomPrefab.transform.localScale.x;
        ROOMXRFOMYBIAS *= _startRoomPrefab.transform.localScale.x;
        GenerateLocation();
        GameManager.instance.CurrentRoom = _startRoom;
        GameManager.instance.Rooms = _rooms;
    }
    public void SetMiniMapImages(Room room, int x, int y)
    {
        _minimap.position -= new Vector3(y * MINIMAPROOMXFROMYBIAS + x * MINIMAPROOMXBIAS, y * MINIMAPROOMYBIAS);
        if (room.IsCleared)
            return;
        for (int i = 0; i < room.MiniMapRoom.Images.Length; i++)
        {
            room.MiniMapRoom.Images[i].enabled = true;
            if (room.IsDoorsExisted[i])
                room.MiniMapRoom.Images[i].sprite = _doors[i];
            else
                room.MiniMapRoom.Images[i].sprite = _walls[i];
        }
    }

    private void GenerateLocation()
    {
        DestroyRooms();
        SpawnRooms();
        SpawnDoors();
        SetRoomInformation();
        _player.position = new Vector2(_startRoom.Y * ROOMXRFOMYBIAS + _startRoom.X * ROOMXBIAS, _startRoom.Y * ROOMYBIAS);
        _minimap.position -= new Vector3(_startRoom.Y * MINIMAPROOMXFROMYBIAS + _startRoom.X * MINIMAPROOMXBIAS, _startRoom.Y * MINIMAPROOMYBIAS);
    }

    private void SpawnRooms()
    {
        List<int> freeSpots = new List<int>() {0,1,2,3,4,5};
        bool canCreateBranch = true;
        int roomTotalCount = Random.Range(_minAmountOfRooms, _maxAmountOfRooms + 1);
        int roomCount = 1;
        int t = Random.Range(0, freeSpots.Count);
        freeSpots.Remove(t);
        int x = Random.Range(1, _width - 1);
        int y = Random.Range(1, _height - 1);
        _rooms = new Room[_height, _width];
        InstantiateRoom(x, y, _startRoomPrefab);
        _startRoom = _rooms[y, x];
        while (roomCount < roomTotalCount)
        {
            if (_chanceSaveDirection >= Random.Range(0f, 1f) || !(x + _biasValues[t].X >= 0 && x + _biasValues[t].X < _width && y + _biasValues[t].Y >= 0 && y + _biasValues[t].Y < _height))
            {
                do
                {
                    t = Random.Range(0, 6);
                } while (!(x + _biasValues[t].X >= 0 && x + _biasValues[t].X < _width && y + _biasValues[t].Y >= 0 && y + _biasValues[t].Y < _height));
            }
            if (roomCount % (roomTotalCount / _branchesFromStart) == 0 && canCreateBranch && freeSpots.Count > 0)
            {
                x = _startRoom.X;
                y = _startRoom.Y;
                t = freeSpots[Random.Range(0, freeSpots.Count)];
                freeSpots.Remove(t);
                canCreateBranch = false;
            }
            x += _biasValues[t].X;
            y += _biasValues[t].Y;
            if (_rooms[y, x] == null)
            {
                InstantiateRoom(x, y, _roomPrefabs[Random.Range(0, _roomPrefabs.Length)]);
                roomCount++;
                canCreateBranch = true;
            }
        }
    }
    private void SpawnDoors()
    {
        _current = _startRoom;
        Stack<Room> stack = new Stack<Room>();
        for (int i = 0; i < 5; i++)
        {
            stack.Push(_current);
        }
        do
        {
            List<Room> unvisitedNeighbours = GetNeighbours(_current.X, _current.Y);
            for (int i = unvisitedNeighbours.Count - 1; i >= 0; i--)
            {
                if (unvisitedNeighbours[i].VisitedWhileGeneration)
                    unvisitedNeighbours.Remove(unvisitedNeighbours[i]);
            }
            if (unvisitedNeighbours.Count > 0)
            {
                _chosen = unvisitedNeighbours[Random.Range(0, unvisitedNeighbours.Count)];
                RemoveWalls(_current, _chosen);
                _chosen.VisitedWhileGeneration = true;
                stack.Push(_chosen);
                _current = _chosen;
            }
            else
            {
                _current = stack.Pop();
            }
        } while (stack.Count > 0);

        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                if (_rooms[y, x] == null)
                    continue;
                if (Random.Range(0f, 1f) < _chanceToExtraDoors)
                {
                    List<Room> neighbours = GetNeighbours(x, y);
                    int length = Random.Range(_minExtraDoors, _maxExtraDoors + 1);
                    for (int i = 0; i < length; i++)
                    {
                        if (neighbours.Count <= 0)
                            break;
                        Room roomNextTo = neighbours[Random.Range(0, neighbours.Count)];
                        neighbours.Remove(roomNextTo);
                        RemoveWalls(_rooms[y, x], roomNextTo);
                    }
                }
            }
        }
        List<Room> neighbourst = GetNeighbours(_startRoom.X, _startRoom.Y);
        for (int i = 0; i < 6; i++)
        {
            if (neighbourst.Count <= 0)
                break;
            Room roomNextTo = neighbourst[0];
            neighbourst.Remove(roomNextTo);
            RemoveWalls(_startRoom, roomNextTo);
        }
    }
    private void SetRoomInformation()
    {
        _current = _startRoom;
        _current.Distance = 0;
        bool onWay = false;
        Stack<Room> analysedRooms = new Stack<Room>();
        for (int i = 0; i < 5; i++)
        {
            analysedRooms.Push(_current);
        }
        do
        {
            int index = -1;
            int max = -1;
            bool haveUnvisitedRooms = false;
            for (int i = 5; i >= 0; i--)
            {
                if (_current.IsDoorsExisted[i])
                {
                    _chosen = _rooms[_current.Y + _biasValues[i].Y, _current.X + _biasValues[i].X];
                    if (_chosen.Distance == 99999)
                    {
                        index = i;
                        haveUnvisitedRooms = true;
                    }
                    else
                    {
                        max = Mathf.Max(max, _chosen.Distance);
                        _current.Distance = Mathf.Min(_chosen.Distance + 1, _current.Distance);
                    }
                }
            }
            if (haveUnvisitedRooms)
            {
                _current = _rooms[_current.Y + _biasValues[index].Y, _current.X + _biasValues[index].X];
                analysedRooms.Push(_current);
                onWay = true;
            }
            else
            {
                if (onWay && _current.Distance >= max)
                {
                    onWay = false;
                    _current.IsImpasse = true;
                }
                _current = analysedRooms.Pop();
            }
        } while (analysedRooms.Count > 0);
    }
    private void InstantiateRoom(int x, int y, GameObject prefab)
    {
        GameObject roomGO = Instantiate(prefab, new Vector2(y * ROOMXRFOMYBIAS + x * ROOMXBIAS, y * ROOMYBIAS), Quaternion.identity);
        roomGO.transform.SetParent(transform);
        _rooms[y, x] = roomGO.GetComponent<Room>();
        _rooms[y, x].X = x;
        _rooms[y, x].Y = y;
        GameObject minimaproomGO = Instantiate(_miniMapRoomPrefab, (Vector2)_minimap.transform.position + new Vector2(y * MINIMAPROOMXFROMYBIAS + x * MINIMAPROOMXBIAS, y * MINIMAPROOMYBIAS), Quaternion.identity);
        minimaproomGO.transform.SetParent(_minimap);
        _rooms[y, x].MiniMapRoom = minimaproomGO.GetComponent<MiniMapRoom>();
        for (int i = _rooms[y, x].EnemiesPrefabs.Count - 1; i >= 0; i--)
        {
            if (_rooms[y, x].EnemiesSpots.Count <= 0) break;
            int rand = Random.Range(0, _rooms[y, x].EnemiesSpots.Count);
            GameObject enemyGO = Instantiate(_rooms[y, x].EnemiesPrefabs[i], _rooms[y, x].EnemiesSpots[rand].position, Quaternion.identity);
            enemyGO.transform.SetParent(_rooms[y, x].EnemyContainer);
            _rooms[y, x].EnemiesSpots.RemoveAt(rand);
        }
        roomGO.SetActive(false);
    }
    private void RemoveWalls(Room current, Room chosen)
    {
        int index = _indexValues[new Bias(chosen.X - current.X, chosen.Y - current.Y)];
        current.RemoveWall(index);
        chosen.RemoveWall((index + 3) % 6);
    }
    private List<Room> GetNeighbours(int x, int y)
    {
        List<Room> neighbours = new List<Room>(6);
        if (x > 0 && _rooms[y, x - 1] != null) //Left
            neighbours.Add(_rooms[y, x - 1]);
        if (y < _height - 1 && _rooms[y + 1, x] != null) //Down-Left
            neighbours.Add(_rooms[y + 1, x]);
        if (y < _height - 1 && x < _width - 1 && _rooms[y + 1, x + 1] != null) //Down-Right
            neighbours.Add(_rooms[y + 1, x + 1]);
        if (x < _width - 1 && _rooms[y, x + 1] != null) //Right
            neighbours.Add(_rooms[y, x + 1]);
        if (y > 0 && _rooms[y - 1, x] != null) //Up-Right
            neighbours.Add(_rooms[y - 1, x]);
        if (x > 0 && y > 0 && _rooms[y - 1, x - 1] != null) //Up-Left
            neighbours.Add(_rooms[y - 1, x - 1]);
        return neighbours;
    }
    private void DestroyRooms()
    {
        if (_rooms == null)
            return;
        foreach (Room room in _rooms)
        {
            Destroy(room?.gameObject);
        }
        _rooms = null;
    }
}
public struct Bias
{
    public Bias(int x, int y)
    {
        X = x;
        Y = y;
    }
    public int X;
    public int Y;
}
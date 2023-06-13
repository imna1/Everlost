using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    public static PathFinder instance;
    [SerializeField] public List<WayPoint> Waypoints;
    [SerializeField] private WayPoint _playerWayPoint;
    [SerializeField] private LayerMask _obstacleLayer;
    private WayPoint _current;
    private Queue<WayPoint> _queue = new Queue<WayPoint>();

    private void Awake()
    {
        instance = this;
    }
    public void SetAllNeighbours(List<WayPoint> waypoints)
    {
        Waypoints = waypoints;
        Waypoints.Add(_playerWayPoint);
        List<WayPoint> staticPoints = new List<WayPoint>();
        for (int i = 0; i < Waypoints.Count; i++)
            if(Waypoints[i].IsStatic)staticPoints.Add(Waypoints[i]);
        foreach (var point in staticPoints)
            FindNeighbours(point, staticPoints, false);
    }
    public List<Vector2> GetPath(WayPoint start, float radius)
    {
        return GetPath(start, _playerWayPoint, radius);
    }

    public List<Vector2> GetPath(WayPoint start, WayPoint target, float radius)
    {
        List<Vector2> path = new List<Vector2>();
        if(!Physics2D.Linecast(start.transform.position, target.transform.position, _obstacleLayer))
        {
            path.Add(target.transform.position);
            return path;
        }
        foreach (var item in Waypoints)
        {
            item.IsAnalysed = false;
            item.Distance = Mathf.Infinity;
            item.Neighbours.Remove(target);
        }
        FindNeighbours(start, Waypoints, radius);
        FindNeighbours(target, Waypoints, true);
        start.Distance = 0;
        start.prevPoint = null;
        _queue.Enqueue(start);
        while (_queue.Count > 0)
        {
            _current = _queue.Dequeue();
            if (_current.IsAnalysed) continue;
            for (int i = _current.Neighbours.Count - 1; i >= 0; i--)
            {
                if (_current.Neighbours[i] == null) continue;
                if (!_current.Neighbours[i].IsAnalysed)
                {
                    _queue.Enqueue(_current.Neighbours[i]);
                    float distance = _current.Distance + Vector2.Distance(_current.transform.position, _current.Neighbours[i].transform.position);
                    if (distance < _current.Neighbours[i].Distance)
                    {
                        _current.Neighbours[i].Distance = distance;
                        _current.Neighbours[i].prevPoint = _current;
                    }
                    else if (_current.Neighbours[i].Distance < _current.Distance - distance)
                    {
                        _current.Distance = _current.Neighbours[i].Distance + distance;
                        RewriteDistance();
                        _current.prevPoint = _current.Neighbours[i];
                    }
                }
            }
            
            _current.IsAnalysed = true;
        }
        foreach (var item in start.Neighbours)
        {
            item.Neighbours.Remove(start);
        }

        _current = target;
        while (_current.prevPoint != null)
        {
            path.Insert(0, _current.transform.position);
            _current = _current.prevPoint;
        }
        return path;
    }
    private void FindNeighbours(WayPoint point, List<WayPoint> list, bool isMutually)
    {
        point.Neighbours.Clear();
        for (int i = 0; i < list.Count; i++)
        {
            if (!Physics2D.Linecast(point.transform.position, list[i].transform.position, _obstacleLayer) && list[i] != point)
            {
                point.Neighbours.Add(list[i]);
                if(isMutually) list[i].Neighbours.Add(point);
            }
        }
    }
    private void FindNeighbours(WayPoint point, List<WayPoint> list, float radius)
    {
        point.Neighbours.Clear();
        for (int i = 0; i < list.Count; i++)
        {
            if (!Physics2D.Linecast(point.transform.position + new Vector3(radius, 0), list[i].transform.position, _obstacleLayer) &&
                !Physics2D.Linecast(point.transform.position + new Vector3(-radius, 0), list[i].transform.position, _obstacleLayer) &&
                !Physics2D.Linecast(point.transform.position + new Vector3(0, radius), list[i].transform.position, _obstacleLayer) &&
                !Physics2D.Linecast(point.transform.position + new Vector3(0, -radius), list[i].transform.position, _obstacleLayer) &&
                list[i] != point)
            {
                point.Neighbours.Add(list[i]);
            }
        }
    }
    private void RewriteDistance()
    {
        WayPoint current;
        Queue<WayPoint> queue = new Queue<WayPoint>();
        queue.Enqueue(_current);
        while (queue.Count > 0)
        {
            current = queue.Dequeue();
            foreach (var neighbour in current.Neighbours)
            {
                float distance = current.Distance + Vector2.Distance(current.transform.position, neighbour.transform.position);
                if (distance < neighbour.Distance)
                {
                    queue.Enqueue(neighbour);
                    neighbour.Distance = distance;
                    neighbour.prevPoint = current;
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private WayPoint wayPoint;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _speed;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _distanceToChangeTarget;
    [SerializeField] private float _updatePathTime;
    [SerializeField] private float _colliderRadius;

    private PathFinder _pathFinder;
    private List<Vector2> path;

    private void Start()
    {
        _pathFinder = PathFinder.instance;
        InvokeRepeating("SearchForPath", 0.1f, _updatePathTime);
    }
    private void SearchForPath() => path = _pathFinder.GetPath(wayPoint, _colliderRadius);
    private void FixedUpdate()
    {
        if (path?.Count > 0)
        {
            Vector2 dir = (path[0] - (Vector2)transform.position).normalized;
            float difSpeedX = (dir.x * _speed - _rb.velocity.x) * _acceleration;
            float difSpeedY = (dir.y * _speed - _rb.velocity.y) * _acceleration;
            _rb.AddForce(new Vector2(difSpeedX, difSpeedY));
            if (((Vector2)transform.position - path[0]).sqrMagnitude < _distanceToChangeTarget)
            {
                transform.position = path[0];
                path.RemoveAt(0);
            }
            for (int i = 1; i < path.Count; i++)
            {
                Debug.DrawLine(path[i], path[i - 1]);
            }
            Debug.DrawLine(path[0], transform.position);
        }
    }
}

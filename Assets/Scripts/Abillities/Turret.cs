using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _firePosition;
    [SerializeField] private float _hp;
    [SerializeField] private float _timeBtwShots;
    [SerializeField] private float _range;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _shootAngle;
    [SerializeField] private float _spreadAngle;

    private GameManager _gameManager;
    private Transform _nearestEnemy;
    private Transform _target;
    private float _shortestDistance;
    private float _shootCooldown;

    private void Start()
    {
        _gameManager = GameManager.instance;
        InvokeRepeating("FindTarget", 0.1f, 0.2f);
    }
    private void Update()
    {
        _shootCooldown -= Time.deltaTime;
        if (_target == null || !IsLookingAtTarget() || _shootCooldown > 0f)
            return;
        _shootCooldown = _timeBtwShots;
        Instantiate(_bulletPrefab, _firePosition.position, Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z - 90 + Random.Range(-_spreadAngle, _spreadAngle)));
    }
    private void FindTarget()
    {
        _shortestDistance = _range;
        foreach(var enemy in _gameManager.Enemies)
        {
            float dstToEnemy = Vector2.Distance(_firePosition.position, enemy.position);
            if (dstToEnemy < _shortestDistance)
            {
                _shortestDistance = dstToEnemy;
                _nearestEnemy = enemy;
            }
            if (_shortestDistance < _range)
                _target = _nearestEnemy;
            else
                _target = null;
        }
    }
    private bool IsLookingAtTarget()
    {
        Vector2 directionToTarget = _target.position - transform.position;
        float finalAngle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, finalAngle), _rotationSpeed * Time.deltaTime);
        float actualAngle = transform.rotation.eulerAngles.z;
        if (Mathf.Abs(actualAngle - finalAngle) < _shootAngle || Mathf.Abs(actualAngle - finalAngle) > 360 - _shootAngle)
            return true;
        else
            return false;
    }
}

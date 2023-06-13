using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenAbility : Ability
{
    [SerializeField] private float _smallCooldown;
    [SerializeField] private float _smallCDInRow;
    private float count;
    private void Start()
    {
        count = _smallCDInRow;
    }
    public override float Use(Vector3 playerPos, float rotZ)
    {
        Instantiate(_projectilePrefab, playerPos, Quaternion.Euler(0f, 0f, rotZ + 90 + Random.Range(-_spreadAngle, _spreadAngle)));
        if (count > 0)
        {
            count--;
            return _smallCooldown;
        }
        else
        {
            count = _smallCDInRow;
            return _cooldown;
        }
    }
}

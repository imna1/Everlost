using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAbility : Ability
{
    public override float Use(Vector3 playerPos, float rotZ, out GameObject turret)
    {
        turret = Instantiate(_projectilePrefab, playerPos, Quaternion.Euler(0f, 0f, rotZ - 90));
        return _cooldown;
    }
}

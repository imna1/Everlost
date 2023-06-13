using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scythe : MeleeWeapon
{
    [SerializeField] private MeleeWeaponStroke[] _chargedStrokes;
    public override void ChargedAtack(Vector2 attackPosition)
    {
        if (_projectile != null)
            Destroy(_projectile);
        multiplayer = (int)transform.localScale.x;
        _projectile = Instantiate(proj, transform.position, Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z + _angleOffset * multiplayer));
        _projectile.transform.parent = transform;
        CanRotate = false;
        StartCoroutine(Splash(_chargedStrokes));
    }
}

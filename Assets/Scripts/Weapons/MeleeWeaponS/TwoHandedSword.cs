using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoHandedSword : MeleeWeapon
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private GameObject _chargedProj;
    [SerializeField] private float _stayingAfterChargedAttack;
    public override void ChargedAtack(Vector2 attackPosition)
    {
        if (_projectile != null)
            Destroy(_projectile);
        multiplayer = (int)transform.localScale.x;
        _projectile = Instantiate(_chargedProj, transform.position, Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z + _angleOffset * multiplayer));
        _projectile.GetComponent<TwoHandedProj>().Epicentre = transform.position;
        _playerController.FreezePlayer();
        CanRotate = false;
        Invoke("StopAttack", _stayingAfterChargedAttack);
    }
    private void StopAttack()
    {
        _playerController.CanMove = true;
        EnableRotation();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Katana : MeleeWeapon
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _dashTime;
    [SerializeField] private float _dashSpeed;
    private SpearProj _proj;
    private PlayerController _playerController;
    private void Start()
    {
        CanRotate = true;
        _angleOffset = _sprite.rotation.eulerAngles.z;
        _playerController = _player.GetComponent<PlayerController>();
    }
    public override void ChargedAtack(Vector2 attackPosition)
    {
        if (_projectile != null)
            Destroy(_projectile);
        multiplayer = (int)transform.localScale.x;
        _projectile = Instantiate(proj, transform.position, Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z + _angleOffset * multiplayer));
        _projectile.transform.parent = transform;
        _proj = _projectile.GetComponent<SpearProj>();
        CanRotate = false;
        StartCoroutine(Dash());
    }
    private IEnumerator Dash()
    {
        _playerController.FreezePlayer();
        float t = _dashTime;
        float radians = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
        Vector3 direction = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians)).normalized;
        transform.rotation = Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z - _angleOffset * multiplayer);
        while (!_proj.IsStopped && t > 0)
        {
            _player.Translate(direction * _dashSpeed * Time.deltaTime);
            t -= Time.deltaTime;
            yield return null;
        }
        Destroy(_projectile);
        _playerController.CanMove = true;
        EnableRotation();
    }
}

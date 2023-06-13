using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MeleeWeapon
{
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _tipPosition;
    [SerializeField] private float _speed;
    [SerializeField] private float _timeToReturn;
    [SerializeField] private float _distanceToGrab;
    private SpearProj _proj;
    
    private void Start()
    {
        CanRotate = true;
        _angleOffset = _sprite.rotation.eulerAngles.z;
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
        StartCoroutine(Throw());
    }
    private IEnumerator Throw()
    {
        float radians = _angleOffset * Mathf.Deg2Rad * multiplayer;
        Vector2 direction = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)).normalized;
        Vector2 position = transform.localPosition;
        Transform parent = transform.parent; 
        transform.parent = null;
        transform.rotation = Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z - _angleOffset * multiplayer);
        while (!_proj.IsStopped)
        {
            transform.Translate(direction * _speed * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(_timeToReturn);
        transform.position = _tipPosition.position;
        while (Vector2.Distance(_player.position, transform.position) > _distanceToGrab && Vector2.Distance(_player.position, _tipPosition.position) > _distanceToGrab)
        {
            float rotZ = Mathf.Atan2((_player.position - transform.position).y, (_player.position - transform.position).x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotZ - _angleOffset * multiplayer);
            transform.Translate(direction * _speed * Time.deltaTime);
            yield return null;
        }
        transform.parent = parent;
        transform.localPosition = position;
        Destroy(_projectile);
        EnableRotation();
    }
}

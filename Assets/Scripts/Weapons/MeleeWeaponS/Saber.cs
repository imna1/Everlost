using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saber : MeleeWeapon
{
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _centre;
    [SerializeField] private float _speed;
    [SerializeField] private float _angularSpeed;
    [SerializeField] private float _decceleration;
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
        float radians = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
        Vector3 direction = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians)).normalized;
        Vector2 position = transform.localPosition;
        Transform parent = transform.parent;
        transform.parent = null;
        float speed = _speed;
        while (!_proj.IsStopped && speed > 0)
        {
            transform.position += direction * speed * Time.deltaTime;
            speed -= _decceleration * Time.deltaTime;
            transform.RotateAround(_centre.position, Vector3.forward, _angularSpeed);
            yield return null;
        }
        speed = 0;
        while (Vector2.Distance(_player.position, transform.position) > _distanceToGrab)
        {
            direction = (transform.position - _player.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
            speed -= _decceleration * Time.deltaTime;
            transform.RotateAround(_centre.position, Vector3.forward, _angularSpeed);
            yield return null;
        }
        transform.parent = parent;
        transform.localPosition = position;
        Destroy(_projectile);
        EnableRotation();
    }
}

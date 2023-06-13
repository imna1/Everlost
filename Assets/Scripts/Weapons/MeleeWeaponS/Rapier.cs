using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rapier : MeleeWeapon
{
    [SerializeField] private float _maxOffset;
    [SerializeField] private float _lungeDuration;
    [SerializeField] private float _lungeSpeed;
    [SerializeField] private int _lungeCount;
    public override void ChargedAtack(Vector2 attackPosition)
    {
        if (_projectile != null)
            Destroy(_projectile);
        multiplayer = (int)transform.localScale.x;
        _projectile = Instantiate(proj, transform.position, Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z + _angleOffset * multiplayer));
        _projectile.transform.parent = transform;
        CanRotate = false;
        StartCoroutine(Lunge(attackPosition));
    }
    private IEnumerator Lunge(Vector2 attackPosition)
    {
        Vector2 offsetDirection = new Vector2((attackPosition - (Vector2)transform.position).y, - (attackPosition - (Vector2)transform.position).x).normalized;
        Vector2 position = transform.localPosition;
        float t = _lungeDuration;
        for (int i = 0; i < _lungeCount; i++)
        {
            transform.localPosition = position + Random.Range(-_maxOffset, _maxOffset) * offsetDirection;
            Vector3 direction = (attackPosition - (Vector2)transform.position).normalized;
            float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotZ - _angleOffset * multiplayer);
            while (t > 0)
            {
                transform.position += direction * _lungeSpeed * Time.deltaTime;
                t -= Time.deltaTime;
                yield return null;
            }
            while (t < _lungeDuration)
            {
                transform.position -= direction * _lungeSpeed * Time.deltaTime;
                t += Time.deltaTime;
                yield return null;
            }
        }
        transform.localPosition = position;
        Destroy(_projectile);
        EnableRotation();
    }
}

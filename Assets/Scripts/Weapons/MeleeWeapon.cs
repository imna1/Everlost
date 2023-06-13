using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    [SerializeField] protected MeleeWeaponStroke[] _strokes;
    [SerializeField] protected Transform _sprite;
    protected float _angleOffset;
    protected float t;
    protected Quaternion _startRotation;
    protected GameObject _projectile;
    protected int multiplayer;
    private void Start()
    {
        CanRotate = true;
        _angleOffset = _sprite.rotation.eulerAngles.z;
    }
    public override void Atack()
    {
        if (_projectile != null)
            Destroy(_projectile);
        multiplayer = (int)transform.localScale.x;
        _projectile = Instantiate(proj, transform.position, Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z + _angleOffset * multiplayer));
        _projectile.transform.parent = transform;
        CanRotate = false;
        StartCoroutine(Splash(_strokes));
    }
    protected IEnumerator Splash(MeleeWeaponStroke[] strokes)
    {
        for (int j = 0; j < strokes.Length; j++)
        {
            yield return new WaitForSeconds(strokes[j].Delay);
            int count = (int)strokes[j].Angle / 179;
            float speed = strokes[j].Angle / strokes[j].TimeToStroke;
            float actualSpeed = 179 / speed;
            t = 0;
            _startRotation = Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z);
            for (int i = 0; i < count; i++)
            {
                while (t < 1)
                {
                    t += Time.deltaTime / actualSpeed;
                    transform.rotation = Quaternion.Slerp(_startRotation, Quaternion.Euler(0f, 0f, _startRotation.eulerAngles.z - 179 * multiplayer), t);
                    yield return new WaitForEndOfFrame();
                }
                _startRotation = transform.rotation;
                t = 0;
            }
            actualSpeed = strokes[j].Angle % 179 / speed;
            while (t < 1)
            {
                t += Time.deltaTime / actualSpeed;
                transform.rotation = Quaternion.Slerp(_startRotation, Quaternion.Euler(0f, 0f, _startRotation.eulerAngles.z - (strokes[j].Angle % 179) * multiplayer), t);
                yield return new WaitForEndOfFrame();
            }
        }
        Destroy(_projectile);
        EnableRotation();
    }
}

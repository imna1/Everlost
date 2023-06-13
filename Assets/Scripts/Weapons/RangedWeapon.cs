using UnityEngine;

public class RangedWeapon : Weapon
{
    [SerializeField] private float _spreadAngle;
    [SerializeField] private float _chargedSpreadAngle;
    [SerializeField] private float _chargedDamageMultiplayer;
    [SerializeField] private float _kickbackForce;
    [SerializeField] private Rigidbody2D _playerRb;
    public override void Atack()
    {
        Instantiate(proj, transform.position, Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z - 90 + Random.Range(-_spreadAngle, _spreadAngle)));
    }
    public override void ChargedAtack(Vector2 attackPosition)
    {
        float zRotation = transform.rotation.eulerAngles.z - 90 + Random.Range(-_chargedSpreadAngle, _chargedSpreadAngle);
        var go = Instantiate(proj, transform.position, Quaternion.Euler(0f, 0f, zRotation));
        go.GetComponent<Projectile>().Damage *= _chargedDamageMultiplayer;
        float xForce = Mathf.Cos((zRotation + 90) * Mathf.Deg2Rad);
        float yForce = Mathf.Sin((zRotation + 90) * Mathf.Deg2Rad);
        var a = new Vector2(xForce, yForce);
        _playerRb.AddForce(-a * _kickbackForce, ForceMode2D.Impulse);
    }
}

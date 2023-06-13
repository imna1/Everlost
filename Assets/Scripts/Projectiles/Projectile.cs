using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Projectile : MonoBehaviour
{
    public float Damage;
    [SerializeField] protected float _rageAmount;
    [SerializeField] protected float _damageSpread;
    protected CharacteristicManager cm;
    protected void Awake()
    {
        cm = CharacteristicManager.instance;
    }

    protected virtual void DealDamage(Enemy enemy, float damage)
    {
        damage += Random.Range(-_damageSpread, _damageSpread) * damage;
        damage = damage * cm.attackDamageMultiplayer * (1 + (cm.rageDamageMultiplayer - 1) * cm.rage); // нет разделения на классы при умножении
        enemy.TakeDamage(damage);
        cm.UpdateRage(_rageAmount);
    }
}

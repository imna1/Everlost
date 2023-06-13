using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScytheProj : MeleeProj
{
    [SerializeField] private float LifeStealPercentage;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            DealDamage(collider.gameObject.GetComponent<Enemy>(), Damage, LifeStealPercentage);
        }
    }
    private void DealDamage(Enemy enemy, float damage, float lifeSteal)
    {
        damage += Random.Range(-_damageSpread, _damageSpread) * damage;
        damage = damage * cm.attackDamageMultiplayer * (1 + (cm.rageDamageMultiplayer - 1) * cm.rage); // нет разделения на классы при умножении
        enemy.TakeDamage(damage);
        cm.UpdateRage(_rageAmount);
        cm.UpdateHP(damage * lifeSteal);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeProj : Projectile
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            DealDamage(collider.gameObject.GetComponent<Enemy>(), Damage);
        }
    }
}

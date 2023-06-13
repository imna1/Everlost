using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearProj : MeleeProj
{
    [HideInInspector] public bool IsStopped = false;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            DealDamage(collider.gameObject.GetComponent<Enemy>(), Damage);
        }
        if (collider.gameObject.tag == "Obstacle")
        {
            IsStopped = true;
        }
    }
}

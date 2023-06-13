using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistantProj : Projectile
{
    [SerializeField] protected float _speed;
    [SerializeField] protected float _lifeTime;
    private void Start()
    {
        Destroy(gameObject, _lifeTime);
    }

    protected void Update()
    {
        transform.Translate(Vector2.up * _speed * Time.deltaTime);
    }
    protected void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            DealDamage(collider.gameObject.GetComponent<Enemy>(), Damage);
            Destroy(gameObject);
        }
        else if (collider.gameObject.tag == "Obstacle")
        {
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveProj : DistantProj
{
    [SerializeField] private float _explosiveRange;
    [SerializeField] private float _explosiveMaxDamage;
    [SerializeField] private float _percentMinAreaDamage; // от 0 до 1
    private void Start()
    {
        Destroy(gameObject, _lifeTime);
    }
    private void OnDestroy()
    {
        for (int i = GameManager.instance.Enemies.Count - 1; i >= 0; i--)
        {
            float distance = Vector2.Distance(transform.position, GameManager.instance.Enemies[i].position);
            if (distance <= _explosiveRange)
            {
                float damage = _explosiveMaxDamage * (_percentMinAreaDamage + (_explosiveRange - distance) / _explosiveRange * (1 - _percentMinAreaDamage));
                DealDamage(GameManager.instance.Enemies[i].gameObject.GetComponent<Enemy>(), damage);
            }
        }
    }
}

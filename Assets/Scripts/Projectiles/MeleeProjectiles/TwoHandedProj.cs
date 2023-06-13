using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoHandedProj : MeleeProj
{
    [HideInInspector] public Vector2 Epicentre;
    [SerializeField] private float _lifeTime;
    [SerializeField] private float _expandSpeed;
    [SerializeField] private float _knockbackForce;
    private Vector3 _scale;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            DealDamage(collider.gameObject.GetComponent<Enemy>(), Damage);
            collider.gameObject.GetComponent<Rigidbody2D>().AddForce(((Vector2)collider.transform.position - Epicentre).normalized * _knockbackForce, ForceMode2D.Impulse);
        }
    }
    private void Start()
    {
        _scale = new Vector3(_expandSpeed, _expandSpeed, _expandSpeed);
        Destroy(gameObject, _lifeTime);
    }
    private void Update()
    {
        transform.localScale += _scale * Time.deltaTime;
    }
}

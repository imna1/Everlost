using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject _amountOfDamageTextPrefab;
    [SerializeField] private Transform _textPosition;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _textLifeTime;
    [SerializeField] private float _health;
    public WayPoint WayPoint;
    public float RadiusToSpotPlayer;
    public float ScreamDistance;
    public bool IsActive = false;

    private float _valueOnText;
    private GameObject _amountOfDamageTextObj;

    public void TakeDamage(float amount)
    {
        _health -= amount;
        if(_health <= 0)
        {
            Die();
        }
        if (!IsActive)
        {
            SwitchToActive();
        }
        if(_amountOfDamageTextObj == null)
        {
            _valueOnText = amount;
        }
        else
        {
            _valueOnText += amount;
            Destroy(_amountOfDamageTextObj);
        }
        _amountOfDamageTextObj = Instantiate(_amountOfDamageTextPrefab, _textPosition.position, Quaternion.identity);
        _amountOfDamageTextObj.GetComponent<AmountOfDamageTMP>().Text = Mathf.Round(_valueOnText).ToString();
        Destroy(_amountOfDamageTextObj, _textLifeTime);
    }
    public void SwitchToActive()
    {
        IsActive = true;
        GetComponent<EnemyController>().enabled = true;
    }
    private void Die()
    {
        GameManager.instance.OnEnemyDied(transform);
        Destroy(gameObject);
    }
}

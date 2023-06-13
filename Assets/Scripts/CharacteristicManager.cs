using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacteristicManager : MonoBehaviour
{
    public static CharacteristicManager instance;
    [SerializeField] private PlayerUI playerUI;

    public float rage;
    public float HP;

    public float rageTime;
    public float rageDamageMultiplayer;
    public float attackDamageMultiplayer;
    public float attackSpeed;
    public float criticalAttackChance;
    public float criticalAttackDamage;
    public float damageSpread;
    public float moveSpeed = 10;
    public float accelration = 6;
    public float deccelration = 5;
    public float moveSpeedinInvisibilityMultiplayer = 1;
    public float quantityofHealthPoints;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        UpdateHP(0);
    }
    private void Update()
    {
        UpdateRage(-Time.deltaTime / rageTime);
    }
    public void UpdateRage(float amount)
    {
        rage = Mathf.Clamp(rage + amount, 0, 1);
        playerUI.UpdateRageBar(rage / 360 * 135);
    }
    public void UpdateHP(float amount)
    {
        HP = Mathf.Clamp(HP + amount, 0, quantityofHealthPoints);
        playerUI.UpdateHpBar(HP / quantityofHealthPoints / 360 * 135);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacteristics : MonoBehaviour
{
    [Header("Movement")]
    public float Speed;
    public float Accelration;
    public float Deccelration;
    public float InvisibleSpeed;
    [Header("Attack")]
    public GameObject[] Weapons;
    public float Damage;
    public float AttackSpeed;
    public float CritChance;
    public float CritDamage;
    public float DamageSpread;
    public float MeleeRange;
    public float RangedAtackDistance;
    public float RangeWeaponReload;
    public float ArrowFlightSpeed;
    [Header("Hp")]
    public float MaxHp;
    public float HpRecovery;
    [Header("Stamina")]
    public float MaxStamina;
    public float StaminaRecovery;
    [Header("Mana")]
    public float MaxMana;
    public float ManaRecovery;
    [Header("Defence")]
    public float MagicalDefenseCoef;
    public float PhysycalDefenseCoef;
    [Header("Other")]
    public float RollRange;
    public float RollCount;
    public float InvisibilityTime;

/*Arrow Fligt Speed
Attack Damage
Attack Speed
Critical Attack Chance
Critical Attack Damage
Damage Spread
Health Points Recovery Speed
Invisibility Time
Magical Defence Coefficient
Mana Points Recovery Speed
Melee Attack Range
Move Speed
Move Speed in Invisibility
Physical Defence Coefficient
Quantity of Health Points
Quantity of Mana Points
Quantity of Rolls
Quantity of Stamina Points
Range Weapon Reload Speed
Ranged Attack Range
Roll Range
Stamina Points Recovery Speed
*/

}

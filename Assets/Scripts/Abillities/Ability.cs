using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Ability : MonoBehaviour
{
    [SerializeField] protected float _cooldown;
    [SerializeField] protected GameObject _projectilePrefab;
    [SerializeField] protected float _spreadAngle;
    public virtual float Use(Vector3 playerPos, float rotZ)
    {
        Instantiate(_projectilePrefab, playerPos, Quaternion.Euler(0f, 0f, rotZ + 90 + Random.Range(-_spreadAngle, _spreadAngle)));
        return _cooldown;
    }
    public virtual float Use(Vector3 playerPos, float rotZ, out GameObject proj)
    {
        proj = Instantiate(_projectilePrefab, playerPos, Quaternion.Euler(0f, 0f, rotZ + 90 + Random.Range(-_spreadAngle, _spreadAngle)));
        return _cooldown;
    }
}

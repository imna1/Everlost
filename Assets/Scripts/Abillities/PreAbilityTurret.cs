using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreAbilityTurret : PreAbility
{
    private GameObject _currentTurret;
    private GameManager _gameManager;
    private void Start()
    {
        _gameManager = GameManager.instance;
    }
    public override bool Use(Vector3 playerPos, float rotZ, int index)
    {
        if (index >= _ability.Count) return false;
        if (_currentTurret != null) Destroy(_currentTurret);
        _cooldown = _ability[index].Use(playerPos, rotZ, out _currentTurret);
        _gameManager.SetRoomChild(_currentTurret.transform);
        _cooldownTime = _cooldown;
        _text.gameObject.SetActive(true);
        _coveringSprite.gameObject.SetActive(true);
        return true;
    }
}

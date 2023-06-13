using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PreAbility : MonoBehaviour
{
    [SerializeField] protected Image _coveringSprite;
    [SerializeField] protected TextMeshProUGUI _text;
    [SerializeField] protected List<Ability> _ability;
    
    protected float _cooldownTime;
    protected float _cooldown;
    public bool CanChoose() => _cooldownTime <= 0 && _ability.Count > 0;

    public virtual bool Use(Vector3 playerPos, float rotZ, int index)
    {
        if(index >= _ability.Count) return false;
        _cooldown = _ability[index].Use(playerPos, rotZ);
        _cooldownTime = _cooldown;
        _text.gameObject.SetActive(true);
        _coveringSprite.gameObject.SetActive(true);
        return true;
    }
    protected void Update()
    {
        _cooldownTime -= Time.deltaTime;
        if (_cooldownTime > 0)
        {
            _coveringSprite.fillAmount = _cooldownTime / _cooldown;
            _text.text = Mathf.Ceil(_cooldownTime).ToString();
        }
        else
        {
            _text.gameObject.SetActive(false);
            _coveringSprite.gameObject.SetActive(false);
        }
    }
}

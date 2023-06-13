using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Image _HpBar;
    [SerializeField] private Image _RageBar;
    [SerializeField] private GameObject Player;
    private void Update()
    {
        transform.position = Player.transform.position;
    }
    public void UpdateHpBar(float value)
    {
        _HpBar.fillAmount = value;
    }
    public void UpdateRageBar(float value)
    {
        _RageBar.fillAmount = value;

    }
}

using UnityEngine;
using TMPro;

public class AmountOfDamageTMP : MonoBehaviour
{
    [SerializeField] private TextMeshPro _textMeshPro;
    public string Text;
    private void Start()
    {
        _textMeshPro.text = Text;
    }
}
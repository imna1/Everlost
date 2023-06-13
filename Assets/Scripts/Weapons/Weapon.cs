using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public GameObject proj;
    public float TimeToCharge;
    public float Cooldown;
    public float ChargedCooldown;
    [HideInInspector] public bool CanRotate;

    private void Start()
    {
        CanRotate = true;
    }
    public virtual void Atack()
    {

    }
    public virtual void ChargedAtack(Vector2 attackPosition)
    {
        
    }
    protected virtual void EnableRotation()
    {
        CanRotate = true;
    }
}

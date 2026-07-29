using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private PlayerStateConfig config;
    private int damage;
    
    void Start()
    {
        damage = config.lightAttackDamage;
    }

    public int DealDamage()
    {
        return damage;
    }
}
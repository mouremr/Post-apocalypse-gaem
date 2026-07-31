using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private PlayerStateConfig config;
    private int damage;
    
    //todo: Make this work for light AND heavy attack
    void Start()
    {
        damage = config.lightAttackDamage;
    }

    public int DealDamage()
    {
        return damage;
    }
}
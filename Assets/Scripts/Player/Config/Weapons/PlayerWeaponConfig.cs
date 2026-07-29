using UnityEngine;

// [CreateAssetMenu(fileName = "playerWeaponConfig", menuName = "Player/Player Weapon Config")]
// public class PlayerWeaponConfig : ScriptableObject
// {
//     public int lightAttackCost = 5;
//     public int lightAttackDamage = 3;

//     public int heavyAttackCost = 10;
//     public int heavyAttackDamage = 7;
// }


[System.Serializable]
public class AttackData
{
    public string attackTrigger;   // matches the Animator trigger name, e.g. "lightAttack"
    public int staminaCost;
    public int damage;
    public float force;
}

[CreateAssetMenu(fileName = "playerWeaponConfig", menuName = "Player/Player Weapon Config")]
public class PlayerWeaponConfig : ScriptableObject
{
    public string weaponName;
    public AttackData lightAttack;
    public AttackData heavyAttack;
}
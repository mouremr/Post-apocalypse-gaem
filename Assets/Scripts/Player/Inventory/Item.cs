using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public Sprite itemIcon;
    public string itemName;
}

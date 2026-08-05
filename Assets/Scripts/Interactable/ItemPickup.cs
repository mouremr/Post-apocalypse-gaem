using UnityEngine;

public class ItemPickup : Interactible
{
    [SerializeField] private Item item;
    [SerializeField] private int quantity = 1;

    public override void Interact()
    {
        bool added = InventoryManager.Instance.AddItem(item);
        if (added)
        {
            Destroy(gameObject);
        }
    }
}
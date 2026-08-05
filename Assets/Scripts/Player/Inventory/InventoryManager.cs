using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    [SerializeField] private GameObject InventoryScreen;
    [SerializeField] private InventorySlot[] inventorySlots;
    private bool InventoryEnabled;
    [SerializeField] private GameObject inventoryItemprefrab;
    
    private void Awake()
    {
        Instance = this;
    }

    public void ToggleInventory()
    {
        InventoryEnabled = !InventoryEnabled;
        Debug.Log(InventoryEnabled);
        InventoryScreen.SetActive(InventoryEnabled);
        Time.timeScale = InventoryEnabled ? 0f : 1f;
    }

    public bool AddItem(Item item)
    {
        Debug.Log("Adding item");
        for(int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>(); //edit to be inventoryitem
            if(itemInSlot == null) 
            {
                Debug.Log("Open slot found");
                SpawnNewItem(item, slot);
                return true;
            }
            
        }
        return false;
    }

    public void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemprefrab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }
}

using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject InventoryScreen;
    private PlayerInput input;
    public bool InventoryEnabled { get; private set; }
    
    void Start()
    {
        InventoryEnabled = false;
        input = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        //maybe move to grounded state so that inv cant be opened midair or while fighting
        if (input.ToggleInventory)
        {
            InventoryEnabled = !InventoryEnabled;
            ToggleInventory();
        }
    }

    private void ToggleInventory()
    {
        InventoryScreen.SetActive(InventoryEnabled);
        Time.timeScale = InventoryEnabled ? 0f : 1f;
        input.PlayerControlsEnabled = !InventoryEnabled;
    }
}

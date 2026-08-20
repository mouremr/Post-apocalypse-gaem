using UnityEngine;

public class ItemPickup : Interactible
{
    [SerializeField] private Item item;
    [SerializeField] private int quantity = 1;
    private readonly float BobSpeed = 2.3f;
    private readonly float BobHeight = .1f;
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    public override void Interact()
    {
        bool added = InventoryManager.Instance.AddItem(item);
        if (added)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * BobSpeed) * BobHeight;

        transform.position = startPosition + new Vector3(0, yOffset, 0);
    }
}
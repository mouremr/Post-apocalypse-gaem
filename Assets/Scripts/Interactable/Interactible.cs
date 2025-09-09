using UnityEngine;

public abstract class Interactible : MonoBehaviour
{
    [SerializeField] private string interactionText = "Interact";
    
    public string InteractionText => interactionText;
    
    public abstract void Interact(GameObject player);
    
    // Optional: Visual feedback when player is nearby
    public virtual void OnPlayerNearby(GameObject player) { }
    public virtual void OnPlayerLeft(GameObject player) { }
}
using UnityEngine;
using System.Collections.Generic;

public class InteractionDetector : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 1.5f;
    [SerializeField] private LayerMask interactibleLayer;
    
    public Interactible CurrentInteractible { get; private set; }
    public bool HasInteractible => CurrentInteractible != null;
    
    private List<Interactible> nearbyInteractibles = new List<Interactible>();
    
    private void Update()
    {
        FindClosestInteractible();
    }
    
    private void FindClosestInteractible()
    {
        //Interactible previousInteractible = CurrentInteractible;
        CurrentInteractible = null;
        
        float closestDistance = Mathf.Infinity;
        
        foreach (var interactible in nearbyInteractibles)
        {
            float distance = Vector2.Distance(transform.position, interactible.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                CurrentInteractible = interactible;
            }
        }
    }
    
    // Visualize detection radius in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        
        if (CurrentInteractible != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, CurrentInteractible.transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var interactible = other.GetComponent<Interactible>() ?? other.GetComponentInParent<Interactible>();
        if(interactible != null) Debug.Log("Interactible found: " + other.gameObject.name);
        if (interactible != null) nearbyInteractibles.Add(interactible);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var interactible = other.GetComponent<Interactible>() ?? other.GetComponentInParent<Interactible>();
        if (interactible != null) nearbyInteractibles.Remove(interactible);
    }
}
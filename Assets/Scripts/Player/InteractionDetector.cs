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
        DetectInteractibles();
        FindClosestInteractible();
        
        // Debug: Show current interactible in console
        if (CurrentInteractible != null)
        {
            Debug.Log($"Near interactible: {CurrentInteractible.name}");
        }
    }
    
    private void DetectInteractibles()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(
            transform.position, 
            detectionRadius, 
            interactibleLayer
        );
        
        // Debug: Show how many colliders detected
        //Debug.Log($"Detected {hitColliders.Length} colliders in layer {LayerMask.LayerToName(6)}");
        
        // Clear previous list
        nearbyInteractibles.Clear();
        
        // Add new interactibles
        foreach (var collider in hitColliders)
        {
            Interactible interactible = collider.GetComponent<Interactible>();
            if (interactible == null)
            {
                interactible = collider.GetComponentInParent<Interactible>();
            }
            if (interactible != null)
            {
                //Debug.Log($"Found interactible: {interactible.name}");
                nearbyInteractibles.Add(interactible);
            }
        }
    }
    
    private void FindClosestInteractible()
    {
        Interactible previousInteractible = CurrentInteractible;
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
        
        // Debug when interactible changes
        if (previousInteractible != CurrentInteractible)
        {
            if (CurrentInteractible != null)
                Debug.Log($"Now targeting: {CurrentInteractible.name}");
            else
                Debug.Log("No interactible targeted");
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
}
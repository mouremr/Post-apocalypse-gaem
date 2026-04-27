using UnityEngine;

public class FadeForeground : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private float fadedAlpha = 0.5f;
    [SerializeField] private float fadeSpeed = 5f;

    private bool playerInside = false;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
            playerInside = true;
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
            playerInside = false;
    }

    void Update()
    {
        float targetAlpha = playerInside ? fadedAlpha : 1f;

        Color c = sprite.color;
        c.a = Mathf.Lerp(c.a, targetAlpha, Time.deltaTime * fadeSpeed);
        sprite.color = c;
    }
}

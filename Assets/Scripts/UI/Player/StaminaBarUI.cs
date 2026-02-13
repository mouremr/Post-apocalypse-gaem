using UnityEngine;
using UnityEngine.UI;

public class StaminaBarUI : MonoBehaviour
{
    [Header("References")]
    public Image fillImage;
    public CanvasGroup canvasGroup;
    public Transform player;

    [Header("Positioning")]
    public Vector3 offset = new Vector3(0, 1.2f, 0);

    [Header("Fade Settings")]
    public float fadeSpeed = 3f;
    public float visibleAlpha = 1f;
    public float hiddenAlpha = 0f;

    void LateUpdate()
    {
        transform.position = player.position + offset;

        float staminaPercent = PlayerState.GetCurrentStamina() / PlayerState.GetMaxStamina();

        fillImage.fillAmount = staminaPercent;


        //fade bar
        float targetAlpha =
            staminaPercent >= 0.99f ? hiddenAlpha : visibleAlpha;

        canvasGroup.alpha = Mathf.Lerp(
            canvasGroup.alpha,
            targetAlpha,
            Time.deltaTime * fadeSpeed
        );
    }
}
    
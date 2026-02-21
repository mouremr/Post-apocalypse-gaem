using UnityEngine;
using UnityEngine.UI;

public class StaminaBarUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image fillImage;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject player;

    [Header("Positioning")]
    [SerializeField] private Vector3 offset = new Vector3(0, 1.2f, 0);

    [Header("Fade Settings")]
    [SerializeField] private float fadeSpeed = 3f;
    [SerializeField] private float visibleAlpha = 1f;
    [SerializeField] private float hiddenAlpha = 0f;

    private StateMachine playerStateMachine;


    void Start()
    {
        playerStateMachine = player.GetComponent<StateMachine>();
    }

    void LateUpdate()
    {
        transform.position = playerTransform.position + offset;

        float staminaPercent = playerStateMachine.CurrentStamina / playerStateMachine.MaxStamina;

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
    
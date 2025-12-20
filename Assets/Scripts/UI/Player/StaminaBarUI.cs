using UnityEngine;
using UnityEngine.UI;

public class StaminaBarUI : MonoBehaviour
{
    [SerializeField] private Image staminaFill;

    void Update()
    {
        float current = PlayerState.GetCurrentStamina();
        float max = PlayerState.GetMaxStamina();

        staminaFill.fillAmount = current / max;
    }
}
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("References")]
    public Image fillImage;

    void LateUpdate()
    {

        // Update fill
        float healthPercent = PlayerState.GetCurrentHealth() / PlayerState.GetMaxHealth();

        Debug.Log(healthPercent);

        fillImage.fillAmount = healthPercent;
    }
}

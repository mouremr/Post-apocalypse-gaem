using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("References")]
    public Image fillImage;
    [SerializeField] private GameObject player; 
    

    private StateMachine playerStateMachine;


    void Start()
    {
        playerStateMachine = player.GetComponent<StateMachine>();
    }

    void LateUpdate()
    {

        // Update fill
        float healthPercent = playerStateMachine.CurrentHealth / playerStateMachine.MaxHealth;

        fillImage.fillAmount = healthPercent;
    }
}

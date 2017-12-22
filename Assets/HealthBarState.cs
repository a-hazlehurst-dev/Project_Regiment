
using UnityEngine;
using UnityEngine.UI;

public class HealthBarState : MonoBehaviour
{
    public Slider healthBar;
    public Slider staminaBar;
    public Brain Brain;
	
	void Update () {

	    if (!Brain.Character.IsActive())
	    {
	        staminaBar.gameObject.SetActive(false);
	        healthBar.gameObject.SetActive(false);

	        return;
	    }

	    staminaBar.maxValue = Brain.Character.MaxStamina;
	    healthBar.maxValue = Brain.Character.MaxHitPoints;

        CalculateHealth();
	    CalculateStamina();
	}
    private void CalculateStamina()
    {
        if (Mathf.CeilToInt(staminaBar.value) == Brain.Character.Stamina)
            return;

        staminaBar.value = Brain.Character.Stamina;
    }
    private void CalculateHealth()
    {
        if (Mathf.CeilToInt(healthBar.value) == Brain.Character.HitPoints)
            return;

        healthBar.value = Brain.Character.HitPoints;
    }
}

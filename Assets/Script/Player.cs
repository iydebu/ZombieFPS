using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    public Slider StaminaSlider;
    public Slider HealthSlider;
    public float maxHealth = 100f;

    private float maxStamina = 100.0f;
    private float currentStamina;
    private float currentHealth;
    private Coroutine recharge;
    private WaitForSeconds rechargeTime = new WaitForSeconds(0.1f);

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        currentStamina = maxStamina;
        StaminaSlider.maxValue = maxStamina;
        StaminaSlider.value = maxStamina;

        currentHealth = maxHealth;
        HealthSlider.maxValue = maxHealth;
        HealthSlider.value = maxHealth;
    }
    private void FixedUpdate()
    {
        if (currentHealth <= 0)
        {
            Debug.Log("GameOver!");
            Time.timeScale = 0f;
        }
    }
    public void TakeDamage(float damageValue)
    {
        currentHealth -= damageValue;
        Debug.Log("Player Health: " + currentHealth);
        HealthSlider.value = currentHealth;
    }

    public void UseStamina(float StaminaValue)
    {
        if(currentStamina - StaminaValue >= 0)
        {
            currentStamina -= StaminaValue * 1.5f;
            StaminaSlider.value = currentStamina;
            
            //Debug.Log(currentStamina);
            if(recharge != null)
            {
                StopCoroutine(recharge);
            }
            recharge = StartCoroutine(ReChargeStamina());
        }
        else
        {
            //Debug.Log("Mot enough Stamina");
        }
    }

    private IEnumerator ReChargeStamina()
    {
        yield return new WaitForSeconds(2);

        while(currentStamina < maxStamina)
        {
            currentStamina += maxHealth/100;
            StaminaSlider.value = currentStamina;
            yield return rechargeTime;
        }
    }
}

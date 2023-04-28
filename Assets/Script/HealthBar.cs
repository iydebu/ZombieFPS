using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Transform healthBar;
    [SerializeField] private Image healthFill;
    [SerializeField] private float lerpSpeed = 5f;
    [SerializeField] private float offsetY = 1f;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private GameObject healthBarObj;

    private Transform mainCamera;
    private Coroutine healthBarCoroutine;
    private float maxHealth;
    private float currentHealth;

    private void Start()
    {
        mainCamera = Camera.main.transform;
        maxHealth = enemyManager.GetHealth();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        // Look towards the main camera
        healthBar.LookAt(mainCamera.position + mainCamera.forward * 100f);
        healthBar.Rotate(0f, 180f, 0f);

        // Update the health fill amount
        float fillAmount = Mathf.Lerp(healthFill.fillAmount, currentHealth / maxHealth, Time.deltaTime * lerpSpeed);
        healthFill.fillAmount = fillAmount;

        // Hide the health bar if enemy is dead
        if (enemyManager != null && enemyManager.GetHealth() <= 0f)
        {
            gameObject.SetActive(false);
        }
    }

    public void SetHealth(float health)
    {
        if (healthBarCoroutine != null)
        {
            StopCoroutine(healthBarCoroutine);
        }
        healthBarObj.SetActive(true);
        currentHealth = health;
        maxHealth = enemyManager.GetMaxHealth();
        healthBarCoroutine = StartCoroutine(ShowHealthBar());
    }

    private IEnumerator ShowHealthBar()
    {
        yield return new WaitForSeconds(3f);
        healthBarObj.SetActive(false);
    }
}
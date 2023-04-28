using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float health = 100f;

    private void FixedUpdate()
    {
        if (health <= 0)
        {
            Debug.Log("GameOver!");
            Time.timeScale = 0f;
        }
    }
    public void TakeDamage(float damageValue)
    {
        health -= damageValue;
        Debug.Log("Player Health: " + health);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamage : MonoBehaviour
{
    [SerializeField] private EnemyManager m_EnemyManager;
    public void DealDamageToPlayer()
    {
       m_EnemyManager.DealDamageToPlayer();
    }
}

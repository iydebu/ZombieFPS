using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;
    [SerializeField] private float health = 100f;
    [SerializeField] private float maxhealth = 100f;
    [SerializeField] private float damage = 20f;
    [SerializeField] private float Playerdamage = 10f;
    [SerializeField] private float runningDistance = 10f;
    [SerializeField] private float walkingDistance = 4f;
    [SerializeField] private float distanceToPlayer;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private Transform attackRay;
    [SerializeField] float attackDistance;

    private bool isDead = false;

    private void Awake()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (!isDead)
        {
            HandleEnemyAttack();
            ManageEnemyBehavior();
        }
    }

    private void ManageEnemyBehavior()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        agent.destination = player.transform.position;

            if (distanceToPlayer <= walkingDistance)
            {
                agent.speed = 0.8f;
                SetAnimatorBooleans(false, true);
            }
            else if (distanceToPlayer <= runningDistance)
            {
                agent.speed = 5f;
                SetAnimatorBooleans(true, false);
            }
            else
            {
                agent.speed = 0.8f;
                SetAnimatorBooleans(false, true);
            }
    }

    private void SetAnimatorBooleans(bool isRunning, bool isWalking)
    {
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isWalking", isWalking);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Damage();
        }
    }

    private void Damage()
    {
        health -= damage;
        healthBar.SetHealth(health);
        if (health <= 0 && !isDead)
        {
            isDead = true;
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        animator.SetTrigger("isDead");
        DisableComponents();
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
    }

    private void DisableComponents()
    {
        agent.enabled = false;
        GetComponent<Collider>().enabled = false;
    }

    public float GetHealth()
    {
        return health;
    }

    public float GetMaxHealth()
    {
        return maxhealth;
    }

    private void HandleEnemyAttack()
    {
        Debug.DrawRay(attackRay.position, transform.forward * attackDistance, Color.red);
        RaycastHit hit;

        if (Physics.Raycast(attackRay.position, transform.forward, out hit, attackDistance))
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                //Debug.Log("Attacking");
                animator.SetTrigger("isAttacking");

            }
        }
    }

    public void DealDamageToPlayer()
    {
        // Check if the player is within the attack range
        float distanceToPlayer = Vector3.Distance(attackRay.position, player.transform.position);
        if (distanceToPlayer <= attackDistance+0.5)
        {
            Debug.Log("Player is within attack range");
            // Send damage to the player
            //player.GetComponent<Player>().TakeDamage(Playerdamage);
            Player.instance.TakeDamage(Playerdamage);
            SoundManager.Sfx.playManPain();
        }
    }


}

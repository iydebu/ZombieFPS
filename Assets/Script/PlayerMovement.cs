using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private float sprintSpeed = 12f;
    [SerializeField] private float walkSpeed = 6f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 3f;

    [Header("Ground Settings")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    [Header("Sprint Settings")]
    [SerializeField] private float sprintDuration = 3f;
    [SerializeField] private float sprintCooldown = 5f;
    private bool isSprinting = false;
    private float sprintTimer = 0f;
    private float sprintCooldownTimer = 0f;

    [Header("Shoot Settings")]
    [SerializeField] private ShootScript shoot;

    private bool isGrounded;
    private Vector3 velocity;
    private float currentSpeed;

    private void Update()
    {
        CheckGrounded();
        ApplyGravity();
        ApplyMovement();
        ApplyJump();
        UpdateSprintTimer();
    }

    private void FixedUpdate()
    {
        CheckSprint();
    }

    private void CheckGrounded()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
        }
    }

    private void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void ApplyMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        currentSpeed = isSprinting ? sprintSpeed : walkSpeed;
        controller.Move(move * currentSpeed * Time.deltaTime);
    }

    private void ApplyJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void CheckSprint()
    {
        if (Input.GetKey(KeyCode.LeftShift) && !isSprinting && sprintCooldownTimer <= 0f)
        {
            isSprinting = true;
            sprintTimer = sprintDuration;
        }
    }

    private void UpdateSprintTimer()
    {
        if (sprintTimer > 0f)
        {
            sprintTimer -= Time.deltaTime;
            Player.instance.UseStamina(0.1f);
            if (sprintTimer <= 0f)
            {
                isSprinting = false;
                sprintCooldownTimer = sprintCooldown;
            }
        }
        else if (sprintCooldownTimer > 0f)
        {
            sprintCooldownTimer -= Time.deltaTime;
        }
    }
}
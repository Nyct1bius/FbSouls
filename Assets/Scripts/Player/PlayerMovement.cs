using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float runJumpSpeed;

    private Vector3 moveDirection;
    private Vector3 velocity;

    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float runJumpHeight;

    private CharacterController charController;

    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    private bool isRunning;

    [SerializeField] private Transform playerCam;

    [SerializeField] private Animator anim;

    [SerializeField] private GameObject meleeHitbox;

    private bool isAttacking;

    private void Start()
    {
        charController = GetComponentInParent<CharacterController>();

        anim = GetComponent<Animator>();  
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        moveDirection = new Vector3(horizontal, 0, vertical);

        if (isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isRunning)
                {
                    velocity.y = Mathf.Sqrt(runJumpHeight * -2 * gravity);
                }
                else
                {
                    velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
                }

                anim.SetTrigger("Jump");
            }
        }
        else
        {
            if (isRunning)
            {
                moveSpeed = runJumpSpeed;
            }
            else
            {
                moveSpeed = jumpSpeed;
            }
        }

        //ANDANDO
        if (!isAttacking)
        {
            if (moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
            {
                if (isGrounded)
                {
                    moveSpeed = walkSpeed;

                    anim.SetBool("Idle", false);
                    anim.SetBool("Walking", true);
                    anim.SetBool("Running", false);
                    anim.SetBool("Dead", false);
                }
                isRunning = false;
            }
            //CORRENDO
            if (moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
            {
                if (isGrounded)
                {
                    moveSpeed = runSpeed;

                    anim.SetBool("Idle", false);
                    anim.SetBool("Walking", false);
                    anim.SetBool("Running", true);
                    anim.SetBool("Dead", false);
                }
                isRunning = true;
            }
            //PARADO
            if (moveDirection == Vector3.zero)
            {
                isRunning = false;
            }

            if (moveDirection.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + playerCam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            }
            else
            {
                anim.SetBool("Idle", true);
                anim.SetBool("Walking", false);
                anim.SetBool("Running", false);
                anim.SetBool("Dead", false);
            }

            moveDirection *= moveSpeed;
            velocity.y += gravity * Time.deltaTime;

            charController.Move(velocity * Time.deltaTime);
            charController.Move(moveDirection * Time.deltaTime);
        }

        if (isGrounded && Input.GetButtonDown("Fire1"))
        {
            isAttacking = true;
            anim.SetTrigger("Melee");
        }
    }

    public void MeleeHitboxOn()
    {
        meleeHitbox.SetActive(true);
    }
    public void MeleeHitboxOff()
    {
        meleeHitbox.SetActive(false);
        isAttacking = false;
    }
}

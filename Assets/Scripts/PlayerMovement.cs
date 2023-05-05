using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpSpeed;

    private Vector3 moveDirection;
    private Vector3 velocity;

    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpHeight;

    private CharacterController charController;
    //private Rigidbody rb;

    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    [SerializeField] private Transform playerCam;

    private void Start()
    {
        charController = GetComponent<CharacterController>();
        //rb = GetComponent<Rigidbody>();
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
        //float horizontal = Input.GetAxis("Horizontal");

        moveDirection = new Vector3(0, 0, vertical);
        moveDirection = transform.TransformDirection(moveDirection);

        if (isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }
        else
        {
            moveSpeed = jumpSpeed;
        }

        //ANDANDO
        if (moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
        {
            Walk();
        }
        //CORRENDO
        if (moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
        {
            Run();
        }
        //PARADO
        if (moveDirection == Vector3.zero)
        {
            Idle();
        }

        moveDirection *= moveSpeed;

        velocity.y += gravity * Time.deltaTime;
        charController.Move(velocity * Time.deltaTime);

        if (moveDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }

        charController.Move(moveDirection * Time.deltaTime);
    }

    private void Walk()
    {
        if (isGrounded)
        {
            moveSpeed = walkSpeed;
        }
        Debug.Log("Walking");
    }
    private void Run()
    {
        if (isGrounded)
        {
            moveSpeed = runSpeed;
        }
        Debug.Log("Running");
    }
    private void Idle()
    {
        Debug.Log("Idle");
    }

    private void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        //rb.AddForce(transform.up * jumpHeight, ForceMode.Impulse);
        Debug.Log("Jumped");
    }
}

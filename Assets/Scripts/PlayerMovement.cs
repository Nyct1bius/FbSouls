using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    public float movementSpeed, jumpForce;

    private bool canJump;

    private void Start()
    {
        canJump = true;
    }

    public void Jump()
    {
        if (canJump)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            canJump = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!canJump)
        {
            if (collision.collider.CompareTag("Ground"))
            {
                canJump = true;
            }
        }
    }
}

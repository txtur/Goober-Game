using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovement : MonoBehaviour
{
    Rigidbody rb;
    public float moveSpeed, jumpForce;

    private Vector2 moveInput;

    public LayerMask ground;
    public float playerHeight;
    public bool isGrounded;

    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;

    void Update()
    {
        rb = GetComponent<Rigidbody>();

        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize();

        rb.velocity = new Vector3(moveInput.x * moveSpeed, rb.velocity.y, moveInput.y * moveSpeed);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, playerHeight * 0.5f + 0.2f, ground))
        {
            isGrounded = true;
        } else {
            isGrounded = false;
        }

        if(Input.GetKeyDown(jumpKey) && isGrounded)
        {
            rb.velocity += new Vector3(0f, jumpForce, 0f);
        }
    }

    void FixedUpdate()
    {
        
    }
}

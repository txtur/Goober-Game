using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerController : MonoBehaviour
{
    private PlatformerAnimation anim;
    public Rigidbody rb;

    [Header("Mouvement")] 
    public Vector2 moveInput;
    [SerializeField] float runMaxSpeed = 9;
    [SerializeField] float accel = 11;
    [Header("Jump")]
    [SerializeField] float jumpForce = 8;
    [SerializeField] float coyoteTime = 0.1f;
    float LastPressedJumpTime;
    float LastOnGroundTime;
    bool isJumping;

    [Header("Ground Check")]
    [SerializeField] float playerHeight;
    [SerializeField] LayerMask ground;

    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<PlatformerAnimation>();
    }

    void Update()
    {
        #region Input
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(jumpKey))
            OnJumpInput();
        #endregion

        //ground check
        if (Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, ground))
        {
            LastOnGroundTime = coyoteTime;
            
        }

        //clear jumping state if not ascending
        if (isJumping && rb.velocity.y < 0)
            isJumping = false;
        //timers
        LastPressedJumpTime -= Time.deltaTime;
        LastOnGroundTime -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        Move();

        //jump
        if (CanJump() && LastPressedJumpTime > 0)
        {
            isJumping = true;
            Jump();

            anim.jump = true;
        }

        if(rb.velocity.y < -0.2f)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * 1.5f * Time.deltaTime;
        } else if (rb.velocity.y > -0.2f && !Input.GetKey(jumpKey))
        {
            rb.velocity += Vector3.up * Physics.gravity.y * 3 * Time.deltaTime;
        }
    }

    void Move()
    {
        float targetSpeedX = moveInput.x * runMaxSpeed;
        targetSpeedX = Mathf.Lerp(rb.velocity.x, targetSpeedX, 1f);
        float targetSpeedZ = moveInput.y * runMaxSpeed;
        targetSpeedZ = Mathf.Lerp(rb.velocity.z, targetSpeedZ, 1f);

        float accelRateX;
        float accelRateZ;
        //get acceleration value based on if you're accelerating or decelerating
        //change multiplier if airborne
        if(LastOnGroundTime > 0)
        {
            accelRateX = (Mathf.Abs(targetSpeedX) > 0.1f) ? accel : accel * 2;
            accelRateZ = (Mathf.Abs(targetSpeedZ) > 0.1f) ? accel : accel * 2;
        }
        else 
        {
            accelRateX = (Mathf.Abs(targetSpeedX) > 0.1f) ? accel * 0.65f : accel * 1.3f;
            accelRateZ = (Mathf.Abs(targetSpeedZ) > 0.1f) ? accel * 0.65f : accel * 1.3f;
        }
        //increase acceleration at the peak of the jump, makes it more bouncy and responsive
        if(isJumping && Mathf.Abs(rb.velocity.y) < 1)
        {
            accelRateX *= 1.1f;
            targetSpeedX *= 1.3f;
            accelRateZ *= 1.1f;
            targetSpeedZ *= 1.3f;
        }

        //allow the player to move at a higher speed than the max speed so long as its in the same direction
        if (Mathf.Abs(rb.velocity.x) > Mathf.Abs(targetSpeedX) && Mathf.Sign(rb.velocity.x) == Mathf.Sign(targetSpeedX) && Mathf.Abs(targetSpeedX) > 0.01f && LastOnGroundTime < 0)
            accelRateX = 0;
        if (Mathf.Abs(rb.velocity.z) > Mathf.Abs(targetSpeedZ) && Mathf.Sign(rb.velocity.z) == Mathf.Sign(targetSpeedZ) && Mathf.Abs(targetSpeedZ) > 0.01f && LastOnGroundTime < 0)
            accelRateZ = 0;

        //Calculate difference between current velocity and desired velocity
        float speedDifX = targetSpeedX - rb.velocity.x;
        float movementX = speedDifX * accelRateX;
        float speedDifZ = targetSpeedZ - rb.velocity.z;
        float movementZ = speedDifZ * accelRateZ;

        //Apply speed
        rb.AddForce(new Vector3(movementX, 0f, movementZ), ForceMode.Force);
    }

    void Jump()
    {
        //reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
    public void OnJumpInput()
	{
		LastPressedJumpTime = 0.1f;
	}
    public bool CanJump()
    {
		return LastOnGroundTime > 0 && !isJumping && this.gameObject.CompareTag("Player");
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float airSpeed = 3f;
    public float jumpImpulse = 10f;

    Vector2 moveInput;
    TouchingDirections touchingDirections;

    private bool spacePressed = false;

    [SerializeField]
    private bool _isMoving = false;
    public bool IsMoving { 
        get {
            return _isMoving;
        } 
        private set{
            _isMoving = value;
            animator.SetBool(AnimationStrings.moving, value);  
        } 
    }

    [SerializeField]
    private bool _isRunning = false;
    public bool IsRunning {
        get {
            return _isRunning;
        }
        private set {
            _isRunning = value;
            animator.SetBool(AnimationStrings.running, value);
        }
    }

    private bool _isFacingRight = true;
    public bool IsFacingRight {
        get {
            return _isFacingRight;
        }
        private set {
            _isFacingRight = value;
            transform.localScale = new Vector2(_isFacingRight? 1 : -1, 1);
        }
    }

    Rigidbody2D rb;
    Animator animator;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
    }

    void FixedUpdate()
    {
        if(!touchingDirections.IsOnWall) {
            if(touchingDirections.IsGrounded)
                rb.velocity = new Vector2(moveInput.x * (IsRunning ? runSpeed : walkSpeed), rb.velocity.y);
            else
                rb.velocity = new Vector2(moveInput.x * airSpeed, rb.velocity.y);
            animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
        }
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();

        IsMoving = moveInput != Vector2.zero;
        if(moveInput.x != 0)
            IsFacingRight = moveInput.x > 0;
    }

    public void OnRun(InputAction.CallbackContext ctx)
    {
        if(ctx.started) {
            IsRunning = true;
        } 
        else if(ctx.canceled) {
            IsRunning = false;
        }
    }

    public void OnJump(InputAction.CallbackContext ctx) {
        // TODO: Check if alive aswell
        if(ctx.started && touchingDirections.IsGrounded) {
            spacePressed = true;
            rb.velocity = new Vector2(moveInput.x, jumpImpulse);
        } else if(ctx.canceled && spacePressed) {
            spacePressed = false;
        }
        animator.SetBool(AnimationStrings.preaparingJump, spacePressed);
    }
}

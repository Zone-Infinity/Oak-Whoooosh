using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 8f;

    Vector2 moveInput;

    private bool _isMoving = false;
    public bool IsMoving { 
        get {
            return _isMoving;
        } 
        private set{
            _isMoving = value;
            animator.SetBool(AnimationString.moving, value);
            
        } 
    }

    private bool _isRunning = false;
    public bool IsRunning {
        get {
            return _isRunning;
        }
        private set {
            _isRunning = value;
            animator.SetBool(AnimationString.running, value);
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
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput.x * (IsRunning ? runSpeed : walkSpeed), rb.velocity.y);
    }

    public void onMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();

        IsMoving = moveInput != Vector2.zero;
        IsFacingRight = moveInput.x > 0;
    }

    public void onRun(InputAction.CallbackContext ctx)
    {
        if(ctx.started) {
            IsRunning = true;
        } 
        else if(ctx.canceled) {
            IsRunning = false;
        }
    }
}

internal class AnimationString
{
    internal static readonly string moving = "isMoving";
    internal static readonly string running = "isRunning";
}
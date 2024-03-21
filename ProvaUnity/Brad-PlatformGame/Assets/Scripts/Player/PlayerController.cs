using System;
using Unity.XR.Oculus.Input;
using UnityEngine;

public class PlayerController : Damageable
{
    [Header("Movement properties")]
    [SerializeField] private float velocity;
    [SerializeField] private float jumpForce = 5;
    [Header("Attack properties")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask attackLayer;

    private Rigidbody2D rigidbody;
    private SpriteRenderer spriteRenderer;

    private float moveDirection;
    private bool canJump = false;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        SetupInputListeners();
    }

    private void FixedUpdate()
    {
        Move();
        FlipSpriteAccordingToMoveDirection();
    }

    private void Move()
    {
        moveDirection =
            GameManager.Instance.inputManager.MoveDirection;

        rigidbody.velocity = 
            new Vector2(moveDirection * velocity, 
                          rigidbody.velocity.y);
    }

    private void FlipSpriteAccordingToMoveDirection()
    {
        if (moveDirection > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (moveDirection < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void HandleAttack()
    {
        if (canJump == false) return;
        Collider2D[] hittedObjects =
            Physics2D.OverlapCircleAll(attackPoint.position, attackRange, attackLayer);

        foreach (Collider2D collider in hittedObjects)
        {
            if (collider.TryGetComponent(out Damageable hit))
            {
                hit.TakeDamage();
            }
        }
    }

    public override void TakeDamage()
    {
        lives--;
        CheckAndHandleLife();
    }

    private void CheckAndHandleLife()
    {
        GameManager.Instance.InvokeOnPlayerGetHurtEvent();
        Animator animator = GetComponent<Animator>();
        if (lives <= 0)
        {
            animator.SetTrigger("dead"); 
            GameManager.Instance.inputManager.DisableInput();
            GameManager.Instance.InvokeOnPlayerDieEvent();
            rigidbody.bodyType = RigidbodyType2D.Static;
            GetComponent<BoxCollider2D>().enabled = false;
        }
        else
        {
            animator.SetTrigger("hurt");
        }
    }

    private void HandleJump()
    {
        if (canJump)
        {
            rigidbody.velocity += Vector2.up * jumpForce;
            canJump = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Floor"))
        {
            canJump = true;
        }
    }

    private void SetupInputListeners()
    {
        GameManager.Instance.inputManager.OnJump += HandleJump;
    }
    
    public bool GetCanJump()
    {
        return canJump;
    }

    private void OnDisable()
    {
        GameManager.Instance.inputManager.OnJump -= HandleJump;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
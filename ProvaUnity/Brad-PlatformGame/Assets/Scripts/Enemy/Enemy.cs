using System;
using System.Collections;
using UnityEngine;

public class Enemy : Damageable
{
    [SerializeField] private float velocity = 5;
    [SerializeField] private int attackDamage;
    [SerializeField] private int waitingTime;
    [SerializeField] private Transform pointLeft;
    [SerializeField] private Transform pointRight;

    private Rigidbody2D rigidbody;
    private Transform currentPointToMove;
    private Animator animator;

    private bool canMove = true;
    private float elapsedTime = 0;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        currentPointToMove = pointRight;
    }

    private void FixedUpdate()
    {
        if (canMove == false)
        {
            WaitToMoveAgain();
            return;
        }
        HandleMovementDirectionAndMove();
        HandleReachedPointAndChangeTargetPoint();
    }

    private void WaitToMoveAgain()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= waitingTime)
        {
            elapsedTime = 0;
            canMove = true;
        }
    }

    private void HandleMovementDirectionAndMove()
    {
        if (canMove == false) return;
        animator.SetBool("isMoving", true);
        if (currentPointToMove == pointRight)
        {
            rigidbody.velocity = Vector2.right * velocity;
        }
        else
        {
            rigidbody.velocity = Vector2.left * velocity;
        }
    }

    private void HandleReachedPointAndChangeTargetPoint()
    {
        if (Vector2.Distance(transform.position, pointRight.position) < 1f && 
            currentPointToMove == pointRight)
        {
            FlipCharacter();
            StopCharacter();
            currentPointToMove = pointLeft;
        }
        else if (Vector2.Distance(transform.position, pointLeft.position) < 1f && 
                 currentPointToMove == pointLeft)
        {
            FlipCharacter();
            StopCharacter();
            currentPointToMove = pointRight;
        }
    }

    private void FlipCharacter()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void StopCharacter()
    {
        print("Stopping enemy");
                               //new Vector3(0,0,0)
        rigidbody.velocity = Vector3.zero;
        canMove = false;
        animator.SetBool("isMoving", false);
    }
    
    public override void TakeDamage()
    {
        lives--;
        CheckAndHandleHealth();
    }

    private void CheckAndHandleHealth()
    {
        if (lives <= 0)
        {
            GameManager.Instance.InvokeOnEnemyDieEvent();
            StopCharacter();
            animator.SetTrigger("die");
            StartCoroutine(DestroyEnemy(1));
        }
        else
        {
            animator.SetTrigger("hurt");
        }
    }

    private IEnumerator DestroyEnemy(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.TryGetComponent(out Damageable hit))
        {
            hit.TakeDamage();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(pointLeft.position, 0.3f);
        Gizmos.DrawWireSphere(pointRight.position, 0.3f);
        
        Gizmos.DrawLine(pointRight.position, pointLeft.position);
    }
}

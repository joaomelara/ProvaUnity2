using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator animator;
    private PlayerController playerController;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
    }

    private void Start()
    {
        GameManager.Instance.inputManager.OnAttack += HandleAttackAnim;
    }

    private void Update()
    {
        UpdateAnimParameters();
    }

    private void UpdateAnimParameters()
    {
        animator.SetBool("isMoving", CheckIsMoving());
        if (playerController.GetCanJump())
        {
            animator.SetBool("isJumping", false);
        }
        else
        {
            animator.SetBool("isJumping", true);
        }
    }

    private bool CheckIsMoving()
    {
        //Ao invés de fazermos isso
        // if (GameManager.Instance.inputManager.MoveDirection != 0)
        // {
        //     return true;
        // }
        // else
        // {
        //     return false;
        // }

        //Vamos fazer isso
        return GameManager.Instance.inputManager.MoveDirection != 0;
    }

    private void HandleAttackAnim()
    {
        if (playerController.GetCanJump() == false) return;
        animator.SetTrigger("attack");
    }

    public Animator GetAnimator()
    {
        return animator;
    }

    private void OnDestroy()
    {
        GameManager.Instance.inputManager.OnAttack -= HandleAttackAnim;
    }
}
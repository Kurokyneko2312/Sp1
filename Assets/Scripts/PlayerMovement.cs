using System;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputActionReference move;
    [SerializeField] private InputActionReference jump;
    private float moveDirection;

    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float jumpForce = 200f;
    [SerializeField] private int maxJumps = 2;
    [SerializeField] private Transform leftFoot, rightFoot;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float raycastDistance = 0.25f;
    [SerializeField] private AudioClip[] jumpSounds;
    [SerializeField] private ParticleSystem jumpParticleSystem;
    [SerializeField] private InputActionReference dash;
    [SerializeField] private float dashForce = 15f;
    [SerializeField] private float dashDuration = 0.15f;

    private int jumpsRemaining;
    bool canMove = true;

    private bool isDashing;
    private bool canDash = true;
    private float normalGravity;

    private AudioSource audioSource;
    private Rigidbody2D rgbd;
    private SpriteRenderer rend;
    private Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rgbd = GetComponent<Rigidbody2D>();
        normalGravity = rgbd.gravityScale;
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        jumpsRemaining = maxJumps;


        jump.action.started += Jump;
        dash.action.started += Dash;
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = move.action.ReadValue<float>();

        anim.SetFloat("MoveSpeed", Mathf.Abs(rgbd.linearVelocity.x));
        anim.SetFloat("VerticalSpeed", rgbd.linearVelocity.y);
        anim.SetBool("IsGrounded", CheckIsGrounded());

        if (CheckIsGrounded())
        {
            canDash = true;
        }

        if (moveDirection < 0f)
        {
            FlipSprite(true);
        }

        if (moveDirection > 0f)
        {
            FlipSprite(false);
        }
    }

    private void FixedUpdate()
    {
        if(!canMove)
        {
            return;
        }

        if (isDashing)
        {
            return;
        }

        rgbd.linearVelocity = new Vector2(moveDirection * moveSpeed * Time.deltaTime, rgbd.linearVelocity.y);
    }

    private void OnDisable()
    {
        jump.action.started -= Jump;
        dash.action.started -= Dash;

    }

    private void FlipSprite(bool direction)
    {
        rend.flipX = direction;
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (CheckIsGrounded())
        {
            jumpsRemaining = maxJumps;
        }

        if (jumpsRemaining <= 0)
        {
            return;
        }

        rgbd.linearVelocity = new Vector2(rgbd.linearVelocity.x, 0f);
        rgbd.AddForce(Vector2.up * jumpForce);

        jumpsRemaining--;

        if (jumpsRemaining == maxJumps - 1)
        {
            jumpParticleSystem.Play();
        }

        int randomJumpSound = UnityEngine.Random.Range(0, jumpSounds.Length);
        audioSource.PlayOneShot(jumpSounds[randomJumpSound]);
    }
    private bool CheckIsGrounded()
    {
        RaycastHit2D leftHit = Physics2D.Raycast(leftFoot.position, Vector2.down, raycastDistance, whatIsGround);
        RaycastHit2D rightHit = Physics2D.Raycast(rightFoot.position, Vector2.down, raycastDistance, whatIsGround);

        if (leftHit.collider != null && leftHit || rightHit.collider != null && rightHit)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void TakeKnockback(float knockbackForce, float upwardsForce)
    {
        canMove = false;
        rgbd.AddForce(new Vector2(knockbackForce, upwardsForce));
        Invoke(nameof(CanMoveAgain), 0.25f);
    }

    private void CanMoveAgain()
    {
        canMove = true;
    }

    private void Dash(InputAction.CallbackContext context)
    {
        if (!canDash || isDashing)
        {
            return;
        }

        StartCoroutine(DashCoroutine());
    }

    private IEnumerator DashCoroutine()
    {
        isDashing = true;
        canDash = false;

        float dashDirection = moveDirection;

        if (dashDirection == 0)
        {
            dashDirection = rend.flipX ? -1f : 1f;
        }

        rgbd.gravityScale = 0f;

        rgbd.linearVelocity = new Vector2(
            dashDirection * dashForce,
            0f
        );

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;

        rgbd.gravityScale = normalGravity;
    }

}

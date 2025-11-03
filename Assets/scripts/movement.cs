using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class movement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float tapForce = 5f;
    [SerializeField] private float forwardSpeed = 3f;
    [SerializeField] private float upwardAngle = 45f;
    [SerializeField] private float gravityScale = 1f;
    [SerializeField] private float maxVerticalSpeed = 5f;
    [SerializeField] private float holdingMultiplier = 2f;
    [SerializeField] private float collisionForce = 5f;

    [Header("Player Animations")]
    [SerializeField] private RuntimeAnimatorController[] otherPlayerAnims;
    [SerializeField] private Animator animator;

    [Header("Starting Settings")]
    [SerializeField] private float startingAnimationDuration = 1f;
    [SerializeField] private float startingFlingDuration = 1f;
    [SerializeField] private float flingSpeedMulitplier = 2.75f;
    [SerializeField] private float flingUpwardForce = 8f;
    [SerializeField] PipeAnimationController pipeScript;

    [Header("Jump Effect")]
    [SerializeField] private ParticleSystem jumpTrail; // Particle System Reference
    [SerializeField] private TrailRenderer speedBoostTrail; // Assign Trail Renderer in Inspector

    private Rigidbody2D rb;
    private bool isHolding = false;
    public bool hasTapped = false;
    private Vector2 forwardDirection;
    private float angleInRadians;
    private float currentSpeed;
    private Vector3 normalSize;

    void Awake()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;

        angleInRadians = upwardAngle * Mathf.Deg2Rad;
        forwardDirection = new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians));
        currentSpeed = forwardSpeed;
        normalSize = transform.localScale;

        if (!animator) animator = GetComponent<Animator>();
        if (!pipeScript) Debug.LogError("PipeAnimationController reference is missing!");

        if (jumpTrail != null) jumpTrail.Stop();
        if (speedBoostTrail != null) speedBoostTrail.emitting = false; // Make sure it's off initially

        if (otherPlayerAnims.Length <= 0)
        {
            Debug.LogError("otherPlayerAnims are not defined well");
            return;
        }

        for (int i = 0; i < otherPlayerAnims.Length; i++)
        {
            if (otherPlayerAnims[i].name.ToLower() == PlayerPrefs.GetString("character"))
                animator.runtimeAnimatorController = otherPlayerAnims[i];
        }

        if (!animator.runtimeAnimatorController)
        {
            Debug.Log("No animator controller set, check otherPlayerAnims and PlayerPrefs");
            return;
        }

    }

    void Update()
    {
        HandleInput();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!hasTapped)
            {
                Invoke(nameof(HandleFirstTap), startingAnimationDuration);
                pipeScript.TriggerPipeAnimation();
                return;
            }
            ApplyForce(tapForce);
        }
        isHolding = Input.GetMouseButton(0);
    }

    private void HandleFirstTap()
    {
        rb.linearVelocity = new Vector2(forwardSpeed * flingSpeedMulitplier, 0);
        rb.gravityScale = gravityScale;
        ApplyForce(flingUpwardForce);
        Invoke(nameof(ReturnControl), startingFlingDuration);
    }
    private void ReturnControl()
    {
        hasTapped = true;
        rb.gravityScale = gravityScale;
        animator.SetBool("hasjumped", false);
    }

    private void HandleMovement()
    {
        if (!hasTapped) return;

        float targetYVelocity = isHolding
            ? Mathf.Lerp(rb.linearVelocity.y, forwardDirection.y * currentSpeed * holdingMultiplier, 0.1f)
            : Mathf.Clamp(rb.linearVelocity.y, -maxVerticalSpeed, maxVerticalSpeed);

        rb.linearVelocity = new Vector2(currentSpeed, targetYVelocity);
        animator.SetBool("isjumping", isHolding);

        if (isHolding || rb.linearVelocity.y > 0.1f)
            if (!jumpTrail.isPlaying) jumpTrail.Play();
        else
            if (jumpTrail.isPlaying) jumpTrail.Stop();
        
    }

    private void ApplyForce(float force)
    {
        animator.SetBool("hasjumped", true);
        animator.SetBool("isjumping", true);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        rb.AddForce(forwardDirection * force, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Flowerbloom"))
        {
            currentSpeed = forwardSpeed * 2f;
        }
        else if (collision.gameObject.CompareTag("flowerclose"))
        {
            currentSpeed = forwardSpeed;
        }
        else if (collision.gameObject.CompareTag("flowerbloomsize"))
        {
            transform.localScale = normalSize * 1.5f;
        }
        else if (collision.gameObject.CompareTag("flowerclosesize"))
        {
            transform.localScale = normalSize;
        }
        else if (collision.gameObject.CompareTag("flowerbloomsize2"))
        {
            transform.localScale = normalSize * 0.5f;
        }
        else if (collision.gameObject.CompareTag("flowerclosesize2"))
        {
            transform.localScale = normalSize;
        }
        else if (collision.gameObject.CompareTag("SpeedBoost"))
        {
            StartCoroutine(BoostSpeed());
        }
    }

    private IEnumerator BoostSpeed()
    {
        currentSpeed = forwardSpeed * 1.8f;

        if(speedBoostTrail)
            speedBoostTrail.emitting = true; // Enable Trail Renderer

        yield return new WaitForSeconds(3f);

        currentSpeed = forwardSpeed;

        if (speedBoostTrail)
            speedBoostTrail.enabled = false; // Disable Trail Renderer
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("abovegr"))
            rb.AddForce(Vector2.down * collisionForce, ForceMode2D.Impulse);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Fields
    // Stats
    [SerializeField] private float playerSpeed = 15f, jumpSpeed = 15f, dashSpeed = 40f, playerGravity = 8f;
    [SerializeField] private float swingDamage = 1f, swingKnockback = 100f;
    private int maxHealth = 6, health;

    // referances
    //[SerializeField] ;
    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;
    private TrailRenderer trailRenderer;
    private Animator animator;
    [SerializeField] private ParticleSystem feetParticles;
    [SerializeField] private PlayerSwingChecker swingChecker;
    [SerializeField] private UIController UIControl;
    [SerializeField] private GameObject chakramPrefab;



    [SerializeField] private float chakramOffset;
    private Vector2 moveDir, dashDir;
    [SerializeField] private int jumpTime = 10, dashTime = 8, dashCooldownBase = 12, swingCooldown = 15, stunTime = 4, invinTime = 12;
    private int jumpTimer = 0, dashTimer = 0, swingTimer = 0, stunTimer = 0, invinTimer = 0;
    private int dashCooldown = 0;
    private bool grounded = true;

    private bool dashAvailable = false, chakramAvailable = false, doubleJumpAvailable = false;
    private bool dashUnlocked = false, chakramUnlocked = false, doubleJumpUnlocked = false;

    #endregion

    #region Unity Methods
    private void Awake()
    {
        // Get referances
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        trailRenderer = gameObject.GetComponent<TrailRenderer>();
        animator = gameObject.GetComponent<Animator>();
    }

    void Start()
    {
        // Set UI
        health = maxHealth;
        UIControl.SetHealthUI(health);
    }

    // Update is called once per frame
    void Update()
    {
        if (dashTimer <= 0 && stunTimer <= 0) // if not dashing nor stunned
        {
            // Check for Horizontal input
            float hInput = Input.GetAxisRaw("Horizontal");
            animator.SetFloat("HInput", Mathf.Abs(hInput)); // Set animator HInput
            moveDir = new Vector2(hInput * playerSpeed, rigidBody.velocity.y);
            if (hInput > 0) spriteRenderer.flipX = false; // flip player sprite
            else if (hInput < 0) spriteRenderer.flipX = true;

            // Scan for Ability input
            if (Input.GetButtonDown("BasicAttack")) BasicAttack(Input.GetAxisRaw("Vertical"));
            if (Input.GetButtonDown("Jump")) Jump();
            if (Input.GetButtonUp("Jump")) jumpTimer = 0; // Stop jump if jump key is released
            if (Input.GetButtonDown("Dash")) Dash();
            if (Input.GetButtonDown("Chakram")) Chakram();
        }
    }
    private void FixedUpdate()
    {
        bool dashing = dashTimer != 0;
        bool stunned = stunTimer != 0;

        if (!dashing && !stunned) // Not dashing nor stunned
        {
            rigidBody.velocity = moveDir;

            if (jumpTimer > 0)
            {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpSpeed);
                --jumpTimer;
            }

            if (dashCooldown > 0) --dashCooldown;

        } else // Abnormal behaviour - jumpTimer will be set to 0 by causes of below cases or handled within
        {
            if (dashing)
            {
                rigidBody.velocity = dashDir;
                if (--dashTimer <= 0) EndDash();
            }

            if (stunned)
            {
                if (--stunTimer <= 0) EndStun();
            }
        }


        if (invinTimer > 0) --invinTimer;
        if (swingTimer > 0) --swingTimer;
    }
    #endregion

    #region Public Methods
    public void HitGround()
    {
        grounded = true;
        dashAvailable = dashUnlocked;
        doubleJumpAvailable = doubleJumpUnlocked;
        animator.SetBool("Grounded", true); // Set animator grounded
    }

    public void LeaveGround()
    {
        grounded = false;
        animator.SetBool("Grounded", false); // Set animator grounded
    }

    // enemy - the enemy being hit
    // dir - the direction in which the enemy is hit
    public void SwingConnect(EnemyLifeform enemy, int dir)
    {
        Vector2 KBDir;

        // Direction specific actions
        switch (dir)
        {
            case 0: // Right
                KBDir = Vector2.right;
                break;
            case 1: // Up
                KBDir = Vector2.up;
                break;
            case 2: // Left
                KBDir = Vector2.left;
                break;
            case 3: // Down
                KBDir = Vector2.down;
                pogo();
                break;
            default:
                KBDir = Vector2.zero;
                Debug.LogWarning("Improper usage of ApplyHits");
                break;
        }

        if (enemy != null) // if collision was an enemy, apply damage
        {
            // Apply hit to enemy
            enemy.HitEnemy(swingDamage, KBDir, swingKnockback);
        }
    }

    public void HitPlayer(int damage, Vector2 KBDir, float knockbackForce)
    {
        if (invinTimer <= 0)
        {
            // Set Fields
            health -= damage;
            stunTimer = stunTime;
            invinTimer = invinTime;

            // Set UI
            UIControl.SetHealthUI(health);

            // Apply Knockback and stun
            Stun();
            rigidBody.velocity = KBDir * knockbackForce;

            // Check mortality
            if (health <= 0) Die();
        }
        
    }

    public void HealthPickup(int healthGain)
    {
        health = Mathf.Min(health + healthGain, maxHealth);
        UIControl.SetHealthUI(health);
    }
    public void DamagePickup(float damageGain)
    {
        swingDamage += damageGain;
    }
    public void UnlockDash()
    {
        dashUnlocked = dashAvailable = true;
    }
    public void UnlockChakram()
    {
        chakramUnlocked = chakramAvailable = true;
    }
    /// <summary>
    /// Called when chakram collides with player,
    /// </summary>
    /// <returns> Returns true if player is grounded at collision </returns>
    public bool ChakramCollide()
    {
        if (!grounded)
        {
            // bounce
            pogo();
            return false;
        }
        return true;
    }
    public void ChakramDestroyed()
    {
        chakramAvailable = true;
    }

    public void UnlockDoubleJump()
    {
        doubleJumpUnlocked = doubleJumpAvailable = true;
    }
    #endregion

    #region Private Methods
    private void BasicAttack(float vertical)
    {
        if (swingTimer > 0) return; // Don't attack if on cooldown
        if (vertical > 0) // Swing Up
        {
            // Play player animation and set up swingChecker
            animator.Play("PlayerSwingUp");
            swingChecker.SwingUp();

        } else if (vertical < 0) // Swing Down
        {
            // Play player animation and set up swingChecker
            animator.Play("PlayerSwingDown");
            swingChecker.SwingDown();

        } else if (spriteRenderer.flipX == false) // Swing Right
        {
            // Play player animation and set up swingChecker
            animator.Play("PlayerSwingH");
            swingChecker.SwingRight();

        } else // Swing Left
        {
            // Play player animation and set up swingChecker
            animator.Play("PlayerSwingH");
            swingChecker.SwingLeft();

        }
        swingTimer = swingCooldown;
    }

    private void pogo()
    {
        jumpTimer = Mathf.Max(jumpTime / 2, jumpTimer);
        dashAvailable = dashUnlocked;
        doubleJumpAvailable = doubleJumpUnlocked;
    }

    // Jumps if either grounded or doubleJumpAvailable is true
    // Fails if both grounded and doubleJumpAvailable are false
    private void Jump()
    {
        if (grounded) // grounded jump
        {
            jumpTimer = jumpTime;
        } else if (doubleJumpAvailable) // double jump
        {
            jumpTimer = jumpTime;
            doubleJumpAvailable = false;
            feetParticles.Play();
        }
    }

    // Dashes if dash is available and not on cooldown
    private void Dash()
    {
        if (dashAvailable && dashCooldown <= 0)
        {
            // Get dashDir
            float temp = 1f;
            if (spriteRenderer.flipX) temp = -1f;
            dashDir = new Vector2(temp, 0) * dashSpeed;

            // Set up dash for FixedUpdate
            dashTimer = dashTime;
            dashAvailable = grounded;
            rigidBody.gravityScale = 0;
            jumpTimer = 0;

            // Set and enable trail
            trailRenderer.emitting = true;
        }
    }

    private void EndDash()
    {
        dashTimer = 0;
        dashCooldown = dashCooldownBase;
        rigidBody.gravityScale = playerGravity;
        trailRenderer.emitting = false;
    }
    private void Chakram()
    {
        if (chakramAvailable) 
        {
            // dir = direction chakram is being cast
            Vector2 dir = Vector2.left;
            if (spriteRenderer.flipX == false) dir = Vector2.right;

            Vector3 chakramPosition = (Vector2) transform.position + dir * chakramOffset;

            ChakramController chakram = Instantiate(chakramPrefab, chakramPosition, Quaternion.identity).GetComponent<ChakramController>();
            chakram.InitializeChakram(dir, this);

            chakramAvailable = false;
        }
    }
    private void Stun()
    {
        stunTimer = stunTime;
    }

    private void EndStun()
    {

    }

    private void Die()
    {
        // stun and respawn?
        // reload level?
        // boot to menu?
    }
    #endregion

}

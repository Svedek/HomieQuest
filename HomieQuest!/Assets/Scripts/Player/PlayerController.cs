using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    #region Fields ================================================================================
    // Stats
    [SerializeField] private float playerSpeed = 15f, jumpSpeed = 15f, dashSpeed = 40f, playerGravity = 8f;
    [SerializeField] private float swingDamage = 1f, swingKnockback = 100f;
    private int maxHealth = 6, health, lives = 3;

    // referances
    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;
    private TrailRenderer trailRenderer;
    private Animator animator;
    [SerializeField] private ParticleSystem feetParticles;
    [SerializeField] private PlayerSwingChecker swingChecker;
    [SerializeField] private UIController UIControl;
    [SerializeField] private GameObject chakramPrefab;


    private PlayerState state = PlayerState.Main;
    [SerializeField] private float chakramOffset;
    private Vector2 moveDir, dashDir;
    private Vector3 lastCheckpoint;
    [SerializeField] private int dashTime = 8, chakramTime = 20, dashCooldownBase = 12;
    private int jumpTime = 11, swingCooldown = 15, stunTime = 4, invinTime = 12, respawnTime = 120;
    private int jumpTimer = 0, dashTimer = 0, chakramTimer = 0, swingTimer = 0, stunTimer = 0, invinTimer = 0, respawnTimer = 0;
    private int dashCooldown = 0;
    private bool grounded = true;

    private bool dashAvailable = false, chakramAvailable = false, doubleJumpAvailable = false;
    private bool dashUnlocked = false, chakramUnlocked = false, doubleJumpUnlocked = false;

    #endregion

    #region Unity Methods ================================================================================
    private void Awake() {
        // Get referances
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        trailRenderer = gameObject.GetComponent<TrailRenderer>();
        animator = gameObject.GetComponent<Animator>();

        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }
    void Start() {

        // Set initial checkpoint
        lastCheckpoint = transform.position;

        // Load data
        DataManager.Data data = DataManager.Instance.LoadData();
        if (data != null) { // if there is data, load it
            maxHealth = data.maxHp;
            lives = data.lives;
            switch (data.powerUps) { // powerUps is 0 through 3
                case 3:
                    UnlockDoubleJump();
                    goto case 2;
                case 2:
                    UnlockDash();
                    goto case 1;
                case 1:
                    UnlockChakram();
                    goto default;
                default:
                    break;
            }
        }

        // Set health and UI
        health = maxHealth;
        UIControl.SetHelthUI(health);
        UIControl.SetHeartsUI(maxHealth/2);
        UIControl.SetLivesUI(lives);
    }

    private void OnDestroy() {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    // Update is called once per frame
    void Update() {
        if (state == PlayerState.Main) // if not dashing nor stunned
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
    private void FixedUpdate() {
        switch (state) {

            case (PlayerState.Main):
                rigidBody.velocity = moveDir;

                if (jumpTimer > 0) {
                    rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpSpeed);
                    --jumpTimer;
                }

                if (dashCooldown > 0) --dashCooldown;
                break;

            case (PlayerState.Dash):
                rigidBody.velocity = dashDir;
                if (--dashTimer <= 0) EndDash();
                break;

            case (PlayerState.Stun):
                if (--stunTimer <= 0) EndStun();
                break;

            case (PlayerState.Dead):
                if (--respawnTimer <= 0) DeathRespawn();
                break;

            default:
                Debug.LogError("PlayerController > FixedUpdate > Unhandled Player State");
                break;
        }

        if (invinTimer > 0) --invinTimer;
        if (swingTimer > 0) --swingTimer;
        if (chakramTimer > 0) if(--chakramTimer == 0) AudioManager.Instance.PlaySFX("ChakramRecharged"); ;
    }
    #endregion

    #region Public Methods ================================================================================
        #region pickup/unlock  ========================================
    public void HealthPickup(int healthGain) {
        health = Mathf.Min(health + healthGain, maxHealth);
        UIControl.SetHelthUI(health);
        AudioManager.Instance.PlaySFX("HealthPickup");
    }
    public void HealthMaxPickup() {
        maxHealth = maxHealth < 21 ? maxHealth + 2 : maxHealth;
        UIControl.SetHeartsUI(maxHealth/2);
        UIControl.SetHelthUI(health = maxHealth);
        AudioManager.Instance.PlaySFX("MaxHealthPickup");
    }
    public void LifePickup() {
        UIControl.SetLivesUI(++lives);
        AudioManager.Instance.PlaySFX("LifePickup");
    }
    public void DamagePickup(float damageGain) {
        swingDamage += damageGain;
        AudioManager.Instance.PlaySFX("PowerUp");
    }
    public void UnlockDash() {
        dashUnlocked = dashAvailable = true;
    }
    public void UnlockChakram() {
        chakramUnlocked = chakramAvailable = true;
    }
    public void UnlockDoubleJump() {
        doubleJumpUnlocked = doubleJumpAvailable = true;
    }
    #endregion
    public void HitGround() {
        grounded = true;
        dashAvailable = dashUnlocked;
        doubleJumpAvailable = doubleJumpUnlocked;
        animator.SetBool("Grounded", true); // Set animator grounded
        AudioManager.Instance.PlaySFX("PlayerHitGround");
    }

    public void LeaveGround() {
        grounded = false;
        animator.SetBool("Grounded", false); // Set animator grounded
    }

    // enemy - the enemy being hit
    // dir - the direction in which the enemy is hit
    public void SwingConnect(EnemyLifeform enemy, int dir) {
        Vector2 KBDir;

        // Direction specific actions
        switch (dir) {
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

        if (enemy != null) { // if collision was an enemy, apply damage
            // Apply hit to enemy
            enemy.HitEnemy(swingDamage, KBDir, swingKnockback);
            AudioManager.Instance.PlaySFX("SwingHitEnemy");
        } else AudioManager.Instance.PlaySFX("SwingHitSpikes");
    }

    public void HitPlayer(int damage, Vector2 KBDir, float knockbackForce) {
        if (state >= PlayerState.Dead) return;
        if (invinTimer <= 0) {
            // Set Fields
            health -= damage;
            stunTimer = stunTime;
            invinTimer = invinTime;

            // Set UI
            UIControl.SetHelthUI(health);

            // Apply Knockback and stun
            Stun();
            rigidBody.velocity = KBDir * knockbackForce;

            // Check mortality
            if (health <= 0) Die();
            else AudioManager.Instance.PlaySFX("PlayerHit");
        }
        
    }
   
    // Called when chakram collides with player, returns true if player is grounded at collision
    public bool ChakramCollide() {
        if (!grounded && state == PlayerState.Main) {
            // bounce
            jumpTimer = Mathf.Max(jumpTime, jumpTimer);
            dashAvailable = dashUnlocked;
            doubleJumpAvailable = doubleJumpUnlocked;
            AudioManager.Instance.PlaySFX("ChakramJump");
            return false;
        }
        return true;
    }
    public void ChakramDestroyed() {
        chakramAvailable = true;
    }
    public bool SetCheckpoint(Vector3 newCheckpoint) { // returns true if checkpoint was set to new location
        bool ret = newCheckpoint != lastCheckpoint;
        lastCheckpoint = newCheckpoint;
        if (ret)
            AudioManager.Instance.PlaySFX("CheckpointSet");
        return ret;
    }

    public void LevelFinish() {
        int powerUps = 0;
        if (doubleJumpUnlocked) ++powerUps;
        if (dashUnlocked) ++powerUps;
        if (chakramUnlocked) ++powerUps;

        AudioManager.Instance.PlaySFX("LevelFinish");

        DataManager.Data newData = new DataManager.Data(-1,maxHealth,lives,powerUps);
        DataManager.Instance.SaveData(newData);
    }

    public void HitRespawnHazard(int damage) {
        HitPlayer(damage, Vector2.down, 0);
        Respawn();
    }
    #endregion

    #region Private Methods ================================================================================
    private void BasicAttack(float vertical) {
        if (state > PlayerState.Main) return;
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
        // Play audio
        AudioManager.Instance.PlaySFX("Swing");
    }

    private void pogo() {
        jumpTimer = Mathf.Max(jumpTime / 2, jumpTimer);
        dashAvailable = dashUnlocked;
        doubleJumpAvailable = doubleJumpUnlocked;
    }

    // Jumps if either grounded or doubleJumpAvailable is true
    // Fails if both grounded and doubleJumpAvailable are false
    private void Jump()
    {
        if (state > PlayerState.Main) return;
        if (grounded) { // grounded jump
            jumpTimer = jumpTime;
            AudioManager.Instance.PlaySFX("Jump1");
        } else if (doubleJumpAvailable) { // double jump
            jumpTimer = jumpTime;
            doubleJumpAvailable = false;
            feetParticles.Play();
            AudioManager.Instance.PlaySFX("Jump2");
        }
    }

    // Dashes if dash is available and not on cooldown
    private void Dash() {
        if (dashAvailable && dashCooldown <= 0 && state < PlayerState.Dash) {
            // Get dashDir
            float temp = 1f;
            if (spriteRenderer.flipX) temp = -1f;
            dashDir = new Vector2(temp, 0) * dashSpeed;

            // Set up dash for FixedUpdate
            dashTimer = dashTime;
            dashAvailable = grounded;
            rigidBody.gravityScale = 0;
            jumpTimer = 0;

            // Set State and enable trail
            SetState(PlayerState.Dash);
            trailRenderer.emitting = true;

            AudioManager.Instance.PlaySFX("Dash");
        }
    }

    private void EndDash()
    {
        dashTimer = 0;
        dashCooldown = dashCooldownBase;
        rigidBody.gravityScale = playerGravity;
        trailRenderer.emitting = false;
        ReturnFromState(PlayerState.Dash);
    }
    private void Chakram() {
        if (!chakramAvailable || state > PlayerState.Main) return;
        if (chakramTimer <= 0) {
            // dir = direction chakram is being cast
            Vector2 dir = Vector2.left;
            if (spriteRenderer.flipX == false) dir = Vector2.right;

            Vector3 chakramPosition = (Vector2) transform.position + dir * chakramOffset;

            ChakramController chakram = Instantiate(chakramPrefab, chakramPosition, Quaternion.identity).GetComponent<ChakramController>();
            chakram.InitializeChakram(dir, this);

            chakramTimer = chakramTime;
            chakramAvailable = false;

            AudioManager.Instance.PlaySFX("ChakramThrow");
        }
    }
    private void Stun()
    {
        stunTimer = stunTime;
        SetState(PlayerState.Stun);
    }
    private void EndStun()
    {
        ReturnFromState(PlayerState.Stun);
    }
    private void Die() {
        SetState(PlayerState.Dead);
        respawnTimer = respawnTime;
        UIControl.SetLivesUI(--lives);
        health = maxHealth;
        AudioManager.Instance.PlaySFX("PlayerDeath");
    }
    private void DeathRespawn() {
        if (lives <= 0) { // Truly perish
            if (GameStateManager.Instance.CurrentGameState != GameState.Victory) {
                GameStateManager.Instance.SetState(GameState.Lose);
                AudioManager.Instance.PlaySFX("GameOver");
            }
        } else { // Kind of perish
            Respawn();
        }
    }
    private void Respawn() {
        transform.position = lastCheckpoint;
        UIControl.SetHelthUI(health);
        state = PlayerState.Main;
    }

    // If target is of greater priority than state, set state to target
    private void SetState(PlayerState target) {
        if (state < target) state = target;
    }
    // If state and target match, sets state to Main
    private void ReturnFromState(PlayerState target) {
        if (state == target) state = PlayerState.Main;
    }
    #endregion

    #region Not fully sure ================================================================================
    private void OnGameStateChanged(GameState newGameState) {
        animator.enabled = newGameState == GameState.Gaming;
        rigidBody.simulated = newGameState == GameState.Gaming;
        enabled = newGameState == GameState.Gaming;
    }
    #endregion
}

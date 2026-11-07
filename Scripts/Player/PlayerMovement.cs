using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float dashSpeed = 10f;

    private Vector2 moveInput;
    public Rigidbody2D rb;
    private PlayerInput controls;
    private BoxCollider2D col;
    public Animator ani;
    public SpriteRenderer sprite_render;
    public Vector2 origin;

    private AnimatorOverrideController new_controller;
    private RuntimeAnimatorController original_controller;

    private PlayerDamageAndHealth playerHealth;
    private PlayerAttacking playerAttacking;

    public bool jumpPressed = false;
    public bool isJumping = false;
    public float jump_time = 0.0f;
    public bool onGround = false;
    public bool touchingWall = false;
    public float wallNormal;
    public bool raycast_touching_wall = false;
    public bool walkingIntoWall = false;
    public float max_jump_time = 0.4f;
    public bool doubleJumpEnabled = false;
    public int jumps_remaining = 0;
    public bool down_jumping = false;
    public bool down_jumped = false;
    public float down_jump_time = 0f;
    public bool crouching = false;
    public bool locked = false;
    public bool dashing = false;
    public float dash_time = 0f;
    public float dash_cooldown = 0f;
    public float set_dash_cooldown = 2f;
    public bool touched_ground_since_dash = false;

    public int super_energy;
    public int super_energy_maximum;

    public bool isSuperAttacking = false;

    public bool canMoveHorizontally = true;
    public bool canMoveVertically = true;
    public bool canAttack = true;
    public bool canUseSkills = true;
    public bool canDash = true;
    public bool canJump = true;
    public bool canSuperAttack = true;

    public bool anything_super_related;

    public bool directionReversed = false;

    public bool isAttacking;
    public bool attemptingJump;
    public bool attemptingCrouch;
    public bool attemptingAttack;
    public bool attemptingSkill;
    public bool attemptingMove;
    public bool attemptingDash;
    public bool attemptingSuperAttack;

    public float super_height;
    public bool super_first_windup;
    public bool super_windingUp;

    public Vector2 attemptedMove;

    public float height;
    public float width;
    public float half_width;
    private Vector2 left;
    private Vector2 right;

    // Only detect objects on the "Ground" and "Platform" layer
    public LayerMask groundLayer;
    // The Trigger Platform Layer
    public LayerMask triggerLayer;
    // The Pit Layer
    public LayerMask pitLayer;

    AnimationClip stunned;
    AnimationClip frozen;
    public bool animator_changed;

    public bool player_just_died;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite_render = GetComponent<SpriteRenderer>();
        controls = GetComponent<PlayerInput>();
        col = GetComponent<BoxCollider2D>();
        ani = GetComponent<Animator>();
        original_controller = ani.runtimeAnimatorController;
        new_controller = new AnimatorOverrideController(ani.runtimeAnimatorController);
        width = col.bounds.size.x;
        half_width = width / 2.0f;
        height = col.bounds.size.y;

        playerHealth = GetComponent<PlayerDamageAndHealth>();
        playerAttacking = GetComponent<PlayerAttacking>();

        stunned = Resources.Load<AnimationClip>("Replacement_Animations/Player/stunned");
        frozen = Resources.Load<AnimationClip>("Replacement_Animations/Player/frozen");
    }
    void Update()
    {
        origin = col.bounds.center;
        Vector2 direction = Vector2.down;
        left = origin + Vector2.left * half_width;
        right = origin + Vector2.right * half_width;

        // Draw the ray (red if not grounded, green if grounded)
        Debug.DrawRay(origin, direction * (height / 2f + 0.1f), onGround ? Color.green : Color.red);
        Debug.DrawRay(left, direction * (height / 2f + 0.1f), onGround ? Color.green : Color.red); // Left Foot Ray
        Debug.DrawRay(right, direction * (height / 2f + 0.1f), onGround ? Color.green : Color.red); //Right Foot Ray

        RaycastHit2D hit_middle = Physics2D.Raycast(origin, Vector2.down, height / 2f + 0.1f, groundLayer);
        RaycastHit2D hit_left = Physics2D.Raycast(left, Vector2.down, height / 2f + 0.1f, groundLayer);
        RaycastHit2D hit_right = Physics2D.Raycast(right, Vector2.down, height / 2f + 0.1f, groundLayer);

        bool touchingGround = hit_middle.collider != null || hit_left.collider != null || hit_right.collider != null;

        // Only count as grounded if touching ground AND moving downward (or stationary)
        if (touchingGround && rb.linearVelocity.y <= 0.01f)
        {
            onGround = true;
            touched_ground_since_dash = true;
            isJumping = false;
            if (down_jump_time == 0f)
                down_jumping = false;
            if (!down_jumping)
                down_jumped = false;
        }
        else
        {
            onGround = false;
        }

        if (attemptingCrouch)
        {
            doCrouch();
        }
        else
        {
            crouching = false;
        }
        // Break crouch if moving
        if (crouching && (isJumping || !onGround || moveInput.x != 0f))
        {
            crouching = false;
        }

        if (isJumping || !onGround || Mathf.Abs(rb.linearVelocity.x) >= 0.1f || playerHealth.parryTime > 0f || anything_super_related || ani.GetBool("isLesserSuperAttacking"))
        {
            crouching = false;
        }

        directionReversed = playerHealth.reverseTime > 0f;

        Vector2 modded_move = moveInput;
        if (directionReversed)
        {
            modded_move = new Vector2(moveInput.x * -1, moveInput.y);
        }
        //walkingIntoWall = touchingWall && raycast_touching_wall && moveInput.x != 0 && Mathf.Sign(moveInput.x) == -Mathf.Sign(wallNormal);
        walkingIntoWall = touchingWall && raycast_touching_wall && moveInput.x != 0 && Mathf.Sign(modded_move.x) == -Mathf.Sign(wallNormal);

        //Debug.DrawRay(origin, Vector2.left * (half_width + 0.05f), walkingIntoWall ? Color.green : Color.red);
        //Debug.DrawRay(origin, Vector2.right * (half_width + 0.05f), walkingIntoWall ? Color.green : Color.red);

        Debug.DrawRay(new Vector2(origin.x, origin.y - (height / 2f - 0.01f)), Vector2.left * (half_width + 0.05f), raycast_touching_wall ? Color.green : Color.red);
        RaycastHit2D try_wall_left = Physics2D.Raycast(new Vector2(origin.x, origin.y - (height / 2f - 0.01f)), Vector2.left, half_width + 0.05f, groundLayer);

        Debug.DrawRay(new Vector2(origin.x, origin.y - (height / 2f - 0.01f)), Vector2.right * (half_width + 0.05f), raycast_touching_wall ? Color.green : Color.red);
        RaycastHit2D try_wall_right = Physics2D.Raycast(new Vector2(origin.x, origin.y - (height / 2f - 0.01f)), Vector2.right, half_width + 0.05f, groundLayer);

        //Debug.DrawRay(new Vector2(origin.x + half_width, origin.y + (height / 2f)), Vector2.up * 10, Color.blue);
        //Debug.DrawRay(new Vector2(origin.x, origin.y + (height / 2f)), Vector2.up * 10, Color.blue);
        //Debug.DrawRay(new Vector2(origin.x - half_width, origin.y + (height / 2f)), Vector2.up * 10, Color.blue);

        if (try_wall_left.collider != null || try_wall_right.collider != null)
        {
            //Debug.Log("woof");
            raycast_touching_wall = true;
        }
        else
        {
            raycast_touching_wall = false;
        }

        if (attemptingSuperAttack && canSuperAttack)
        {
            doSuperAttack();
        }

        if (super_windingUp)
        {
            if (transform.position.y < super_height)
            {
                transform.position += new Vector3(0, Time.deltaTime * 80f, 0);
                if (transform.position.y > super_height)
                {
                    transform.position = new Vector2(transform.position.x,super_height);
                }
            }
            else
            {
                super_windingUp = false;
                isSuperAttacking = true;
                ani.SetBool("isSuperAttacking", true);
            }
        }

        doMove();

        anything_super_related = super_first_windup || super_windingUp || isSuperAttacking;
        if (anything_super_related)
        {
            playerHealth.add_true_invincibility_time(0.5f);
        }

        if (attemptingJump && canJump && !anything_super_related)
        {
            attemptJump();
        }

        if (attemptingAttack && !dashing && !anything_super_related && !ani.GetBool("isLesserSuperAttacking"))
        {
            doAttack();
        }

        else
        {
            isAttacking = false;
        }

        if (attemptingSkill && !dashing && !anything_super_related && !ani.GetBool("isLesserSuperAttacking"))
        {
            useSkill();
        }

        if (locked && !dashing && !anything_super_related)
        {
            doLock();
        }

        if (attemptingDash)
        {
            doDash();
        }

        ani.SetBool("isGrounded", onGround);
        ani.SetBool("isWalking", Mathf.Abs(moveInput.x) != 0 && onGround);
        ani.SetBool("isStopping", Mathf.Abs(rb.linearVelocityX) < 0.1f && Mathf.Abs(rb.linearVelocityX) > 0.01f && onGround);
        ani.SetBool("isStanding", Mathf.Abs(moveInput.x) == 0 && onGround);
        ani.SetBool("isCrouching", crouching);
        ani.SetBool("isFalling", rb.linearVelocityY <= 0.0f);
        ani.SetBool("isAscending", rb.linearVelocityY >= 0.01f);
        ani.SetBool("isDownJumping", down_jumping);
        ani.SetBool("isAttacking", isAttacking);
        ani.SetBool("isParrying", playerHealth.parryTime > 0f);
        ani.SetBool("isDashing", dashing);
        ani.SetBool("isWindingUpSuper", super_first_windup);

        bool isStunned = playerHealth.stunTime > 0f;
        bool isFrozen = playerHealth.freezeTime > 0f;

        bool incapacitated = (isStunned || isFrozen) && !playerHealth.player_died;

        if (incapacitated)
        {
            //AnimationClip display = ((isFrozen && isStunned) || isFrozen) ? frozen : stunned; // Frozen takes priority
            AnimationClip display = (isFrozen) ? frozen : stunned;

            if (!animator_changed)
            {
                new_controller = new AnimatorOverrideController(original_controller);
                new_controller["standing"] = display;
                new_controller["falling"] = display;
                new_controller["jumping"] = display;
                new_controller["attacking"] = display;
                new_controller["walking"] = display;
                new_controller["hurting"] = display;
                ani.runtimeAnimatorController = new_controller;

                animator_changed = true;
            }

            else if (new_controller["standing"] != display)
            {
                new_controller["standing"] = display;
                new_controller["falling"] = display;
                new_controller["jumping"] = display;
                new_controller["attacking"] = display;
                new_controller["walking"] = display;
                new_controller["hurting"] = display;
            }

            canMoveHorizontally = false;
            canMoveVertically = false;
            canAttack = false;
            canUseSkills = false;
            crouching = false;
            canDash = false;
            endDash(true);
            canSuperAttack = false;
        }

        else
        {
            animator_changed = false;
            ani.runtimeAnimatorController = original_controller;
            canMoveHorizontally = true;
            canMoveVertically = true;
            canAttack = sprite_render.enabled;
            canUseSkills = sprite_render.enabled;
            canDash = sprite_render.enabled;
            canSuperAttack = sprite_render.enabled;
        }

        if (playerHealth.player_died)
        {
            ani.SetBool("isDead", playerHealth.player_died);
            player_died();
        }
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.started || context.performed)
        {
            attemptingMove = true;
            attemptedMove = new Vector2(context.ReadValue<Vector2>().x, context.ReadValue<Vector2>().y);
        }
        else if (context.canceled)
        {
            attemptingMove = false;
            attemptedMove = Vector2.zero;
        }
    }
    public void doMove()
    {
        if (!dashing && !anything_super_related)
        {
            if (attemptedMove.x == -1)
            {
                sprite_render.flipX = !directionReversed;
            }
            else if (attemptedMove.x == 1)
            {
                sprite_render.flipX = directionReversed;
            }
        }
        moveInput = attemptedMove;
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            attemptingJump = true;
            jumpPressed = true;
        }

        else if (context.canceled)
        {
            attemptingJump = false;
            jumpPressed = false;
            jump_time = 0f;
        }
    }
    public void OnLock(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            locked = true;
        }

        else if (context.canceled)
        {
            locked = false;
        }
    }
    private void doLock()
    {
        crouching = false;
        moveInput = new Vector2(0f, 0f);
    }
    private void attemptJump()
    {
        if (!canMoveVertically || anything_super_related)
        {
            return;
        }

        if (crouching)
        {
            down_jumping = true;
            down_jumped = true;
            down_jump_time = 0.05f;
        }

        else if (isJumping == false && onGround && down_jumped == false)
        {
            jump_time = max_jump_time;
            doJump();
        }
    }
    private void doJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // the jump
        isJumping = true;
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            attemptingCrouch = true;
        }

        if (context.canceled)
        {
            attemptingCrouch = false;
        }
    }
    private void doCrouch()
    {
        if (!canMoveVertically || anything_super_related)
        {
            return;
        }

        if (!isJumping && onGround && moveInput.x == 0)
        {
            crouching = true;
        }
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            attemptingAttack = true;
        }
        else if (context.canceled)
        {
            attemptingAttack = false;
        }
    }
    private void doAttack()
    {
        if (!canAttack && !anything_super_related)
        {
            isAttacking = false;
        }
        else
        {
            isAttacking = true;
        }
    }
    public void OnSkillUse(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            attemptingSkill = true;
        }
        else if (context.canceled)
        {
            attemptingSkill = false;
        }
    }
    public void OnWeaponSwitch(InputAction.CallbackContext context)
    {
        if (context.started && playerHealth.weaponSwitchCooldown == 0f && !attemptingAttack && !anything_super_related)
        {
            /*if (playerHealth.player_attacking.currentWeapon == PlayerAttacking.WeaponType.Fire)
            {
                playerHealth.player_attacking.switchWeapon(PlayerAttacking.WeaponType.Ice);
            }
            else if (playerHealth.player_attacking.currentWeapon == PlayerAttacking.WeaponType.Ice)
            {
                playerHealth.player_attacking.switchWeapon(PlayerAttacking.WeaponType.Fire);
            }*/

            /*var temp = playerHealth.player_attacking.currentWeapon;
            playerHealth.player_attacking.currentWeapon = playerHealth.player_attacking.lastWeapon;
            playerHealth.player_attacking.lastWeapon = temp;*/
            playerHealth.player_attacking.switchWeapon(playerHealth.player_attacking.lastWeapon);
            playerHealth.RemoveEnergy(50);
            playerHealth.weaponSwitchCooldown = 4f;
        }
    }
    public void DoStage2_Debug(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Time.timeScale = 1.0f;
            SceneManager.LoadScene("Main Menu");
        }
    }
    public void DoDash(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            attemptingDash = true;
        }
        else if (context.canceled)
        {
            attemptingDash = false;
        }
    }
    private void doDash()
    {
        if (canDash && dash_cooldown <= 0f && !dashing && !(playerHealth.slowTime > 0) && !anything_super_related)
        {
            dash_time = 0.5f;
            playerHealth.parryTime = 0f;
            dashing = true;
            if (!onGround)
            {
                touched_ground_since_dash = false;
            }
        }
    }
    public void endDash(bool immediate = false)
    {
        if (immediate)
        {
            dash_time = 0;
            dashing = false;
            dash_cooldown = set_dash_cooldown;
            return;
        }

        dash_time -= Time.fixedDeltaTime;
        if (dash_time <= 0)
        {
            dashing = false;
            dash_cooldown = set_dash_cooldown;
        }
    }
    public void DoSuperAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            attemptingSuperAttack = true;
        }
        else if (context.canceled)
        {
            attemptingSuperAttack = false;
        }
    }
    private void doSuperAttack()
    {
        if (canSuperAttack && playerHealth.blindTime <= 0 && !anything_super_related && playerHealth.hurtTime <= 0 && super_energy >= 500)
        {
            //DEBUG_SUPER_COOLDOWN = 50f;
            endDash(true);
            playerHealth.RemoveEnergy(500);
            super_first_windup = true;
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.linearVelocity = new Vector2(0, 0);
            Time.timeScale = 0.1f;
            ani.speed = 1f / (Time.timeScale);

            LayerMask onlyGround = LayerMask.GetMask("Ground", "BreakableGround");
            RaycastHit2D look_for_ceiling_middle = Physics2D.Raycast(new Vector2(origin.x, origin.y + (height / 2f)), Vector2.up, 20f, onlyGround);
            RaycastHit2D look_for_ceiling_left = Physics2D.Raycast(new Vector2(origin.x - half_width, origin.y + (height / 2f)), Vector2.up, 20f, onlyGround);
            RaycastHit2D look_for_ceiling_right = Physics2D.Raycast(new Vector2(origin.x + half_width, origin.y + (height / 2f)), Vector2.up, 20f, onlyGround);

            float addition = 5f;
            if (look_for_ceiling_middle.collider != null)
            {
                addition = look_for_ceiling_middle.distance / 2f;
            }
            else if (look_for_ceiling_left.collider != null)
            {
                addition = look_for_ceiling_left.distance / 2f;
            }
            else if (look_for_ceiling_right.collider != null)
            {
                addition = look_for_ceiling_right.distance / 2f;
            }
            super_height = transform.position.y + addition;
        }
        else if (canSuperAttack && !anything_super_related && playerHealth.hurtTime <= 0 && !ani.GetBool("isLesserSuperAttacking") && super_energy >= 100)
        {
            playerHealth.RemoveEnergy(100);
            rb.bodyType = RigidbodyType2D.Static;
            playerHealth.parryTime = 0f;
            //rb.linearVelocity = new Vector2(0, 0);
            ani.SetBool("isLesserSuperAttacking", true);
        }
    }
    public void endSuperWindup()
    {
        super_first_windup = false;
        super_windingUp = true;
    }
    public void call_super_attack()
    {
        isSuperAttacking = true;
        playerAttacking.doSuperAttack();
    }
    public void end_super_attacking()
    {
        isSuperAttacking = false;
        ani.SetBool("isSuperAttacking", false);
        Time.timeScale = 1;
        rb.bodyType = RigidbodyType2D.Dynamic;
        ani.speed = 1;
    }
    private void useSkill()
    {
        if (!canUseSkills || anything_super_related || playerHealth.parryCooldown > 0f)
        {
            return;
        }
        else
        {
            playerHealth.parryTime = playerHealth.maxParryTime;
            playerHealth.parryCooldown = playerHealth.maxParryCooldown / 2f + playerHealth.maxParryTime;
        }
    }
    private void player_died()
    {
        controls.enabled = false;
        attemptingJump = false;
        attemptingCrouch = false;
        attemptingAttack = false;
        attemptingSkill = false;
        attemptingMove = false;
        attemptingDash = false;
        attemptingSuperAttack = false;
        ani.SetInteger("deathType", (int) playerHealth.lastHitType);

        if (playerHealth.lastHitType == EnemyAttack.AttackType.Pit)
        {
            if (transform.position.y <= -20)
            {
                transform.position = new Vector2(transform.position.x, -20);
            }
        }

        if (!player_just_died)
        {
            player_just_died = true;
            dash_time = 0;
            rb.linearVelocity = new Vector2(0, 0);
            if (playerHealth.lastHitType == EnemyAttack.AttackType.Pit)
            {
                col.enabled = false;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & triggerLayer) != 0) // If what exited is on the triggerLayer
        {
            down_jumping = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision) // Check for pits
    {
        if (((1 << collision.gameObject.layer) & pitLayer) != 0)
        {
            if (!playerHealth.player_died)
            {
                doJump();
            }
        }
        else
        {
            return;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        touchingWall = false;

        if (((1 << collision.gameObject.layer) & groundLayer) == 0) return;

        foreach (var c in collision.contacts)
        {
            Vector2 n = c.normal;
            //Debug.Log($"normal: {n}, point: {c.point}, player bottom: {col.bounds.center.y - height / 2f}");

            // Mostly-horizontal surface = wall
            if (Mathf.Abs(n.x) > 0.9f && n.y < 0.2f)
            {
                touchingWall = true;
                wallNormal = n.x;
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        touchingWall = false;
    }

    void FixedUpdate()
    {
        float newSpeed = moveSpeed;
        if (playerHealth.slowTime > 0)
        {
            newSpeed = moveSpeed * (1 - playerHealth.slowPotency);
        }

        float theX = newSpeed * moveInput.x;
        if (walkingIntoWall || !canMoveHorizontally || anything_super_related)
        {
            theX = 0.0f;
        }
        if (playerHealth.reverseTime > 0)
        {
            theX *= -1;
        }

        if (dash_time <= 0)
        {
            rb.linearVelocity = new Vector2(theX, rb.linearVelocity.y);
        }
        else
        {
            bool dash_direction = sprite_render.flipX;
            rb.linearVelocity = dash_direction ? new Vector2(-dashSpeed, 0) : new Vector2(dashSpeed, 0);
            endDash();
        }

        if (dash_cooldown > 0 && touched_ground_since_dash)
        {
            dash_cooldown -= Time.fixedDeltaTime;
        }

        if (jumpPressed && jump_time > 0f && !anything_super_related)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jump_time -= Time.fixedDeltaTime; // subtract time passed this, with fixed update
        }

        if (down_jump_time > 0.0f)
        {
            down_jump_time -= Time.fixedDeltaTime;
        }
        else if (down_jump_time < 0.0f)
        {
            down_jump_time = 0.0f;
        }
    }
}


using NUnit.Framework.Internal.Commands;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    private Vector2 moveInput;
    public Rigidbody2D rb;
    private PlayerInput controls;
    private BoxCollider2D col;
    private Animator ani;
    public SpriteRenderer sprite_render;

    private AnimatorOverrideController new_controller;
    private RuntimeAnimatorController original_controller;

    private PlayerDamageAndHealth playerHealth;

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
    public float down_jump_time = 0.0f;
    public bool crouching = false;
    public bool locked = false;

    public bool canMoveHorizontally = true;
    public bool canMoveVertically = true;
    public bool canAttack = true;
    public bool canUseSkills = true;

    public bool directionReversed = false;

    public bool isAttacking;
    public bool attemptingJump;
    public bool attemptingCrouch;
    public bool attemptingAttack;
    public bool attemptingSkill;
    public bool attemptingMove;

    public Vector2 attemptedMove;

    private float height;
    private float width;
    private float half_width;
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
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite_render = GetComponent<SpriteRenderer>();
        controls = GetComponent<PlayerInput>();
        col = GetComponent<BoxCollider2D>();
        ani = GetComponent<Animator>();
        original_controller = ani.runtimeAnimatorController;
        new_controller = new AnimatorOverrideController (ani.runtimeAnimatorController);
        width = col.bounds.size.x;
        half_width = width / 2.0f;
        height = col.bounds.size.y;

        playerHealth = GetComponent<PlayerDamageAndHealth>();

        stunned = Resources.Load<AnimationClip>("Replacement_Animations/Player/stunned");
        frozen = Resources.Load<AnimationClip>("Replacement_Animations/Player/frozen");

    }

    void Update()
    {

        Vector2 origin = col.bounds.center;
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

        if (isJumping || !onGround || Mathf.Abs(rb.linearVelocity.x) >= 0.1f || playerHealth.parryTime > 0f)
        {            
            crouching = false;
        }

        Vector2 modded_move = moveInput;
        if (playerHealth.reverseTime > 0f)
        {
            modded_move = new Vector2(moveInput.x * -1, moveInput.y);
        }
        //walkingIntoWall = touchingWall && raycast_touching_wall && moveInput.x != 0 && Mathf.Sign(moveInput.x) == -Mathf.Sign(wallNormal);
        walkingIntoWall = touchingWall && raycast_touching_wall && moveInput.x != 0 && Mathf.Sign(modded_move.x) == -Mathf.Sign(wallNormal);

        //Debug.DrawRay(origin, Vector2.left * (half_width + 0.05f), walkingIntoWall ? Color.green : Color.red);
        //Debug.DrawRay(origin, Vector2.right * (half_width + 0.05f), walkingIntoWall ? Color.green : Color.red);

        Debug.DrawRay(new Vector2(origin.x,origin.y - (height / 2f - 0.01f)), Vector2.left * (half_width + 0.05f), raycast_touching_wall ? Color.green : Color.red);
        RaycastHit2D try_wall_left = Physics2D.Raycast(new Vector2(origin.x,origin.y - (height / 2f - 0.01f)), Vector2.left, half_width + 0.05f, groundLayer);

        Debug.DrawRay(new Vector2(origin.x, origin.y - (height / 2f - 0.01f)), Vector2.right * (half_width + 0.05f), raycast_touching_wall ? Color.green : Color.red);
        RaycastHit2D try_wall_right = Physics2D.Raycast(new Vector2(origin.x, origin.y - (height / 2f - 0.01f)), Vector2.right, half_width + 0.05f, groundLayer);

        if (try_wall_left.collider != null || try_wall_right.collider != null)
        {
            //Debug.Log("woof");
            raycast_touching_wall = true;
        }
        else
        {
            raycast_touching_wall = false;
        }

        doMove();

        if (attemptingJump)
        {
            attemptJump();
        }

        if (attemptingAttack)
        {
            doAttack();
        }
        else
        {
            isAttacking = false;
        }

        if (attemptingSkill)
        {
            useSkill();
        }

        if (locked)
        {
            doLock();
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
        ani.SetBool("isParrying", (playerHealth.parryTime > 0f));

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

        }

        else
        {
            animator_changed = false;
            ani.runtimeAnimatorController = original_controller;
            canMoveHorizontally = true;
            canMoveVertically = true;
            canAttack = true;
            canUseSkills = true;
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
            /*if (playerHealth.reverseTime > 0)
            {
                attemptedMove = new Vector2(context.ReadValue<Vector2>().x * -1, context.ReadValue<Vector2>().y);
            }
            else
            {
                attemptedMove = new Vector2(context.ReadValue<Vector2>().x, context.ReadValue<Vector2>().y);
            }*/
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
        if (attemptedMove.x == -1)
        {
            sprite_render.flipX = (playerHealth.reverseTime > 0) ? false : true;
        }
        else if (attemptedMove.x == 1)
        {
            sprite_render.flipX = (playerHealth.reverseTime > 0) ? true : false;
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
        if (!canMoveVertically)
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

        if (context.canceled) {
            attemptingCrouch = false;
        }

    }
    private void doCrouch()
    {
        if (!canMoveVertically)
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
        if (!canAttack)
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
        if (context.started && playerHealth.weaponSwitchCooldown == 0f && !attemptingAttack)
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

            playerHealth.weaponSwitchCooldown = 4f;
        }
    }

    public void DoStage2_Debug(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("nope.");
        }
    }
    private void useSkill()
    {
        if (!canUseSkills || playerHealth.parryCooldown > 0f)
        {
            return;
        }
        else
        {
            playerHealth.parryTime = 0.5f;
            playerHealth.parryCooldown = playerHealth.maxParryCooldown;
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
            return;
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
        if (walkingIntoWall || !canMoveHorizontally)
        {
            theX = 0.0f;
        }
        if (playerHealth.reverseTime > 0)
        {
            theX *= -1;
        }
        rb.linearVelocity = new Vector2(theX, rb.linearVelocity.y);

        if (jumpPressed && jump_time > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jump_time -= Time.fixedDeltaTime; // subtract time passed this, with fixed update
        }

        if (down_jump_time > 0.0f)
            down_jump_time -= Time.fixedDeltaTime;
        else if (down_jump_time < 0.0f)
            down_jump_time = 0.0f;

    }

}


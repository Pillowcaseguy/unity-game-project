using UnityEngine;

public class TestBoss5Behavior_Ninja : MonoBehaviour, EnemyDamageable, EnemyMainBodyImplement
{
    public bool is_dead;
    public bool just_died;
    public int max_health;
    public int health;

    public Rigidbody2D rb;
    public BoxCollider2D box;
    public Animator ani;
    public LevelManager_TestBoss5 manager;
    public SpriteRenderer sprite;

    public bool called_to_swap;

    public GameObject jumping_up_ninja;
    public Ninja_JumpingUp current_jumping_ninja;
    public GameObject jumping_down_ninja;
    public Ninja_JumpingDown current_falling_ninja;

    public bool is_invincible;
    public bool is_attacking;

    public bool is_swappable;

    public bool can_flip;
    public bool can_flip_once;
    public bool can_move;
    public float flip_cooldown;

    public bool touching_wall;
    public LayerMask ground_layer;

    public GameObject smoke_bomb;
    public GameObject kunai;
    public GameObject smoke_bomb_throwing_point;

    public GameObject kunai_throwing_point_landing;
    public GameObject kunai_throwing_point_jump;

    public NinjaLandingKickAttack kick_attack;

    public bool onGround;
    public bool first_jump;
    public bool jump_attacking;
    public bool jump_kick_twas_parried;

    public NinjaDiveKickAttack dive_kick_attack;
    public NinjaDiveKickGroundCheck dive_kick_ground;
    public GameObject fire_powder;
    public GameObject fire_powder_spawn_spot;

    public GameObject slashes;
    public int slashes_made;
    public GameObject sword_wave;

    public bool can_sword_dash;
    public bool can_throw_fire_powder;

    public GameObject invisible_wall;
    public GameObject current_invisible_wall_1;
    public GameObject current_invisible_wall_2;
    public GameObject crouch_slash;

    public BoxCollider2D cross_up_prevention;

    public EnemyAttack cross_slash_attack;
    public BoxCollider2D cross_slash_box;
    public bool cross_slash_in_range;

    public EnemyAttack big_slash_attack_bottom;
    public BoxCollider2D big_slash_bottom_box;
    public EnemyAttack big_slash_attack_top;
    public BoxCollider2D big_slash_top_box;
    public GameObject sword_slash_point;

    public GameObject caltrops;

    public float inability_time;
    public float burn_time;
    public float freeze_time;
    public bool frozen_first_time;

    public GameObject tornado;

    /*public bool debug_drop_ninja;
    public bool debug_send_me_back_up;
    public bool debug_jump_ninja;
    public bool debug_jump_into_air;
    public bool debug_swash;
    public bool debug_wave;
    public bool debug_start_slash;
    public bool debug_throw_smoke;
    public bool debug_cross_slash;
    public bool debug_caltrops;
    public bool debug_tornado;*/

    public float slash_cooldown;
    public float smoke_cooldown;
    public float cross_slash_cooldown;
    public float tornado_cooldown;
    public float caltrops_cooldown;
    public float jump_cooldown;
    public float walk_cooldown;
    public float all_actions_cooldown;

    public float walk_time;

    public GameObject dead_ninja;

    public bool debug_kill_me;

    public GameObject burn_effect;
    public TestBoss5NinjaBurnEffect current_burn;

    public GameObject damage_number;

    void Awake()
    {
        ground_layer = LayerMask.GetMask("Ground","BreakableGround");
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        ani = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        health = max_health;
    }

    void Update()
    {
        if (!just_died && is_dead)
        {
            burn_time = 0;
            if (current_burn)
            {
                Destroy(current_burn);
            }
            freeze_time = 0;
            end_attack();
            ani.SetBool("is_dead", is_dead);
            sprite.enabled = false;
            manager.spawn_knockout_wave();
            manager.number_of_dead_bosses += 1;
            var hi = Instantiate(dead_ninja, new Vector2(transform.position.x,transform.position.y + 0.5f), Quaternion.Euler(0, 0, 0)).GetComponent<NinjaDefeatedBehavior>();
            hi.sprite.flipX = sprite.flipX;
            hi.boss = this;
            box.enabled = false;
            just_died = true;
        }

        if (is_dead)
        {
            if (rb.bodyType != RigidbodyType2D.Static)
                rb.bodyType = RigidbodyType2D.Static;
            return;
        }

        if (debug_kill_me)
        {
            TakeDamage(4000);
            debug_kill_me = false;
        }
        float t = Time.deltaTime;

        /*if (debug_drop_ninja)
        {
            summon_falling_ninja();
            debug_drop_ninja = false;
        }
        if (debug_send_me_back_up)
        {
            transform.position = new Vector2(transform.position.x, 12);
            debug_send_me_back_up = false;
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
        if (debug_jump_ninja)
        {
            summon_jumping_ninja();
            debug_jump_ninja = false;
        }*/

        if (current_falling_ninja != null)
        {
            sprite.enabled = false;
            box.enabled = false;
            apply_invincibility(1);
            if (current_burn)
            {
                current_burn.lifetime = -1;
            }
            rb.bodyType = RigidbodyType2D.Static;
            if (current_falling_ninja.fine_to_destroy)
            {
                if (current_falling_ninja.attack.wasParried)
                {
                    is_attacking = true;
                    ani.SetBool("landing", true);
                    ani.SetBool("failedLanding", true);
                }
                else if (current_falling_ninja.attack.wasHit)
                {
                    ani.SetBool("landing", true);
                    is_attacking = true;
                    ani.SetBool("fallHitPlayer", true);
                    can_flip_once = true;
                }
                else if (Mathf.Abs(current_falling_ninja.transform.position.x - manager.player.transform.position.x) > 6 || manager.player_life.movement.dash_time > 0f)
                {
                    ani.SetBool("landing", true);
                    is_attacking = true;
                    ani.SetBool("landing_player_far", true);
                    can_flip_once = true;
                }
                else
                {
                    is_attacking = true;
                    ani.SetBool("landing", true);
                    all_actions_cooldown = 2f;
                }
                transform.position = current_falling_ninja.transform.position;
                rb.bodyType = RigidbodyType2D.Dynamic;
                box.enabled = true;
                sprite.enabled = true;
                apply_invincibility(0);
                instant_flip_check();
                current_falling_ninja.destroy_self();
            }
        }

        if (current_jumping_ninja != null)
        {
            sprite.enabled = false;
            box.enabled = false;
            apply_invincibility(1);
            if (current_burn)
            {
                current_burn.lifetime = -1;
            }
            if (current_jumping_ninja.fine_to_destroy)
            {
                transform.position = new Vector2(0, 15);
                rb.bodyType = RigidbodyType2D.Static;
                current_jumping_ninja.destroy_self();
            }
        }

        countdown_cooldowns(t);

        is_swappable = Mathf.Abs(rb.linearVelocity.x) <= 0.01f && onGround && !is_attacking && inability_time <= 0;
        can_move = !is_attacking;
        can_flip = !(is_attacking || inability_time > 0);

        Vector2 origin = box.bounds.center;
        Vector2 direction = Vector2.down;
        float width = box.bounds.size.x;
        float half_width = width / 2.0f;
        float height = box.bounds.size.y;
        Vector2 left = origin + Vector2.left * half_width;
        Vector2 right = origin + Vector2.right * half_width;
        RaycastHit2D try_wall_left = Physics2D.Raycast(new Vector2(origin.x, origin.y - (height / 2f - 0.01f)), Vector2.left, half_width + 0.05f, ground_layer);
        RaycastHit2D try_wall_right = Physics2D.Raycast(new Vector2(origin.x, origin.y - (height / 2f - 0.01f)), Vector2.right, half_width + 0.05f, ground_layer);

        touching_wall = try_wall_left.collider != null || try_wall_right.collider != null;

        flip_check(t);
        if (ani.GetBool("jump_kicking") || ani.GetBool("jump_throwing"))
        {
            instant_flip_check();
        }

        /*Debug.DrawRay(origin, direction * (height / 2f + 0.1f), onGround ? Color.green : Color.red);
        Debug.DrawRay(left, direction * (height / 2f + 0.1f), onGround ? Color.green : Color.red);
        Debug.DrawRay(right, direction * (height / 2f + 0.1f), onGround ? Color.green : Color.red);*/

        RaycastHit2D hit_middle = Physics2D.Raycast(origin, Vector2.down, height / 2f + 0.1f, ground_layer);
        RaycastHit2D hit_left = Physics2D.Raycast(left, Vector2.down, height / 2f + 0.1f, ground_layer);
        RaycastHit2D hit_right = Physics2D.Raycast(right, Vector2.down, height / 2f + 0.1f, ground_layer);

        onGround = hit_middle.collider != null || hit_left.collider != null || hit_right.collider != null;

        ani.SetBool("is_attacking", is_attacking);
        ani.SetBool("onGround", onGround);

        if (cross_slash_attack.wasParried)
        {
            ani.SetBool("cross_slash_parried", true);
            cross_slash_cooldown = 30f;
            all_actions_cooldown = 0.5f;
        }

        if (freeze_time > 0)
        {
            freeze_time -= Time.deltaTime;
            if (frozen_first_time && freeze_time <= 1f)
            {
                frozen_first_time = false;
                ani.SetBool("got_frozen", false);
            }
            if (freeze_time < 0f)
            {
                freeze_time = 0f;
            }
        }

        if (burn_time > 0)
        {
            if (current_burn == null)
            {
                current_burn = Instantiate(burn_effect, transform.position, Quaternion.Euler(0, 0, 0)).GetComponent<TestBoss5NinjaBurnEffect>();
                current_burn.Initialize(burn_time, gameObject);
            }

            burn_time -= Time.deltaTime;
            if (burn_time <= 0)
            {
                burn_time = 0;
                if (current_burn != null)
                {
                    Destroy(current_burn, 0.3f);
                }
            }
        }

        inability_time = freeze_time;

        if (onGround && !is_attacking && inability_time <= 0 && manager.player_life.player_died && sprite.enabled)
        {
            is_attacking = true;
            ani.SetBool("killed_player", true);
        }

        else if (!is_attacking && called_to_swap && is_swappable && inability_time <= 0 && all_actions_cooldown <= 0 && onGround && current_falling_ninja == null && current_jumping_ninja == null)
        {
            all_actions_cooldown = 10f;
            summon_jumping_ninja();
        }

        else if (!is_attacking && !called_to_swap && is_swappable && inability_time <= 0 && onGround && cross_slash_cooldown <= 0 && cross_slash_in_range && current_falling_ninja == null && current_jumping_ninja == null)
        {
            instant_flip_check();
            is_attacking = true;
            ani.SetBool("cross_slash", true);
            cross_slash_cooldown = 5f;
            all_actions_cooldown = 1.5f;
        }

        else if (!is_attacking && !called_to_swap && inability_time <= 0 && all_actions_cooldown <= 0 && onGround && current_falling_ninja == null && current_jumping_ninja == null)
        {
            bool action_selected = false;
            if (!action_selected && can_sword_dash && slash_cooldown <= 0)
            {
                float small_rng = Random.value;
                if (!(small_rng >= 0.05f && small_rng <= 0.2f)) // 16% chance to bypass attack and do something else
                {
                    action_selected = true;
                    do_dash_and_slash_attack();
                }
            }
            if (health <= max_health / 2f && !action_selected && tornado_cooldown <= 0)
            {
                is_attacking = true;
                action_selected = true;
                ani.SetBool("creating_tornado", true);
                tornado_cooldown = 35f;
                all_actions_cooldown = 5.5f;
            }
            if (!action_selected && smoke_cooldown <= 0)
            {
                instant_flip_check();
                is_attacking = true;
                action_selected = true;
                ani.SetBool("throwing_smoke_bomb", true);
                smoke_cooldown = 20f;
                all_actions_cooldown = 5f;
            }
            if (!action_selected && caltrops_cooldown <= 0)
            {
                float small_rng = Random.value;
                if (!(small_rng >= 0.5f && small_rng <= 0.75f)) // 26% chance to bypass attack
                {
                    is_attacking = true;
                    action_selected = true;
                    ani.SetBool("throwing_caltrops", true);
                    caltrops_cooldown = 12f;
                    all_actions_cooldown = 2f;
                }
            }
            if (!action_selected)
                instant_flip_check();
            if (!action_selected && Mathf.Abs(manager.player.transform.position.x - transform.position.x) <= 2f && can_throw_fire_powder)
            {
                float small_rng = Random.value;
                if (small_rng >= 0.2f && small_rng <= 0.5f) // 31% chance to bypass attack
                {
                    ani.SetBool("using_fire_powder", true);
                    is_attacking = true;
                    action_selected = true;
                    all_actions_cooldown = 2f;
                }
            }
            if (!action_selected && walk_cooldown <= 0f && Mathf.Abs(manager.player.transform.position.x - transform.position.x) >= 7f)
            {
                float small_rng = Random.value;
                if (!(small_rng >= 0.25f && small_rng <= 0.95f)) // 71% chance to bypass moving and jump instead
                {
                    is_attacking = true;
                    action_selected = true;
                    ani.SetBool("is_walking", true);
                    walk_cooldown = 10f;
                }
            }
            if (!action_selected && jump_cooldown <= 0f)
            {
                ani.SetBool("is_jumping", true);
                is_attacking = true;
                action_selected = true;
                jump_cooldown = 4f;
                all_actions_cooldown = 3f;
            }
        }
        // For fire powder
        RaycastHit2D wall_too_close_left_fire = Physics2D.Raycast(new Vector2(origin.x, origin.y - (height / 2f - 0.01f)), Vector2.left, half_width + 0.5f, ground_layer);
        RaycastHit2D wall_too_close_right_fire = Physics2D.Raycast(new Vector2(origin.x, origin.y - (height / 2f - 0.01f)), Vector2.right, half_width + 0.5f, ground_layer);

        can_throw_fire_powder = wall_too_close_left_fire.collider == null && wall_too_close_right_fire.collider == null;

        // For sword dash
        //RaycastHit2D wall_too_close_left_sword = Physics2D.Raycast(new Vector2(origin.x, origin.y - (height / 2f - 0.01f)), Vector2.left, half_width + 7f, ground_layer); 
        //RaycastHit2D wall_too_close_right_sword = Physics2D.Raycast(new Vector2(origin.x, origin.y - (height / 2f - 0.01f)), Vector2.right, half_width + 7f, ground_layer);

        if (manager.player != null)
        {
            RaycastHit2D wall_too_close_left_sword = Physics2D.Raycast(manager.player.transform.position, Vector2.left, 4.5f, ground_layer);
            RaycastHit2D wall_too_close_right_sword = Physics2D.Raycast(manager.player.transform.position, Vector2.right, 4.5f, ground_layer);

            can_sword_dash = !((!sprite.flipX && wall_too_close_left_sword.collider != null) || (sprite.flipX && wall_too_close_right_sword.collider != null));
        }
    }
    public void countdown_cooldowns(float time)
    {
        slash_cooldown -= time;
        if (slash_cooldown < 0)
        {
            slash_cooldown = 0;
        }
        smoke_cooldown -= time;
        if (smoke_cooldown < 0)
        {
            smoke_cooldown = 0;
        }
        cross_slash_cooldown -= time;
        if (cross_slash_cooldown < 0)
        {
            cross_slash_cooldown = 0;
        }
        tornado_cooldown -= time;
        if (tornado_cooldown < 0)
        {
            tornado_cooldown = 0;
        }
        caltrops_cooldown -= time;
        if (caltrops_cooldown < 0)
        {
            caltrops_cooldown = 0;
        }
        jump_cooldown -= time;
        if (jump_cooldown < 0)
        {
            jump_cooldown = 0;
        }
        walk_cooldown -= time;
        if (walk_cooldown < 0)
        {
            walk_cooldown = 0;
        }
        all_actions_cooldown -= time;
        if (all_actions_cooldown < 0)
        {
            all_actions_cooldown = 0;
        }
}
    public void summon_falling_ninja()
    {
        current_falling_ninja = Instantiate(jumping_down_ninja, transform.position, Quaternion.Euler(0,0,0)).GetComponent<Ninja_JumpingDown>();
        current_falling_ninja.rb.linearVelocityY = -20;
        current_falling_ninja.manager = manager;
    }
    public void summon_jumping_ninja()
    {
        current_jumping_ninja = Instantiate(jumping_up_ninja, transform.position, Quaternion.Euler(0, 0, 0)).GetComponent<Ninja_JumpingUp>();
        current_jumping_ninja.sprite.flipX = sprite.flipX;
        called_to_swap = false;
        manager.complete_handshake();
    }
    public void summon_knocked_player()
    {
        Level5KnockedPlayerBehavior temp = Instantiate(manager.knocked_player,
            new Vector2(manager.player.transform.position.x, manager.player.transform.position.y + 0.5f), Quaternion.Euler(0, 0, 0)).GetComponent<Level5KnockedPlayerBehavior>();
        manager.hide_player(true);
        bool player_behind_hit = manager.player.transform.position.x < transform.position.x;
        temp.rb.linearVelocityX = player_behind_hit ? -10 : 10;
        temp.sprite.flipX = player_behind_hit;
    }
    public void apply_invincibility(int i)
    {
        is_invincible = !(i == 0);
    }

    public void toggle_kick(int b)
    {
        kick_attack.my_attack.stunDuration = 4;
        if (b == 1)
        {
            kick_attack.knocked_player_summoned = false;
            kick_attack.my_attack.EnableHitbox();
        }
        else
        {
            kick_attack.my_attack.clearFlags();
            kick_attack.my_attack.DisableHitbox();
            kick_attack.my_attack.disableAttackCycle = false;
            kick_attack.knocked_player_summoned = false;
        }
    }
    public void toggle_kick_no_knockback(int b)
    {
        kick_attack.my_attack.stunDuration = 1;
        if (b == 1)
        {
            kick_attack.knocked_player_summoned = true;
            kick_attack.my_attack.EnableHitbox();
        }
        else
        {
            kick_attack.my_attack.clearFlags();
            kick_attack.my_attack.DisableHitbox();
            kick_attack.my_attack.disableAttackCycle = false;
            kick_attack.knocked_player_summoned = false;
        }
    }
    public void end_attack()
    {
        float change_in_time = 0;
        is_attacking = false;
        can_flip_once = false;

        if (ani.GetBool("landing"))
            change_in_time = 2f;
        ani.SetBool("landing", false);

        if (ani.GetBool("fallHitPlayer"))
            change_in_time = 4f;
        ani.SetBool("fallHitPlayer", false);

        if (ani.GetBool("landing_player_far"))
            change_in_time = 3f;
        ani.SetBool("landing_player_far", false);

        ani.SetBool("failedLanding", false);
        ani.SetBool("using_fire_powder", false);
        ani.SetBool("is_jumping", false);
        ani.SetBool("jump_kicking", false);

        if (ani.GetBool("jump_throwing"))
            change_in_time = 3f;
        ani.SetBool("jump_throwing", false);

        ani.SetBool("jump_kick_parried", false);
        jump_kick_twas_parried = false;
        dive_kick_attack.my_attack.clearFlags();
        dive_kick_attack.my_attack.disableAttackCycle = false;
        dive_kick_attack.my_attack.DisableHitbox();
        ani.SetBool("start_dash", false);
        ani.SetBool("finish_dash", false);
        if (current_invisible_wall_1 != null)
        {
            Destroy(current_invisible_wall_1, 0.5f);
        }
        if (current_invisible_wall_2 != null)
        {
            Destroy(current_invisible_wall_2, 0.5f);
        }
        cross_up_prevention.enabled = false;
        ani.SetBool("throwing_smoke_bomb", false);
        ani.SetBool("start_smoke_dash", false);
        ani.SetBool("do_big_slash", false);
        ani.SetBool("cross_slash", false);
        ani.SetBool("cross_slash_parried", false);
        cross_slash_attack.clearFlags();
        cross_slash_attack.disableAttackCycle = false;
        cross_slash_attack.DisableHitbox();
        talk_to_big_slash(10);
        talk_to_big_slash(11);
        ani.SetBool("throwing_caltrops", false);

        if (ani.GetBool("creating_tornado"))
            change_in_time = 3f;
        ani.SetBool("creating_tornado", false);

        if (ani.GetBool("dive_kick_follow_up"))
            change_in_time = 2f;
        ani.SetBool("dive_kick_follow_up", false);

        frozen_first_time = true;

        apply_invincibility(0);
        if (change_in_time == 0f)
        {
            all_actions_cooldown = 1f;
        }
        else
        {
            all_actions_cooldown = change_in_time;
        }
        if (cross_slash_cooldown <= 0f)
        {
            cross_slash_cooldown = 0.5f;
        }
        rb.bodyType = RigidbodyType2D.Dynamic;
    }
    public void unhurt()
    {
        ani.SetBool("is_hurt", false);
    }
    public void flip_check(float t)
    {
        if (manager != null && manager.has_player)
        {
            if (manager.player.transform.position.x > transform.position.x && !sprite.flipX)
            {
                flip_cooldown -= t;
                if (flip_cooldown <= 0 && (can_flip || can_flip_once))
                {
                    can_flip_once = false;
                    flip_everything();
                }
            }
            else if (manager.player.transform.position.x < transform.position.x && sprite.flipX)
            {
                flip_cooldown -= t;
                if (flip_cooldown <= 0 && (can_flip || can_flip_once))
                {
                    can_flip_once = false;
                    flip_everything();
                }
            }
            else
            {
                flip_cooldown = 0.5f;
            }
        }
    }
    public void instant_flip_check()
    {
        if (manager != null && manager.has_player)
        {
            if (manager.player.transform.position.x > transform.position.x && !sprite.flipX)
            {
                flip_cooldown = 0.5f;
                flip_everything();
            }
            else if (manager.player.transform.position.x < transform.position.x && sprite.flipX)
            {
                flip_cooldown = 0.5f;
                flip_everything();
            }
        }
    }
    public void flip_everything()
    {
        sprite.flipX = !sprite.flipX;
        kunai_throwing_point_landing.transform.localPosition = new Vector2(-kunai_throwing_point_landing.transform.localPosition.x, kunai_throwing_point_landing.transform.localPosition.y);
        fire_powder_spawn_spot.transform.localPosition = new Vector2(-fire_powder_spawn_spot.transform.localPosition.x, fire_powder_spawn_spot.transform.localPosition.y);
        dive_kick_attack.attack_box.offset = new Vector2(-dive_kick_attack.attack_box.offset.x, dive_kick_attack.attack_box.offset.y);
        dive_kick_ground.ground_box.offset = new Vector2(-dive_kick_ground.ground_box.offset.x, dive_kick_ground.ground_box.offset.y);
        kunai_throwing_point_jump.transform.localPosition = new Vector2(-kunai_throwing_point_jump.transform.localPosition.x, kunai_throwing_point_jump.transform.localPosition.y);
        smoke_bomb_throwing_point.transform.localPosition = new Vector2(-smoke_bomb_throwing_point.transform.localPosition.x, smoke_bomb_throwing_point.transform.localPosition.y);
        sword_slash_point.transform.localPosition = new Vector2(-sword_slash_point.transform.localPosition.x, sword_slash_point.transform.localPosition.y);
        big_slash_bottom_box.offset = new Vector2(-big_slash_bottom_box.offset.x, big_slash_bottom_box.offset.y);
        big_slash_top_box.offset = new Vector2(-big_slash_top_box.offset.x, big_slash_top_box.offset.y);
    }
    public void throw_projectiles_landing()
    {
        Vector2 throw_position = kunai_throwing_point_landing.transform.position;
        Vector2 player_position = manager.player.transform.position;

        Vector2 ray_to_player = new Vector2(player_position.x - throw_position.x, player_position.y - throw_position.y);
        float calc_rotation = Mathf.Atan2(ray_to_player.y, ray_to_player.x) * Mathf.Rad2Deg;
        //Debug.Log(calc_rotation);

        Instantiate(kunai, kunai_throwing_point_landing.transform.position, Quaternion.Euler(0, 0, calc_rotation + 180)).GetComponent<TestBoss5_NinjaThrowableBehavior>().speed = 10;
        Instantiate(kunai, kunai_throwing_point_landing.transform.position + transform.right * 0.5f, Quaternion.Euler(0, 0, calc_rotation + 175)).GetComponent<TestBoss5_NinjaThrowableBehavior>().speed = 10;
        Instantiate(kunai, kunai_throwing_point_landing.transform.position + transform.right * 1f, Quaternion.Euler(0, 0, calc_rotation + 185)).GetComponent<TestBoss5_NinjaThrowableBehavior>().speed = 10;
    }
    public void throw_projectile_jump()
    {
        Vector2 throw_position = kunai_throwing_point_landing.transform.position;
        Vector2 player_position = manager.player.transform.position;

        Vector2 ray_to_player = new Vector2(player_position.x - throw_position.x, player_position.y - throw_position.y);
        float calc_rotation = Mathf.Atan2(ray_to_player.y, ray_to_player.x) * Mathf.Rad2Deg;

        float sped = health >= max_health / 2f ? 20 : 22;

        if (health >= max_health / 2f)
        {
            Instantiate(kunai, kunai_throwing_point_jump.transform.position, Quaternion.Euler(0, 0, calc_rotation + 180)).GetComponent<TestBoss5_NinjaThrowableBehavior>().speed = sped;
        }
        else
        {
            Instantiate(kunai, kunai_throwing_point_jump.transform.position, Quaternion.Euler(0, 0, calc_rotation + 180)).GetComponent<TestBoss5_NinjaThrowableBehavior>().speed = sped;
            //Instantiate(kunai, kunai_throwing_point_jump.transform.position, Quaternion.Euler(0, 0, calc_rotation + 185)).GetComponent<TestBoss5_NinjaThrowableBehavior>().speed = sped;
        }
    }
    public void throw_smoke_bomb()
    {
        Vector2 throw_position = kunai_throwing_point_landing.transform.position;
        Vector2 player_position = manager.player.transform.position;

        Vector2 ray_to_player = new Vector2(player_position.x - throw_position.x, player_position.y - throw_position.y);
        float calc_rotation = Mathf.Atan2(ray_to_player.y, ray_to_player.x) * Mathf.Rad2Deg;

        var hi = Instantiate(smoke_bomb, smoke_bomb_throwing_point.transform.position, Quaternion.Euler(0, 0, calc_rotation + 180)).GetComponent<TestBoss5_NinjaSmokeBombBehavior>();
        hi.rotation = calc_rotation;
        hi.speed = 15;
    }
    public void throw_fire_powder()
    {
        rb.AddForce(new Vector2(sprite.flipX ? -150 : 150, 100));
        var fiah = Instantiate(fire_powder, fire_powder_spawn_spot.transform.position, Quaternion.Euler(0, 0, 0)).GetComponent<TestBoss5_NinjaGunpowderBehavior>();
        fiah.sprite.flipX = sprite.flipX;
        if (health <= max_health / 2f)
            fiah.ani.speed = 1.2f;
    }
    public void do_jump()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.linearVelocity = new Vector2(0, 0);

        float jump_force = ani.GetBool("jump_kick_parried") ? 400 : 600;
        rb.AddForce(new Vector2(sprite.flipX ? -100 : 100, jump_force));
    }
    public void do_random_jump_action()
    {
        int action = Random.Range(0, 2);
        if (ani.GetBool("jump_kick_parried") && jump_kick_twas_parried)
        {
            action = 0;
            ani.SetBool("jump_kick_parried", false);
        }

        if (action == 0)
        {
            Invoke("do_jump_kick", 1f);
        }
        else if (action == 1)
        {
            Invoke("do_jump_throw", 1f);
        }
        else
        {
            Invoke("do_jump_kick", 1f);
        }
    }
    public void do_jump_kick()
    {
        if (!is_attacking)
        {
            return;
        }
        rb.linearVelocity = new Vector2(0, 0);
        rb.bodyType = RigidbodyType2D.Kinematic;
        ani.SetBool("jump_kicking", true);
        dive_kick_attack.my_attack.clearFlags();
        dive_kick_attack.my_attack.disableAttackCycle = false;
        dive_kick_attack.my_attack.EnableHitbox();
        Vector2 difference = transform.position - manager.player.transform.position;
        float speed = 25;
        float angle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        float x = speed * -Mathf.Cos(angle * Mathf.Deg2Rad);
        float y = speed * -Mathf.Sin(angle * Mathf.Deg2Rad);
        if (y > 0)
        {
            y = 0;
        }
        rb.linearVelocity = new Vector2(x, y);
    }
    public void do_jump_throw()
    {
        if (!is_attacking)
        {
            return;
        }
        rb.linearVelocity = new Vector2(0, 0);
        rb.bodyType = RigidbodyType2D.Kinematic;
        ani.SetBool("jump_throwing", true);
    }
    public void make_slash()
    {
        slashes_made %= 3;

        if (slashes_made == 2)
        {
            Instantiate(slashes, new Vector2(transform.position.x + (sprite.flipX ? -2f : 3f), transform.position.y + 1f), Quaternion.Euler(0, 0, 0));
        }
        else if (slashes_made == 1)
        {
            Instantiate(slashes, new Vector2(transform.position.x + (sprite.flipX ? -4f : 5f), transform.position.y + 1f), Quaternion.Euler(0, 0, 0));
        }
        else if (slashes_made == 0)
        {
            Instantiate(slashes, new Vector2(transform.position.x + (sprite.flipX ? -6f : 7f), transform.position.y + 1f), Quaternion.Euler(0, 0, 0));
        }
        slashes_made += 1;
    }
    public void make_sword_wave()
    {
        Vector2 throw_position = kunai_throwing_point_landing.transform.position;
        Vector2 player_position = manager.player.transform.position;

        Vector2 ray_to_player = new Vector2(player_position.x - throw_position.x, player_position.y - throw_position.y);
        float calc_rotation = Mathf.Atan2(ray_to_player.y, ray_to_player.x) * Mathf.Rad2Deg;

        float sped = health >= max_health / 2f ? 12 : 15;
        Instantiate(sword_wave, sword_slash_point.transform.position, Quaternion.Euler(0, 0, calc_rotation)).GetComponent<NinjaSwordWaveBehavior>().speed = sped;
    }
    public void make_sword_wave_undirected()
    {
        float angle = sprite.flipX ? 0: 180;

        float sped = health >= max_health / 2f ? 12 : 15;
        Instantiate(sword_wave, sword_slash_point.transform.position, Quaternion.Euler(0, 0, angle)).GetComponent<NinjaSwordWaveBehavior>().speed = sped;
    }
    public void decide_sword_wave_type()
    {
        if (manager.player.transform.position.x >= transform.position.x)
        {
            if (Mathf.Abs(manager.player.transform.position.x - transform.position.x) <= 4f)
            {
                return;
            }
            if (sprite.flipX)
                make_sword_wave();
            else
                make_sword_wave_undirected();
        }
        else if (manager.player.transform.position.x < transform.position.x)
        {
            if (Mathf.Abs(manager.player.transform.position.x - transform.position.x) <= 4f)
                return;

            if (!sprite.flipX)
                make_sword_wave();
            else
                make_sword_wave_undirected();
        }
    }
    public void do_dash_and_slash_attack()
    {
        if (can_sword_dash)
        {
            cross_up_prevention.enabled = true;
            apply_invincibility(1);
            is_attacking = true;
            instant_flip_check();
            ani.SetBool("start_dash", true);
            slash_cooldown = 25f;
            all_actions_cooldown = 4f;
        }
        else
        {
            Debug.Log("too close to wall...");
        }
    }
    public void dash_teleport()
    {
        ani.SetBool("start_dash", false);
        float distance = sprite.flipX ? 4.5f : -4.5f;
        var hi = Instantiate(crouch_slash, new Vector2(transform.position.x + distance, transform.position.y - 1.5f), Quaternion.Euler(0, 0, 0));
        Destroy(hi, 1f);
        hi.GetComponent<SpriteRenderer>().flipX = sprite.flipX;
        transform.position = new Vector2(manager.player.transform.position.x + distance, transform.position.y);
        current_invisible_wall_1 = Instantiate(invisible_wall, new Vector2(manager.player.transform.position.x + (sprite.flipX ? -4.5f : 4.5f), 0), Quaternion.Euler(0, 0, 90));
        //current_invisible_wall_1 = Instantiate(invisible_wall, new Vector2(transform.position.x, 0), Quaternion.Euler(0, 0, 90));
        //transform.position = new Vector2(transform.position.x + (sprite.flipX ? 7f : -7f), transform.position.y);
        Invoke("make_slash", 0.25f);
        Invoke("make_slash", 0.5f);
        Invoke("make_slash", 0.75f);
        current_invisible_wall_2 = Instantiate(invisible_wall, new Vector2(transform.position.x, 0), Quaternion.Euler(0, 0, 90));
        //current_invisible_wall_2 = Instantiate(invisible_wall, new Vector2(transform.position.x, 0), Quaternion.Euler(0, 0, 90));
        ani.SetBool("finish_dash", true);
    }
    public void smoke_follow_up()
    {
        ani.SetBool("throwing_smoke_bomb", false);
        ani.SetBool("start_smoke_dash", true);
    }
    public void do_big_slash()
    {
        is_attacking = true;
        transform.position = new Vector2(manager.player.transform.position.x, -2.5f);
        bool flip_please = Random.Range(0, 2) != 1;
        if (flip_please)
        {
            flip_everything();
            flip_cooldown = 0.5f;
        }
        ani.SetBool("start_smoke_dash", false);
        ani.SetBool("do_big_slash", true);
    }
    public void talk_to_big_slash(int i) // 10 to disable, <10 to enable
    {
        if (i / 10 >= 1)
        {
            if (i % 10 == 0)
            {
                big_slash_attack_bottom.DisableHitbox();
                big_slash_attack_bottom.clearFlags();
                big_slash_attack_bottom.disableAttackCycle = false;
            }
            else if (i % 10 == 1)
            {
                big_slash_attack_top.DisableHitbox();
                big_slash_attack_top.clearFlags();
                big_slash_attack_top.disableAttackCycle = false;
            }
        }

        else if (i % 10 == 0)
        {
            big_slash_attack_bottom.EnableHitbox();
        }
        else if (i % 10 == 1)
        {
            big_slash_attack_top.EnableHitbox();
        }
    }
    public void do_cross_slash()
    {
        cross_slash_attack.EnableHitbox();
    }
    public void produce_caltrops()
    {
        Instantiate(caltrops, new Vector2(transform.position.x + (sprite.flipX ? 2f : -2f), -3.4f), Quaternion.Euler(0, 0, 0));
    }
    public void freeze_me(float f)
    {
        if (freeze_time > 0)
            return;
        else
        {
            end_attack();
            ani.SetBool("got_frozen", true);
            freeze_time = f;
        }
    }
    public void burn_me(float f)
    {
        if (burn_time > 0)
        {
            return;
        }

        burn_time = f;
        current_burn = Instantiate(burn_effect, transform.position, Quaternion.Euler(0, 0, 0)).GetComponent<TestBoss5NinjaBurnEffect>();
        current_burn.Initialize(f, gameObject);
    }
    public void create_tornadoes()
    {
        apply_invincibility(1);
        float first_x = Random.Range(-10f, -5f);
        float second_x = Random.Range(-3f, 3f);
        float third_x = Random.Range(5f, 10f);

        Instantiate(tornado, new Vector2(first_x, -1.1f), Quaternion.Euler(0, 0, 0)).GetComponent<NinjaTornadoBehavior>().manager = manager;
        Instantiate(tornado, new Vector2(second_x, -1.1f), Quaternion.Euler(0, 0, 0)).GetComponent<NinjaTornadoBehavior>().manager = manager;
        Instantiate(tornado, new Vector2(third_x, -1.1f), Quaternion.Euler(0, 0, 0)).GetComponent<NinjaTornadoBehavior>().manager = manager;
    }
    public void check_if_player_close_landing_fail()
    {
        ani.SetBool("failedLanding", false);
        int rng_attack = Random.Range(0, 5); // 2 = landing kick instead
        if (Mathf.Abs(manager.player.transform.position.x - transform.position.x) <= 2f && can_throw_fire_powder && rng_attack != 2)
        {
            instant_flip_check();
            ani.SetBool("using_fire_powder", true);
        }
        else if (Mathf.Abs(manager.player.transform.position.x - transform.position.x) <= 1f)
        {
            ani.SetBool("fallHitPlayer", true);
            all_actions_cooldown = 2f;
        }
        else if (Mathf.Abs(manager.player.transform.position.x - transform.position.x) >= 5f || manager.player_life.movement.dashing)
        {
            ani.SetBool("landing_player_far", true);
            all_actions_cooldown = 2f;
        }
        else
        {
            end_attack();
        }
    }
    public void make_damage_number(int dmg)
    {
        if (manager.current_boss_in_play == 0)
        {
            Instantiate(damage_number, new Vector2(transform.position.x, transform.position.y + 1.5f), Quaternion.Euler(0, 0, 0)).GetComponent<DamageNumbersTestBoss5Behavior>().initialize(dmg);
        }
    }
    public void Hit(PlayerAttack player_attack)
    {
        if (is_dead)
        {
            return;
        }
        if (is_invincible)
        {
            make_damage_number(0);
            player_attack.isBlock();
            return;
        }

        player_attack.isHit();
        if (player_attack.map_wide)
        {
            manager.inflict_area_damage(player_attack.damage / 4);
        }

        if (burn_time > 0)
        {
            player_attack.damage = Mathf.RoundToInt(player_attack.damage * 1.2f);
        }

        if (player_attack.freezeDuration > 0)
        {
            freeze_me(player_attack.freezeDuration);
        }
        if (player_attack.burnDuration > 0)
        {
            burn_me(player_attack.burnDuration);
        }

        TakeDamage(player_attack.damage);

        ani.SetBool("is_hurt", true);
    }
    public void TakeDamage(int damage)
    {
        if (manager.current_boss_in_play == 0)
            make_damage_number(damage);
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            is_dead = true;
        }
    }
    private void FixedUpdate()
    {
        if (ani.GetBool("is_walking"))
        {
            float speed = sprite.flipX ? 3f : -3f;
            rb.linearVelocityX = speed;
            walk_time += Time.deltaTime;
            if (Mathf.Abs(manager.player.transform.position.x - transform.position.x) <= 6f || walk_time > 2f || inability_time > 0f)
            {
                stop_walking();
            }
        }
    }
    public void stop_walking()
    {
        ani.SetBool("is_walking", false);
        rb.linearVelocity = new Vector2(0, 0);
        end_attack();
        walk_time = 0f;
    }
}

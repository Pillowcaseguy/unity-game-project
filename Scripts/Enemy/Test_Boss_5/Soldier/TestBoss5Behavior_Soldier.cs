using UnityEngine;

public class TestBoss5Behavior_Soldier : MonoBehaviour, EnemyDamageable, EnemyMainBodyImplement
{
    public bool is_dead;
    public bool just_died;
    public int max_health;
    public int health;

    public LayerMask ground_layer;
    public Rigidbody2D rb;
    public BoxCollider2D box;
    public Animator ani;
    public SpriteRenderer sprite;
    public LevelManager_TestBoss5 manager;

    public bool called_to_swap;

    public GameObject jumping_down_soldier;
    public Soldier_JumpingDown current_falling_soldier;
    public GameObject helicopter_ladder;
    public SoldierHelicopterBehavior current_helicopter_ladder;

    public bool is_invincible;
    public bool is_attacking;

    public bool is_swappable;

    public bool can_flip;
    public bool can_flip_once;
    public bool can_move;
    public float flip_cooldown;

    public bool touching_wall;
    public bool close_to_wall_left;
    public bool close_to_wall_right;

    public GameObject player_throw_point;

    public GameObject handgun_shoot_point_left;
    public GameObject handgun_shoot_point_right;
    public GameObject handgun_shoot_point_low_left;
    public GameObject handgun_shoot_point_low_right;
    public GameObject shoot_blast;
    public int handgun_ammo;
    public int max_handgun_ammo;

    public GameObject handgun_casing_left;
    public GameObject handgun_casing_right;
    public GameObject handgun_casing_low_left;
    public GameObject handgun_casing_low_right;

    public GameObject bullet_casing;

    public bool player_grabbable;
    public bool player_deadly_grabbable;
    public bool player_crouched_too_early;
    public EnemyAttack charge_attack;
    public bool charge_did_grab;

    public bool onGround;

    public EnemyAttack pistol_whip;
    public BoxCollider2D pistol_whip_hitbox;
    public bool pistol_whip_in_range;

    public float inability_time;
    public float freeze_time;
    public bool frozen_first_time;

    /*public bool debug_drop_soldier;
    public bool debug_send_me_back_up;
    public bool debug_jump_soldier;
    public bool debug_jump_into_air;
    */

    public float grenade_cooldown;
    public float thermite_cooldown;
    public float flashbang_cooldown;
    public float charge_cooldown;
    public float deadly_charge_cooldown;
    public float missile_cooldown;
    public float handgun_shoot_cooldown;
    public float all_actions_cooldown;

    public GameObject fallen_sunglasses;

    public Level5KnockedPlayerBehavior current_knocked_player;
    public SoldierThrowToGroundAttack player_slam_attack;
    public SoldierPunchPlayerAttack player_punch_attack;

    public GameObject bullet;

    public GameObject grenade;
    public GameObject grenade_pin;
    public GameObject missiles;

    public GameObject grenade_throwing_point;
    public GameObject grenade_pin_throwing_point;

    public GameObject thermite;
    public GameObject thermite_throw_point;

    public GameObject flashbang;
    public GameObject flashbang_throwing_point;

    /*public bool debug_throw_thermite;
    public bool debug_throw_grenade;
    public bool debug_throw_flashbang;
    public bool debug_begin_charging;
    public bool debug_begin_deadly_charging;
    public bool debug_begin_shooting;
    public bool debug_melee_attack;
    public bool debug_missile_attack;*/

    public GameObject torso_charge_object;
    public Animator torso_charge_animator;
    public SpriteRenderer torso_charge_sprite;

    public GameObject grab_stun;

    public float reload_hurt_timer;
    public bool hittable_during_reload;

    public GameObject damage_number;
    void Awake()
    {
        ground_layer = LayerMask.GetMask("Ground", "BreakableGround");
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        ani = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        handgun_ammo = 6;
        max_handgun_ammo = 6;
    }
    void Start()
    {
        health = max_health;
    }
    void Update()
    {
        if (!just_died && is_dead)
        {
            /*burn_time = 0;
            if (current_fire)
            {
                Destroy(current_fire);
            }*/
            freeze_time = 0;
            //transform.position = new Vector2(transform.position.x, -2.56f);
            end_attack();
            ani.SetBool("is_dead", is_dead);
            //sprite.enabled = false;
            manager.spawn_knockout_wave();
            manager.number_of_dead_bosses += 1;
            var hi = Instantiate(fallen_sunglasses, new Vector2(transform.position.x, transform.position.y + 0.5f), Quaternion.Euler(0, 0, 0)).GetComponent<Soldier_Sunglasses_Behavior>();
            hi.sprite.flipX = sprite.flipX;
            box.enabled = false;
            just_died = true;
        }

        if (is_dead)
        {
            if (rb.bodyType != RigidbodyType2D.Static)
                rb.bodyType = RigidbodyType2D.Static;
            return;
        }

        float t = Time.deltaTime;

        /*if (debug_drop_soldier)
        {
            summon_falling_soldier();
            debug_drop_soldier = false;
        }
        if (debug_send_me_back_up)
        {
            transform.position = new Vector2(transform.position.x, 12);
            debug_send_me_back_up = false;
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
        if (debug_jump_soldier)
        {
            summon_jumping_soldier();
            debug_jump_soldier = false;
        }
        if (debug_throw_thermite)
        {
            //throw_thermite();
            ani.SetBool("throwing_thermite", true);
            debug_throw_thermite = false;
        }
        if (debug_throw_grenade)
        {
            //pull_pin();
            //throw_grenade();
            ani.SetBool("throwing_grenade", true);
            debug_throw_grenade = false;
        }
        if (debug_throw_flashbang)
        {
            //pull_pin();
            //throw_flashbang();
            ani.SetBool("throwing_flashbang", true);
            debug_throw_flashbang = false;
        }
        if (debug_begin_charging)
        {
            start_charging();
            debug_begin_charging = false;
        }
        if (debug_begin_shooting)
        {
            begin_shooting();
            debug_begin_shooting = false;
        }
        if (debug_begin_deadly_charging)
        {
            start_deadly_charging();
            debug_begin_deadly_charging = false;
        }
        if (debug_melee_attack)
        {
            do_melee_attack();
            debug_melee_attack = false;
        }
        if (debug_missile_attack)
        {
            ani.SetBool("call_missiles", true);
            debug_missile_attack = false;
        }*/

        if (current_falling_soldier != null)
        {
            sprite.enabled = false;
            box.enabled = false;
            apply_invincibility(1);
            rb.bodyType = RigidbodyType2D.Static;
            if (current_falling_soldier.fine_to_destroy)
            {
                if (current_falling_soldier.attack.wasParried)
                {
                    //Debug.Log("parried!");
                    is_attacking = true;
                    ani.SetBool("landing", true);
                    all_actions_cooldown = 4f;
                    ani.SetBool("failedLanding", true);
                }
                else if (current_falling_soldier.attack.wasHit)
                {
                    //Debug.Log("hit!");
                    ani.SetBool("landing", true);
                    is_attacking = true;
                    ani.SetBool("fallHitPlayer", true);
                    all_actions_cooldown = 4f;
                    can_flip_once = true;
                }
                else if (Mathf.Abs(current_falling_soldier.transform.position.x - manager.player.transform.position.x) > 6 || manager.player_life.movement.dash_time > 0f)
                {
                    //Debug.Log("far!");
                    ani.SetBool("landing", true);
                    is_attacking = true;
                    ani.SetBool("landing_player_far", true);
                    all_actions_cooldown = 3f;
                    can_flip_once = true;
                }
                else
                {
                    //Debug.Log("nothing!");
                    is_attacking = true;
                    ani.SetBool("landing", true);
                    all_actions_cooldown = 2f;
                }
                transform.position = current_falling_soldier.transform.position;
                rb.bodyType = RigidbodyType2D.Dynamic;
                box.enabled = true;
                sprite.enabled = true;
                apply_invincibility(0);
                instant_flip_check();
                handgun_ammo = max_handgun_ammo;
                current_falling_soldier.destroy_self();
            }
        }

        if (current_helicopter_ladder != null)
        {
            box.enabled = false;
            apply_invincibility(1);
            if (current_helicopter_ladder.reached_destination)
            {
                sprite.enabled = false;
                current_helicopter_ladder.reached_destination = false;
            }
            if (current_helicopter_ladder.fine_to_destroy)
            {
                ani.SetBool("wind_blowing", false);
                transform.position = new Vector2(0, 15);
                rb.bodyType = RigidbodyType2D.Static;
                current_helicopter_ladder.destroy_self();
            }
        }

        countdown_cooldowns(t);

        is_swappable = Mathf.Abs(rb.linearVelocity.x) <= 0.01f && onGround && !is_attacking && inability_time <= 0 && !is_dead;
        can_move = !is_attacking;
        can_flip = !(is_attacking || inability_time > 0) && !ani.GetBool("wind_blowing");

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

        RaycastHit2D try_wall_left2 = Physics2D.Raycast(new Vector2(origin.x, origin.y - (height / 2f - 0.01f)), Vector2.left, half_width + 1.8f, ground_layer);
        RaycastHit2D try_wall_right2 = Physics2D.Raycast(new Vector2(origin.x, origin.y - (height / 2f - 0.01f)), Vector2.right, half_width + 1.8f, ground_layer);

        close_to_wall_left = try_wall_left2.collider != null;
        close_to_wall_right = try_wall_right2.collider != null;

        flip_check(t);

        RaycastHit2D hit_middle = Physics2D.Raycast(origin, Vector2.down, height / 2f + 0.1f, ground_layer);
        RaycastHit2D hit_left = Physics2D.Raycast(left, Vector2.down, height / 2f + 0.1f, ground_layer);
        RaycastHit2D hit_right = Physics2D.Raycast(right, Vector2.down, height / 2f + 0.1f, ground_layer);

        onGround = hit_middle.collider != null || hit_left.collider != null || hit_right.collider != null;

        ani.SetBool("is_attacking", is_attacking);
        ani.SetBool("onGround", onGround);
        ani.SetBool("reload_hittable", hittable_during_reload && reload_hurt_timer <= 0);

        if (freeze_time > 0)
        {
            freeze_time -= Time.deltaTime;
            if (frozen_first_time && freeze_time <= 1.8f)
            {
                frozen_first_time = false;
                ani.SetBool("got_frozen", false);
            }
            if (freeze_time < 0f)
            {
                freeze_time = 0f;
            }
        }

        inability_time = freeze_time;

        if (torso_charge_object.activeSelf)
        {
            torso_charge_sprite.flipX = sprite.flipX;
        }

        if (manager.current_boss_in_play == 1 && manager.player_life.player_died && sprite.enabled)
        {
            is_attacking = true;
            ani.SetBool("killed_player", true);
        }

        else if (!is_attacking && called_to_swap && is_swappable && inability_time <= 0 && all_actions_cooldown <= 0 && onGround && current_falling_soldier == null && current_helicopter_ladder == null)
        {
            all_actions_cooldown = 10f;
            summon_jumping_soldier();
        }

        else if (!is_attacking && !called_to_swap && inability_time <= 0 && all_actions_cooldown <= 0 && onGround && current_falling_soldier == null && current_helicopter_ladder == null)
        {
            bool action_selected = false;
            if (!action_selected && handgun_shoot_cooldown <= 0)
            {
                instant_flip_check();
                is_attacking = true;
                action_selected = true;
                handgun_shoot_cooldown = 20f;
                all_actions_cooldown = 5f;
                begin_shooting();
            }
            if (health <= max_health / 2f && !action_selected && missile_cooldown <= 0)
            {
                instant_flip_check();
                is_attacking = true;
                action_selected = true;
                ani.SetBool("call_missiles", true);
                missile_cooldown = 20f;
                all_actions_cooldown = 4f;
            }
            if (!action_selected && thermite_cooldown <= 0)
            {
                float small_rng = Random.value;
                if (!(small_rng >= 0.5f && small_rng <= 0.75f)) // 26% chance to bypass attack
                {
                    instant_flip_check();
                    is_attacking = true;
                    action_selected = true;
                    ani.SetBool("throwing_thermite", true);
                    thermite_cooldown = 25f;
                    all_actions_cooldown = 2f;
                }
            }
            if (!action_selected && charge_cooldown <= 0)
            {
                float small_rng = Random.value;
                if (!(small_rng >= 0.5f && small_rng <= 0.64f)) // 15% chance to bypass attack
                {
                    instant_flip_check();
                    if ((!sprite.flipX && close_to_wall_left) || (sprite.flipX && close_to_wall_right))
                    {

                    }
                    else
                    {
                        is_attacking = true;
                        action_selected = true;
                        charge_cooldown = 25f;
                        all_actions_cooldown = 2f;
                        start_charging();
                    }
                }
            }
            if (health <= max_health * 0.75f && !action_selected && deadly_charge_cooldown <= 0)
            {
                float small_rng = Random.value;
                if (!(small_rng >= 0.5f && small_rng <= 0.64f)) // 15% chance to bypass attack
                {
                    instant_flip_check();
                    is_attacking = true;
                    action_selected = true;
                    deadly_charge_cooldown = 22f;
                    all_actions_cooldown = 2f;
                    start_deadly_charging();
                }
            }
            if (!action_selected && grenade_cooldown <= 0)
            {
                float small_rng = Random.value;
                if (!(small_rng >= 0.5f && small_rng <= 0.75f)) // 26% chance to bypass attack
                {
                    instant_flip_check();
                    is_attacking = true;
                    action_selected = true;
                    ani.SetBool("throwing_grenade", true);
                    grenade_cooldown = 10f;
                    all_actions_cooldown = 2f;
                }
            }
            if (!action_selected && flashbang_cooldown <= 0)
            {
                instant_flip_check();
                is_attacking = true;
                action_selected = true;
                ani.SetBool("throwing_flashbang", true);
                flashbang_cooldown = 15f;
                all_actions_cooldown = 2f;
            }
            if (!action_selected)
                instant_flip_check();
            if (!action_selected && (Mathf.Abs(manager.player.transform.position.x - transform.position.x) <= 2f || pistol_whip_in_range))
            {
                float small_rng = Random.value;
                instant_flip_check();
                is_attacking = true;
                action_selected = true;
                all_actions_cooldown = 2f;
                do_melee_attack();
            }
        }
    }
    public void summon_falling_soldier()
    {
        current_falling_soldier = Instantiate(jumping_down_soldier, transform.position, Quaternion.Euler(0, 0, 0)).GetComponent<Soldier_JumpingDown>();
        current_falling_soldier.rb.linearVelocityY = -20;
        current_falling_soldier.manager = manager;
    }
    public void summon_jumping_soldier()
    {
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.linearVelocity = Vector2.zero;
        ani.SetBool("wind_blowing", true);
        float spawn_x = sprite.flipX ? 21.9f : -21.9f;
        current_helicopter_ladder = Instantiate(helicopter_ladder, new Vector2(spawn_x, 4.2f), Quaternion.Euler(0, 0, 0)).GetComponent<SoldierHelicopterBehavior>();
        current_helicopter_ladder.boss = this;
        current_helicopter_ladder.sprite.flipX = sprite.flipX;
        current_helicopter_ladder.ladder_sprite.flipX = sprite.flipX;
        current_helicopter_ladder.target_x = transform.position.x;
        current_helicopter_ladder.traveling_left = sprite.flipX;
        current_helicopter_ladder.rb.linearVelocityX = sprite.flipX ? -12 : 12;

        called_to_swap = false;
        manager.complete_handshake();
    }
    public void hide_player()
    {
        manager.hide_player(true);
    }
    public void grab_player()
    {
        manager.player_life.movement.rb.linearVelocity = new Vector2(0, 0);
        manager.player_life.movement.rb.bodyType = RigidbodyType2D.Kinematic;
        manager.player.transform.position = player_throw_point.transform.position;
    }
    public void summon_knocked_player()
    {
        current_knocked_player = Instantiate(manager.knocked_player,
            new Vector2(player_throw_point.transform.position.x, player_throw_point.transform.position.y), Quaternion.Euler(0, 0, 0)).GetComponent<Level5KnockedPlayerBehavior>();
        bool player_behind_hit = manager.player.transform.position.x <= transform.position.x;
        current_knocked_player.do_not_get_up = true;
        current_knocked_player.rb.linearVelocityX = player_behind_hit ? -5f : 5f;
        current_knocked_player.rb.linearVelocityY = -10f;
        current_knocked_player.sprite.flipX = player_behind_hit;
    }
    public void slam_player_scan()
    {
        player_slam_attack.scan_for_player = true;
    }
    public void punch_player()
    {
        player_punch_attack.attack.stunDuration = 3;
        if (current_knocked_player != null)
        {
            current_knocked_player.ani.SetBool("player_got_punched", true);
            player_punch_attack.attack_player();
            if (!manager.player_life.player_died)
            {
                current_knocked_player.ani.SetBool("player_is_dead", false);
            }
        }
    }
    public void stab_player()
    {
        player_punch_attack.attack.stunDuration = 4;
        if (current_knocked_player != null)
        {
            current_knocked_player.ani.SetBool("player_got_punched", true);
            player_punch_attack.attack_player();
            for (int i = 0; i < 25; i++)
            {
                var particle = Instantiate(manager.hit_particle, manager.player.transform.position, Quaternion.Euler(0, 0, 0)).GetComponent<SpriteRenderer>();
                particle.color = Color.white;
            }
            if (!manager.player_life.player_died)
            {
                current_knocked_player.ani.SetBool("player_is_dead", false);
            }
        }
    }
    public void empty_stun()
    {
        manager.player_life.Hit_No_Collider(grab_stun);
    }
    public void countdown_cooldowns(float t)
    {
        grenade_cooldown -= t;
        if (grenade_cooldown < 0)
        {
            grenade_cooldown = 0;
        }

        thermite_cooldown -= t;
        if (thermite_cooldown < 0)
        {
            thermite_cooldown = 0;
        }

        flashbang_cooldown -= t;
        if (flashbang_cooldown < 0)
        { 
            flashbang_cooldown = 0;
        }

        charge_cooldown -= t;
        if (charge_cooldown < 0)
        {
            charge_cooldown = 0;
        }

        deadly_charge_cooldown -= t;
        if (deadly_charge_cooldown < 0)
        {
            deadly_charge_cooldown = 0;
        }

        missile_cooldown -= t;
        if (missile_cooldown < 0)
        {
            missile_cooldown = 0;
        }

        handgun_shoot_cooldown -= t;
        if (handgun_shoot_cooldown < 0)
        {
            handgun_shoot_cooldown = 0;
        }

        all_actions_cooldown -= t;
        if (all_actions_cooldown < 0)
        {
            all_actions_cooldown = 0;
        }

        reload_hurt_timer -= t;
        if (reload_hurt_timer < 0)
        {
            reload_hurt_timer = 0;
        }
    }
    public void finish_landing()
    {
        bool is_punching = ani.GetBool("fallHitPlayer");
        end_attack();
        if (is_punching)
        {
            is_attacking = true;
            ani.SetBool("fallHitPlayer", true);
        }
    }
    public void throw_random()
    {
        int random_value = Random.Range(0, 3);

        if (random_value == 0)
        {
            thermite_cooldown = 25f;
            ani.SetBool("throwing_thermite", true);
        }
        else if (random_value == 1)
        {
            grenade_cooldown = 10f;
            ani.SetBool("throwing_grenade", true);
        }
        else if (random_value == 2)
        {
            flashbang_cooldown = 15f;
            ani.SetBool("throwing_flashbang", true);
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

        player_punch_attack.attack.clearFlags();
        player_punch_attack.attack.disableAttackCycle = false;

        player_slam_attack.attack.clearFlags();
        player_slam_attack.attack.disableAttackCycle = false;

        ani.SetBool("is_charging", false);
        ani.SetBool("is_deadly_charging", false);
        ani.SetBool("start_charging", false);

        disable_torso();

        ani.SetBool("slowing_down", false);

        ani.SetBool("is_shooting_any", false);
        ani.SetBool("is_shooting_low", false);
        ani.SetBool("is_shooting_high", false);

        ani.SetBool("is_reloading", false);

        ani.SetBool("melee_attack", false);

        pistol_whip.clearFlags();
        pistol_whip.disableAttackCycle = false;
        pistol_whip.DisableHitbox();

        rb.linearVelocityX = 0;

        if (ani.GetBool("caught_player"))
            change_in_time = 5f;
        ani.SetBool("caught_player", false);
        ani.SetBool("caught_player_knife", false);

        frozen_first_time = true;

        if (ani.GetBool("throwing_thermite"))
            change_in_time = 5f;
        ani.SetBool("throwing_thermite", false);

        if (ani.GetBool("throwing_grenade"))
            change_in_time = 3f;
        ani.SetBool("throwing_grenade", false);

        ani.SetBool("throwing_flashbang", false);

        if (ani.GetBool("call_missiles"))
            change_in_time = 3f;
        ani.SetBool("call_missiles", false);

        apply_invincibility(0);
        if (change_in_time == 0f)
        {
            all_actions_cooldown = 1.5f;
        }
        else
        {
            all_actions_cooldown = change_in_time;
        }
        player_crouched_too_early = false;
        rb.bodyType = RigidbodyType2D.Dynamic;
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
    public void unhurt()
    {
        ani.SetBool("is_hurt", false);
    }
    public void unblock()
    {
        ani.SetBool("blocking_stun", false);
    }
    public void flip_everything()
    {
        sprite.flipX = !sprite.flipX;
        player_throw_point.transform.localPosition = new Vector2(-player_throw_point.transform.localPosition.x, player_throw_point.transform.localPosition.y);
        grenade_throwing_point.transform.localPosition = new Vector2(-grenade_throwing_point.transform.localPosition.x, grenade_throwing_point.transform.localPosition.y);
        grenade_pin_throwing_point.transform.localPosition = new Vector2(-grenade_pin_throwing_point.transform.localPosition.x, grenade_pin_throwing_point.transform.localPosition.y);
        thermite_throw_point.transform.localPosition = new Vector2(-thermite_throw_point.transform.localPosition.x, thermite_throw_point.transform.localPosition.y);
        flashbang_throwing_point.transform.localPosition = new Vector2(-flashbang_throwing_point.transform.localPosition.x, flashbang_throwing_point.transform.localPosition.y);
        handgun_shoot_point_left.transform.localPosition = new Vector2(-handgun_shoot_point_left.transform.localPosition.x, handgun_shoot_point_left.transform.localPosition.y);
        handgun_shoot_point_low_left.transform.localPosition = new Vector2(-handgun_shoot_point_low_left.transform.localPosition.x, handgun_shoot_point_low_left.transform.localPosition.y);
        handgun_shoot_point_right.transform.localPosition = new Vector2(-handgun_shoot_point_right.transform.localPosition.x, handgun_shoot_point_right.transform.localPosition.y);
        handgun_shoot_point_low_right.transform.localPosition = new Vector2(-handgun_shoot_point_low_right.transform.localPosition.x, handgun_shoot_point_low_right.transform.localPosition.y);
        handgun_casing_left.transform.localPosition = new Vector2(-handgun_casing_left.transform.localPosition.x, handgun_casing_left.transform.localPosition.y);
        handgun_casing_low_left.transform.localPosition = new Vector2(-handgun_casing_low_left.transform.localPosition.x, handgun_casing_low_left.transform.localPosition.y);
        handgun_casing_right.transform.localPosition = new Vector2(-handgun_casing_right.transform.localPosition.x, handgun_casing_right.transform.localPosition.y);
        handgun_casing_low_right.transform.localPosition = new Vector2(-handgun_casing_low_right.transform.localPosition.x, handgun_casing_low_right.transform.localPosition.y);
    }
    public void apply_invincibility(int i)
    {
        is_invincible = !(i == 0);
    }
    public void throw_thermite()
    {
        Vector2 throw_position = thermite_throw_point.transform.position;
        Vector2 player_position = manager.player.transform.position;

        Vector2 ray_to_player = new Vector2(player_position.x - throw_position.x, player_position.y - throw_position.y);
        float calc_rotation = Mathf.Atan2(ray_to_player.y, ray_to_player.x);

        var hi = Instantiate(thermite, thermite_throw_point.transform.position, Quaternion.Euler(0, 0, calc_rotation)).GetComponent<SoldierMolotovBehavior>();
        hi.manager = manager;
        float speed = 5;
        hi.rb.linearVelocityX = Mathf.Cos(calc_rotation) * speed * 1.5f;
        if (Mathf.Sin(calc_rotation) * speed <= 0)
        {
            hi.rb.linearVelocityY = 1.5f;
        }
        else
        {
            hi.rb.linearVelocityY = Mathf.Sin(calc_rotation) * speed * 4;
        }
    }
    public void pull_pin()
    {
        float starting_x = sprite.flipX ? -200 : 200;

        var hi = Instantiate(grenade_pin, grenade_pin_throwing_point.transform.position, Quaternion.Euler(0, 0, 0)).GetComponent<SoldierGrenadePinBehavior>();
        hi.rb.AddForce(new Vector2(starting_x, 200));
    }
    public void throw_grenade()
    {
        Vector2 throw_position = thermite_throw_point.transform.position;
        Vector2 player_position = manager.player.transform.position;

        Vector2 ray_to_player = new Vector2(player_position.x - throw_position.x, player_position.y - throw_position.y);
        float calc_rotation = Mathf.Atan2(ray_to_player.y, ray_to_player.x);

        var hi = Instantiate(grenade, grenade_throwing_point.transform.position, Quaternion.Euler(0, 0, 0)).GetComponent<SoldierGrenadeBehavior>();

        float speed = 10;
        hi.rb.linearVelocityX = Mathf.Cos(calc_rotation) * speed;

        if (Mathf.Sin(calc_rotation) * speed <= 2)
        {
            hi.rb.linearVelocityY = 2;
        }
        else
        {
            hi.rb.linearVelocityY = Mathf.Sin(calc_rotation) * 8f;
        }
    }
    public void throw_flashbang()
    {
        var hi = Instantiate(flashbang, flashbang_throwing_point.transform.position, Quaternion.Euler(0, 0, 0)).GetComponent<SoldierFlashbangBehavior>();
        hi.manager = manager;
        if (manager.player.transform.position.x >= transform.position.x)
        {
            hi.rb.AddForce(new Vector2(200, 150));
        }
        else
        {
            hi.rb.AddForce(new Vector2(-200, 150));
        }
    }
    public void start_charging()
    {
        is_attacking = true;
        ani.SetBool("start_charging", true);
        ani.SetBool("is_charging", true);
    }
    public void start_deadly_charging()
    {
        is_attacking = true;
        ani.SetBool("start_charging", true);
        ani.SetBool("is_deadly_charging", true);
    }
    public void enable_torso()
    {
        ani.SetBool("start_charging", false);
        torso_charge_object.SetActive(true);
        if (ani.GetBool("is_charging"))
        {
            torso_charge_animator.SetBool("is_charging", true);
        }
        if (ani.GetBool("is_deadly_charging"))
        {
            torso_charge_animator.SetBool("is_deadly_charging", true);
        }
    }
    public void disable_torso()
    {
        if (torso_charge_object.activeSelf)
        {
            torso_charge_animator.SetBool("is_charging", false);
            torso_charge_animator.SetBool("is_deadly_charging", false);
            torso_charge_animator.SetBool("knife_deflecting", false);
            torso_charge_object.SetActive(false);
        }
    }
    public void begin_shooting()
    {
        ani.SetBool("is_shooting_any", true);
        is_attacking = true;

        int mode = Random.Range(0, 8);
        //Debug.Log("shoot mode: " + mode);
        if (mode == 0 || mode == 6) // Shoot high, low, high
        {

        }
        else if (mode == 1 || mode == 7) // Shoot low, high, low
        {

        }
        else if (mode == 2) // Shoot high, low
        {

        }
        else if (mode == 3) // Shoot low, high
        {

        }
        else if (mode == 4) // Shoot high
        {

        }
        else if (mode == 5) // Shoot low
        {

        }

        ani.SetInteger("shoot_RNG", mode);
    }
    public void shoot_high(int type = 0)
    {
        if (type == 0)
        {
            if (handgun_ammo > 0)
            {
                handgun_ammo -= 1;
                Instantiate(shoot_blast, handgun_shoot_point_left.transform.position, Quaternion.Euler(0, 0, 0)).GetComponent<BlastExist>().sprite.flipX = sprite.flipX;
                Instantiate(bullet_casing, handgun_shoot_point_left.transform.position, Quaternion.Euler(0, 0, 0)).GetComponent<SoldierShellCasingBehavior>().fly_left = !sprite.flipX;
                if (!manager.player_life.movement.crouching)
                {
                    Vector2 throw_position = handgun_shoot_point_left.transform.position;
                    Vector2 player_position = manager.player.transform.position;

                    Vector2 ray_to_player = new Vector2(player_position.x - throw_position.x, player_position.y - throw_position.y);
                    float calc_rotation = Mathf.Atan2(ray_to_player.y, ray_to_player.x) * Mathf.Rad2Deg;

                    Instantiate(bullet, handgun_shoot_point_left.transform.position, Quaternion.Euler(0, 0, calc_rotation)).GetComponent<SoldierBulletBehavior>().Initialize(25, 5, true, manager, transform.position.x);
                }
                else
                {
                    Instantiate(bullet, handgun_shoot_point_left.transform.position, Quaternion.Euler(0, 0, sprite.flipX ? 0 : 180)).GetComponent<SoldierBulletBehavior>().Initialize(25, 5, true, manager, transform.position.x);
                }
            }
        }
        else if (type == 1)
        {
            if (handgun_ammo > 0)
            {
                handgun_ammo -= 1;
                Instantiate(shoot_blast, handgun_shoot_point_right.transform.position, Quaternion.Euler(0, 0, 0)).GetComponent<BlastExist>().sprite.flipX = sprite.flipX;
                Instantiate(bullet_casing, handgun_shoot_point_right.transform.position, Quaternion.Euler(0, 0, 0)).GetComponent<SoldierShellCasingBehavior>().fly_left = !sprite.flipX;
                if (!manager.player_life.movement.crouching)
                {
                    Vector2 throw_position = handgun_shoot_point_right.transform.position;
                    Vector2 player_position = manager.player.transform.position;

                    Vector2 ray_to_player = new Vector2(player_position.x - throw_position.x, player_position.y - throw_position.y);
                    float calc_rotation = Mathf.Atan2(ray_to_player.y, ray_to_player.x) * Mathf.Rad2Deg;

                    Instantiate(bullet, handgun_shoot_point_right.transform.position, Quaternion.Euler(0, 0, calc_rotation)).GetComponent<SoldierBulletBehavior>().Initialize(25, 5, true, manager, transform.position.x);
                }
                else
                {
                    Instantiate(bullet, handgun_shoot_point_right.transform.position, Quaternion.Euler(0, 0, sprite.flipX ? 0 : 180)).GetComponent<SoldierBulletBehavior>().Initialize(25, 5, true, manager, transform.position.x);
                }
            }
        }
        else
        {
            return;
        }
        ani.SetBool("is_shooting_high", true);
        ani.SetBool("is_shooting_low", false);
    }
    public void shoot_low(int type = 0)
    {
        if (type == 0)
        {
            if (handgun_ammo > 0)
            {
                handgun_ammo -= 1;
                Instantiate(shoot_blast, handgun_shoot_point_low_right.transform.position, Quaternion.Euler(0, 0, 0)).GetComponent<BlastExist>().sprite.flipX = sprite.flipX;
                Instantiate(bullet_casing, handgun_casing_low_right.transform.position, Quaternion.Euler(0, 0, 0)).GetComponent<SoldierShellCasingBehavior>().fly_left = !sprite.flipX;
                if (manager.player_life.movement.onGround)
                {
                    Vector2 throw_position = handgun_shoot_point_low_right.transform.position;
                    Vector2 player_position = manager.player.transform.position;

                    Vector2 ray_to_player = new Vector2(player_position.x - throw_position.x, player_position.y - throw_position.y);
                    float calc_rotation = Mathf.Atan2(ray_to_player.y, ray_to_player.x) * Mathf.Rad2Deg;

                    Instantiate(bullet, handgun_shoot_point_low_right.transform.position, Quaternion.Euler(0, 0, calc_rotation)).GetComponent<SoldierBulletBehavior>().Initialize(20, 5, false, manager, transform.position.x);
                }
                else
                {
                    Instantiate(bullet, handgun_shoot_point_low_right.transform.position, Quaternion.Euler(0, 0, sprite.flipX ? 0 : 180)).GetComponent<SoldierBulletBehavior>().Initialize(20, 5, false, manager, transform.position.x);
                }
            }
        }
        else if (type == 1)
        {
            if (handgun_ammo > 0)
            {
                handgun_ammo -= 1;
                Instantiate(shoot_blast, handgun_shoot_point_low_left.transform.position, Quaternion.Euler(0, 0, 0)).GetComponent<BlastExist>().sprite.flipX = sprite.flipX;
                Instantiate(bullet_casing, handgun_casing_low_left.transform.position, Quaternion.Euler(0, 0, 0)).GetComponent<SoldierShellCasingBehavior>().fly_left = !sprite.flipX;
                if (manager.player_life.movement.onGround)
                {
                    Vector2 throw_position = handgun_shoot_point_low_left.transform.position;
                    Vector2 player_position = manager.player.transform.position;

                    Vector2 ray_to_player = new Vector2(player_position.x - throw_position.x, player_position.y - throw_position.y);
                    float calc_rotation = Mathf.Atan2(ray_to_player.y, ray_to_player.x) * Mathf.Rad2Deg;

                    Instantiate(bullet, handgun_shoot_point_low_left.transform.position, Quaternion.Euler(0, 0, calc_rotation)).GetComponent<SoldierBulletBehavior>().Initialize(20, 5, false, manager, transform.position.x);
                }
                else
                {
                    Instantiate(bullet, handgun_shoot_point_low_left.transform.position, Quaternion.Euler(0, 0, sprite.flipX ? 0 : 180)).GetComponent<SoldierBulletBehavior>().Initialize(20, 5, false, manager, transform.position.x);
                }
            }
        }
        else
        {
            return;
        }
        ani.SetBool("is_shooting_low", true);
        ani.SetBool("is_shooting_high", false);
    }
    public void check_reload()
    {
        if (handgun_ammo <= 0)
        {
            ani.SetBool("is_reloading", true);
            ani.SetBool("is_shooting_any", false);
        }
    }
    public void complete_reload()
    {
        handgun_ammo = max_handgun_ammo;
    }
    public void set_reload_blocking(int i)
    {
        hittable_during_reload = !(i == 0);
    }
    public void do_melee_attack()
    {
        ani.SetBool("melee_attack", true);
    }
    public void activate_melee_hitbox(int activate)
    {
        if (activate == 1)
        {
            pistol_whip.EnableHitbox();
        }
        else
        {
            pistol_whip.clearFlags();
            pistol_whip.disableAttackCycle = false;
            pistol_whip.DisableHitbox();
        }
    }
    public void walk_fade_out()
    {
        sprite.color = new Vector4(sprite.color.r,sprite.color.g,sprite.color.b,sprite.color.a - 0.34f);
    }
    public void freeze_me(float f)
    {
        if (freeze_time > 0)
            return;
        else
        {
            end_attack();
            ani.SetBool("got_frozen", true);
            freeze_time = f - 1f;
        }
    }
    public void call_missiles()
    {
        var m1 = Instantiate(missiles, new Vector2(manager.player.transform.position.x, -3.5f), Quaternion.Euler(0, 0, 320)).GetComponent<SoldierMissileBehavior>();
        m1.manager = manager;
        m1.speed = 25;
        m1.rotation = 320;
        m1.transform.position = m1.transform.position + m1.transform.up * 20;

        var m2 = Instantiate(missiles, new Vector2(manager.player.transform.position.x, -3.5f), Quaternion.Euler(0, 0, 40)).GetComponent<SoldierMissileBehavior>();
        m2.manager = manager;
        m2.speed = 25;
        m2.rotation = 40;
        m2.transform.position = m2.transform.position + m2.transform.up * 20;

        var m3 = Instantiate(missiles, new Vector2(manager.player.transform.position.x, -3.5f), Quaternion.Euler(0, 0, 300)).GetComponent<SoldierMissileBehavior>();
        m3.manager = manager;
        m3.speed = 25;
        m3.rotation = 300;
        m3.transform.position = m3.transform.position + m3.transform.up * 20;

        var m4 = Instantiate(missiles, new Vector2(manager.player.transform.position.x, -3.5f), Quaternion.Euler(0, 0, 60)).GetComponent<SoldierMissileBehavior>();
        m4.manager = manager;
        m4.speed = 25;
        m4.rotation = 60;
        m4.transform.position = m4.transform.position + m4.transform.up * 20;

        var m5 = Instantiate(missiles, new Vector2(manager.player.transform.position.x + 5f, -3.5f), Quaternion.Euler(0, 0, 310)).GetComponent<SoldierMissileBehavior>();
        m5.manager = manager;
        m5.speed = 25;
        m5.rotation = 310;
        m5.transform.position = m5.transform.position + m5.transform.up * 20;

        var m6 = Instantiate(missiles, new Vector2(manager.player.transform.position.x - 5f, -3.5f), Quaternion.Euler(0, 0, 50)).GetComponent<SoldierMissileBehavior>();
        m6.manager = manager;
        m6.speed = 25;
        m6.rotation = 50;
        m6.transform.position = m6.transform.position + m6.transform.up * 20;
    }
    public void check_first_charge()
    {
        if (!manager.first_low_charge)
        {
            manager.first_low_charge = true;
            var hi = Instantiate(manager.Burn_Tip_Text).GetComponent<Level5KnockoutText>();
            hi.text.text = "Crouch at the right time to avoid the charge!";
            hi.rect.position = new Vector2(-4.5f, 3f);
        }
    }
    public void check_first_deadly_charge()
    {
        if (!manager.first_high_charge)
        {
            manager.first_high_charge = true;
            var hi = Instantiate(manager.Burn_Tip_Text).GetComponent<Level5KnockoutText>();
            hi.text.text = "Jump to avoid the charge!";
            hi.rect.position = new Vector2(-4.5f, 3f);
        }
    }
    public void FixedUpdate()
    {
        if ((ani.GetBool("is_charging") && Mathf.Abs(transform.position.x - manager.player.transform.position.x) > 4 || ani.GetBool("start_charging")) && manager.player_life.movement.crouching)
        {
            player_crouched_too_early = true;
        }
        if ((ani.GetBool("is_charging") || ani.GetBool("is_deadly_charging")) && !ani.GetBool("start_charging"))
        {
            if (!close_to_wall_left && !sprite.flipX)
            {
                if (ani.GetBool("is_charging"))
                    rb.linearVelocityX = -10;
                else if (ani.GetBool("is_deadly_charging"))
                    rb.linearVelocityX = -14;
            }
            else if (!close_to_wall_right && sprite.flipX)
            {
                if (ani.GetBool("is_charging"))
                    rb.linearVelocityX = 10;
                else if (ani.GetBool("is_deadly_charging"))
                    rb.linearVelocityX = 14;
            }
            else
            {
                ani.SetBool("slowing_down", true);
                torso_charge_animator.SetBool("slowing_down", true);
                if (ani.GetBool("is_charging"))
                    rb.linearVelocityX /= 1.05f;
                else if (ani.GetBool("is_deadly_charging"))
                    rb.linearVelocityX /= 1.10f;
            }

            if (ani.GetBool("is_charging") && player_grabbable && manager.player_life.trueInvincibilityTime <= 0)
            {
                rb.linearVelocityX = 0;
                ani.SetBool("is_charging", false);
                ani.SetBool("caught_player_knife", true);
            }

            else if (ani.GetBool("is_deadly_charging") && player_deadly_grabbable && manager.player_life.trueInvincibilityTime <= 0)
            {
                rb.linearVelocityX = 0;
                ani.SetBool("is_deadly_charging", false);
                ani.SetBool("caught_player", true);
            }
        }

        if (ani.GetBool("slowing_down") && (Mathf.Abs(rb.linearVelocityX) < 0.01f || touching_wall))
        {
            end_attack();
        }

        if (pistol_whip.gameObject.layer == pistol_whip.active_layer)
        {
            if (!sprite.flipX)
            {
                rb.linearVelocityX = -4;
            }
            else
            {
                rb.linearVelocityX = 4;
            }
        }

        if (ani.GetBool("is_reloading") && Mathf.Abs(transform.position.x - manager.player.transform.position.x) < 2.5f && !manager.player_life.movement.crouching)
        {
            if (!ani.GetBool("melee_attack"))
            {
                do_melee_attack();
            }
        }
        if (ani.GetBool("is_shooting_any") && Mathf.Abs(transform.position.x - manager.player.transform.position.x) < 2.5f && !manager.player_life.movement.crouching)
        {
            ani.SetBool("is_shooting_any", false);
            do_melee_attack();
        }
    }
    public void make_damage_number(int dmg)
    {
        if (manager.current_boss_in_play == 1)
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
            if (player_attack.ignoresInvincibility && ani.GetBool("is_reloading"))
            {

            }
            else
            {
                if (reload_hurt_timer <= 0 && ani.GetBool("is_reloading"))
                {
                    ani.SetBool("is_hurt", true);
                }
                make_damage_number(0);
                player_attack.isBlock();
                return;
            }
        }
        if (ani.GetBool("is_charging") && !ani.GetBool("slowing_down") && !player_attack.ignoresInvincibility)
        {
            player_attack.isBlock();
            if (!sprite.flipX && transform.position.x > manager.player.transform.position.x || sprite.flipX && transform.position.x < manager.player.transform.position.x)
            {
                torso_charge_animator.SetBool("knife_deflecting", true);
            }
            make_damage_number(0);
            return;
        }

        player_attack.isHit();
        if (player_attack.map_wide)
        {
            manager.inflict_area_damage(player_attack.damage / 4);
        }

        if (player_attack.freezeDuration > 0)
        {
            freeze_me(player_attack.freezeDuration);
        }

        if (player_attack.stunDuration > 0)
        {
            ani.SetBool("blocking_stun", true);
            end_attack();
        }

        TakeDamage(player_attack.damage);

        if (!ani.GetBool("blocking_stun"))
        {
            ani.SetBool("is_hurt", true);
        }

        if (ani.GetBool("is_reloading") && reload_hurt_timer <= 0)
        {
            reload_hurt_timer = 0.6f;
        }
    }

    public void TakeDamage(int damage)
    {
        if (manager.current_boss_in_play == 1)
            make_damage_number(damage);
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            is_dead = true;
        }
    }
}

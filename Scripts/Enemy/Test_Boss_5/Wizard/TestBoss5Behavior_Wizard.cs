using UnityEngine;
public class TestBoss5Behavior_Wizard : MonoBehaviour, EnemyDamageable, EnemyMainBodyImplement
{
    public bool is_dead;
    public bool just_died;
    public int max_health;
    public int health;

    public LayerMask ground_layer;
    public LayerMask both_ground_layer;
    public LayerMask player_layer;
    public Rigidbody2D rb;
    public BoxCollider2D box;
    public Animator ani;
    public SpriteRenderer sprite;
    public LevelManager_TestBoss5 manager;

    public bool called_to_swap;
    public bool coming_in;

    public bool is_invincible;
    public bool is_attacking;

    public bool is_swappable;
    public bool is_swapping;

    public bool can_flip;
    public bool can_flip_once;
    public bool can_move;
    public float flip_cooldown;

    public float inability_time;
    public float stun_time;
    public bool stunned_first_time;
    public float blind_time;
    public bool blinded_first_time;

    public bool onGround;

    public EnemyAttack telefrag_attack;
    public bool summoned_knocked_player_first_time;
    public EnemyAttack touch_damage;

    public SpriteRenderer aura_sprite;
    public Animator aura_ani;

    public float dash_cooldown;
    public float ice_storm_cooldown;
    public float curse_cooldown;
    public float fireball_cooldown;
    public float fly_cooldown;
    public float light_cooldown;
    public float earth_cooldown;
    public float stab_cooldown;
    public float all_actions_cooldown;

    public GameObject dash_particle;
    public GameObject dash_hit_effect;
    public EnemyAttack top_dash;
    public bool top_dash_hit_first;
    public EnemyAttack bottom_dash;
    public bool bottom_dash_hit_first;
    public EnemyAttack third_dash;
    public bool third_dash_hit_first;

    public GameObject ice_crystal;
    public GameObject ice_platform;
    public TestBoss5WizardIcePlatformBehavior current_ice_platform;
    public GameObject ice_background;
    public TestBoss5WizardIceBackgroundBehavior current_ice_background;

    public GameObject curse_hit;

    public GameObject fireball;
    public GameObject fireball_shoot_point;

    public bool dived;
    public GameObject ground_break;
    public GameObject crack;
    public GameObject crack_point;

    public GameObject light_cross;
    public GameObject light_sword;

    public GameObject rock;
    public GameObject rock_shoot_point;

    public GameObject portal;
    public GameObject current_portal;
    public WizardPortalBehavior current_portal_logic;

    public GameObject dagger_throw_point;
    public GameObject dagger;
    public GameObject broken_dagger;

    public Level5KnockedPlayerBehavior current_knocked_player;

    public GameObject wizard_hat;
    public GameObject wizard_hat_point;

    public GameObject stun_effect;
    public TestBoss5_WizardStunEffect current_stun_effect;

    public bool debug_dash;
    public bool debug_ice_storm;
    public bool debug_curse;
    public bool debug_fireball;
    public bool debug_fly;
    public bool debug_light;
    public bool debug_earth;
    public bool debug_stab;
    public bool debug_end_attack;

    public GameObject damage_number;
    public GameObject firework;
    void Awake()
    {
        ground_layer = LayerMask.GetMask("Ground");
        both_ground_layer = LayerMask.GetMask("Ground", "BreakableGround");
        player_layer = LayerMask.GetMask("Player_Damage");
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        ani = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        sprite.color = new Vector4(1, 1, 1, 0);

        stunned_first_time = true;
        blinded_first_time = true;
    }
    void Start()
    {
        health = max_health; // I intend 2400 but he needs rebalancing
    }

    // Update is called once per frame
    void Update()
    {
        if (!just_died && is_dead)
        {
            stun_time = 0;
            blind_time = 0;
            inability_time = 0;
            end_attack();
            ani.SetBool("is_dead", is_dead);
            manager.spawn_knockout_wave();
            manager.number_of_dead_bosses += 1;
            toggle_aura_state(0);
            toggle_reflection(0);

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

        if (debug_dash)
        {
            start_dash();
            debug_dash = false;
        }
        if (debug_ice_storm)
        {
            start_ice_storm();
            debug_ice_storm = false;
        }
        if (debug_curse)
        {
            cast_curse();
            debug_curse = false;
        }
        if (debug_fireball)
        {
            cast_fireball();
            debug_fireball = false;
        }
        if (debug_fly)
        {
            start_flying();
            debug_fly = false;
        }
        if (debug_light)
        {
            start_summoning_light();
            debug_light = false;
        }
        if (debug_earth)
        {
            start_summoning_earth();
            debug_earth = false;
        }
        if (debug_stab)
        {
            start_making_portal();
            debug_stab = false;
        }
        if (debug_end_attack)
        {
            end_attack();
            debug_end_attack = false;
        }

        if (!top_dash_hit_first && (top_dash.wasBlocked || top_dash.wasHit || top_dash.wasParried))
        {
            top_dash_hit_first = true;
            //Destroy(Instantiate(dash_hit_effect, new Vector2(transform.position.x, transform.position.y - 0.5f), Quaternion.Euler(0, 0, 0)), 0.5f);
            Destroy(Instantiate(dash_hit_effect, manager.player.transform.position, Quaternion.Euler(0, 0, 0)), 0.5f);
        }

        if (!bottom_dash_hit_first && (bottom_dash.wasBlocked || bottom_dash.wasHit || bottom_dash.wasParried))
        {
            bottom_dash_hit_first = true;
            //Destroy(Instantiate(dash_hit_effect, new Vector2(transform.position.x, transform.position.y - 0.5f), Quaternion.Euler(0, 0, 0)), 0.5f);
            Destroy(Instantiate(dash_hit_effect, manager.player.transform.position, Quaternion.Euler(0, 0, 0)), 0.5f);
        }

        if (!third_dash_hit_first && (third_dash.wasBlocked || third_dash.wasHit || third_dash.wasParried))
        {
            third_dash_hit_first = true;
            Destroy(Instantiate(dash_hit_effect, manager.player.transform.position, Quaternion.Euler(0, 0, 0)), 0.5f);
        }

        if (!summoned_knocked_player_first_time && telefrag_attack.wasHit)
        {
            summoned_knocked_player_first_time = true;
            summon_knocked_player();
            toggle_telefrag(0);
        }

        countdown_cooldowns(t);

        is_swappable = !is_attacking && inability_time <= 0 && !is_dead;
        can_move = !is_attacking;
        can_flip = !(is_attacking || inability_time > 0);

        flip_check(t);

        if (blind_time > 0)
        {
            blind_time -= Time.deltaTime;
            if (blinded_first_time && blind_time <= 1f)
            {
                blinded_first_time = false;
                ani.SetBool("got_blinded", false);
                ani.SetBool("is_hurt", false);
            }
            if (blind_time < 0f)
            {
                blind_time = 0f;
            }
        }

        if (stun_time > 0)
        {
            stun_time -= Time.deltaTime;
            if (stunned_first_time && stun_time <= 1f)
            {
                stunned_first_time = false;
                ani.SetBool("got_stunned", false);
                ani.SetBool("is_hurt", false);
            }
            if (stun_time < 0f)
            {
                stun_time = 0f;
            }
        }

        inability_time = blind_time + stun_time;

        if (manager.current_boss_in_play == 2 && manager.player_life.player_died && sprite.enabled && !is_attacking)
        {
            all_actions_cooldown = 10f;
            ani.SetBool("killed_player", true);
        }

        else if (!is_attacking && called_to_swap && is_swappable && inability_time <= 0 && all_actions_cooldown <= 0 && !is_swapping && touch_damage.gameObject.layer == touch_damage.active_layer)
        {
            all_actions_cooldown = 10f;
            is_swapping = true;
            ani.SetBool("teleporting_exit", true);
        }

        else if (manager.current_boss_in_play == 2 && !ani.GetBool("teleporting_exit") && !ani.GetBool("teleporting_enter") && !coming_in && !is_attacking && !called_to_swap && inability_time <= 0 && all_actions_cooldown <= 0 && touch_damage.gameObject.layer == touch_damage.active_layer)
        {
            bool action_selected = false;
            if (!action_selected && fireball_cooldown <= 0f && Mathf.Abs(manager.player.transform.position.x - transform.position.x) >= 3.5f)
            {
                float small_rng = Random.value;
                if (!(small_rng >= 0.05f && small_rng <= 0.2f)) // 16% chance to bypass attack and do something else
                {
                    is_attacking = true;
                    action_selected = true;
                    cast_fireball();
                    fireball_cooldown = Random.Range(15f, 20f);
                }
            }
            if (!action_selected && curse_cooldown <= 0f)
            {
                float small_rng = Random.value;
                if (!(small_rng >= 0.05f && small_rng <= 0.2f)) // 16% chance to bypass attack and do something else
                {
                    is_attacking = true;
                    action_selected = true;
                    cast_curse();
                    curse_cooldown = Random.Range(20f, 25f);
                }
            }
            if (!action_selected && ice_storm_cooldown <= 0f)
            {
                float small_rng = Random.value;
                if (!(small_rng >= 0.5f && small_rng <= 0.75f)) // 26% chance to bypass attack
                {
                    is_attacking = true;
                    action_selected = true;
                    start_ice_storm();
                    ice_storm_cooldown = 50f;
                }
            }
            if (!action_selected && stab_cooldown <= 0f)
            {
                float small_rng = Random.value;
                if (!(small_rng >= 0.5f && small_rng <= 0.75f)) // 26% chance to bypass attack
                {
                    is_attacking = true;
                    action_selected = true;
                    start_making_portal();
                    stab_cooldown = Random.Range(25f, 30f);
                }
            }
            if (!action_selected && fly_cooldown <= 0 && Mathf.Abs(transform.position.x) <= 11)
            {
                float small_rng = Random.value;
                if (!(small_rng >= 0.05f && small_rng <= 0.2f)) // 16% chance to bypass attack and do something else
                {
                    is_attacking = true;
                    action_selected = true;
                    start_flying();
                    fly_cooldown = Random.Range(20f, 25f);
                }
            }
            if (!action_selected && light_cooldown <= 0f)
            {
                float small_rng = Random.value;
                {
                    is_attacking = true;
                    start_summoning_light();
                    action_selected = true;
                    light_cooldown = 35f;
                }
            }
            if (!action_selected && dash_cooldown <= 0f)
            {
                float small_rng = Random.value;
                if (!(small_rng >= 0.5f && small_rng <= 0.75f)) // 26% chance to bypass attack
                {
                    is_attacking = true;
                    start_dash();
                    action_selected = true;
                    dash_cooldown = Random.Range(25f, 30f);
                }
            }
            if (!action_selected && earth_cooldown <= 0f)
            {
                is_attacking = true;
                action_selected = true;
                start_summoning_earth();
                earth_cooldown = 10f;
            }
        }

        ani.SetBool("is_attacking", is_attacking);
        ani.SetBool("on_ground", onGround);
        //Debug.DrawRay(transform.position, (manager.player.transform.position - transform.position), Color.red);
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
    public void teleport_implications(int starting)
    {
        ani.SetBool("is_hurt", false);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        sprite.flipY = false;
        bool started = starting != 0; // 1 = become invincible

        box.enabled = !started;
        apply_invincibility(started);
        rb.linearVelocity = Vector2.zero;
        toggle_touch_damage(0);

        if (started)
        {
            toggle_aura_state(0);
        }
    }
    public void random_position_teleport()
    {
        if (ani.GetBool("third_dash"))
        {
            apply_invincibility(true);
            float newX = (Random.Range(0, 2) == 1) ? -12f : 12f;
            newX *= Random.Range(0f, 1f);
            float difference = Mathf.Abs(newX - manager.player.transform.position.x);
            bool ahead = newX > manager.player.transform.position.x;
            if (difference <= 4f)
            {
                newX += (4f - difference) * (ahead ? 1 : -1);
            }
            transform.position = new Vector2(newX, Random.Range(-4f, 4f));
            instant_flip_check();
            return;
        }
        else if (ani.GetBool("starting_dash"))
        {
            float newX = (Random.Range(0, 2) == 1) ? -12f : 12f;
            transform.position = new Vector2(newX, -2f);

            if (newX <= 0)
            {
                if (!sprite.flipX)
                {
                    flip_everything();
                }
            }
            else
            {
                if (sprite.flipX)
                {
                    flip_everything();
                }
            }
            return;
        }

        if (ani.GetBool("ice_storm"))
        {
            float newX = 0f;
            transform.position = new Vector2(newX, 2.5f);
            instant_flip_check();
            return;
        }
        if (ani.GetBool("summoning_light"))
        {
            float random_x = Random.Range(-10f, 10f);
            float random_y = Random.Range(-2f, 2f);
            transform.position = new Vector2(random_x, random_y);
            instant_flip_check();
            return;
        }
        if (ani.GetBool("revenge"))
        {
            float newX = manager.player.transform.position.x;

            float offset = Random.Range(-2.5f, 2.5f);

            transform.position = new Vector2(newX + offset, 2.5f);
            instant_flip_check();
            return;
        }

        float f = Random.Range(0, 1f);
        if ((f >= 0.4f && f < 0.7f) || coming_in) // 30% chance to try to telefrag
        {
            if (coming_in)
            {
                curse_cooldown = Random.Range(10f, 20f);
                ice_storm_cooldown = Random.Range(25f, 35f);
            }
            coming_in = false;
            float newX = manager.player_life.movement.dash_time > 0 ? (manager.player.transform.position.x + manager.player_life.movement.dash_time * manager.player_life.movement.dashSpeed * (manager.player_life.movement.sprite_render.flipX ? -1 : 1)) : manager.player.transform.position.x;
            transform.position = new Vector2(newX, -2f);
            instant_flip_check();
        }
        else
        {
            float random_x = Random.Range(-10f, 10f);
            float random_y = Random.Range(-2f, 2f);
            transform.position = new Vector2(random_x, random_y);
            instant_flip_check();
        }
    }
    public void toggle_telefrag(int i)
    {
        if (i != 0)
        {
            telefrag_attack.clearFlags();
            telefrag_attack.disableAttackCycle = false;
            telefrag_attack.EnableHitbox();
        }
        else
        {
            telefrag_attack.clearFlags();
            telefrag_attack.disableAttackCycle = false;
            telefrag_attack.DisableHitbox();
        }
    }
    public void toggle_touch_damage(int i)
    {
        if (i != 0)
        {
            touch_damage.EnableHitbox();
        }
        else
        {
            touch_damage.DisableHitbox();
        }
    }
    public void toggle_reflection(int i)
    {
        if (i != 0)
        {
            gameObject.tag = "Reflective";
        }
        else
        {
            gameObject.tag = "Untagged";
        }
    }
    public void random_aura()
    {
        int rng = Random.Range(0, 6);
        if (rng == 3)
        {
            toggle_aura_state(2);
            toggle_reflection(1);
        }
        else
        {
            toggle_aura_state(1);
            toggle_reflection(0);
        }
    }
    public void toggle_aura_state(int i)
    {
        if (i == 1)
        {
            box.enabled = true;
            aura_ani.SetBool("pink", true);
            aura_ani.SetBool("blue", false);
        }
        else if (i == 2)
        {
            box.enabled = true;
            aura_ani.SetBool("pink", false);
            aura_ani.SetBool("blue", true);
        }
        else
        {
            aura_ani.SetBool("pink", false);
            aura_ani.SetBool("blue", false);
        }
    }
    public void toggle_sprite_a(float amount)
    {
        sprite.color = new Vector4(sprite.color.r, sprite.color.g, sprite.color.b, sprite.color.a + amount);
    }
    public void check_exit()
    {
        if (is_swapping)
        {
            transform.position = new Vector2(0, 15);
            is_swapping = false;
            called_to_swap = false;
            manager.complete_handshake();
            ani.SetBool("teleporting_exit", false);
        }
        else
        {
            ani.SetBool("teleporting_exit", false);
            ani.SetBool("teleporting_enter", true);
        }
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
    public void apply_invincibility(bool i)
    {
        is_invincible = i;
    }
    public void invincible_workaround(int i)
    {
        apply_invincibility(i != 0);
    }
    public void unhurt()
    {
        ani.SetBool("is_hurt", false);
    }
    public void countdown_cooldowns(float t)
    {
        float new_time = (is_attacking ? t / 2f : t);

        if (inability_time > 0)
            new_time = 0;

        dash_cooldown -= new_time;
        if (dash_cooldown < 0)
        {
            dash_cooldown = 0;
        }

        ice_storm_cooldown -= new_time;
        if (ice_storm_cooldown < 0)
        {
            ice_storm_cooldown = 0;
        }

        curse_cooldown -= new_time;
        if (curse_cooldown < 0) { curse_cooldown = 0; }

        fireball_cooldown -= new_time;
        if (fireball_cooldown < 0)
        {
            fireball_cooldown = 0;
        }

        fly_cooldown -= new_time;
        if (fly_cooldown < 0) { fly_cooldown = 0; }

        light_cooldown -= new_time;
        if (light_cooldown < 0)
        {
            light_cooldown = 0;
        }

        earth_cooldown -= new_time;
        if (earth_cooldown < 0)
        {
            earth_cooldown = 0;
        }
        
        stab_cooldown -= new_time;
        if (stab_cooldown < 0)
        {
            stab_cooldown = 0;
        }

        all_actions_cooldown -= new_time;
        if (all_actions_cooldown < 0)
        {
            all_actions_cooldown = 0;
        }

        /*stun_time -= t;
        if (stun_time < 0)
        {
            stun_time = 0;
        }

        blind_time -= t;
        if (blind_time < 0)
        {
            blind_time = 0;
        }*/
    }
    public void flip_everything()
    {
        sprite.flipX = !sprite.flipX;
        fireball_shoot_point.transform.localPosition = new Vector2(-fireball_shoot_point.transform.localPosition.x, fireball_shoot_point.transform.localPosition.y);
        crack_point.transform.localPosition = new Vector2(-crack_point.transform.localPosition.x, crack_point.transform.localPosition.y);
        rock_shoot_point.transform.localPosition = new Vector2(-rock_shoot_point.transform.localPosition.x, rock_shoot_point.transform.localPosition.y);
        dagger_throw_point.transform.localPosition = new Vector2(-dagger_throw_point.transform.localPosition.x, dagger_throw_point.transform.localPosition.y);
        wizard_hat_point.transform.localPosition = new Vector2(-wizard_hat_point.transform.localPosition.x, wizard_hat_point.transform.localPosition.y);
    }
    public void start_dash()
    {
        is_attacking = true;
        teleport_implications(0);
        ani.SetBool("teleporting_exit", true);
        ani.SetBool("starting_dash", true);
    }
    public void do_dash()
    {
        ani.SetBool("is_dashing", true);
    }
    public void finish_dash()
    {
        ani.SetBool("teleporting_enter", true);
    }
    public void activate_top_dash(int i)
    {
        if (i == 0)
        {
            top_dash.disableAttackCycle = false;
            top_dash.clearFlags();
            top_dash.DisableHitbox();
        }
        else
        {
            top_dash.EnableHitbox();
        }
    }
    public void activate_bottom_dash(int i)
    {
        if (i == 0)
        {
            bottom_dash.disableAttackCycle = false;
            bottom_dash.clearFlags();
            bottom_dash.DisableHitbox();
        }
        else
        {
            bottom_dash.EnableHitbox();
        }
    }
    public void activate_third_dash(int i)
    {
        if (i == 0)
        {
            third_dash.disableAttackCycle = false;
            third_dash.clearFlags();
            third_dash.DisableHitbox();
        }
        else
        {
            third_dash.EnableHitbox();
        }
    }
    public void check_third_dash()
    {
        if (max_health * 0.75f >= health)
        {
            if (ani.GetBool("third_dash"))
            {
                ani.SetBool("third_dash", false);
            }
            else
            {
                ani.SetBool("third_dash", true);
            }
        }
    }
    public void angle_grab()
    {
        if (ani.GetBool("third_dash"))
        {
            instant_flip_check();
            Vector2 start_position = transform.position;
            Vector2 end_position = manager.player.transform.position;

            Vector2 ray_to_player = new Vector2(end_position.x - start_position.x, end_position.y - start_position.y);
            float calc_rotation = Mathf.Atan2(ray_to_player.y, ray_to_player.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, calc_rotation);
            calc_rotation = (calc_rotation % 360 + 360) % 360;
            if (calc_rotation > 90 && calc_rotation < 270)
            {
                sprite.flipY = true;
                sprite.flipX = true;
            }
        }

        if (ani.GetBool("diving") && !dived)
        {
            instant_flip_check();
            dived = true;
            Vector2 start_position = transform.position;
            Vector2 end_position = new Vector2(manager.player.transform.position.x, -3f);

            Vector2 ray_to_player = new Vector2(end_position.x - start_position.x, end_position.y - start_position.y);
            float calc_rotation = Mathf.Atan2(ray_to_player.y, ray_to_player.x);

            float speed = 30f;

            if (ani.GetBool("revenge"))
                speed += 3f;

            float x = speed * Mathf.Cos(calc_rotation);
            float y = speed * Mathf.Sin(calc_rotation);

            if (y > -1f)
            {
                y = -1;
            }

            rb.linearVelocity = new Vector2(x, y);
        }
    }
    public void start_ice_storm()
    {
        is_attacking = true;
        teleport_implications(0);
        ani.SetBool("teleporting_exit", true);
        ani.SetBool("ice_storm", true);
        current_ice_background = Instantiate(ice_background, new Vector2(0, 0), Quaternion.Euler(0, 0, 0)).GetComponent<TestBoss5WizardIceBackgroundBehavior>();
        current_ice_background.appearance_delay = 1f;
        current_ice_platform = Instantiate(ice_platform, new Vector2(Random.Range(-8f, 8f), -2f), Quaternion.Euler(0, 0, 0)).GetComponent<TestBoss5WizardIcePlatformBehavior>();
        current_ice_platform.delay_before_visible = 1f;
    }
    public void spawn_ice_wave()
    {
        bool from_above = Random.Range(0, 2) == 1;
        int angle;

        if (from_above)
        {
            angle = Random.Range(3, 6); // 135 - 225
            if (angle == 4) { angle += Random.Range(0, 2) == 1 ? -1 : 1; }
        }
        else
        {
            angle = Random.Range(-1, 2); // -45 = 315 & 0 - 45
            if (angle == 0) { angle += Random.Range(0, 2) == 1 ? -1 : 1; }
            if (angle == -1) { angle = 7; }
        }

        if (from_above)
        {
            for (int i = 0; i < 120; i++)
            {
                var crystal = Instantiate(ice_crystal, new Vector2((i - 60) * 0.35f + Random.Range(-0.05f, 0.05f), 5f), Quaternion.Euler(0, 0, angle * 45)).GetComponent<TestBoss5WizardIceCrystalBehavior>();
                float randomized_time = Random.Range(0.25f, 0.75f);
                crystal.rotation = angle * 45;
                crystal.speed = Random.Range(15f, 18f);
                crystal.time_to_appear = randomized_time;
                crystal.time_to_fall = randomized_time + Random.Range(1f, 1.25f);
                crystal.the_boss = this;
            }
        }
        else
        {
            for (int i = 0; i < 120; i++)
            {
                var crystal = Instantiate(ice_crystal, new Vector2((i - 60) * 0.35f + Random.Range(-0.05f, 0.05f), -6f), Quaternion.Euler(0, 0, angle * 45)).GetComponent<TestBoss5WizardIceCrystalBehavior>();
                crystal.ground_does_not_break = true;
                float randomized_time = Random.Range(0.25f, 0.75f);
                crystal.rotation = angle * 45;
                crystal.speed = Random.Range(15f, 18f);
                crystal.time_to_appear = randomized_time;
                crystal.time_to_fall = randomized_time + Random.Range(1f, 1.25f);
                crystal.the_boss = this;
            }
        }
    }
    public void finish_ice_storm()
    {
        ani.SetBool("teleporting_exit", true);
    }

    public void cast_curse()
    {
        is_attacking = true;
        instant_flip_check();
        ani.SetBool("cast_curse", true);
    }
    public void place_curse()
    {
        if (manager.has_player)
        {
            manager.player_life.Hit_No_Collider(curse_hit);
        }
    }
    public void cast_fireball()
    {
        is_attacking = true;
        instant_flip_check();
        ani.SetBool("cast_fireball", true);
    }
    public void shoot_fireball()
    {
        float x1 = 100;
        float x2 = 200;
        float x3 = 300;
        float x4 = 400;
        float x5 = 500;

        float multiplier = sprite.flipX ? 1 : -1;

        var f1 = Instantiate(fireball, fireball_shoot_point.transform.position, Quaternion.Euler(0, 0, 0)).GetComponent<WizardFireballBehavior>();
        f1.rb.AddForce(new Vector2(x1 * multiplier, 400));

        var f2 = Instantiate(fireball, fireball_shoot_point.transform.position, Quaternion.Euler(0, 0, 0)).GetComponent<WizardFireballBehavior>();
        f2.rb.AddForce(new Vector2(x2 * multiplier, 400));

        var f3 = Instantiate(fireball, fireball_shoot_point.transform.position, Quaternion.Euler(0, 0, 0)).GetComponent<WizardFireballBehavior>();
        f3.rb.AddForce(new Vector2(x3 * multiplier, 400));

        var f4 = Instantiate(fireball, fireball_shoot_point.transform.position, Quaternion.Euler(0, 0, 0)).GetComponent<WizardFireballBehavior>();
        f4.rb.AddForce(new Vector2(x4 * multiplier, 400));

        var f5 = Instantiate(fireball, fireball_shoot_point.transform.position, Quaternion.Euler(0, 0, 0)).GetComponent<WizardFireballBehavior>();
        f5.rb.AddForce(new Vector2(x5 * multiplier, 400));
    }
    public void start_flying()
    {
        is_attacking = true;
        ani.SetBool("flying", true);
    }
    public void actually_fly()
    {
        float x = sprite.flipX ? -7f : 7f;

        rb.linearVelocity = new Vector2(x, 4f);
    }
    public void begin_diving()
    {
        instant_flip_check();
        ani.SetBool("diving", true);
        dived = false;
        ani.SetBool("dive_recovery", false);
    }
    public void dive_recovery_preliminary()
    {
        toggle_touch_damage(0);
        float x = sprite.flipX ? -5f : 5f;

        rb.linearVelocity = new Vector2(x, 3f);
    }
    public void dive_recovery()
    {
        ani.SetBool("teleporting_exit", true);
        rb.linearVelocity = new Vector2(0f, -1f);
    }
    public void actually_fly_revenge()
    {
        apply_invincibility(true);
        float x = sprite.flipX ? -10f : 10f;

        rb.linearVelocity = new Vector2(x, 6f);
    }
    public void start_summoning_light()
    {
        is_attacking = true;
        teleport_implications(0);
        ani.SetBool("teleporting_exit", true);
        ani.SetBool("summoning_light", true);
    }
    public void summon_light()
    {
        if (max_health / 2f < health)
        {
            for (int i = 0; i < 6; i++)
            {
                var hi = Instantiate(light_cross, new Vector2(4.8f * (i - 2.5f) * Random.Range(0.9f, 1.1f), 8f), Quaternion.Euler(0,0,Random.Range(0,360))).GetComponentInChildren<WizardLightCrossBehavior>();
                hi.fall_delay = Random.Range(1f, 1.75f);
            }
        }
        else
        {
            for (int i = 0; i < 6; i++)
            {
                var hi = Instantiate(light_sword, new Vector2(4.5f * (i - 2.5f) * Random.Range(0.9f, 1.1f), 10f), Quaternion.Euler(0, 0, Random.Range(0, 360))).GetComponentInChildren<WizardLightSwordBehavior>();
                hi.fall_delay = Random.Range(0.5f, 1.25f);
                hi.wizard = this;
                hi.spinning_left = Random.Range(0, 2) == 1 ? true : false;
            }
        }
    }
    public void finish_light()
    {
        ani.SetBool("teleporting_exit", true);
    }
    public void start_summoning_earth()
    {
        is_attacking = true;
        ani.SetBool("summoning_earth", true);
    }
    public void summon_earth(int time)
    {
        instant_flip_check();
        float x = 150 * time;

        float multiplier = sprite.flipX ? 1 : -1;

        var f1 = Instantiate(rock, rock_shoot_point.transform.position, Quaternion.Euler(0, 0, 0)).GetComponent<WizardRockBehavior>();
        f1.rb.AddForce(new Vector2(x * multiplier, Random.Range(250f, 350f)));
    }
    public void finish_earth()
    {
        ani.SetBool("teleporting_exit", true);
    }
    public void start_making_portal()
    {
        is_attacking = true;
        toggle_touch_damage(0);
        ani.SetBool("casting_portal", true);
    }
    public void make_portal()
    {
        bool left = Random.Range(0, 2) == 1;
        current_portal = Instantiate(portal, manager.player.transform.position, Quaternion.Euler(0, 0, 0));
        current_portal_logic = current_portal.GetComponent<WizardPortalBehavior>();
        current_portal_logic.left = left;
        current_portal_logic.the_boss = this;
    }
    public void portal_stab()
    {
        if (current_portal_logic)
        {
            current_portal_logic.ani.SetBool("stabbing", true);
        }
    }
    public void portal_pullout()
    {
        instant_flip_check();
        if (current_portal_logic)
        {
            current_portal_logic.ani.SetBool("disappear", true);
        }
    }
    public void finish_portal()
    {
        Destroy(current_portal);
    }
    public void throw_dagger()
    {
        instant_flip_check();
        Vector2 start_position = transform.position;
        Vector2 end_position = new Vector2(manager.player.transform.position.x, manager.player.transform.position.y + 0.5f);

        Vector2 ray_to_player = new Vector2(end_position.x - start_position.x, end_position.y - start_position.y);
        float calc_rotation = Mathf.Atan2(ray_to_player.y, ray_to_player.x);

        if (ani.GetBool("intact_dagger"))
        {
            var hi = Instantiate(dagger, dagger_throw_point.transform.position, Quaternion.Euler(0, 0, 0)).GetComponent<WizardDaggerBehavior>();

            float x = Mathf.Cos(calc_rotation) * 14f;
            float y = Mathf.Sin(calc_rotation) * 14f;

            hi.rb.linearVelocity = new Vector2(x, y);
        }
        else if (ani.GetBool("broken_dagger"))
        {
            var hi = Instantiate(broken_dagger, dagger_throw_point.transform.position, Quaternion.Euler(0, 0, 0)).GetComponent<WizardDaggerBehavior>();

            float x = Mathf.Cos(calc_rotation) * 14f;
            float y = Mathf.Sin(calc_rotation) * 14f;

            hi.rb.linearVelocity = new Vector2(x, y);
        }
    }
    public void toss_hat()
    {
        var hi = Instantiate(wizard_hat, wizard_hat_point.transform.position, Quaternion.Euler(0, 0, 0)).GetComponent<Wizard_WizardHatBehavior>();
        hi.rb.AddForce(new Vector2(sprite.flipX ? -100 : 100, 300));
    }
    public void temporary_fadeout()
    {
        sprite.color = new Vector4(sprite.color.r, sprite.color.g, sprite.color.b, sprite.color.a - 0.34f);
    }
    public void set_up_revenge()
    {
        is_attacking = true;
        ani.SetBool("teleporting_exit", true);
        ani.SetBool("revenge", true);
    }
    public void stun_me(float f)
    {
        if (stun_time > 0)
            return;
        else
        {
            toggle_aura_state(0);
            toggle_reflection(0);
            bool keep_dash = false;
            if (ani.GetBool("is_dashing"))
            {
                keep_dash = true;
            }
            end_attack();
            ani.SetBool("is_dashing", keep_dash);
            ani.SetBool("dash_related", keep_dash);
            if (!keep_dash)
                change_rb_type();
            ani.SetBool("got_stunned_first_time", true);
            ani.SetBool("got_stunned", true);
            stun_time = f;
            current_stun_effect = Instantiate(stun_effect, new Vector2(transform.position.x, transform.position.y + 1f), Quaternion.Euler(0, 0, 0)).GetComponent<TestBoss5_WizardStunEffect>();
            current_stun_effect.Initialize(f, gameObject);
        }
    }
    public void blind_me(float f)
    {
        if (blind_time > 0)
            return;
        else
        {
            toggle_aura_state(0);
            toggle_reflection(0);
            bool keep_dash = false;
            if (ani.GetBool("is_dashing"))
            {
                keep_dash = true;
            }
            end_attack();
            ani.SetBool("is_dashing", keep_dash);
            ani.SetBool("dash_related", keep_dash);
            ani.SetBool("got_blinded_first_time", true);
            ani.SetBool("got_blinded", true);
            blind_time = f;
        }
    }
    public void unset_first_times()
    {
        ani.SetBool("got_blinded_first_time", false);
        ani.SetBool("got_stunned_first_time", false);
    }
    public void change_rb_type()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 1;
    }
    public void end_attack()
    {
        float change_in_time = 0;
        is_attacking = false;
        can_flip_once = false;

        sprite.color = new Vector4(1, 1, 1, 1);

        //manager.time_since_swap = 53; // TEMPORARY

        teleport_implications(0);

        if (summoned_knocked_player_first_time)
        {
            change_in_time = 4.5f;
            summoned_knocked_player_first_time = false;
        }
        toggle_telefrag(0);

        ani.SetBool("teleporting_enter", false);

        top_dash_hit_first = false;
        bottom_dash_hit_first = false;
        third_dash_hit_first = false;
        activate_top_dash(0);
        activate_bottom_dash(0);
        activate_third_dash(0);

        ani.SetBool("starting_dash", false);
        ani.SetBool("is_dashing", false);
        ani.SetBool("third_dash", false);

        if (current_ice_platform)
        {
            current_ice_platform.start_falling();
            current_ice_platform = null;
        }
        if (current_ice_background)
        {
            current_ice_background.fading_out = true;
            current_ice_background = null;
        }
        ani.SetBool("ice_storm", false);

        if (ani.GetBool("cast_curse"))
        {
            change_in_time = 1.0f;
        }
        ani.SetBool("cast_curse", false);

        if (ani.GetBool("cast_fireball"))
        {
            change_in_time = 0.5f;
        }
        ani.SetBool("cast_fireball", false);

        ani.SetBool("flying", false);
        ani.SetBool("diving", false);
        ani.SetBool("dive_recovery", false);
        dived = false;

        ani.SetBool("summoning_light", false);
        ani.SetBool("summoning_earth", false);

        ani.SetBool("casting_portal", false);
        ani.SetBool("broken_dagger", false);
        ani.SetBool("intact_dagger", false);
        Destroy(current_portal);

        if (current_portal_logic != null)
        {
            current_portal_logic.ani.SetBool("disappear", true);
        }
        Destroy(current_portal_logic);

        ani.SetBool("hit_wall", false);
        ani.SetBool("revenge", false);

        blinded_first_time = true;
        stunned_first_time = true;

        ani.SetBool("got_stunned", false);
        ani.SetBool("got_stunned_first_time", false);
        ani.SetBool("got_blinded", false);
        ani.SetBool("got_blinded_first_time", false);

        ani.SetBool("dash_related", false);

        if (current_stun_effect)
        {
            Destroy(current_stun_effect.gameObject);
        }

        if (change_in_time == 0f)
        {
            all_actions_cooldown = 1.5f;
        }
        else
        {
            all_actions_cooldown = change_in_time;
        }

        ani.SetBool("is_hurt", false);

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
    }
    public void make_damage_number(int dmg)
    {
        if (manager.current_boss_in_play == 2 && sprite.color.a > 0)
        {
            Instantiate(damage_number, new Vector2(transform.position.x, transform.position.y + 1.5f), Quaternion.Euler(0, 0, 0)).GetComponent<DamageNumbersTestBoss5Behavior>().initialize(dmg);
        }
    }
    public void create_firework()
    {
        float x = Random.Range(-7f, 7f);
        float y = Random.Range(4f, 7f);

        var hi = Instantiate(firework, rock_shoot_point.transform.position, Quaternion.Euler(0, 0, 0)).GetComponent<WizardFireworkBehavior>();
        hi.rb.linearVelocity = new Vector2(x, y);
    }
    public void Hit(PlayerAttack player_attack)
    {
        if (is_dead)
        {
            return;
        }
        if (is_invincible || !box.enabled)
        {
            make_damage_number(0);
            player_attack.isBlock();
            return;
        }

        var attempt = Physics2D.Raycast(transform.position, (manager.player.transform.position - transform.position).normalized, (manager.player.transform.position - transform.position).magnitude, both_ground_layer);
        if (attempt.collider == null)
        {
            
        }
        else
        {
            make_damage_number(0);
            player_attack.isBlock();
            return;
        }

        if ((ani.GetBool("summoning_light") || ani.GetBool("ice_storm")) && !player_attack.ignoresInvincibility)
            player_attack.damage /= 2;

        player_attack.isHit();
        TakeDamage(player_attack.damage);
        if (player_attack.map_wide)
        {
            manager.inflict_area_damage(player_attack.damage / 4);
        }

        if (player_attack.blindDuration > 0)
        {
            blind_me(player_attack.blindDuration);
        }
        else if (player_attack.stunDuration > 0)
        {
            stun_me(player_attack.stunDuration + 2);
        }

        ani.SetBool("is_hurt", true);
    }
    public void TakeDamage(int damage)
    {
        if (manager.current_boss_in_play == 2)
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
        if (ani.GetBool("third_dash") && ani.GetBool("is_dashing"))
        {
            rb.linearVelocityX = transform.right.x * 40f;
            rb.linearVelocityY = transform.right.y * 40f;
            if (Mathf.Abs(transform.position.x) >= 16f || Mathf.Abs(transform.position.y) >= 8f)
            {
                ani.SetBool("is_dashing", false);
                ani.SetBool("starting_dash", false);
                rb.linearVelocity = Vector2.zero;
                instant_flip_check();
            }
            else
            {
                Instantiate(dash_particle, new Vector2(transform.position.x, transform.position.y - 0.5f), Quaternion.Euler(0, 0, 0));
            }
        }
        else if (ani.GetBool("is_dashing") && inability_time > 0)
        {
            rb.linearVelocity = new Vector2(sprite.flipX ? 15f : -15f, 0f);
            if (!sprite.flipX && transform.position.x <= -12f)
            {
                ani.SetBool("is_dashing", false);
                ani.SetBool("hit_wall", true);
                TakeDamage(200);
                rb.linearVelocity = Vector2.zero;
                sprite.flipX = true;
            }
            else if (sprite.flipX && transform.position.x >= 12f)
            {
                ani.SetBool("is_dashing", false);
                ani.SetBool("hit_wall", true);
                TakeDamage(200);
                rb.linearVelocity = Vector2.zero;
                sprite.flipX = false;
            }
        }
        else if (ani.GetBool("is_dashing"))
        {
            rb.linearVelocity = new Vector2(sprite.flipX ? 30f : -30f, 0f);
            if (!sprite.flipX && transform.position.x <= -12f)
            {
                ani.SetBool("is_dashing", false);
                rb.linearVelocity = Vector2.zero;
                sprite.flipX = true;
            }
            else if (sprite.flipX && transform.position.x >= 12f)
            {
                ani.SetBool("is_dashing", false);
                rb.linearVelocity = Vector2.zero;
                sprite.flipX = false;
            }
            else
            {
                Instantiate(dash_particle, new Vector2(transform.position.x, transform.position.y - 0.5f), Quaternion.Euler(0, 0, 0));
            }
        }
        if (ani.GetBool("diving") && transform.position.y <= -3f)
        {
            ani.SetBool("dive_recovery", true);
            ani.SetBool("diving", false);
            Destroy(Instantiate(ground_break, new Vector2(transform.position.x, -0.6f), Quaternion.Euler(0, 0, 0)), 0.6f);
            Instantiate(crack, new Vector2(transform.position.x, -4.6f), Quaternion.Euler(0, 0, 0));

            float x = sprite.flipX ? -15f : 15f;

            rb.linearVelocity = new Vector2(x, 3f);
            var hi = GameManager.Instance.current_camera.GetComponent<CameraShake>();
            if (hi != null)
            {
                if (!ani.GetBool("revenge"))
                    hi.shakeTime = 1f;
                else
                    hi.shakeTime = 1.25f;
            }
        }
        if (inability_time > 0f && onGround)
        {
            rb.gravityScale = 0;
            rb.linearVelocityY = 0f;
        }
    }
    private void OnTriggerStay2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & player_layer) != 0)
        {
            if (touch_damage.gameObject.layer == LayerMask.NameToLayer("Active_Enemy_Attack"))
            {
                manager.player_life.Hit_No_Collider(touch_damage.gameObject);
            }
        }
        if (((1 << col.gameObject.layer) & ground_layer) != 0)
        {
            onGround = true;
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & player_layer) != 0)
        {
            touch_damage.DisableHitbox();
        }

        if (((1 << col.gameObject.layer) & ground_layer) != 0)
        {
            onGround = false;
        }
    }
}

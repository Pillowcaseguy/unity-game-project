using UnityEngine;

public class TestBoss4Behavior : MonoBehaviour, EnemyDamageable, EnemyMainBodyImplement
{
    public GameObject level_manager;
    public LevelManager_TestBoss4 level_manager_behavior;

    public Animator ani;
    public SpriteRenderer sprite;
    public Rigidbody2D rb;
    public BoxCollider2D box;

    public bool is_dead;
    public bool just_died;
    public int health;
    public int starting_health;

    public bool can_flip;
    public float flip_cooldown;
    public bool can_flip_once;

    public bool is_attacking;

    public bool is_dashing;

    public int phase = 0;
    public bool phase_change_queued;

    public LayerMask player_layer;
    public LayerMask ground_layer;

    public GameObject throwing_sword;
    public GameObject throwing_sword_point;

    public BoxCollider2D sword_detection_hitbox;
    public bool player_in_sword_range;

    public TestBoss4SwordSlashManager swordy;

    public GameObject ice_spike;
    public GameObject snowball;

    public Vector2 known_player_position;

    public bool player_in_range;

    public GameObject body_spike_object;
    public EnemyAttack body_spikes;

    public GameObject blocking_attack_object;
    public TestBoss4BlockingAttacksHitbox blocking_attack_hitbox;

    public bool is_blocking;
    public bool is_invincible;

    public float stun_time_phase_1;
    public bool just_stunned = true;
    public bool was_blocking;

    public GameObject stun_effect;
    public GameObject current_stun;

    public GameObject spiral;
    public GameObject crystal;

    public GameObject ice_map_attack;
    public GameObject current_ice_map_attack;

    public float raising_spike_cooldown;
    public bool last_spikes_was_multiple;
    public float sword_slash_cooldown;
    public float blocking_cooldown;
    public float air_attack_cooldown;
    public float body_attack_cooldown;
    public float ground_attack_cooldown;
    public float map_wide_attack_cooldown;
    public float throwing_snowball_cooldown;
    public float all_attacks_cooldown;

    public float burn_time;
    public GameObject fire_effect;
    public GameObject current_fire;

    public float blind_time;
    public bool just_blinded = true;
    public float inability_time;

    public float freeze_time;

    public float jump_cooldown;
    public float dash_cooldown;
    public float fireball_cooldown;
    public float melee_cooldown;
    public float reflection_cooldown;
    public float flamethrower_cooldown;
    public float fire_background_cooldown;
    public float fire_wall_cooldown;
    public float meteor_cooldown;

    public EnemyAttack dash_starting_hit;
    public GameObject dash_fire_trail;
    public FireTrailBehavior current_fire_trail;
    public BoxCollider2D dive_attack_hitbox;
    public EnemyAttack dive_attack;
    public GameObject throwing_fireball;
    public EnemyAttack melee_attack_1;
    public EnemyAttack melee_attack_2;
    public EnemyAttack melee_attack_3;
    public BoxCollider2D melee_attack_1_hb;
    public BoxCollider2D melee_attack_2_hb;
    public BoxCollider2D melee_attack_3_hb;
    public GameObject pitchfork_attack_point;
    public EnemyAttack flamethrower_attack;
    public BoxCollider2D flamethrower_hitbox;

    public bool is_jumping;
    public bool is_diving;

    public GameObject fire_background;
    public TestBoss4FireBackgroundBehavior current_fire_background;

    public GameObject meteor;

    public GameObject fire_wall;
    public TestBoss4FirewallBehavior fire_wall_left;
    public TestBoss4FirewallBehavior fire_wall_right;

    public GameObject fire_spike;

    public float dash_target_x;

    public GameObject middle_fireball;
    public int fireball_patterns_shot;
    void Start()
    {
        ani = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        health = starting_health;
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        player_layer = LayerMask.GetMask("Player_Damage");
        ground_layer = LayerMask.GetMask("Ground","BreakableGround");

        last_spikes_was_multiple = true;
        raising_spike_cooldown = Random.Range(5f, 10f);
        sword_slash_cooldown = Random.Range(5f, 10f);
        blocking_cooldown = 15f;
        air_attack_cooldown = 2f;
        ground_attack_cooldown = Random.Range(2f, 6f);
        map_wide_attack_cooldown = 20f;
        throwing_snowball_cooldown = Random.Range(2f, 6f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!just_died && is_dead)
        {
            burn_time = 0;
            if (current_fire)
            {
                Destroy(current_fire);
            }
            stun_time_phase_1 = 0;
            if (current_stun)
            {
                Destroy(current_stun);
            }
            blind_time = 0;
            freeze_time = 0;
            transform.position = new Vector2(transform.position.x, -2.56f);
            if (fire_wall_left)
            {
                fire_wall_left.lifetime = 0;
            }
            if (fire_wall_right)
            {
                fire_wall_right.lifetime = 0;
            }
            just_died = true;
            ani.SetBool("is_dead", is_dead);
        }

        if (is_dead)
        {
            return;
        }
        
        if (transform.position.x > 15 || transform.position.x < -15)
        {
            end_attack();
            transform.position = new Vector2(0, -2.56f);
        }

        float t = Time.deltaTime;
        countdown_cooldowns(t);

        inability_time = stun_time_phase_1 + blind_time + freeze_time;

        can_flip = !(is_attacking || ani.GetBool("phase_change") || inability_time > 0 || is_dead) || ani.GetBool("dashing_with_sword");

        if (can_flip_once)
        {
            can_flip = true;
        }

        if (!(level_manager == null || !level_manager_behavior.has_player))
        {
            if (level_manager_behavior.player.transform.position.x > transform.position.x && !sprite.flipX)
            {
                flip_cooldown -= t;
                if (flip_cooldown <= 0 && can_flip || can_flip_once)
                {
                    can_flip_once = false;
                    flip_everything();
                }
            }
            else if (level_manager_behavior.player.transform.position.x < transform.position.x && sprite.flipX)
            {
                flip_cooldown -= t;
                if (flip_cooldown <= 0 && can_flip || can_flip_once)
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

        if (!is_attacking && phase == 0 && phase_change_queued)
        {
            phase_change_queued = false;
            burn_time = 0;
            if (current_fire)
            {
                Destroy(current_fire);
            }
            stun_time_phase_1 = 0;
            if (current_stun)
            {
                Destroy(current_stun);
            }
            blind_time = 0;
            freeze_time = 0;
            phase_change();
        }

        if (stun_time_phase_1 > 0)
        {
            if (was_blocking && just_stunned)
            {
                just_stunned = false;
                was_blocking = false;
                ani.SetBool("is_stunned_phase_1", true);
                stop_blocking_attacks();
            }
            else if (just_stunned)
            {
                just_stunned = false;
                ani.SetBool("is_stunned_no_ice", true);
            }

            if (current_stun == null)
            {
                current_stun = Instantiate(stun_effect);
                current_stun.GetComponent<TestBoss4StunEffect>().Initialize(stun_time_phase_1, gameObject);
            }

            stun_time_phase_1 -= Time.deltaTime;
            if (stun_time_phase_1 <= 0)
            {
                ani.SetBool("is_stunned_phase_1", false);
                ani.SetBool("is_stunned_no_ice", false);
                just_stunned = true;
                stun_time_phase_1 = 0;
                all_attacks_cooldown = 0.5f;
                if (current_stun != null)
                {
                    Destroy(current_stun);
                }
            }
        }

        if (blind_time > 0)
        {
            if (just_blinded)
            {
                stop_blocking_attacks();
                just_blinded = false;
                ani.SetBool("is_blinded_first_time", true);
                ani.SetBool("is_blinded", true);
            }
            blind_time -= Time.deltaTime;
            if (blind_time <= 0)
            {
                blind_time = 0;
                just_blinded = true;
                all_attacks_cooldown = 0.5f;
                ani.SetBool("is_blinded", false);
            }
        }

        if (burn_time > 0)
        {
            if (current_fire == null)
            {
                current_fire = Instantiate(fire_effect);
                current_fire.GetComponent<TestBoss4BurnEffect>().Initialize(burn_time, gameObject);
            }

            burn_time -= Time.deltaTime;
            if (burn_time <= 0)
            {
                burn_time = 0;
                if (current_fire != null)
                {
                    Destroy(current_fire, 0.3f);
                }
            }
        }

        if (freeze_time > 0)
        {
            ani.SetBool("is_frozen", true);
            freeze_time -= Time.deltaTime;
            if (freeze_time <= 0)
            {
                ani.SetBool("is_frozen", false);
                freeze_time = 0;
                all_attacks_cooldown = 0.5f;
            }
        }

        if (phase == 0 && ani.GetBool("dashing_with_sword"))
        {
            if (player_in_sword_range)
            {
                stop_dash();
                ani.SetBool("slashing_sword", true);
                ani.SetBool("dashing_with_sword", false);
            }
        }

        if (phase == 0)
        {
            roll_attacking_phase_1();
        }
        else if (phase == 1)
        {
            roll_attacking_phase_2();
        }

        ani.SetBool("is_attacking", is_attacking);
        ani.SetBool("player_in_spike_range", player_in_range);
        //ani.SetBool("is_jumping", is_jumping);
        //ani.SetBool("is_diving", is_diving);
    }
    public void FixedUpdate()
    {
        if (is_dashing && phase == 0)
        {
            float speed = 15;
            rb.linearVelocityX = sprite.flipX ? speed : speed * -1;
        }
        if (is_jumping && phase == 1)
        {
            rb.linearVelocityY = 20;
            if (transform.position.y > 20 && !is_diving)
            {
                rb.linearVelocityY = 0;
                is_jumping = false;
                ani.SetBool("is_jumping", false);
                is_diving = true;
                ani.SetBool("is_diving", true);
                dive_attack.clearFlags();
                dive_attack.disableAttackCycle = false;
                dive_attack.EnableHitbox();
                transform.position = new Vector2(level_manager_behavior.player.transform.position.x, transform.position.y);
            }
        }
        else if (is_diving && phase == 1)
        {
            rb.linearVelocityY = -20;
        }
        else if (is_dashing && phase == 1)
        {
            float speed = 15;
            rb.linearVelocityX = sprite.flipX ? speed : speed * -1;
            if (dash_target_x < 0 && transform.position.x <= dash_target_x)
            {
                rb.linearVelocityX = 0;
                transform.position = new Vector2(-9, transform.position.y);
                update_dash();
                stop_dash_P2();
                end_attack();
            }
            if (dash_target_x > 0 && transform.position.x >= dash_target_x)
            {
                rb.linearVelocityX = 0;
                transform.position = new Vector2(9, transform.position.y);
                update_dash();
                stop_dash_P2();
                end_attack();
            }
        }
    }
    public void flip_everything()
    {
        sprite.flipX = !sprite.flipX;
        throwing_sword_point.transform.localPosition = new Vector2(-throwing_sword_point.transform.localPosition.x, throwing_sword_point.transform.localPosition.y);
        swordy.slash_1.offset = new Vector2(-swordy.slash_1.offset.x, swordy.slash_1.offset.y);
        swordy.slash_2.offset = new Vector2(-swordy.slash_2.offset.x, swordy.slash_2.offset.y);
        swordy.slash_3.offset = new Vector2(-swordy.slash_3.offset.x, swordy.slash_3.offset.y);
        swordy.slash_4.offset = new Vector2(-swordy.slash_4.offset.x, swordy.slash_4.offset.y);
        swordy.slash_5.offset = new Vector2(-swordy.slash_5.offset.x, swordy.slash_5.offset.y);
        swordy.slash_6.offset = new Vector2(-swordy.slash_6.offset.x, swordy.slash_6.offset.y);
        pitchfork_attack_point.transform.localPosition = new Vector2(-pitchfork_attack_point.transform.localPosition.x, pitchfork_attack_point.transform.localPosition.y);
        melee_attack_1_hb.offset = new Vector2(-melee_attack_1_hb.offset.x, melee_attack_1_hb.offset.y);
        melee_attack_2_hb.offset = new Vector2(-melee_attack_2_hb.offset.x, melee_attack_2_hb.offset.y);
        melee_attack_3_hb.offset = new Vector2(-melee_attack_3_hb.offset.x, melee_attack_3_hb.offset.y);
        dive_attack_hitbox.offset = new Vector2(-dive_attack_hitbox.offset.x, dive_attack_hitbox.offset.y);
        flamethrower_hitbox.offset = new Vector2(-flamethrower_hitbox.offset.x, flamethrower_hitbox.offset.y);
        sword_detection_hitbox.offset = new Vector2(-sword_detection_hitbox.offset.x, sword_detection_hitbox.offset.y);
        //body_spike_object.transform.localPosition = new Vector2(-body_spike_object.transform.localPosition.x, body_spike_object.transform.localPosition.y);
    }
    public void countdown_cooldowns(float time)
    {
        if (phase == 1 && ani.GetBool("is_map_attacking"))
        {
            time /= 2f;
        }

        raising_spike_cooldown -= time;
        if (raising_spike_cooldown <= 0)
        {
            raising_spike_cooldown = 0;
        }

        sword_slash_cooldown -= time;
        if (sword_slash_cooldown <= 0)
        {
            sword_slash_cooldown = 0;
        }

        blocking_cooldown -= time;
        if (blocking_cooldown <= 0) 
        {
            blocking_cooldown = 0;
        }

        air_attack_cooldown -= time;
        if (air_attack_cooldown <= 0) 
        {  
            air_attack_cooldown = 0;
        }

        body_attack_cooldown -= time;
        if (body_attack_cooldown <= 0)
        {
            body_attack_cooldown = 0;
        }

        ground_attack_cooldown -= time;
        if (ground_attack_cooldown <= 0)
        {
            ground_attack_cooldown = 0;
        }

        map_wide_attack_cooldown -= time;
        if (map_wide_attack_cooldown <= 0)
        {
            map_wide_attack_cooldown = 0;
        }

        throwing_snowball_cooldown -= time;
        if (throwing_snowball_cooldown <= 0)
        {
            throwing_snowball_cooldown = 0;
        }

        jump_cooldown -= time;
        if (jump_cooldown <= 0)
        {
            jump_cooldown = 0;
        }

        dash_cooldown -= time;
        if (dash_cooldown <= 0)
        {
            dash_cooldown = 0;
        }

        fireball_cooldown -= time;
        if (fireball_cooldown <= 0)
        {
            fireball_cooldown = 0;
        }

        melee_cooldown -= time;
        if (melee_cooldown <= 0)
        {
            melee_cooldown = 0;
        }

        meteor_cooldown -= time;
        if (meteor_cooldown <= 0)
        {
            meteor_cooldown = 0;
        }

        reflection_cooldown -= time;
        if (reflection_cooldown <= 0)
        {
            reflection_cooldown = 0;
        }

        flamethrower_cooldown -= time;
        if (flamethrower_cooldown <= 0)
        {
            flamethrower_cooldown = 0;
        }

        fire_background_cooldown -= time;
        if (fire_background_cooldown <= 0)
        {
            fire_background_cooldown = 0;
        }

        fire_wall_cooldown -= time;
        if (fire_wall_cooldown <= 0)
        {
            fire_wall_cooldown = 0;
        }

        all_attacks_cooldown -= time;
        if (all_attacks_cooldown <= 0)
        {
            all_attacks_cooldown = 0;
        }
    }
    public void end_attack()
    {
        float preset_all_attacks = 0f;
        can_flip_once = false;

        is_attacking = false;
        is_blocking = false;
        blocking_attack_hitbox.col.enabled = false;
        is_invincible = false;
        ani.SetBool("slashing_sword", false);
        ani.SetBool("making_sword", false);
        ani.SetBool("dashing_with_sword", false);
        for (int x = 0; x < 6; x++)
        {
            swordy.deactivate_hitbox(x);
        }
        stop_dash();
        ani.SetBool("creating_ice_spike", false);
        ani.SetBool("creating_many_ice_spikes", false);
        ani.SetBool("throwing_snowball", false);
        ani.SetBool("is_body_spiking", false);
        ani.SetBool("is_blocking", false);
        ani.SetBool("throwing_spiral", false);
        ani.SetBool("air_attack", false);
        ani.SetBool("is_map_attacking", false);

        ani.SetBool("is_jumping", false);
        ani.SetBool("is_diving", false);
        dive_attack.DisableHitbox();
        ani.SetBool("fire_background_attacking", false);
        if (current_fire_background != null)
        {
            Destroy(current_fire_background.gameObject);
        }
        ani.SetBool("is_flamethrowing", false);
        ani.SetBool("is_dashing", false);
        if (current_fire_trail != null)
        {
            stop_dash_P2();
            Destroy(current_fire_trail.gameObject, 0.5f);
        }
        ani.SetBool("is_reflecting", false);
        ani.SetBool("creating_fire_spike", false);
        ani.SetBool("creating_many_fire_spikes", false);
        ani.SetBool("creating_fire_wall", false);
        ani.SetBool("is_melee_attacking", false);
        if (ani.GetBool("creating_meteors"))
        {
            preset_all_attacks = 2.5f;
        }
        ani.SetBool("creating_meteors", false);
        ani.SetBool("throwing_fireballs", false);
        ani.SetBool("actually_map_attacking_fire", false);

        if (preset_all_attacks == 0f)
        {
            all_attacks_cooldown = 1.0f;
        }
        else
        {
            all_attacks_cooldown = preset_all_attacks;
        }
    }
    public void start_generating_sword()
    {
        is_attacking = true;
        ani.SetBool("making_sword", false);
    }
    public void finished_generating_sword()
    {
        ani.SetBool("dashing_with_sword", true);
    }
    public void start_dash()
    {
        is_dashing = true;
    }
    public void stop_dash()
    {
        is_dashing = false;
        rb.linearVelocityX = 0;
    }
    public void throw_sword()
    {
        Vector2 throw_pos = throwing_sword_point.transform.position;
        Vector2 player_pos = level_manager_behavior.player.transform.position;

        Vector2 ray_to_player = new Vector2(player_pos.x - throw_pos.x, player_pos.y - throw_pos.y);
        //Debug.DrawLine(snipe_position, player_position, Color.red);
        float calc_rotation = Mathf.Atan2(ray_to_player.y, ray_to_player.x) * Mathf.Rad2Deg;

        GameObject thrown_sword = Instantiate(throwing_sword, throwing_sword_point.transform.position, Quaternion.Euler(0, 0, calc_rotation));
        var x = thrown_sword.GetComponent<IceSwordBehavior>();
        //float speed = 10 * (sprite.flipX ? 1 : -1);
        x.initialize(5, 15);
    }
    public void talk_to_slashes(int activate) // Ugh... okay - key: 0 = deactivate hitbox 1, 10 = activate hitbox 1.
    {
        if (activate / 10 == 0)
        {
            swordy.deactivate_hitbox(activate % 10);
        }
        else if (activate / 10 == 1)
        {
            swordy.activate_hitbox(activate % 10);
        }
    }
    public void grab_current_player_location()
    {
        known_player_position = level_manager_behavior.player.transform.position;
    }
    public void raise_ice_spike()
    {
        is_attacking = true;
        IceSpikeBehavior ice_spikey = Instantiate(ice_spike, new Vector2(known_player_position.x, -4.5f), Quaternion.Euler(0, 0, 0)).GetComponent<IceSpikeBehavior>();
        ice_spikey.can_fling_player = true;
    }
    public void raise_many_ice_spikes()
    {
        /*for (int i = -24; i <= 24; i++)
        {
            float difference = known_player_position.x - (i / 2f);
            if (Mathf.Abs(difference) < 0.5f)
            {
                continue;
            }
            Instantiate(ice_spike, new Vector2(i / 2f, -4.5f), Quaternion.Euler(0, 0, 0));
        }*/
        is_attacking = true;
        for (int i = -12; i <= 12; i++)
        {
            float difference = known_player_position.x - i;
            if (Mathf.Abs(difference) < 1.4f)
            {
                continue;
            }
            Instantiate(ice_spike, new Vector2(i, -4.5f), Quaternion.Euler(0, 0, 0));
        }
    }
    public void throw_snowball()
    {
        is_attacking = true;
        Vector2 throw_pos = throwing_sword_point.transform.position;
        Vector2 player_pos = level_manager_behavior.player.transform.position;

        Vector2 ray_to_player = new Vector2(player_pos.x - throw_pos.x, player_pos.y - throw_pos.y);
        float calc_rotation = Mathf.Atan2(ray_to_player.y, ray_to_player.x) * Mathf.Rad2Deg;

        SnowballBehavior hi = Instantiate(snowball, throwing_sword_point.transform.position, Quaternion.Euler(0, 0, calc_rotation)).GetComponent<SnowballBehavior>();
        //float speed = 10 * (sprite.flipX ? 1 : -1);
        hi.initialize(5, 15);
    }
    public void activate_body_spikes()
    {
        is_attacking = true;
        ani.SetBool("hit_while_blocking", false);
        is_blocking = false;
        body_spikes.clearFlags();
        body_spikes.disableAttackCycle = false;
        body_spikes.EnableHitbox();
    }
    public void deactivate_body_spikes()
    {
        body_spikes.clearFlags();
        body_spikes.disableAttackCycle = false;
        body_spikes.DisableHitbox();
    }
    public void start_blocking_attacks()
    {
        is_blocking = true;
        is_attacking = true;
        blocking_attack_hitbox.col.enabled = true;
    }
    public void stop_blocking_attacks()
    {
        is_blocking = false;
        blocking_attack_hitbox.col.enabled = false;
        end_attack();
    }
    public void block_started_hitting()
    {
        ani.SetBool("hit_while_blocking", true);
        if (flip_cooldown < 0.5f)
        {
            flip_everything();
        }
    }
    public void block_finished_hitting()
    {
        ani.SetBool("hit_while_blocking", false);
    }
    public void throw_spiral()
    {
        float direction = (level_manager_behavior.player.transform.position.x > transform.position.x) ? 8 : -8;
        SpiralBehavior s = Instantiate(spiral, new Vector2(throwing_sword_point.transform.position.x, -3.8f), Quaternion.Euler(0, 0, 0)).GetComponent<SpiralBehavior>();
        s.initialize(5, direction);
    }
    public void summon_crystals()
    {
        CrystalBehavior c1 = Instantiate(crystal, transform.position, Quaternion.Euler(0, 0, 0)).GetComponent<CrystalBehavior>();
        c1.Initialize(10, 10, 315, 0, false, level_manager_behavior.player,gameObject);

        CrystalBehavior c2 = Instantiate(crystal, transform.position, Quaternion.Euler(0, 0, 0)).GetComponent<CrystalBehavior>();
        c2.Initialize(10, 10, 315, 2f, false, level_manager_behavior.player, gameObject);

        CrystalBehavior c3 = Instantiate(crystal, transform.position, Quaternion.Euler(0, 0, 0)).GetComponent<CrystalBehavior>();
        c3.Initialize(10, 10, 315, -2f, false, level_manager_behavior.player, gameObject);

        //CrystalBehavior c4 = Instantiate(crystal, transform.position, Quaternion.Euler(0, 0, 0)).GetComponent<CrystalBehavior>();
        //c4.Initialize(10, 10, 315, -4f, false, level_manager_behavior.player, gameObject);

        CrystalBehavior c5 = Instantiate(crystal, transform.position, Quaternion.Euler(0, 0, 0)).GetComponent<CrystalBehavior>();
        c5.Initialize(10, 10, 225, 0, true, level_manager_behavior.player, gameObject);

        CrystalBehavior c6 = Instantiate(crystal, transform.position, Quaternion.Euler(0, 0, 0)).GetComponent<CrystalBehavior>();
        c6.Initialize(10, 10, 225, 2f, true, level_manager_behavior.player, gameObject);

        CrystalBehavior c7 = Instantiate(crystal, transform.position, Quaternion.Euler(0, 0, 0)).GetComponent<CrystalBehavior>();
        c7.Initialize(10, 10, 225, -2f, true, level_manager_behavior.player, gameObject);

        //CrystalBehavior c8 = Instantiate(crystal, transform.position, Quaternion.Euler(0, 0, 0)).GetComponent<CrystalBehavior>();
        //c8.Initialize(10, 10, 225, -4f, true, level_manager_behavior.player, gameObject);
    }
    public void create_map_attack()
    {
        is_invincible = true;
        current_ice_map_attack = Instantiate(ice_map_attack);
        current_ice_map_attack.GetComponent<TestBoss4MapAttackBehavior_Ice>().initialize(gameObject, this, level_manager_behavior.player);
        Invoke("do_actual_map_attack", 2.5f);
    }
    public void do_actual_map_attack()
    {
        current_ice_map_attack.GetComponent<TestBoss4MapAttackBehavior_Ice>().do_attack();
        Destroy(current_ice_map_attack, 0.5f);
        is_invincible = false;
        Invoke("end_attack", 0.5f);
    }
    public void phase_change()
    {
        box.enabled = false;
        phase += 1;
        is_invincible = true;
        ani.SetBool("phase_change", true);
        ani.SetInteger("phase", phase);
        end_attack();
    }
    public void start_changing_colors()
    {
        level_manager_behavior.start_color_changing = true;
    }
    public void phase_change_finished()
    {
        ani.SetBool("phase_change", false);
        end_attack();
        box.enabled = true;

        last_spikes_was_multiple = true;
        raising_spike_cooldown = Random.Range(5f, 10f);
        reflection_cooldown = 25f;
        flamethrower_cooldown = 5f;
        map_wide_attack_cooldown = 20f;
        jump_cooldown = 0f;
        dash_cooldown = 3f;
        melee_cooldown = 5f;
        body_attack_cooldown = 5f;
        meteor_cooldown = 10f;
        fireball_cooldown = Random.Range(2f, 6f);
        fire_background_cooldown = 4f;
        fire_wall_cooldown = 10f;
    }
    public void stun_me(float f)
    {
        if (inability_time > 0)
            return;
        else
        {
            end_attack();
            stun_time_phase_1 = f;
        }
    }
    public void burn_me(float f)
    {
        if (burn_time > 0)
            return;
        else
        {
            burn_time = f;
        }
    }
    public void blind_me(float f)
    {
        if (inability_time > 0)
            return;
        else
        {
            end_attack();
            blind_time = f;
        }
    }
    public void freeze_me(float f)
    {
        if (inability_time > 0)
            return;
        else
        {
            end_attack();
            freeze_time = f;
        }
    }
    public void blinded_first_time()
    {
        ani.SetBool("is_blinded_first_time", false);
    }
    public void start_blocking_attacks_P2()
    {
        is_blocking = true;
        is_attacking = true;
        blocking_attack_hitbox.col.enabled = true;
        blocking_attack_hitbox.gameObject.tag = "Reflective";
    }
    public void stop_blocking_attacks_P2()
    {
        is_blocking = false;
        blocking_attack_hitbox.gameObject.tag = "Untagged";
        blocking_attack_hitbox.col.enabled = false;
        end_attack();
    }
    public void roll_attacking_phase_1()
    {
        if (!is_attacking && all_attacks_cooldown <= 0f && phase == 0 && inability_time <= 0 && !phase_change_queued)
        {
            bool made_a_move = false;

            if (map_wide_attack_cooldown <= 0f)
            {
                /*if (flip_cooldown < 0.5f)
                {
                    flip_everything(!sprite.flipX);
                }*/
                map_wide_attack_cooldown = 40f;
                ani.SetBool("is_map_attacking", true);
                made_a_move = true;
            }
            else if (blocking_cooldown <= 0f)
            {
                /*if (flip_cooldown < 0.5f)
                {
                    flip_everything(!sprite.flipX);
                }*/
                blocking_cooldown = 40f;
                ani.SetBool("is_blocking", true);
                made_a_move = true;
            }
            else if (raising_spike_cooldown <= 0f)
            {
                raising_spike_cooldown = Random.Range(10f, 15f);
                if (last_spikes_was_multiple)
                {
                    ani.SetBool("creating_ice_spike", true);
                    last_spikes_was_multiple = false;
                }
                else
                {
                    ani.SetBool("creating_many_ice_spikes", true);
                    last_spikes_was_multiple = true;
                }
                made_a_move = true;
            }
            else if (sword_slash_cooldown <= 0f)
            {
                sword_slash_cooldown = Random.Range(20f, 25f);
                ani.SetBool("making_sword", true);
                can_flip_once = true;
                made_a_move = true;
            }
            else if (body_attack_cooldown <= 0f && player_in_range)
            {
                body_attack_cooldown = 5.0f;
                ani.SetBool("is_body_spiking", true);
                made_a_move = true;
            }
            else if (air_attack_cooldown <= 0f)
            {
                air_attack_cooldown = Random.Range(8f, 12f);
                ani.SetBool("air_attack", true);
                made_a_move = true;
            }
            else if (ground_attack_cooldown <= 0f)
            {
                ground_attack_cooldown = Random.Range(8f, 12f);
                ani.SetBool("throwing_spiral", true);
                can_flip_once = true;
                made_a_move = true;
            }
            else if (throwing_snowball_cooldown <= 0f)
            {
                throwing_snowball_cooldown = Random.Range(8f, 12f);
                ani.SetBool("throwing_snowball", true);
                can_flip_once = true;
                made_a_move = true;
            }

            if (made_a_move)
            {
                is_attacking = true;
                //is_moving = false;
            }
        }
    }
    public void roll_attacking_phase_2()
    {
        if (!is_attacking && all_attacks_cooldown <= 0f && phase == 1 && inability_time <= 0 && !phase_change_queued)
        {
            bool made_a_move = false;

            if (map_wide_attack_cooldown <= 0f)
            {
                /*if (flip_cooldown < 0.5f)
                {
                    flip_everything(!sprite.flipX);
                }*/
                map_wide_attack_cooldown = 60f;
                is_invincible = true;
                ani.SetBool("is_map_attacking", true);
                made_a_move = true;
            }
            else if (reflection_cooldown <= 0f)
            {
                /*if (flip_cooldown < 0.5f)
                {
                    flip_everything(!sprite.flipX);
                }*/
                reflection_cooldown = 60f;
                is_invincible = true;
                ani.SetBool("is_reflecting", true);
                made_a_move = true;
            }
            else if (raising_spike_cooldown <= 0f)
            {
                raising_spike_cooldown = Random.Range(25f, 30f);
                can_flip_once = true;
                if (last_spikes_was_multiple)
                {
                    ani.SetBool("creating_fire_spike", true);
                    last_spikes_was_multiple = false;
                }
                else
                {
                    ani.SetBool("creating_many_fire_spikes", true);
                    last_spikes_was_multiple = true;
                }
                made_a_move = true;
            }
            else if (fire_wall_cooldown <= 0f)
            {
                fire_wall_cooldown = 60f;
                is_invincible = true;
                can_flip_once = true;
                ani.SetBool("creating_fire_wall", true);
                made_a_move = true;
            }
            else if (meteor_cooldown <= 0f)
            {
                meteor_cooldown = Random.Range(30f, 35f);
                ani.SetBool("creating_meteors", true);
                made_a_move = true;
            }
            else if (fire_background_cooldown <= 0f)
            {
                fire_background_cooldown = Random.Range(25f, 30f);
                ani.SetBool("fire_background_attacking", true);
                can_flip_once = true;
                made_a_move = true;
            }
            else if (jump_cooldown <= 0f)
            {
                jump_cooldown = Random.Range(25f, 30f);
                ani.SetBool("is_jumping", true);
                is_invincible = true;
                made_a_move = true;
            }
            else if (melee_cooldown <= 0f && player_in_range)
            {
                melee_cooldown = 15f;
                can_flip_once = true;
                ani.SetBool("is_melee_attacking", true);
                made_a_move = true;
            }
            else if (dash_cooldown <= 0f)
            {
                dash_cooldown = Random.Range(30f, 35f);
                ani.SetBool("is_dashing", true);
                made_a_move = true;
            }
            else if (flamethrower_cooldown <= 0f)
            {
                flamethrower_cooldown = Random.Range(20f, 25f);
                ani.SetBool("is_flamethrowing", true);
                can_flip_once = true;
                made_a_move = true;
            }
            else if (fireball_cooldown <= 0f)
            {
                fireball_cooldown = 10f;
                ani.SetBool("throwing_fireballs", true);
                can_flip_once = true;
                made_a_move = true;
            }

            if (made_a_move)
            {
                is_attacking = true;
            }
        }
    }
    public void throw_fireball(float f)
    {
        if (flip_cooldown < 0.5f)
        {
            flip_everything();
        }
        Vector2 throw_pos = throwing_sword_point.transform.position;
        Vector2 player_pos = level_manager_behavior.player.transform.position;

        Vector2 ray_to_player = new Vector2(player_pos.x - throw_pos.x, player_pos.y - throw_pos.y);
        float calc_rotation = Mathf.Atan2(ray_to_player.y, ray_to_player.x) * Mathf.Rad2Deg;

        if (!ani.GetBool("is_reflecting"))
        {
            TestBoss4ThrowingFireballBehavior hi = Instantiate(throwing_fireball, throwing_sword_point.transform.position, Quaternion.Euler(0, 0, calc_rotation + 180)).GetComponent<TestBoss4ThrowingFireballBehavior>();
            hi.initialize(5, f);
        }
        else
        {
            TestBoss4ThrowingFireballBehavior hi = Instantiate(throwing_fireball, pitchfork_attack_point.transform.position, Quaternion.Euler(0, 0, calc_rotation + 180)).GetComponent<TestBoss4ThrowingFireballBehavior>();
            hi.initialize(5, f);
        }
        //float speed = 10 * (sprite.flipX ? 1 : -1);
    }
    public void start_jump()
    {
        is_jumping = true;
    }
    public void stop_dive()
    {
        rb.linearVelocityY = 0;
        is_diving = false;
        ani.SetBool("is_diving", false);
    }
    public void create_background_fire()
    {
        current_fire_background = Instantiate(fire_background, new Vector2(0,-4.5f), Quaternion.Euler(0, 0, 0)).GetComponent<TestBoss4FireBackgroundBehavior>();
        current_fire_background.sprite.size = new Vector2(25, 1.28f);
    }
    public void raise_background_fire(float f)
    {
        current_fire_background.change_scale(f);
    }
    public void disable_background_fire()
    {
        current_fire_background.attack.DisableHitbox();
    }
    public void start_flamethrowing()
    {
        flamethrower_attack.clearFlags();
        flamethrower_attack.disableAttackCycle = false;
        flamethrower_attack.EnableHitbox();
    }
    public void stop_flamethrowing()
    {
        flamethrower_attack.DisableHitbox();
    }
    public void create_meteor()
    {
        int left_or_right = Random.Range(0, 2);

        if (left_or_right == 0)
        {
            MeteorBehavior m1 = Instantiate(meteor, new Vector2(-15, 12), Quaternion.Euler(0, 0, 0)).GetComponent<MeteorBehavior>();
            m1.falling_rightwards = true;

            MeteorBehavior m2 = Instantiate(meteor, new Vector2(-9, 13), Quaternion.Euler(0, 0, 0)).GetComponent<MeteorBehavior>();
            m2.falling_rightwards = true;

            MeteorBehavior m3 = Instantiate(meteor, new Vector2(-23, 14), Quaternion.Euler(0, 0, 0)).GetComponent<MeteorBehavior>();
            m3.falling_rightwards = true;
        }
        else
        {
            MeteorBehavior m1 = Instantiate(meteor, new Vector2(15, 12), Quaternion.Euler(0, 0, 0)).GetComponent<MeteorBehavior>();
            m1.falling_rightwards = false;

            MeteorBehavior m2 = Instantiate(meteor, new Vector2(9, 13), Quaternion.Euler(0, 0, 0)).GetComponent<MeteorBehavior>();
            m2.falling_rightwards = false;

            MeteorBehavior m3 = Instantiate(meteor, new Vector2(23, 14), Quaternion.Euler(0, 0, 0)).GetComponent<MeteorBehavior>();
            m3.falling_rightwards = false;
        }
    }
    public void start_dash_P2()
    {
        ani.SetBool("is_dashing", false);
        dash_target_x = sprite.flipX ? 9 : -9;
        dash_starting_hit.clearFlags();
        dash_starting_hit.disableAttackCycle = false;
        dash_starting_hit.EnableHitbox();
        current_fire_trail = Instantiate(dash_fire_trail, new Vector2(transform.position.x, -3.0f), Quaternion.Euler(0, 0, 0)).GetComponent<FireTrailBehavior>();
        current_fire_trail.start = new Vector2(transform.position.x, -3.0f);
        is_dashing = true;
    }
    public void stop_dash_P2()
    {
        current_fire_trail.attack.EnableHitbox();
        is_dashing = false;
    }
    public void update_dash()
    {
        dash_starting_hit.DisableHitbox();
        current_fire_trail.target = new Vector2(transform.position.x, -3.0f);
    }
    public void create_fire_walls()
    {
        fire_wall_left = Instantiate(fire_wall, new Vector2(-9, 0.6f), Quaternion.Euler(0,0,0)).GetComponent<TestBoss4FirewallBehavior>();
        fire_wall_right = Instantiate(fire_wall, new Vector2(9, 0.6f), Quaternion.Euler(0,0,0)).GetComponent<TestBoss4FirewallBehavior>();

        fire_wall_left.initialize(40, level_manager_behavior.player, true);

        fire_wall_right.initialize(40, level_manager_behavior.player, false);
        fire_wall_right.sprite.flipX = true;
    }

    public void fire_wall_rise_begin()
    {
        is_invincible = false;
        fire_wall_left.start_growing();
        fire_wall_right.start_growing();
    }
    public void talk_to_melee(int activate) // Key: 0 = deactivate hitbox one; 10 = activate hitbox one.
    {
        int ten = activate / 10;
        int remainder = activate % 10;

        if (ten == 0)
        {
            if (remainder == 0)
            {
                melee_attack_1.DisableHitbox();
                melee_attack_1.clearFlags();
                melee_attack_1.disableAttackCycle = false;
            }
            else if (remainder == 1)
            {
                melee_attack_2.DisableHitbox();
                melee_attack_2.clearFlags();
                melee_attack_2.disableAttackCycle = false;
            }
            else if (remainder == 2)
            {
                melee_attack_3.DisableHitbox();
                melee_attack_3.clearFlags();
                melee_attack_3.disableAttackCycle = false;
            }
        }
        else
        {
            if (remainder == 0)
            {
                melee_attack_1.EnableHitbox();
            }
            else if (remainder == 1)
            {
                melee_attack_2.EnableHitbox();
            }
            else if (remainder == 2)
            {
                melee_attack_3.EnableHitbox();
            }
        }
    }
    public void raise_fire_spike()
    {
        is_attacking = true;
        FireSpikeBehavior fire_spikey = Instantiate(fire_spike, new Vector2(known_player_position.x, -4.5f), Quaternion.Euler(0, 0, 0)).GetComponent<FireSpikeBehavior>();
        fire_spikey.can_fling_player = true;
    }
    public void raise_many_fire_spikes()
    {
        /*for (int i = -24; i <= 24; i++)
        {
            float difference = known_player_position.x - (i / 2f);
            if (Mathf.Abs(difference) < 0.5f)
            {
                continue;
            }
            Instantiate(ice_spike, new Vector2(i / 2f, -4.5f), Quaternion.Euler(0, 0, 0));
        }*/
        is_attacking = true;
        for (int i = -12; i <= 12; i++)
        {
            float difference = known_player_position.x - i;
            if (Mathf.Abs(difference) < 1.4f)
            {
                continue;
            }
            Instantiate(fire_spike, new Vector2(i, -4.5f), Quaternion.Euler(0, 0, 0));
        }
    }
    public void start_map_attacking()
    {
        transform.position = new Vector2(0, 1);
        fireball_patterns_shot = 0;
        ani.SetBool("actually_map_attacking_fire", true);
        ani.SetBool("is_map_attacking", false);
    }
    public void map_attacking_perform()
    {
        if (fireball_patterns_shot >= 8)
        {
            ani.SetBool("actually_map_attacking_fire", false);
            end_attack();
            transform.position = new Vector2(0, -2.56f);
            return;
        }

        int pattern = Random.Range(0, 4);

        if (pattern == 0)
        {
            for (int i = 0; i < 8; i++)
            {
                MiddleFireballBehavior mf1 = Instantiate(middle_fireball, transform.position, Quaternion.Euler(0,0,0)).GetComponentInChildren<MiddleFireballBehavior>();
                mf1.initialize(5, 4, i * 45);
            }
        }
        else if (pattern == 1)
        {
            for (int i = 0; i < 8; i++)
            {
                MiddleFireballBehavior mf1 = Instantiate(middle_fireball, transform.position, Quaternion.Euler(0, 0, 0)).GetComponentInChildren<MiddleFireballBehavior>();
                mf1.initialize(5, 4, (i * 45) + 22.5f);
            }
        }
        else if (pattern == 2)
        {
            for (int i = 0; i < 8; i++)
            {
                MiddleFireballBehavior mf1 = Instantiate(middle_fireball, transform.position, Quaternion.Euler(0, 0, 0)).GetComponentInChildren<MiddleFireballBehavior>();
                mf1.initialize(5, 4, i * 45, 1);
            }
        }
        else if (pattern == 3)
        {
            for (int i = 0; i < 8; i++)
            {
                MiddleFireballBehavior mf1 = Instantiate(middle_fireball, transform.position, Quaternion.Euler(0, 0, 0)).GetComponentInChildren<MiddleFireballBehavior>();
                mf1.initialize(5, 4, i * 45, -1);
            }
        }

        fireball_patterns_shot += 1;
    }
    public void OnTriggerStay2D(Collider2D col)
    {
        if (is_diving && ((1 << col.gameObject.layer) & ground_layer) != 0)
        {
            MeteorStrikeGround attempt = col.GetComponent<MeteorStrikeGround>();
            if (attempt != null)
            {
                stop_dive();
                transform.position = new Vector2(transform.position.x, -2.56f);
                if (level_manager_behavior.player.GetComponent<PlayerMovement>().onGround)
                {
                    level_manager_behavior.player_life.Hit_No_Collider(attempt.gameObject);
                }
                var maybe = GameManager.Instance.current_camera.GetComponent<CameraShake>();
                if (maybe != null)
                {
                    maybe.shakeOnHurt(0.5f);
                }
            }
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
            player_attack.isBlock();
            return;
        }
        if (is_blocking)
        {
            ani.SetBool("hit_while_blocking", true);
            return;
        }
        else
        {
            player_attack.isHit();
            if (burn_time > 0)
            {
                player_attack.damage = Mathf.RoundToInt(player_attack.damage * 1.2f);
            }
            TakeDamage(player_attack.damage);
            /*if (burn_time > 0)
            {
                player_attack.damage = Mathf.RoundToInt(player_attack.damage / 1.2f);
            }*/

            if (phase == 0)
            {
                if (player_attack.stunDuration > 0)
                {
                    stun_me(player_attack.stunDuration);
                }
                if (player_attack.burnDuration > 0)
                {
                    burn_me(player_attack.burnDuration);
                }
            }
            if (phase == 1)
            {
                if (player_attack.freezeDuration > 0)
                {
                    freeze_me(player_attack.freezeDuration);
                }
            }
            if (player_attack.blindDuration > 0)
            {
                blind_me(player_attack.blindDuration);
            }
        }
    }
    public void TakeDamage(int damage)
    {
        if (is_dead)
        {
            return;
        }

        if (phase == 0 && health - damage <= starting_health / 2)
        {
            health = starting_health / 2;
            end_attack();
            phase_change_queued = true;
            is_invincible = true;
        }
        else
        {
            health -= damage;
            if (health <= 0)
            {
                health = 0;
                end_attack();
                is_dead = true;
            }
        }
        /*if (health <= starting_health / 2 && !phase_changed)
        {
            phase_change = true;
        }*/
    }
}

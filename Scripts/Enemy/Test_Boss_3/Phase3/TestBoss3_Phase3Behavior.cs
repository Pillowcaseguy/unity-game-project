using UnityEngine;

public class TestBoss3_Phase3Behavior : MonoBehaviour,EnemyDamageable
{
    public Animator ani;
    public Rigidbody2D rb;
    public BoxCollider2D box;
    public SpriteRenderer sprite;

    public GameObject level_manager;
    public LevelManager_TestBoss3_Phase3 level_manager_behavior;

    public GameObject ground_panel_left;
    public GameObject ground_panel_right;
    public GroundPanelBehavior_P3 ground_panel_behavior_left;
    public GroundPanelBehavior_P3 ground_panel_behavior_right;

    public GameObject lifted_panel_left;
    public GameObject lifted_panel_right;
    public LiftedPanelBehavior_P3 lifted_panel_behavior_left;
    public LiftedPanelBehavior_P3 lifted_panel_behavior_right;

    public GameObject spikes;
    public GameObject spikes_creation_point;
    public SpikesBehavior_P3 current_spikes;

    public bool spikes_attack_hittable;

    public GameObject laser_eye_left;
    public GameObject laser_eye_right;

    public GameObject laser_eye_left_shooting;
    public GameObject laser_eye_right_shooting;

    public bool is_dead;

    public float panel_cooldown;
    public float laser_cooldown;
    public float blind_cooldown;
    public float super_attack_cooldown;
    public float electrify_ground_cooldown;
    public float crate_cooldown;
    public float shield_creation_cooldown;
    public float all_attacks_cooldown;

    //public float try_healing_player_cooldown;

    public int super_attack_laser = 1; 
    public int super_attack_dive = 1; 
    public int super_attack_energy = 1;
    public bool super_attacking;
    public int current_super_attack;

    public GameObject afterimage;

    public GameObject panel_lift_point_left;
    public GameObject panel_lift_point_right;

    public EyeLaserBehavior_P3 left_eye_laser;
    public EyeLaserBehavior_P3 right_eye_laser;

    public bool phase_change_queued;
    public bool phase_change_done;

    public bool is_moving;
    public bool can_flip;
    public float flip_cooldown;
    public bool is_attacking;
    public bool is_invincible;

    public bool query_flippable;

    public int health;
    public int maximum_health = 2000;

    public GameObject energy_ball;
    public GameObject energy_ball_impact;

    public GameObject diving_boss;
    public GameObject current_diving_boss;
    public DivingBossBehavior diving_boss_behavior;
    public GameObject dive_impact;

    public ElectrifiedGroundCollective electric_ground_collective;

    public TestBoss3_Phase3WeaknessField weak_field;

    public GameObject shield_bubble;
    public GameObject current_shield_bubble;
    public ShieldBubbleBehavior shield_bubble_behavior;

    public GameObject high_attack_point;
    public GameObject laser_projectile;
    public GameObject crate;

    /*public AnimatorOverrideController aoc;

    public AnimationClip energy_facing_left;
    public AnimationClip energy_facing_left_lower;
    public AnimationClip energy_facing_right;
    public AnimationClip energy_facing_right_lower;*/

    public int energy_shot = 0;
    void Start()
    {
        health = maximum_health;
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        //aoc = new AnimatorOverrideController(ani.runtimeAnimatorController);
        //ani.runtimeAnimatorController = aoc;

        health = maximum_health;
        flip_cooldown = 0.5f;
        panel_cooldown = Random.Range(5f, 15f);
        laser_cooldown = Random.Range(10f, 20f);
        electrify_ground_cooldown = Random.Range(5f, 10f);
        crate_cooldown = Random.Range(5f, 10f);
        blind_cooldown = Random.Range(8f, 15f);
        shield_creation_cooldown = 8f;
        super_attack_cooldown = 30f;
        //super_attack_cooldown = 999f;
        //try_healing_player_cooldown = 30f;
        is_moving = true;
        left_eye_laser.facing_left = true;
        right_eye_laser.facing_left = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (is_dead)
        {
            is_moving = false;
            return;
        }

        float f = Time.deltaTime;
        reduce_timers(f);

        can_flip = !is_attacking;

        if (query_flippable)
            can_flip = true;

        if (left_eye_laser.activated && right_eye_laser.activated)
        {
            if (left_eye_laser.finished_loop && right_eye_laser.finished_loop)
            {
                deactivate_lasers();
                end_super_attack();
            }
        }

        if (energy_shot >= 12)
        {
            end_super_attack();
            energy_shot = 0;
        }

        if (current_diving_boss != null)
        {
            sprite.enabled = false;
            if (diving_boss_behavior.hit_ground)
            {
                end_super_attack();
            }
        }

        if (current_shield_bubble != null)
        {
            current_shield_bubble.transform.position = transform.position;
            if (shield_bubble_behavior.sprite != null)
                shield_bubble_behavior.sprite.flipX = sprite.flipX ? true : false;
        }

        if (lifted_panel_behavior_left != null && lifted_panel_behavior_right != null)
        {
            ani.SetBool("is_raising", false);
            ani.SetBool("is_throwing", true);
        }

        if (!(level_manager == null || !level_manager_behavior.has_player)) // Flippable
        {
            if (level_manager_behavior.player.transform.position.x > gameObject.transform.position.x && !sprite.flipX)
            {
                if (count_down_flip(f))
                {
                    if (can_flip)
                    {
                        flip_everything(true);
                    }
                }
            }
            else if (level_manager_behavior.player.transform.position.x < gameObject.transform.position.x && sprite.flipX)
            {
                if (count_down_flip(f))
                {
                    if (can_flip)
                    {
                        flip_everything(false);
                    }
                }
            }
            else
            {
                flip_cooldown = 0.5f;
            }
        }

        if (!is_attacking && all_attacks_cooldown <= 0)
        {
            bool made_a_move = false;
            if (phase_change_queued)
            {
                laser_cooldown = 2f;
                panel_cooldown = 2f;
                electrify_ground_cooldown = 2f;
                super_attack_cooldown = 2f;
            }

            if (super_attack_cooldown <= 0f)
            {
                super_attack_cooldown = 60f;

                int random = Random.Range(0, 3);

                if (super_attack_dive == 0 && super_attack_energy == 0 && super_attack_laser == 0)
                {
                    super_attack_dive = super_attack_energy = super_attack_laser = 1;
                }

                if (random == 0) // Dive
                {
                    if (super_attack_dive == 0)
                    {
                        if (super_attack_energy == 0)
                        {
                            current_super_attack = 2;
                            super_attack_laser = 0;
                        }
                        else
                        {
                            current_super_attack = 1;
                            super_attack_energy = 0;
                        }
                    }
                    else
                    {
                        current_super_attack = random;
                        super_attack_dive = 0;
                    }
                }
                else if (random == 1) // Energy
                {
                    if (super_attack_energy == 0)
                    {
                        if (super_attack_laser == 0)
                        {
                            current_super_attack = 0;
                            super_attack_dive = 0;
                        }
                        else
                        {
                            current_super_attack = 2;
                            super_attack_laser = 0;
                        }
                    }
                    else
                    {
                        current_super_attack = random;
                        super_attack_energy = 0;
                    }
                }
                else // Laser
                {
                    if (super_attack_laser == 0)
                    {
                        if (super_attack_dive == 0)
                        {
                            current_super_attack = 1;
                            super_attack_energy = 0;
                        }
                        else
                        {
                            current_super_attack = 0;
                            super_attack_dive = 0;
                        }
                    }
                    else
                    {
                        current_super_attack = random;
                        super_attack_laser = 0;
                    }
                }

                start_super_attack();
                ani.SetInteger("current_super_attack", current_super_attack);
                ani.SetBool("is_super_attacking", true);
                super_attacking = true;
                made_a_move = true;
            }

            else if (laser_cooldown <= 0f)
            {
                laser_cooldown = Random.Range(15f, 25f);
                ani.SetBool("is_shooting", true);
                made_a_move = true;
            }
            else if (panel_cooldown <= 0f)
            {
                panel_cooldown = Random.Range(15f, 20f);
                ani.SetBool("is_raising", true);
                made_a_move = true;
            }
            else if (crate_cooldown <= 0f)
            {
                crate_cooldown = Random.Range(15f, 20f);
                ani.SetBool("is_slamming", true);
                made_a_move = true;
            }
            else if (electrify_ground_cooldown <= 0f)
            {
                electrify_ground_cooldown = Random.Range(15f, 20f);
                ani.SetBool("is_electrifying", true);
                made_a_move = true;
            }
            else if (blind_cooldown <= 0f)
            {
                blind_cooldown = Random.Range(12f, 18f);
                ani.SetBool("is_flashing", true);
                made_a_move = true;
            }
            else if (spikes_attack_hittable)
            {
                ani.SetBool("is_pushing", true);
                made_a_move = true;
            }

            if (made_a_move)
            {
                query_flippable = query_flipping();
                is_attacking = true;
                is_moving = false;
            }
        }

        ani.SetBool("is_moving", is_moving);
    }
    private void FixedUpdate()
    {
        if (is_moving)
        {
            if (sprite.flipX)
            {
                rb.linearVelocityX = 3f;
            }
            else
            {
                rb.linearVelocityX = -3f;
            }
        }
        else
        {
            rb.linearVelocityX = 0;
        }
    }
    public void Hit(PlayerAttack player_attack)
    {
        if (is_invincible)
        {
            return;
        }

        player_attack.isHit();

        if (weak_field.applying_weakness)
        {
            TakeDamage(player_attack.damage / 2);
        }
        else
        {
            TakeDamage(player_attack.damage);
        }
        
    }
    public void TakeDamage(int damage)
    {

        if (current_shield_bubble != null)
        {
            /*shield_bubble_behavior.damage(damage);
            if (shield_bubble_behavior.is_broken)
                current_shield_bubble = null;
            return;*/
            if (!shield_bubble_behavior.is_broken)
            {
                shield_bubble_behavior.damage(damage);
                return;
            }

        }
        else
        {
            if (shield_bubble_behavior)
                shield_bubble_behavior = null;
        }

        health -= damage;
        if (health < maximum_health / 2 && !phase_change_done && !phase_change_queued)
        {
            phase_change_queued = true;
            blind_cooldown = 0f;
        }

        if (health <= 0)
        {
            //end_attack();
            is_dead = true;
            ani.SetBool("is_dead", is_dead);
        }
    }
    public void flip_everything(bool yes_flip)
    {
        sprite.flipX = yes_flip;
        laser_eye_left.GetComponent<EyeLaserBehavior_P3>().Flip();
        laser_eye_right.GetComponent<EyeLaserBehavior_P3>().Flip();
        laser_eye_left_shooting.GetComponent<LaserShootSpotBehavior>().Flip();
        laser_eye_right_shooting.GetComponent<LaserShootSpotBehavior>().Flip();
    }
    public bool count_down_flip(float time)
    {
        flip_cooldown -= time;
        if (flip_cooldown <= 0f)
        {
            return true;
        }
        return false;
    }
    public bool query_flipping()
    {
        if (ani.GetBool("is_shooting") || ani.GetBool("is_super_attacking"))
        {
            return true;
        }
        return false;
    }
    public void reduce_timers(float f)
    {
        laser_cooldown -= f;
        if (laser_cooldown <= 0f)
        {
            laser_cooldown = 0f;
        }

        panel_cooldown -= f;
        if (panel_cooldown <= 0f)
        {
            panel_cooldown = 0f;
        }

        blind_cooldown -= f;
        if (blind_cooldown <= 0f)
        {
            blind_cooldown = 0f;
        }

        electrify_ground_cooldown -= f;
        if (electrify_ground_cooldown <= 0f)
        {
            electrify_ground_cooldown = 0f;
        }

        crate_cooldown -= f;
        if (crate_cooldown <= 0f)
        {
            crate_cooldown = 0f;
        }

        shield_creation_cooldown -= f;
        if (shield_creation_cooldown <= 0f && !super_attacking)
        {
            create_shield();
            shield_creation_cooldown = 40f;
        }

        if (!super_attacking)
        {
            super_attack_cooldown -= f;
        }
        if (super_attack_cooldown <= 0f)
        {
            super_attack_cooldown = 0f;
        }

        all_attacks_cooldown -= f;
        if (all_attacks_cooldown <= 0f)
        {
            all_attacks_cooldown = 0f;
        }

        /*try_healing_player_cooldown -= f;
        if (try_healing_player_cooldown <= 0f)
        {
            level_manager_behavior.player_life.Heal(1);
            try_healing_player_cooldown = 30f;
        }*/
    }
    public void end_attack()
    {

        is_attacking = false;
        is_moving = true;
        current_spikes = null;
        lifted_panel_left = null;
        lifted_panel_right = null;
        lifted_panel_behavior_left = null;
        lifted_panel_behavior_right = null;
        query_flippable = false;
        is_invincible = false;
        super_attacking = false;

        if (left_eye_laser.activated || right_eye_laser.activated)
        {
            deactivate_lasers();
        }

        all_attacks_cooldown = 1.5f;

        if (current_shield_bubble && !shield_bubble_behavior.sprite.enabled)
        {
            shield_bubble_behavior.sprite.enabled = true;
        }
        weak_field.override_show = false;
        sprite.color = Color.white;

        ani.SetBool("is_pushing", false);
        ani.SetBool("is_flashing", false);
        ani.SetBool("is_throwing", false);
        ani.SetBool("is_electrifying", false);
        ani.SetBool("is_slamming", false);
        ani.SetBool("ending_super_attack", false);
    }
    public void create_shield() 
    {
        if (current_shield_bubble && !shield_bubble_behavior.is_broken)
        {
            shield_bubble_behavior.shield_health = 100;
            shield_bubble_behavior.ani.SetBool("is_shining", true);
        }
        else
        {
            current_shield_bubble = Instantiate(shield_bubble,transform.position, Quaternion.Euler(0,0,0));
            shield_bubble_behavior = current_shield_bubble.GetComponent<ShieldBubbleBehavior>();
            shield_bubble_behavior.shield_max_health = 100;
        }
    }
    public void start_throwing_panel()
    {
        ground_panel_behavior_left = Instantiate(ground_panel_left, new Vector2(transform.position.x - 2.0f, -6.0f), Quaternion.Euler(0, 0, 0)).GetComponent<GroundPanelBehavior_P3>();
        ground_panel_behavior_left.the_boss = this;
        ground_panel_behavior_left.left = true;
        ground_panel_behavior_right = Instantiate(ground_panel_right, new Vector2(transform.position.x + 2.0f, -6.0f), Quaternion.Euler(0, 0, 0)).GetComponent<GroundPanelBehavior_P3>();
        ground_panel_behavior_right.the_boss = this;
        ground_panel_behavior_right.left = false;
    }
    public void grow_panel(float f)
    {
        if (ground_panel_left != null)
        {
            ground_panel_left.tag = "Reflective";
            ground_panel_behavior_left.target_height = f;
        }

        if (ground_panel_right != null)
        {
            ground_panel_right.tag = "Reflective";
            ground_panel_behavior_right.target_height = f;
        }
    }
    public void finished_lifting_panel()
    {
        ground_panel_behavior_left.destroy();
        ground_panel_behavior_right.destroy();
    }
    public void move_panel()
    {
        if (lifted_panel_left != null)
        {
            lifted_panel_behavior_left.step_to_point();
        }

        if (lifted_panel_right != null)
        {
            lifted_panel_behavior_right.step_to_point();
        }
    }
    public void snap_panel()
    {
        if (lifted_panel_left != null)
        {
            lifted_panel_behavior_left.go_to_target();
        }

        if (lifted_panel_right != null)
        {
            lifted_panel_behavior_right.go_to_target();
        }
    }
    public void actually_throw_panel()
    {
        if (lifted_panel_left != null)
        {
            lifted_panel_behavior_left.increase_phase();
        }

        if (lifted_panel_right != null)
        {
            lifted_panel_behavior_right.increase_phase();
        }
    }
    public void shoot_lasers()
    {
        float shoot_angle;
        if (level_manager_behavior.has_player)
        {
            shoot_angle = Mathf.Atan2(level_manager_behavior.player.transform.position.y - laser_eye_left_shooting.transform.position.y,
                level_manager_behavior.player.transform.position.x - laser_eye_left_shooting.transform.position.x) * Mathf.Rad2Deg;
        }
        else
        {
            shoot_angle = (sprite.flipX) ? 0 : 180;
        }

        Vector2 direction = Quaternion.Euler(0, 0, shoot_angle) * Vector2.right;
        Vector2 offset = direction * 3.2f;

        Vector2 new_position = (Vector2) laser_eye_left_shooting.transform.position + offset / 4f;

        LaserProjectileBehavior left_thingy = Instantiate(laser_projectile, new_position,
            Quaternion.Euler(0, 0, shoot_angle)).GetComponent<LaserProjectileBehavior>();
        left_thingy.initialize(15);
        //LaserProjectileBehavior right_thingy = Instantiate(laser_projectile, laser_eye_right.transform.position, Quaternion.Euler(0, 0, shoot_angle)).GetComponent<LaserProjectileBehavior>();
        //right_thingy.initialize(15);
    }
    public void finish_shooting()
    {
        ani.SetBool("is_shooting", false);
    }
    public void blind_attack()
    {
        if (phase_change_queued)
        {
            level_manager_behavior.player_life.Hit_No_Collider(gameObject);
            phase_change_queued = false;
            phase_change_done = true;
            return;
        }

        float player_distance = level_manager_behavior.player.transform.position.x - transform.position.x;
        bool facing_left = level_manager_behavior.player.GetComponent<PlayerMovement>().sprite_render.flipX;

        if (Mathf.Abs(player_distance) < 2.5f)
        {
            level_manager_behavior.player_life.Hit_No_Collider(gameObject);
            return;
        }

        if (player_distance <= -2.5f && !facing_left)
        {
            level_manager_behavior.player_life.Hit_No_Collider(gameObject);
            return;
        }

        else if (player_distance >= 2.5f && facing_left)
        {
            level_manager_behavior.player_life.Hit_No_Collider(gameObject);
            return;
        }
    }
    public void electrify_ground()
    {
        electric_ground_collective.start();
    }
    public void stop_electrify_ground()
    {
        electric_ground_collective.end();
    }
    public void drop_crate()
    {
        Instantiate(crate, new Vector2(level_manager_behavior.player.transform.position.x, 9.3f), Quaternion.Euler(0, 0, 0));
    }

    public void create_spikes()
    {
        var temp = Instantiate(spikes, spikes_creation_point.transform.position, Quaternion.Euler(0, 0, 0));
        current_spikes = temp.GetComponent<SpikesBehavior_P3>();
        current_spikes.initialize(3.84f, spikes_creation_point.transform.position.x, !sprite.flipX);
    }

    public void grow_spikes()
    {
        if (!current_spikes)
        {
            return;
        }
        current_spikes.increase_size();
    }

    public void color_spikes()
    {
        if (!current_spikes)
        {
            return;
        }
        current_spikes.sprite.color = Color.black;
        current_spikes.end.GetComponent<SpikesEndBehavior_P3>().sprite.color = Color.black;
    }
    public void destroy_spikes()
    {
        if (current_spikes)
        {
            current_spikes.destroy();
            current_spikes = null;
        }
    }
    public void start_super_attack()
    {
        is_invincible = true;

        if (current_shield_bubble != null) {
            shield_bubble_behavior.sprite.enabled = false;
        }

        weak_field.override_show = true;
        Instantiate(afterimage, transform.position, Quaternion.Euler(0, 0, 0));
        transform.position = high_attack_point.transform.position;

        if (current_super_attack == 0)
        {
            sprite.color = Color.yellow;
        }
        else if (current_super_attack == 1)
        {
            sprite.color = Color.rebeccaPurple;
        }
        else if (current_super_attack == 2)
        {
            sprite.color = Color.red;
        }
        else
        {
            sprite.color = Color.white;
        }

    }
    public void end_super_attack()
    {
        if (current_super_attack == 0)
        {
            if (diving_boss_behavior != null)
            {
                transform.position = new Vector2(current_diving_boss.transform.position.x, -3.6f);
                sprite.enabled = true;
                diving_boss_behavior.finish_dive();
                diving_boss_behavior = null;
            }
            else
            {
                transform.position = new Vector2(transform.position.x, -3.6f);
            }
        }

        else if (current_super_attack == 1)
        {
            Instantiate(afterimage, transform.position, Quaternion.Euler(0, 0, 0));
            transform.position = new Vector2(transform.position.x, -3.6f);
        }

        else if (current_super_attack == 2)
        {
            Instantiate(afterimage, transform.position, Quaternion.Euler(0, 0, 0));
            transform.position = new Vector2(transform.position.x, -3.6f);
        }
        ani.SetBool("is_super_attacking", false);
        ani.SetBool("ending_super_attack", true);
    }
    /*public void check_clip(int angle)
    {
        if (angle < 90)
        {

        }
    }*/
    public void create_energy_ball()
    {
        int the_angle = Random.Range(224, 316);
        //check_clip(the_angle);
        energy_shot += 1;
        Instantiate(energy_ball, transform.position, Quaternion.Euler(0, 0, the_angle));
    }
    public void do_dive()
    {
        float the_angle = Mathf.Atan2(-10,level_manager_behavior.player.transform.position.x - gameObject.transform.position.x) * Mathf.Rad2Deg + 360;
        current_diving_boss = Instantiate(diving_boss, transform.position, Quaternion.Euler(0, 0, the_angle));
        diving_boss_behavior = current_diving_boss.GetComponent<DivingBossBehavior>();
    }
    public void shoot_lasers_super()
    {
        left_eye_laser.start_rotation();
        left_eye_laser.activated = true;

        right_eye_laser.start_rotation();
        right_eye_laser.activated = true;

    }
    public void deactivate_lasers()
    {
        left_eye_laser.activated = false;
        left_eye_laser.finished_loop = false;
        right_eye_laser.activated = false;
        left_eye_laser.finished_loop = false;
    }
}

using Unity.VisualScripting;
using UnityEngine;

public class TestBoss3_Phase2Behavior : MonoBehaviour,EnemyDamageable
{

    public Animator ani;
    public Rigidbody2D rb;
    public BoxCollider2D box;
    public SpriteRenderer sprite;

    public int health;
    public int maximum_health = 1700;

    public bool is_moving;
    public bool can_flip;
    public float flip_cooldown;
    public bool is_attacking;

    public GameObject level_manager;
    public LevelManager_TestBoss3_Phase2 level_manager_behavior;

    public GameObject ground_panel;
    public GroundPanelBehavior ground_panel_behavior;

    public GameObject lifted_panel;
    public LiftedPanelBehavior lifted_panel_behavior;

    public GameObject spikes;
    public GameObject spikes_creation_point;
    public SpikesBehavior current_spikes;

    public bool spikes_attack_hittable;

    public GameObject laser_eye_left;
    public GameObject laser_eye_right;

    public bool high_attack_hittable;
    public EnemyAttack high_attack_attack;

    public bool is_dead;

    public float panel_cooldown;
    public float laser_cooldown;
    public float air_attack_cooldown;
    public float blind_cooldown;
    public float all_attacks_cooldown;

    public bool can_flip_once;

    public GameObject panel_lift_point;
    public Boss3Phase2PanelThrowPoint panel_lift_point_behavior;

    public EyeLaserBehavior left_eye_laser;
    public EyeLaserBehavior right_eye_laser;

    public bool phase_change_queued;
    public bool phase_change_done;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();

        health = maximum_health;
        flip_cooldown = 0.5f;
        panel_cooldown = Random.Range(5f,15f);
        laser_cooldown = Random.Range(10f,20f);
        air_attack_cooldown = 0f;
        blind_cooldown = Random.Range(8f,15f);
        is_moving = true;
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

        if (left_eye_laser.activated && right_eye_laser.activated)
        {
            if (left_eye_laser.finished_loop && right_eye_laser.finished_loop)
            {
                ani.SetBool("is_shooting", false);
            }
        }

        if (lifted_panel_behavior != null)
        {
            ani.SetBool("is_raising", false);
            ani.SetBool("is_throwing", true);
            if (can_flip_once)
            {
                can_flip = true;
            }
            can_flip_once = false;
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
                air_attack_cooldown = 2f;
            }

            if (laser_cooldown <= 0f)
            {
                laser_cooldown = Random.Range(15f,25f);
                ani.SetBool("is_shooting", true);
                made_a_move = true;
            }
            else if (panel_cooldown <= 0f)
            {
                panel_cooldown = Random.Range(15f,20f);
                ani.SetBool("is_raising",true);
                made_a_move = true;
            }
            else if (air_attack_cooldown <= 0f && high_attack_hittable)
            {
                air_attack_cooldown = 5.0f;
                ani.SetBool("is_clapping", true);
                made_a_move = true;
            }
            else if (blind_cooldown <= 0f)
            {
                blind_cooldown = Random.Range(12f,18f);
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
                rb.linearVelocityX = 2f;
            }
            else
            {
                rb.linearVelocityX = -2f;
            }
        }
        else
        {
            rb.linearVelocityX = 0;
        }
    }
    public void Hit(PlayerAttack player_attack)
    {
        player_attack.isHit();
        TakeDamage(player_attack.damage);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health < maximum_health / 2 && !phase_change_done && !phase_change_queued)
        {
            phase_change_queued = true;
            blind_cooldown = 0f;
        }

        if (health <= 0)
        {
            end_attack();
            death_cleanup();
            is_dead = true;
            ani.SetBool("is_dead", is_dead);
        }
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
    public void start_throwing_panel()
    {
        float move = sprite.flipX ? 2.5f : -2.5f;
        ground_panel_behavior = Instantiate(ground_panel, new Vector2(transform.position.x + move,-4.5f), Quaternion.Euler(0,0,0)).GetComponent<GroundPanelBehavior>();
        ground_panel_behavior.the_boss = this;
    }
    public void grow_panel(float f)
    {
        if (ground_panel != null)
        {
            ground_panel_behavior.target_height = f;
        }
    }
    public void end_attack()
    {
        is_attacking = false;
        is_moving = true;
        current_spikes = null;
        can_flip_once = true;
        lifted_panel = null;
        lifted_panel_behavior = null;
        if (left_eye_laser.activated || right_eye_laser.activated)
        {
            deactivate_lasers();
        }

        all_attacks_cooldown = 1.5f;

        ani.SetBool("is_pushing", false);
        ani.SetBool("is_flashing", false);
        ani.SetBool("is_clapping", false);
        ani.SetBool("is_throwing", false);
    }
    public void create_spikes()
    {
        var temp = Instantiate(spikes, spikes_creation_point.transform.position, Quaternion.Euler(0, 0, 0));
        current_spikes = temp.GetComponent<SpikesBehavior>();
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

    public void shrink_spikes()
    {
        if (!current_spikes)
        {
            return;
        }
        current_spikes.decrease_size();
    }
    public void destroy_spikes()
    {
        if (current_spikes)
        {
            current_spikes.destroy();
            current_spikes = null;
        }
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

        air_attack_cooldown -= f;
        if (air_attack_cooldown <= 0f)
        {
            air_attack_cooldown = 0f;
        }

        blind_cooldown -= f;
        if (blind_cooldown <= 0f)
        {
            blind_cooldown = 0f;
        }

        all_attacks_cooldown -= f;
        if (all_attacks_cooldown <= 0f)
        {
            all_attacks_cooldown = 0f;
        }

    }
    public void finished_lifting_panel()
    {
        ground_panel_behavior.destroy();
    }
    public void move_panel()
    {
        if (lifted_panel != null)
        {
            lifted_panel_behavior.step_to_point();
        }
    }
    public void actually_throw_panel()
    {
        if (lifted_panel != null)
        {
            lifted_panel_behavior.increase_phase();
        }
    }
    public void shoot_lasers()
    {
        left_eye_laser.facing_left = sprite.flipX ? false : true;
        left_eye_laser.start_rotation();
        left_eye_laser.activated = true;

        right_eye_laser.facing_left = sprite.flipX ? false : true;
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
    public void flip_everything(bool yes_flip)
    {
        sprite.flipX = yes_flip;
        left_eye_laser.Flip();
        right_eye_laser.Flip();
        panel_lift_point_behavior.Flip();
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

        if (Mathf.Abs(player_distance) < 1.5f) {
            level_manager_behavior.player_life.Hit_No_Collider(gameObject);
            return;
        }

        if (player_distance <= -1.5f && !facing_left)
        {
            level_manager_behavior.player_life.Hit_No_Collider(gameObject);
            return;
        }

        else if (player_distance >= 1.5f && facing_left)
        {
            level_manager_behavior.player_life.Hit_No_Collider(gameObject);
            return;
        }
    }
    public void high_attack(int activate)
    {
        if (activate == 1)
        {
            high_attack_attack.clearFlags();
            high_attack_attack.disableAttackCycle = false;
            high_attack_attack.EnableHitbox();
        }
        else
        {
            high_attack_attack.DisableHitbox();
        }
    }
    public void set_flippable_once()
    {
        can_flip_once = true;
    }
    public void death_cleanup()
    {
        is_attacking = false;
        is_moving = true;
        if (current_spikes)
        {
            current_spikes.destroy();
            current_spikes = null;
        }
        if (lifted_panel)
        {
            lifted_panel_behavior.destroy_self();
            lifted_panel = null;
            lifted_panel_behavior = null;
        }
        if (left_eye_laser.activated || right_eye_laser.activated)
        {
            deactivate_lasers();
        }
    }
}

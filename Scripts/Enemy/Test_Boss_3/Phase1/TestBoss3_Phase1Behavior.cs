using UnityEngine;

public class TestBoss3_Phase1Behavior : MonoBehaviour, EnemyDamageable
{

    public int health;
    public int starting_health = 1500;

    public GameObject laser_center;
    public ElectricLaserRotationPoint laser_behavior;
    public float move_delay = 1.5f;

    public GameObject drone;
    public Animator boss_ani;
    public Animator chamber_ani;

    public GameObject level_manager;
    public LevelManager_TestBoss3_Phase1 level_manager_behavior;

    public float attack_time;
    public float reflective_time;

    public bool is_attacking;
    public bool is_reflective;

    public bool is_dead;

    public bool speed_up_queued;
    public float speed_up_time_delay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = starting_health;
        laser_behavior = laser_center.GetComponent<ElectricLaserRotationPoint>();
        speed_up_time_delay = 0;
        speed_up_queued = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (move_delay > 0)
        {
            move_delay -= Time.deltaTime;
            laser_behavior.can_move = false;
            if (move_delay <= 0)
            {
                laser_behavior.can_move = true;
                move_delay = 0;
            }
        }

        if (is_dead)
        {
            laser_behavior.can_move = false;
            laser_behavior.not_attacking = true;
            return;
        }

        if (!is_reflective) {
            attack_time += Time.deltaTime;
            reflective_time += Time.deltaTime;
        }

        if (attack_time > 7 && !is_attacking)
        {
            is_attacking = true;
            boss_ani.SetBool("is_attacking", is_attacking);
        }

        if (!is_attacking && reflective_time > 12)
        {
            is_reflective = true;
            reflective_time = 0;
            attack_time = 0;
            chamber_ani.SetBool("turn_reflective", is_reflective);
        }

        if (!speed_up_queued)
        {
            speed_up_time_delay = Random.Range(12f,18f);
            speed_up_queued = true;
        }

        if (speed_up_time_delay > 0)
        {
            speed_up_time_delay -= Time.deltaTime;
            if (speed_up_time_delay <= 0)
            {
                speed_up();
            }
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
        if (health <= 0)
        {
            is_dead = true;
            chamber_ani.SetBool("is_dead",true);
            boss_ani.SetBool("is_dead", true);
        }
    }
    public void summon_drone()
    {
        if (level_manager_behavior != null && level_manager_behavior.has_player) {

            if (level_manager_behavior.player.transform.position.x <= 0)
            {
                GameObject drone1 = Instantiate(drone, new Vector2(12,Random.Range(-3f, 0.5f)),Quaternion.Euler(0,0,0));
                drone1.GetComponent<DroneBehavior>().initialize(level_manager_behavior.player);
                GameObject drone2 = Instantiate(drone, new Vector2(-12,Random.Range(0f, 2.5f)),Quaternion.Euler(0,0,0));
                drone2.GetComponent<DroneBehavior>().initialize(level_manager_behavior.player);
                GameObject drone3 = Instantiate(drone, new Vector2(12,Random.Range(2.5f, 5.5f)),Quaternion.Euler(0,0,0));
                drone3.GetComponent<DroneBehavior>().initialize(level_manager_behavior.player);
            }
            else
            {
                GameObject drone1 = Instantiate(drone, new Vector2(-12, Random.Range(-3f, 0.5f)), Quaternion.Euler(0, 0, 0));
                drone1.GetComponent<DroneBehavior>().initialize(level_manager_behavior.player);
                GameObject drone2 = Instantiate(drone, new Vector2(12, Random.Range(0f, 2.5f)), Quaternion.Euler(0, 0, 0));
                drone2.GetComponent<DroneBehavior>().initialize(level_manager_behavior.player);
                GameObject drone3 = Instantiate(drone, new Vector2(-12, Random.Range(2.5f, 5.5f)), Quaternion.Euler(0, 0, 0));
                drone3.GetComponent<DroneBehavior>().initialize(level_manager_behavior.player);
            }
        }
    }
    public void end_attack()
    {
        is_attacking = false;
        attack_time = 0;
        boss_ani.SetBool("is_attacking", is_attacking);
    }
    public void set_reflective(int yes)
    {
        if (yes == 1)
        {
            gameObject.tag = "Reflective";
            Invoke("disable_reflective",5f);
        }
        else
        {
            gameObject.tag = "Untagged";
            is_reflective = false;
        }
    }
    public void disable_reflective()
    {
        chamber_ani.SetBool("turn_reflective",false);
    }
    public void speed_up()
    {
        int ra = Random.Range(0, 3);

        if (ra == 0)
        {
            laser_behavior.move_wait_time = 0.03f;
            Invoke("speed_down", Random.Range(10f, 20f));
        }
        else if (ra == 1)
        {
            laser_behavior.rotation_step = laser_behavior.rotation_step > 0 ? 2 : -2;
            Invoke("speed_down", Random.Range(10f, 20f));
        }
        else if (ra == 2)
        {
            move_delay = 1.5f;
            laser_behavior.can_move = false;
            laser_behavior.rotation_step *= -1;
            speed_up_queued = false;
        }
    }
    public void speed_down()
    {
        laser_behavior.move_wait_time = 0.05f;
        laser_behavior.rotation_step = laser_behavior.rotation_step > 0 ? 1 : -1;
        speed_up_queued = false;
    }

}

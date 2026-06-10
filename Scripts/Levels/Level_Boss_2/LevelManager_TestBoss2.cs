using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager_TestBoss2 : MonoBehaviour
{

    public GameObject boss;
    public TestBoss2Behavior boss_life;
    public EnemyActiveBossComponent boss_primary;
    public bool has_boss;

    public GrapplingBossBehavior grappling_boss;

    public GameObject player;
    public PlayerDamageAndHealth player_life;
    public bool has_player;

    public GameObject shooter_enemy;
    public GameObject current_left_shooter;
    public GameObject current_right_shooter;
    public ShooterAI left_ai;
    public ShooterAI right_ai;
    public GameObject left_spawn;
    public GameObject right_spawn;
    public bool called_left;
    public bool called_right;

    public GameObject left_high;
    public GameObject left_low;
    public GameObject right_high;
    public GameObject right_low;
    public GameObject grapple_end_point;

    public float left_fade;
    public float right_fade;

    public bool phase_changed = false;
    public bool just_started = true;

    public bool level_loading;

    public GameObject spawn_point;

    public EnemyActiveBossComponent side_building_left = null;
    public EnemyActiveBossComponent side_building_right = null;

    // Update is called once per frame
    void Update()
    {

        if (boss == null)
        {
            TestBoss2Behavior b = FindAnyObjectByType<TestBoss2Behavior>();
            if (b != null)
            {
                boss = b.gameObject;
                boss_life = b;
                boss_primary = b.GetComponent<EnemyActiveBossComponent>();
                has_boss = true;
            }
        }

        /*if (player_life == null)
        {
            var attempt = GameObject.FindWithTag("Player");
            
            if (attempt != null)
            {
                GameManager.Instance.current_player = attempt;
                player = attempt;
                player_life = player.GetComponent<PlayerDamageAndHealth>();
                has_player = true;
            }
        }*/

        /*if (has_player)
        {
            if (player.transform.position.y < -6)
            {
                player.transform.position = new Vector2(-9, -3);
            }
        }*/

        if (player_life == null)
        {
            GameManager.Instance.playerUI.set_health_bar = false;
            GameManager.Instance.instantiate_player(spawn_point.transform.position, 6, 6);
            player = GameManager.Instance.current_player;
            player_life = GameManager.Instance.player_health;
            has_player = true;
        }

        if (has_boss && boss_life.phase == 1)
        {
            if (!current_left_shooter && !called_left)
            {
                Invoke("spawn_left", Mathf.Max(18, Random.Range(0, 28)));
                called_left = true;
            }

            if (!current_right_shooter && !called_right)
            {
                if (just_started)
                {
                    Invoke("spawn_right", 1.5f);
                    called_right = true;
                    just_started = false;
                }
                else
                {
                    Invoke("spawn_right", Mathf.Max(18, Random.Range(0, 28)));
                    called_right = true;
                }
            }

        }

        else if (has_boss && boss_life.phase != 1 && !phase_changed)
        {
            CancelInvoke("spawn_left");
            CancelInvoke("spawn_right");
            phase_changed = true;
        }

        if (has_boss)
        {
            if (boss_life.phase == 1)
            {
                if (side_building_left == null)
                {
                    side_building_left = boss_life.left_building.GetComponentInChildren<EnemyActiveBossComponent>();
                }

                if (side_building_right == null)
                {
                    side_building_right = boss_life.right_building.GetComponentInChildren<EnemyActiveBossComponent>();
                }

                if (side_building_left != null && side_building_right != null)
                {
                    if (boss_life.left.boss_is_on)
                    {
                        side_building_left.isPrimary();
                        side_building_right.notPrimary();
                    }
                    else if (boss_life.right.boss_is_on)
                    {
                        side_building_right.isPrimary();
                        side_building_left.notPrimary();
                    }
                    else if (boss_life.left.building_health > 0)
                    {
                        side_building_left.isPrimary();
                        side_building_right.notPrimary();
                    }
                    else if (boss_life.right.building_health > 0)
                    {
                        side_building_right.isPrimary();
                        side_building_left.notPrimary();
                    }
                    else
                    {
                        side_building_left.notPrimary();
                        side_building_right.notPrimary();
                    }
                }
            }
            else if (boss_life.phase == 2)
            {
                if (boss_life.current_swinging_boss != null && grappling_boss == null)
                {
                    grappling_boss = boss_life.current_swinging_boss.GetComponent<GrapplingBossBehavior>();
                    grappling_boss.GetComponent<EnemyActiveBossComponent>().isPrimary();
                    boss_primary.notPrimary();
                }
                else if (boss_life.current_swinging_boss == null && !boss_primary.isPrimaryTarget)
                {
                    boss_primary.isPrimary();
                }
            }
        }

        if (has_boss && has_player)
        {
            if (boss_life.health <= 0)
            {
                player_life.add_true_invincibility_time(1); ;
                if (!level_loading)
                {
                    Invoke("load_next_level", 5f);
                    level_loading = true;
                }
            }

            if (player_life.health <= 0)
            {
                if (!level_loading)
                {
                    Invoke("load_next_level", 5f);
                    level_loading = true;
                }
            }
        }

        if (left_fade > 0)
        {
            left_fade -= Time.deltaTime;

            if (left_fade < 0)
            {
                left_fade = 0;
            }

            if (current_left_shooter != null)
            {
                left_ai.sprite.color = new Color(1.0f, 1.0f, 1.0f, (4 - left_fade) * 0.25f);
            }

        }

        if (right_fade > 0)
        {
            right_fade -= Time.deltaTime;

            if (right_fade < 0)
            {
                right_fade = 0;
            }

            if (current_right_shooter != null)
            {
                right_ai.sprite.color = new Color(1.0f, 1.0f, 1.0f, (4 - right_fade) * 0.25f);
            }
        }

    }
    public void spawn_left()
    {
        if (!current_left_shooter)
        {
            current_left_shooter = Instantiate(shooter_enemy, left_spawn.transform.position, Quaternion.Euler(0, 0, 0));
            left_ai = current_left_shooter.GetComponent<ShooterAI>();
            left_ai.sprite.flipX = true;
            left_ai.player = player;
            left_ai.shootPoint.Flip();
            left_ai.sprite.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            left_ai.bullet_speed = 5;
            left_fade = 4.0f;
            called_left = false;
        }
    }
    public void spawn_right()
    {
        if (!current_right_shooter)
        {
            current_right_shooter = Instantiate(shooter_enemy, right_spawn.transform.position, Quaternion.Euler(0, 0, 0));
            right_ai = current_right_shooter.GetComponent<ShooterAI>();
            right_ai.player = player;
            right_ai.sprite.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            right_ai.bullet_speed = 5;
            right_fade = 4.0f;
            called_right = false;
        }
    }
    public void load_next_level()
    {
        SceneManager.LoadScene("Main Menu");
    }
}

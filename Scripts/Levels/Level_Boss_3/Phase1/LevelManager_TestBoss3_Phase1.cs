using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager_TestBoss3_Phase1 : MonoBehaviour
{
    public GameObject boss;
    public TestBoss3_Phase1Behavior boss_life;
    public bool has_boss;

    public GameObject player;
    public PlayerDamageAndHealth player_life;
    public bool has_player;

    public bool just_started = true;

    public bool level_loading;

    public GameObject spawn_point;

    // Update is called once per frame
    void Update()
    {

        if (boss == null)
        {
            TestBoss3_Phase1Behavior b = FindAnyObjectByType<TestBoss3_Phase1Behavior>();
            if (b != null)
            {
                boss = b.gameObject;
                boss_life = b;
                b.level_manager = gameObject;
                b.level_manager_behavior = this;
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

        if (player_life == null)
        {
            GameManager.Instance.playerUI.set_health_bar = false;
            GameManager.Instance.instantiate_player(spawn_point.transform.position, 12, 12);
            player = GameManager.Instance.current_player;
            player_life = GameManager.Instance.player_health;
            has_player = true;
        }

        if (has_boss && has_player)
        {
            if (boss_life.health <= 0)
            {
                player_life.trueInvincibilityTime = 1f;
                if (!level_loading)
                {
                    GameManager.Instance.carryover_health = player_life.health;
                    Invoke("load_next_phase", 7f);
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
    }
    public void load_next_phase()
    {
        SceneManager.LoadScene("TestBoss3_Phase2");
    }

    public void load_next_level()
    {
        SceneManager.LoadScene("Main Menu");
    }
}

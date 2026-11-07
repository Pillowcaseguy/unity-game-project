using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager_TestBoss4 : MonoBehaviour
{
    public GameObject boss;
    public TestBoss4Behavior boss_life;
    public bool has_boss;

    public GameObject player;
    public PlayerDamageAndHealth player_life;
    public bool has_player;

    public bool level_loading;

    public GameObject spawn_point;

    public GameObject background;
    public Image background_color;

    public SpriteRenderer ground1;
    public SpriteRenderer ground2;
    public SpriteRenderer ground3;
    public SpriteRenderer ground4;

    public bool start_color_changing;

    // Update is called once per frame
    void Update()
    {
        has_boss = true;

        if (start_color_changing)
        {
            float change = Time.unscaledDeltaTime / 5;

            background_color.color = new Vector4(background_color.color.r + change, background_color.color.g - change, background_color.color.b - change, 48f / 255f);
            if (background_color.color.r > 1.0f)
            {
                background_color.color = new Vector4(1.0f, background_color.color.g, background_color.color.b, 48f / 255f);
            }

            ground1.color = new Vector4(ground1.color.r + change, ground1.color.g - change, ground1.color.b - change, 1);
            if (ground1.color.r > 1.0f)
            {
                ground1.color = new Vector4(1, ground1.color.g, ground1.color.b, 1);
            }

            ground2.color = new Vector4(ground2.color.r + change, ground2.color.g + change, ground2.color.b - change, 1);
            if (ground2.color.r > 1.0f)
            {
                ground2.color = new Vector4(1, ground2.color.g, ground2.color.b, 1);
            }
            if (ground2.color.g > 0.39f)
            {
                ground2.color = new Vector4(ground2.color.r, 0.39f, ground2.color.b, 1);
            }

            ground3.color = new Vector4(ground3.color.r + change, ground3.color.g + change, ground3.color.b - change, 1);
            if (ground3.color.r > 1.0f)
            {
                ground3.color = new Vector4(1, ground3.color.g, ground3.color.b, 1);
            }
            if (ground3.color.g > 0.39f)
            {
                ground3.color = new Vector4(ground3.color.r, 0.39f, ground3.color.b, 1);
            }

            ground4.color = new Vector4(ground4.color.r + change, ground4.color.g + change, ground4.color.b - change, 1);
            if (ground4.color.r > 1.0f)
            {
                ground4.color = new Vector4(1, ground4.color.g, ground4.color.b, 1);
            }
            if (ground4.color.g > 0.39f)
            {
                ground4.color = new Vector4(ground4.color.r, 0.39f, ground4.color.b, 1);
            }

            if (ground1.color.r == 1f)
            {
                start_color_changing = false;
            }
        }

        if (player_life == null)
        {
            GameManager.Instance.playerUI.set_health_bar = false;
            GameManager.Instance.instantiate_player(spawn_point.transform.position, 10, 10);
            player = GameManager.Instance.current_player;
            player_life = GameManager.Instance.player_health;
            has_player = true;
        }

        if (has_boss && has_player)
        {
            if (boss_life.health <= 0)
            {
                player_life.add_true_invincibility_time(1);
                if (!level_loading)
                {
                    //GameManager.Instance.carryover_health = player_life.health;
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
    }
    public void load_next_level()
    {
        SceneManager.LoadScene("Main Menu");
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager_Tutorial : MonoBehaviour
{
    public GameObject player;
    public PlayerDamageAndHealth player_life;
    public bool has_player;

    public bool level_loading;

    public GameObject spawn_point;
    public EnemyAttack mine;

    public Camera main;

    public float heal_cooldown = 1.0f;

    public GameObject death_text;
    public GameObject sad_wolf_thingy;
    public GameObject current_sad_wolf;

    public GameObject tutorial_swordsman;
    public GameObject current_swordsman;
    public GameObject tutorial_swordsman_spawnpoint;
    public float swordsman_spawn_cooldown = 2.0f;

    void Start()
    {
        main = Camera.main;
        mine = GetComponent<EnemyAttack>();
        if (player_life == null)
        {
            GameManager.Instance.playerUI.set_health_bar = false;
            GameManager.Instance.instantiate_player(spawn_point.transform.position, 4, 4);
            player = GameManager.Instance.current_player;
            player_life = GameManager.Instance.player_health;
            has_player = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (has_player)
        {
            player_life.maxParryCooldown = 2f;
            if (player.transform.position.y <= -50 && player_life.health > 0)
            {
                if (!death_text.gameObject.activeSelf)
                {
                    death_text.SetActive(true);
                }
                player_life.Hit_No_Collider(gameObject);
                main.GetComponent<CameraFollowPlayer>().enabled = false;
                main.GetComponent<CameraShake>().cameraCurrentY = -3;
                main.transform.position = new Vector3(0, -3, -10);
            }

            if (player_life.health < player_life.maximum_health)
            {
                heal_cooldown -= Time.deltaTime;
            }

            if (heal_cooldown <= 0)
            {
                heal_cooldown = 0.8f;
                player_life.Heal(1);
            }

            if (player_life.blindTime > 0 && current_sad_wolf == null)
            {
                current_sad_wolf = Instantiate(sad_wolf_thingy, player.transform.position, Quaternion.Euler(0, 0, 0));
            }
            else if (current_sad_wolf != null)
            {
                current_sad_wolf.transform.position = player.transform.position;
                if (player_life.blindTime <= 0)
                {
                    Destroy(current_sad_wolf);
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
        if (current_swordsman == null)
        {
            if (swordsman_spawn_cooldown <= 0f)
            {
                current_swordsman = Instantiate(tutorial_swordsman, tutorial_swordsman_spawnpoint.transform.position, Quaternion.Euler(0, 0, 0));
                swordsman_spawn_cooldown = 4.0f;
            }
            if (swordsman_spawn_cooldown > 0f)
            {
                swordsman_spawn_cooldown -= Time.deltaTime;
            }
        }
    }
    public void load_next_level()
    {
        SceneManager.LoadScene("Main Menu");
    }
}

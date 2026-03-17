using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LevelManager_TestBoss1 : MonoBehaviour
{

    public GameObject boss;
    public TestBoss1Behavior boss_life;
    public bool has_boss;

    public GameObject player;
    public PlayerDamageAndHealth player_life;
    public bool has_player;

    public GameObject current_foxman;
    public GameObject current_pigtoy;
    public GameObject current_foxman_alert;
    public GameObject current_pigtoy_alert;

    public GameObject foxman;
    public GameObject pigtoy;

    public GameObject foxman_alert;
    public GameObject pigtoy_alert;

    public bool foxman_queued;
    public bool pigtoy_queued;

    public float current_rng_number_foxman;
    public float current_rng_number_pigtoy;

    public bool level_loading = false;

    // Update is called once per frame
    void Update()
    {
        if (boss == null)
        {
            TestBoss1Behavior b = FindAnyObjectByType<TestBoss1Behavior>();
            if (b != null)
            {
                boss = b.gameObject;
                boss_life = b;
                has_boss = true;
            }
        }

        if (player_life == null)
        {
            var attempt = GameObject.FindWithTag("Player");

            if (attempt != null)
            {
                GameManager.Instance.current_player = attempt;
                player = attempt;
                player_life = player.GetComponent<PlayerDamageAndHealth>();
                has_player = true;
            }
        }

        if (current_foxman == null && !foxman_queued)
        {
            FoxmanBehavior attempt = FindAnyObjectByType<FoxmanBehavior>();
            if (attempt != null)
            {
                current_foxman = attempt.gameObject;
                attempt.cameraShake = GameManager.Instance.current_camera.GetComponent<CameraShake>();
            }

            if (!current_foxman)
            {
                Invoke("spawn_foxman", 20f);
                Invoke("spawn_foxman_alert", 15f);
                foxman_queued = true;
            }
            
        }

        if (current_pigtoy == null && !pigtoy_queued)
        {
            PigToy attempt = FindAnyObjectByType<PigToy>();
            if (attempt != null)
            {
                current_pigtoy = attempt.gameObject;
            }

            if (!current_pigtoy)
            {
                pigtoy_queued = true;
                Invoke("spawn_pigtoy", 15f);
                Invoke("spawn_pigtoy_alert", 10f);
            }
            
        }

        if (current_foxman != null)
        {
            CancelInvoke("spawn_foxman");
            CancelInvoke("spawn_foxman_alert");
            if (current_foxman_alert != null)
            {
                Destroy(current_foxman_alert);
            }
        }

        if (current_pigtoy != null)
        {
            CancelInvoke("spawn_pigtoy");
            CancelInvoke("spawn_pigtoy_alert");
            if (current_pigtoy_alert != null)
            {
                Destroy(current_pigtoy_alert);
            }
        }

        if (has_boss && has_player)
        {
            if (boss_life.health <= 0) // Boss Death - Replace with victory.
            {
                player_life.trueInvincibilityTime = 1.0f;
                if (!level_loading)
                {
                    Invoke("load_next_level", 5f);
                    level_loading = true;
                }
            }

            if (player_life.health <= 0) // Player Death
            {
                if (!level_loading)
                {
                    Invoke("load_next_level", 5f);
                    level_loading = true;
                }
            }
        }
    }

    void spawn_pigtoy()
    {
        Instantiate(pigtoy, new Vector2(current_rng_number_pigtoy, 10f), Quaternion.Euler(0,0,0));
        // -2 -> 4
        // y = 10
        pigtoy_queued = false;
    }
    void spawn_pigtoy_alert()
    {
        current_rng_number_pigtoy = UnityEngine.Random.Range(-2f, 4f);
        current_pigtoy_alert = Instantiate(pigtoy_alert, new Vector2(current_rng_number_pigtoy, 4f), Quaternion.Euler(0, 0, 0));
    }
    void spawn_foxman()
    {
        GameObject hi = Instantiate(foxman, new Vector2(current_rng_number_foxman, -3.5f), Quaternion.Euler(0, 0, 0));
        GameObject gii = hi.GetComponentInChildren<FoxmanSpriteBehavior>().gameObject;
        gii.GetComponent<Transform>().rotation = Quaternion.Euler(0, 0, 340);
        // -4 -> 0
        // y = -3.5
        foxman_queued = false;
    }
    void spawn_foxman_alert()
    {
        current_rng_number_foxman = UnityEngine.Random.Range(-4f, 0f);
        current_foxman_alert = Instantiate(foxman_alert, new Vector2(current_rng_number_foxman, -3.5f), Quaternion.Euler(0, 0, 0));
    }
    void load_next_level()
    {
        SceneManager.LoadScene("Main Menu");
    }
}

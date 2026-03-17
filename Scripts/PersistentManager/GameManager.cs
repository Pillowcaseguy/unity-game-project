using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public GameObject blind_effect;
    public Camera current_camera;
    public GameObject current_player;
    public PlayerDamageAndHealth player_health;
    public PlayerUI playerUI;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // kill the duplicate
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        current_camera = Camera.main;
        blind_effect.SetActive(false);
        playerUI = GetComponentInChildren<PlayerUI>();
    }
    public void set_current_player(GameObject player)
    {
        player_health = null;
        current_player = player;
    }
    void Update()
    {
        if (current_camera == null)
        {
            current_camera = Camera.main;
        }

        if (current_player == null)
        {
            GameObject temp = GameObject.FindGameObjectWithTag("Player");
            if (temp != null)
            {
                current_player = temp;
            }
            playerUI.gameObject.SetActive(false);
            player_health = null;
        }

        else
        {
            if (!playerUI.gameObject.activeSelf)
            {
                playerUI.gameObject.SetActive(true);
            }
        }

        if (current_player != null && player_health == null)
        {
            PlayerDamageAndHealth attempt = current_player.GetComponent<PlayerDamageAndHealth>();
            if (attempt != null)
            {
                player_health = attempt;
                update_weapon();
            }
        }

        if (current_player != null && player_health != null)
        {
            if (player_health.blindTime > 0)
            {
                blind_effect.SetActive(true);
            }
            else
            {
                blind_effect.SetActive(false);
            }

            playerUI.updatePortrait(player_health);
            playerUI.updateHealthBar(player_health);
        }
        else
        {
            blind_effect.SetActive(false);
        }
    }
    public void update_weapon()
    {
        if (current_player != null)
        {
            playerUI.get_current_weapon(current_player.GetComponent<PlayerAttacking>());
        }
    }
}

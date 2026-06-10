using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public GameObject blind_effect;
    public Camera current_camera;

    public GameObject current_player;
    public PlayerDamageAndHealth player_health;
    public PlayerMovement player_movement;

    public PlayerUI playerUI;

    public static PlayerAttacking.WeaponType[] implemented_weapons = {PlayerAttacking.WeaponType.Ice,PlayerAttacking.WeaponType.Fire,
        PlayerAttacking.WeaponType.Gem,PlayerAttacking.WeaponType.Light};

    public int first_list_index = 0;
    public int second_list_index = 1;

    public PlayerAttacking.WeaponType first_wep = implemented_weapons[0];
    public PlayerAttacking.WeaponType second_wep = implemented_weapons[1];

    public GameObject player_prefab;

    public int carryover_health;
    public int carryover_energy;

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
        player_movement = null;
        current_player = player;
    }
    void Update()
    {
        first_wep = implemented_weapons[first_list_index];
        second_wep = implemented_weapons[second_list_index];

        if (current_camera == null)
        {
            current_camera = Camera.main;
        }

        if (current_player == null)
        {
            playerUI.gameObject.SetActive(false);
            player_health = null;
            player_movement = null;
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

        if (current_player != null && player_movement == null)
        {
            PlayerMovement attempt = current_player.GetComponent<PlayerMovement>();
            if (attempt != null)
            {
                player_movement = attempt;
            }
        }

        if (current_player != null && player_health != null && player_movement != null)
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
            playerUI.updateEnergyBar(player_movement);
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
            PlayerAttacking hi = current_player.GetComponent<PlayerAttacking>();
            if (hi == null)
            {
                return;
            }
            playerUI.get_current_weapon(hi);
        }
    }
    public void instantiate_player(Vector2 position, int max_health, int health, int energy = 0)
    {
        int new_health = health;
        if (current_player != null)
        {
            return;
        }
        else
        {
            if (player_health != null)
            {
                player_health = null;
            }
        }
        current_player = Instantiate(player_prefab, position, Quaternion.Euler(0, 0, 0));
        player_health = current_player.GetComponent<PlayerDamageAndHealth>();
        //player_health.initialize(max_health, primary_weapon, secondary_weapon);
        if (max_health < new_health)
        {
            new_health = max_health;
        }
        player_health.initialize(max_health, new_health, first_wep, second_wep, energy);
    }
}

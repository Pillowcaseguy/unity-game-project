using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static PlayerAttacking;
public class PlayerUI : MonoBehaviour
{

    public GameObject background_bar;
    public GameObject background_square;
    public GameObject portrait_head;
    public GameObject portrait_left_eye;
    public GameObject portrait_left_eye_bag;
    public GameObject portrait_right_eye;
    public GameObject portrait_right_eye_bag;
    public GameObject portrait_mouth;
    public GameObject fire_in_portrait;
    public GameObject weapon_portrait;
    public GameObject secondary_weapon_portrait;

    public Image bg_bar;
    public Image bg_sq;
    public Image head;
    public Image left_eye;
    public Image right_eye;
    public Image mouth;

    public Image weapon_type;
    public Image secondary_weapon_type;

    public Sprite normal_eye;
    public Sprite big_eye;
    public Sprite white_eye;
    public Sprite injured_eye;
    public Sprite injured_eye_flipped;
    public Sprite x_eye;
    public Sprite frozen_eye;
    public Sprite slow_eye;
    public Sprite dizzy_eye_1;
    public Sprite dizzy_eye_2;

    public Sprite happy_mouth;
    public Sprite slightly_happy_mouth;
    public Sprite neutral_mouth;
    public Sprite sad_mouth;
    public Sprite very_sad_mouth;
    public Sprite dizzy_mouth;

    public Sprite normal_head;
    public Sprite cursed_head;

    public Image portrait_bandage;

    public Sprite weapon_ice;
    public Sprite weapon_fire;
    public Sprite weapon_gem;
    public Sprite weapon_water;
    public Sprite weapon_earth;
    public Sprite weapon_light;
    public Sprite weapon_dark;

    public GameObject health_bar;
    public GameObject health_fill;

    public RectTransform health_bar_t;
    public RectTransform health_fill_t;

    public bool set_health_bar = false;

    public TextMeshProUGUI super_energy_text;
    public RectTransform energy_bar_t;

    void Start()
    {
        bg_bar = background_bar.GetComponent<Image>();
        bg_sq = background_square.GetComponent<Image>();
        head = portrait_head.GetComponent<Image>();
        left_eye = portrait_left_eye.GetComponent<Image>();
        right_eye = portrait_right_eye.GetComponent<Image>();
        mouth = portrait_mouth.GetComponent<Image>();
        health_bar_t = health_bar.GetComponent<RectTransform>();
        health_fill_t = health_fill.GetComponent<RectTransform>();
        weapon_type = weapon_portrait.GetComponent<Image>();
        secondary_weapon_type = secondary_weapon_portrait.GetComponent<Image>();
    }

    public void updatePortrait(PlayerDamageAndHealth player_health)
    {
        double health_ratio = (double)player_health.health / player_health.maximum_health;
        Color final_bg_bar;
        Color final_bg_sq;
        Sprite final_head;
        Sprite final_left_eye;
        Sprite final_right_eye;
        Sprite final_mouth;

        final_bg_bar = roll_bar(player_health);
        final_bg_sq = roll_square(player_health);
        final_head = roll_head(player_health);
        final_left_eye = roll_left_eye(player_health, health_ratio);
        final_right_eye = roll_right_eye(player_health, health_ratio);
        final_mouth = roll_mouth(player_health, health_ratio);

        /*if (player_health.weaknessTime > 0 && !portrait_left_eye_bag.activeSelf)
        {
            portrait_left_eye_bag.SetActive(true);
        }
        else if (player_health.weaknessTime <= 0 && portrait_left_eye_bag.activeSelf)
        {
            portrait_left_eye_bag.SetActive(false);
        }

        if (player_health.weaknessTime > 0 && !portrait_right_eye_bag.activeSelf)
        {
            portrait_right_eye_bag.SetActive(true);
        }
        else if (player_health.weaknessTime <= 0 && portrait_right_eye_bag.activeSelf)
        {
            portrait_right_eye_bag.SetActive(false);
        }*/

        if (player_health.burnTime > 0 && !fire_in_portrait.activeSelf)
        {
            fire_in_portrait.SetActive(true);
        }
        else if (player_health.burnTime <= 0 && fire_in_portrait.activeSelf)
        {
            fire_in_portrait.SetActive(false);
        }

        if (health_ratio <= 0.2)
        {
            portrait_bandage.enabled = true;
        }
        else
        {
            portrait_bandage.enabled = false;
        }

        bg_bar.color = final_bg_bar;
        bg_sq.color = final_bg_sq;
        head.sprite = final_head;
        left_eye.sprite = final_left_eye;
        right_eye.sprite = final_right_eye;
        mouth.sprite = final_mouth;

    }
    public Color roll_bar(PlayerDamageAndHealth h)
    {
        return new Color(0, 0, 0, 0.8f);
    }
    public Color roll_square(PlayerDamageAndHealth h)
    {
        if (h.health <= 0)
        {
            return Color.white;
        }

        if (h.burnTime > 0)
        {
            return Color.orange;
        }

        if (h.curseTime > 0)
        {
            return Color.mediumPurple;
        }

        if (h.freezeTime > 0)
        {
            return Color.cornflowerBlue;
        }

        return Color.white;

    }
    public Sprite roll_head(PlayerDamageAndHealth h)
    {
        if (h.health <= 0 || h.curseTime <= 0)
        {
            return normal_head;
        }

        return cursed_head;

    }
    public Sprite roll_left_eye(PlayerDamageAndHealth h, double ratio)
    {

        if (h.health <= 0)
        {
            return x_eye;
        }

        if (h.hurtTime > 0)
        {
            return injured_eye;
        }

        if (ratio <= 0.4)
        {
            return injured_eye;
        }

        if (h.stunTime > 0)
        {
            return x_eye;
        }

        if (h.blindTime > 0)
        {
            return white_eye;
        }

        if (h.reverseTime > 0)
        {
            return slow_eye;
        }

        if (h.freezeTime > 0)
        {
            return frozen_eye;
        }

        if (h.burnTime > 0)
        {
            return big_eye;
        }

        if (h.slowTime > 0 || h.weaknessTime > 0)
        {
            return slow_eye;
        }

        return normal_eye;

    }
    public Sprite roll_right_eye(PlayerDamageAndHealth h, double ratio)
    {

        if (h.health <= 0)
        {
            return x_eye;
        }

        if (h.hurtTime > 0)
        {
            return injured_eye_flipped;
        }

        if (h.stunTime > 0)
        {
            return x_eye;
        }

        if (h.blindTime > 0)
        {
            return white_eye;
        }

        if (ratio <= 0.2)
        {
            return dizzy_eye_1;
        }

        if (h.reverseTime > 0)
        {
            return dizzy_eye_2;
        }

        if (h.freezeTime > 0)
        {
            return frozen_eye;
        }

        if (h.burnTime > 0)
        {
            return big_eye;
        }

        if (h.slowTime > 0 || h.weaknessTime > 0)
        {
            return slow_eye;
        }

        return normal_eye;
    }
    public Sprite roll_mouth(PlayerDamageAndHealth h, double ratio)
    {

        if (h.health <= 0)
        {
            return sad_mouth;
        }

        if (h.hurtTime > 0)
        {
            return sad_mouth;
        }

        if (h.burnTime > 0)
        {
            return very_sad_mouth;
        }

        if (h.reverseTime > 0)
        {
            return dizzy_mouth;
        }

        /*if (ratio <= 0.15)
        {
            return very_sad_mouth;
        }*/

        if (h.stunTime > 0 || h.curseTime > 0 || h.blindTime > 0 || h.freezeTime > 0)
        {
            return sad_mouth;
        }

        if (ratio <= 0.75)
        {

            if (ratio <= 0.6)
            {
                if (ratio <= 0.3)
                {
                    return sad_mouth;
                }

                return neutral_mouth;

            }

            if (h.weaknessTime > 0 || h.slowTime > 0)
            {
                return neutral_mouth;
            }

            return slightly_happy_mouth;

        }

        if (h.weaknessTime > 0 || h.slowTime > 0)
        {
            return neutral_mouth;
        }

        return happy_mouth;

    }
    public void updateHealthBar(PlayerDamageAndHealth player_health)
    {
        if (!set_health_bar)
        {
            health_bar_t.sizeDelta = new Vector2(128f * Mathf.CeilToInt(player_health.maximum_health / 2f), health_bar_t.sizeDelta.y);
            health_fill_t.anchoredPosition = new Vector2(-32 * player_health.maximum_health + 1, health_fill_t.anchoredPosition.y);
            set_health_bar = true;
        }
        int starting_base = player_health.health / 2;
        int remainder = player_health.health % 2;
        float fillWidth = Mathf.Min(starting_base * 128 + remainder * 66 - 1, health_bar_t.sizeDelta.x);
        health_fill_t.sizeDelta = new Vector2(fillWidth, health_bar_t.sizeDelta.y);
    }
    public void updateEnergyBar(PlayerMovement playerMovement)
    {
        updateEnergyText(playerMovement);
        float fillWidth = playerMovement.super_energy;
        energy_bar_t.sizeDelta = new Vector2(fillWidth, energy_bar_t.sizeDelta.y);
    }
    public void updateEnergyText(PlayerMovement playerMovement)
    {
        int the_energy = playerMovement.super_energy;
        super_energy_text.text = the_energy.ToString();
        if (the_energy < 100)
        {
            super_energy_text.color = Color.white;
        }
        else if (the_energy >= 100 && the_energy < 500)
        {
            super_energy_text.color = Color.yellow;
        }
        else
        {
            super_energy_text.color = Color.red;
        }
    }
    public void get_current_weapon(PlayerAttacking weapon)
    {
        WeaponType x = weapon.currentWeapon;

        if (x == WeaponType.Ice)
        {
            weapon_type.sprite = weapon_ice;
        }
        else if (x == WeaponType.Fire)
        {
            weapon_type.sprite = weapon_fire;
        }
        else if (x == WeaponType.Gem)
        {
            weapon_type.sprite = weapon_gem;
        }
        else if (x == WeaponType.Earth)
        {
            weapon_type.sprite = weapon_earth;
        }
        else if (x == WeaponType.Water)
        {
            weapon_type.sprite = weapon_water;
        }
        else if (x == WeaponType.Light)
        {
            weapon_type.sprite = weapon_light;
        }
        else if (x == WeaponType.Dark)
        {
            weapon_type.sprite = weapon_dark;
        }
        else
        {
            weapon_type.sprite = weapon_ice;
        }

        WeaponType y = weapon.lastWeapon;

        if (y == WeaponType.Ice)
        {
            secondary_weapon_type.sprite = weapon_ice;
        }
        else if (y == WeaponType.Fire)
        {
            secondary_weapon_type.sprite = weapon_fire;
        }
        else if (y == WeaponType.Gem)
        {
            secondary_weapon_type.sprite = weapon_gem;
        }
        else if (y == WeaponType.Earth)
        {
            secondary_weapon_type.sprite = weapon_earth;
        }
        else if (y == WeaponType.Water)
        {
            secondary_weapon_type.sprite = weapon_water;
        }
        else if (y == WeaponType.Light)
        {
            secondary_weapon_type.sprite = weapon_light;
        }
        else if (y == WeaponType.Dark)
        {
            secondary_weapon_type.sprite = weapon_dark;
        }
        else
        {
            secondary_weapon_type.sprite = weapon_ice;
        }

    }
}

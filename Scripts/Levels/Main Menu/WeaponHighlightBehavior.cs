using UnityEngine;

public class WeaponHighlight : MonoBehaviour
{
    public Sprite ice_picture;
    public Sprite fire_picture;
    public Sprite gem_picture;
    public Sprite light_picture;

    public SpriteRenderer sprite;

    public bool secondary;
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        PlayerAttacking.WeaponType weapon_to_use = secondary ? GameManager.Instance.second_wep : GameManager.Instance.first_wep;
        Sprite used_sprite = null;

        if (weapon_to_use == PlayerAttacking.WeaponType.Ice) {
            used_sprite = ice_picture;
        }
        else if (weapon_to_use == PlayerAttacking.WeaponType.Fire)
        {
            used_sprite = fire_picture;
        }
        else if (weapon_to_use == PlayerAttacking.WeaponType.Gem)
        {
            used_sprite = gem_picture;
        }
        else if (weapon_to_use == PlayerAttacking.WeaponType.Light)
        {
            used_sprite = light_picture;
        }

        sprite.sprite = used_sprite;

    }
}

using UnityEngine;

public class StickyFireEffectBehavior : MonoBehaviour
{
    public LevelManager_TestBoss5 manager;
    public Animator ani;
    public SpriteRenderer sprite;
    void Awake()
    {
        ani = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        sprite.flipX = manager.player_life.movement.sprite_render.flipX;
        if (manager.has_player)
        {
            if (!sprite.enabled)
            {
                sprite.enabled = true;
            }
            transform.position = manager.player.transform.position;
        }
        else
        {
            sprite.enabled = false;
        }
        if (manager.delay_to_extinguish_super_burn > 0)
        {
            ani.SetBool("intense_burning", true);
        }
        else
        {
            ani.SetBool("intense_burning", false);
            if (manager.player_life.movement.dash_time >= 0.5f)
            {
                manager.extinguish_super_burn();
            }
        }
    }
}

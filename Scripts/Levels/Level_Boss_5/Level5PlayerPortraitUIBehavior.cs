using UnityEngine;
using UnityEngine.UI;

public class Level5PlayerPortraitUIBehavior : MonoBehaviour
{
    public Image player_bloodstains;

    public LevelManager_TestBoss5 manager;
    public PlayerDamageAndHealth player_health;
    public Image death_x;

    void Update()
    {
        if (manager == null || !manager.has_player)
        {
            return;
        }

        if (player_health == null && manager.has_player)
        {
            player_health = manager.player_life;
        }

        float health_ratio = (float) player_health.health / player_health.maximum_health;
        modify_bloodstains(health_ratio);
    }

    public void modify_bloodstains(float health_ratio)
    {
        if (player_health.health <= 0)
        {
            player_bloodstains.sprite = manager.bloodstains_stage3;
            death_x.enabled = true;
        }
        else if (health_ratio >= 0.75f)
        {
            player_bloodstains.sprite = manager.bloodstains_stage0;
        }
        else if (health_ratio >= 0.5f && health_ratio < 0.75f)
        {
            player_bloodstains.sprite = manager.bloodstains_stage1;
        }
        else if (health_ratio >= 0.25f && health_ratio < 0.5f)
        {
            player_bloodstains.sprite = manager.bloodstains_stage2;
        }
        else if (health_ratio < 0.25f && health_ratio > 0f)
        {
            player_bloodstains.sprite = manager.bloodstains_stage3;
        }
    }
}

using UnityEngine;

public class TestBoss3_Phase3WeaknessField : MonoBehaviour
{
    public GameObject the_boss;
    public TestBoss3_Phase3Behavior the_boss_behavior;
    public GameObject the_player;
    public PlayerDamageAndHealth player_health;
    public SpriteRenderer sprite;
    public float application_cooldown;
    public float set_cooldown = 1.0f;

    public bool applying_weakness = false;
    public bool override_show = false;

    void Start()
    {
        application_cooldown = set_cooldown;
        sprite = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if (application_cooldown <= 0f && the_player == null)
        {
            if (the_boss_behavior.level_manager_behavior != null && the_boss_behavior.level_manager_behavior.has_player)
            {
                the_player = the_boss_behavior.level_manager_behavior.player;
                player_health = the_boss_behavior.level_manager_behavior.player_life;
            }
            else
            {
                application_cooldown = set_cooldown;
            }
        }

        if (the_player != null && application_cooldown <= 0f)
        {
            if (Mathf.Abs(the_boss.transform.position.x - the_player.transform.position.x) > 8.5f)
            {
                applying_weakness = true;
                application_cooldown = set_cooldown;
            }
            else
            {
                applying_weakness = false;
            }
        }

        if (applying_weakness && !override_show)
        {
            if (!sprite.enabled)
            {
                sprite.enabled = true;
            }
        }
        else
        {
            if (sprite.enabled)
            {
                sprite.enabled = false;
            }
        }

        if (application_cooldown > 0f)
        {
            application_cooldown -= Time.deltaTime;
            if (application_cooldown <= 0f)
            {
                application_cooldown = 0f;
            }
        }
    }
}

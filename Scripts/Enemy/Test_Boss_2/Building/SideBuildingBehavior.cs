using UnityEngine;

public class SideBuildingBehavior : MonoBehaviour,EnemyDamageable
{

    private GameObject the_boss;
    GameObject level_manager;
    LevelManager_TestBoss2 manager;
    public GameObject jumping_point;

    private SideBuildingCoverBehavior cover;

    Animator ani;

    public bool boss_is_on;
    public bool destroyed = false;

    public int max_health = 600;
    public int building_health;

    public int damage;
    void Start()
    {
        cover = GetComponentInChildren<SideBuildingCoverBehavior>();
        ani = cover.gameObject.GetComponent<Animator>();
        building_health = max_health;
    }

    // Update is called once per frame
    void Update()
    {
        if (level_manager == null)
        {
            LevelManager_TestBoss2 attempt = FindAnyObjectByType<LevelManager_TestBoss2>();
            if (attempt != null)
            {
                manager = FindAnyObjectByType<LevelManager_TestBoss2>();
                level_manager = manager.gameObject;
            }
        }

        if (the_boss == null && manager != null)
        {
            if (manager.has_boss)
            {
                the_boss = manager.boss;
            }
        }
        
        if (manager == null || the_boss == null)
        {
            return;
        }

        if (!destroyed)
        {
            if (building_health > max_health * 0.67)
            {
                damage = 0;
            }

            else if (building_health <= max_health * 0.67 && building_health > max_health * 0.33)
            {
                damage = 1;
            }

            else if (building_health <= max_health * 0.33 && building_health > 0)
            {
                damage = 2;
            }

            else
            {
                damage = 3;
                destroyed = true;
            }

            ani.SetInteger("damage", damage);

        }
    }
    public void Hit(PlayerAttack player_attack)
    {
        if (!boss_is_on)
        {
            player_attack.isHit();
            TakeDamage(1);
        }
        else
        {
            player_attack.isHit();
            TakeDamage(player_attack.damage);
        }
    }

    public void TakeDamage(int damage)
    {
        building_health -= damage;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == the_boss)
        {
            boss_is_on = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == the_boss)
        {
            boss_is_on = false;
        }
    }
}

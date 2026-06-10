using UnityEngine;

public class TutorialDummyBehavior : MonoBehaviour, EnemyDamageable, EnemyMainBodyImplement
{
    public Animator ani;
    public SpriteRenderer sprite;

    public float stun_duration;
    public float freeze_duration;
    public float blind_duration;
    public float burn_duration;

    public GameObject damage_number_spawn_point;
    public GameObject Damage_Numbers;

    public GameObject burn_effect;
    public TestBoss4BurnEffect current_burn;

    public LevelManager_Tutorial level_manager;
    public void Hit(PlayerAttack player_attack)
    {
        player_attack.isHit();

        if (stun_duration + freeze_duration + blind_duration <= 0f)
        {
            if (player_attack.freezeDuration > 0)
            {
                freeze_duration = player_attack.freezeDuration;
                ani.SetBool("is_frozen", true);
            }

            if (player_attack.stunDuration > 0)
            {
                stun_duration = player_attack.stunDuration;
                ani.SetBool("is_stunned", true);
            }

            if (player_attack.blindDuration > 0)
            {
                blind_duration = player_attack.blindDuration;
                ani.SetBool("is_blinded", true);
            }
        }

        //bool modified_damage = false;
        if (burn_duration > 0)
        {
            player_attack.damage = Mathf.RoundToInt(player_attack.damage * 1.2f);
            //modified_damage = true;
        }

        if (player_attack.burnDuration > 0)
        {
            burn_duration = player_attack.burnDuration;
            if (current_burn == null)
            {
                current_burn = Instantiate(burn_effect, transform.position, Quaternion.Euler(0, 0, 0)).GetComponent<TestBoss4BurnEffect>();
                current_burn.Initialize(player_attack.burnDuration, gameObject);
            }
            else
            {
                if (!current_burn.is_destroyed)
                {
                    current_burn.lifetime = player_attack.burnDuration;
                }
                else
                {
                    Destroy(current_burn.gameObject);
                    current_burn = Instantiate(burn_effect, transform.position, Quaternion.Euler(0, 0, 0)).GetComponent<TestBoss4BurnEffect>();
                    current_burn.Initialize(player_attack.burnDuration, gameObject);
                }
            }
        }

        ani.SetBool("is_hit", true);
        TakeDamage(player_attack.damage);

        /*if (modified_damage)
        {
            player_attack.damage = Mathf.RoundToInt(player_attack.damage / 1.2f);
        }*/
    }

    public void TakeDamage(int damage)
    {
        DamageNumbersBehavior hi = Instantiate(Damage_Numbers, damage_number_spawn_point.transform.position, Quaternion.Euler(0, 0, 0)).GetComponent<DamageNumbersBehavior>();
        hi.initialize(damage);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ani = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        float time = Time.deltaTime;
        countdown_debuffs(time);

        if (level_manager.player.transform.position.x > transform.position.x)
        {
            sprite.flipX = true;
        }
        else
        {
            sprite.flipX = false;
        }
        ani.SetBool("is_stunned", stun_duration > 0);
        ani.SetBool("is_frozen", freeze_duration > 0);
        ani.SetBool("is_blinded", blind_duration > 0);
    }
    public void countdown_debuffs(float t)
    {
        if (stun_duration > 0)
        {
            stun_duration -= t;
            if (stun_duration <= 0)
            {
                stun_duration = 0;
                unset_hit();
            }
        }

        if (freeze_duration > 0)
        {
            freeze_duration -= t;
            if (freeze_duration <= 0)
            {
                freeze_duration = 0;
                unset_hit();
            }
        }

        if (blind_duration > 0)
        {
            blind_duration -= t;
            if (blind_duration <= 0)
            {
                blind_duration = 0;
                unset_hit();
            }
        }

        if (burn_duration > 0)
        {
            burn_duration -= t;
            if (burn_duration <= 0)
            {
                burn_duration = 0;
            }
        }
    }
    public void unset_hit()
    {
        ani.SetBool("is_hit", false);
    }
}

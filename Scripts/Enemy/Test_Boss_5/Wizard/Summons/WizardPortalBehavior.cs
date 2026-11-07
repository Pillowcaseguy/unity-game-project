using UnityEngine;
using static UnityEngine.ParticleSystem;

public class WizardPortalBehavior : MonoBehaviour
{
    public TestBoss5Behavior_Wizard the_boss;

    public Animator ani;
    public SpriteRenderer sprite;
    public EnemyAttack attack;

    public GameObject wall;
    public GameObject gem_particle;
    public GameObject break_point;

    public bool left;
    public bool following_player;
    public bool parried_first_time;
    public bool hit_first_time;
    void Awake()
    {
        ani = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        attack = GetComponent<EnemyAttack>();
        following_player = true;
    }
    void FixedUpdate()
    {
        if (the_boss != null && the_boss.manager.has_player)
        {
            if (following_player)
            {
                if (!left)
                {
                    transform.position = new Vector2(the_boss.manager.player.transform.position.x + 0.8f, the_boss.manager.player.transform.position.y);
                    sprite.flipY = true;
                    transform.rotation = Quaternion.Euler(0, 0, 180);
                }
                else
                {
                    transform.position = new Vector2(the_boss.manager.player.transform.position.x - 0.8f, the_boss.manager.player.transform.position.y);
                    sprite.flipY = false;
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }
        }

        if (attack.wasParried && !parried_first_time)
        {
            parried_first_time = true;
            ani.SetBool("broken", true);
            the_boss.ani.SetBool("broken_dagger", true);
            for (int i = 0; i < 10; i++)
            {
                Instantiate(gem_particle, break_point.transform.position, Quaternion.Euler(0, 0, 0));
            }
        }
        if (attack.wasHit && !hit_first_time)
        {
            hit_first_time = true;
            for (int i = 0; i < 20; i++)
            {
                var hi = Instantiate(the_boss.manager.hit_particle, the_boss.manager.player.transform.position, Quaternion.Euler(0, 0, 0)).GetComponent<SpriteRenderer>();
                hi.color = Color.white;
            }
        }
    }
    public void activate_wall()
    {
        if (the_boss != null && the_boss.manager.has_player)
        {
            if (!left)
            {
                Destroy(Instantiate(wall, new Vector2(the_boss.manager.player.transform.position.x + 1.0f, 0), Quaternion.Euler(0, 0, 90)), 1.0f);
            }
            else
            {
                Destroy(Instantiate(wall, new Vector2(the_boss.manager.player.transform.position.x - 1.0f, 0), Quaternion.Euler(0, 0, 90)), 1.0f);
            }
        }
    }
    public void stop_following_player()
    {
        following_player = false;
        var hi = the_boss.manager.player_life;
        if (hi != null && hi.movement.dash_time <= 0.19f)
        {
            transform.position = new Vector2(transform.position.x + (hi.movement.dash_time * hi.movement.dashSpeed) * (hi.movement.sprite_render.flipX ? -1 : 1), transform.position.y);
        }
    }
    public void activate_attack(int i)
    {
        if (i != 0)
        {
            attack.EnableHitbox();
        }
        else
        {
            attack.DisableHitbox();
        }
    }
    public void set_dagger()
    {
        if (!the_boss.ani.GetBool("broken_dagger"))
        {
            the_boss.ani.SetBool("intact_dagger", true);
        }
    }
    public void destroy_me()
    {
        Destroy(gameObject);
    }
}

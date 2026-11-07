using UnityEngine;

public class WizardLightSwordBehavior : MonoBehaviour, EnemyDamageable
{
    public Rigidbody2D rb;
    public SpriteRenderer sprite;
    public EnemyAttack fall_damage_1;
    public Animator ani;

    public EnemyAttack touch_damage;
    public float damage_timer;
    public bool hit_ground;
    public bool hit_ground_first_time;
    public float lifetime;
    public float time_in_ground;

    public GameObject ground_impact;
    public bool ground_impact_created;
    public GameObject light_particle;
    public GameObject crack;

    public GameObject poi1;

    public float fall_delay;
    public bool falling;

    public GameObject current_ground_impact_1;

    public GameObject parent;

    public bool spinning_left;
    public bool landed_left;
    public bool parried_first_time;
    public bool started_flying;

    public GameObject small_bullet;
    public TestBoss5Behavior_Wizard wizard;
    private void Awake()
    {
        lifetime = 12f;
        damage_timer = 2.5f;
    }
    void FixedUpdate()
    {
        if (hit_ground)
        {
            damage_timer -= Time.deltaTime;
        }
        if (damage_timer <= 0f && touch_damage.gameObject.layer == touch_damage.inactive_layer)
        {
            touch_damage.disableAttackCycle = false;
            touch_damage.EnableHitbox();
        }
        else
        {
            touch_damage.DisableHitbox();
        }

        lifetime -= Time.deltaTime;
        if (lifetime <= 0f)
        {
            destroy_me();
        }
        
        if (fall_delay > 0f)
        {
            fall_delay -= Time.deltaTime;
        }
        else if (fall_delay <= 0f && !falling)
        {
            fall_delay = 0f;
            falling = true;
            rb.linearVelocityY = -10f;
        }

        if (fall_damage_1.wasParried && !parried_first_time)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + 0.5f);
            parried_first_time = true;
            fall_damage_1.DisableHitbox();
            rb.linearVelocity = Vector2.zero;
            rb.linearVelocityY = 10f;
            rb.bodyType = RigidbodyType2D.Dynamic;
            spinning_left = !spinning_left;
        }

        if (rb.linearVelocityY < -10f)
        {
            rb.linearVelocityY = -10f;
        }

        if (touch_damage.wasHit && damage_timer <= 0)
        {
            damage_timer = 2f;
        }

        if (hit_ground && !hit_ground_first_time)
        {
            hit_ground_first_time = true;
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.linearVelocity = Vector2.zero;
            if (!ground_impact_created)
            {
                ground_impact_created = true;
                float z = (transform.rotation.eulerAngles.z % 360 + 360) % 360;
                if ((z >= 0 && z < 81) || (z > 279 && z <= 360))
                {
                    current_ground_impact_1 = Instantiate(ground_impact, new Vector2(poi1.transform.position.x, -2.4f), Quaternion.Euler(0, 0, 0));
                    Instantiate(crack, new Vector2(poi1.transform.position.x, -4.6f), Quaternion.Euler(0, 0, 0)).GetComponent<WizardGroundCrackBehavior>().time_before_fade = 6;
                    var attempt = GameManager.Instance.current_camera.GetComponent<CameraShake>();
                    if (attempt != null)
                    {
                        attempt.shakeTime = 0.7f;
                    }
                }
                else if (z > 80 && z < 280)
                {
                    if (z > 80 && z < 181)
                    {
                        var hi = parent.GetComponent<SpriteRenderer>();
                        if (hi != null)
                        {
                            hi.enabled = false;
                        }
                        parent.transform.position = new Vector2(parent.transform.position.x, -4f);
                        ani.SetBool("clatter_right", true);
                        landed_left = false;
                    }
                    else if (z > 180 && z < 280)
                    {
                        var hi = parent.GetComponent<SpriteRenderer>();
                        if (hi != null)
                        {
                            hi.enabled = false;
                        }
                        parent.transform.position = new Vector2(parent.transform.position.x, -4f);
                        ani.SetBool("clatter_left", true);
                        landed_left = true;
                    }
                }
            }
        }

        if (sprite.color.a < 1f)
        {
            sprite.color = new Vector4(sprite.color.r, sprite.color.g, sprite.color.b, sprite.color.a + 1 / 37f);
        }

        if (!hit_ground && fall_delay <= 0f)
        {
            if (spinning_left)
                sprite.transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z - 8);
            else
                sprite.transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + 8);
        }
        else
        {
            time_in_ground += Time.deltaTime;
            if (time_in_ground >= 5f)
            {
                if (ani.GetBool("clatter_left") || ani.GetBool("clatter_right"))
                {
                    if (spinning_left)
                        sprite.transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z - 15);
                    else
                        sprite.transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + 15);

                    if (!started_flying)
                    {
                        started_flying = true;
                        touch_damage.cannotBeParried = false;
                        touch_damage.clearFlags();

                        Vector2 start_position = transform.position;
                        Vector2 end_position = new Vector2(0, 0);
                        if (wizard != null)
                        {
                            end_position = wizard.manager.player.transform.position;
                        }
                        Vector2 ray_to_player = new Vector2(end_position.x - start_position.x, end_position.y - start_position.y);
                        float calc_rotation = Mathf.Atan2(ray_to_player.y, ray_to_player.x);

                        rb.linearVelocityX = Mathf.Cos(calc_rotation) * 4f;
                        rb.linearVelocityY = Mathf.Sin(calc_rotation) * 4f;
                    }

                    if (!touch_damage.cannotBeParried && touch_damage.anyFlag())
                    {
                        destroy_me();
                        return;
                    }
                }
                else
                {
                    destroy_me();
                    return;
                }
            }
        }

        if (hit_ground && sprite.color.b >= 150 / 255f)
        {
            sprite.color = new Vector4(sprite.color.r, sprite.color.g, sprite.color.b - 0.7f / 255f, sprite.color.a);
        }
    }
    public void change_rotation(int f)
    {
        transform.rotation = Quaternion.Euler(0, 0, f);
    }
    public void destroy_me()
    {
        for (int i = 0; i < 50; i++)
        {
            Instantiate(light_particle, transform.position, Quaternion.Euler(0, 0, 0));
        }
        Destroy(current_ground_impact_1);
        Destroy(parent);
    }

    public void TakeDamage(int damage)
    {

    }

    public void Hit(PlayerAttack player_attack)
    {
        player_attack.isBlock();
    }
}

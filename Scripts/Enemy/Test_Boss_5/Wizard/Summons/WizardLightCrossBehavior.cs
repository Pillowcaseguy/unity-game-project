using UnityEngine;

public class WizardLightCrossBehavior : MonoBehaviour, EnemyDamageable
{
    public Rigidbody2D rb;
    public SpriteRenderer sprite;
    public EnemyAttack fall_damage_1;
    public EnemyAttack fall_damage_2;

    public EnemyAttack touch_damage;
    public float damage_timer;
    public bool hit_ground;
    public bool hit_ground_first_time;
    public float lifetime;
    public float time_in_ground;

    public GameObject ground_impact;
    public bool ground_impact_created;
    public GameObject light_particle;

    public GameObject poi1;
    public GameObject poi2;
    public GameObject poi3;
    public GameObject poi4;

    public float fall_delay;
    public bool falling;

    public GameObject current_ground_impact_1;
    public GameObject current_ground_impact_2;

    public GameObject parent;
    public GameObject crack;
    private void Awake()
    {
        lifetime = 8f;
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

        if (fall_damage_1.anyFlag() || hit_ground)
        {
            fall_damage_2.DisableHitbox();
        }
        if (fall_damage_2.anyFlag() || hit_ground)
        {
            fall_damage_1.DisableHitbox();
        }

        if (fall_damage_1.wasParried || fall_damage_2.wasParried)
        {
            fall_damage_1.DisableHitbox();
            fall_damage_2.DisableHitbox();
            destroy_me();
        }

        if (touch_damage.wasHit && damage_timer <= 0)
        {
            damage_timer = 2f;
        }

        if (hit_ground && !hit_ground_first_time)
        {
            hit_ground_first_time = true;
            rb.linearVelocity = Vector2.zero;
            if (!ground_impact_created)
            {
                ground_impact_created = true;
                float z = (transform.rotation.eulerAngles.z % 360 + 360) % 360;
                if ((z > 0 && z < 46) || (z > 314 && z < 360))
                {
                    if (z == 45 || z == 315)
                    {
                        current_ground_impact_1 = Instantiate(ground_impact, new Vector2(poi1.transform.position.x, -2.5f), Quaternion.Euler(0, 0, 0));
                        current_ground_impact_2 = Instantiate(ground_impact, new Vector2(poi2.transform.position.x, -2.5f), Quaternion.Euler(0, 0, 0));
                    }
                    else
                    {
                        current_ground_impact_1 = Instantiate(ground_impact, new Vector2(poi1.transform.position.x, -2.5f), Quaternion.Euler(0, 0, 0));
                        Instantiate(crack, new Vector2(poi1.transform.position.x, -4.6f), Quaternion.Euler(0, 0, 0)).GetComponent<WizardGroundCrackBehavior>().time_before_fade = 6;
                    }
                }

                else if (z > 45 && z < 136)
                {
                    if (z == 135)
                    {
                        current_ground_impact_1 = Instantiate(ground_impact, new Vector2(poi3.transform.position.x, -2.5f), Quaternion.Euler(0, 0, 0));
                        current_ground_impact_2 = Instantiate(ground_impact, new Vector2(poi4.transform.position.x, -2.5f), Quaternion.Euler(0, 0, 0));
                    }
                    else
                    {
                        current_ground_impact_1 = Instantiate(ground_impact, new Vector2(poi4.transform.position.x, -2.5f), Quaternion.Euler(0, 0, 0));
                        Instantiate(crack, new Vector2(poi4.transform.position.x, -4.6f), Quaternion.Euler(0, 0, 0)).GetComponent<WizardGroundCrackBehavior>().time_before_fade = 6;
                    }
                }
                else if (z > 135 && z < 226)
                {
                    if (z == 225)
                    {
                        current_ground_impact_1 = Instantiate(ground_impact, new Vector2(poi2.transform.position.x, -2.5f), Quaternion.Euler(0, 0, 0));
                        current_ground_impact_2 = Instantiate(ground_impact, new Vector2(poi3.transform.position.x, -2.5f), Quaternion.Euler(0, 0, 0));
                    }
                    else
                    {
                        current_ground_impact_1 = Instantiate(ground_impact, new Vector2(poi3.transform.position.x, -2.5f), Quaternion.Euler(0, 0, 0));
                        Instantiate(crack, new Vector2(poi3.transform.position.x, -4.6f), Quaternion.Euler(0, 0, 0)).GetComponent<WizardGroundCrackBehavior>().time_before_fade = 6;
                    }
                }
                else if (z > 225 && z < 315)
                {
                    current_ground_impact_1 = Instantiate(ground_impact, new Vector2(poi2.transform.position.x, -2.5f), Quaternion.Euler(0, 0, 0));
                    Instantiate(crack, new Vector2(poi2.transform.position.x, -4.6f), Quaternion.Euler(0, 0, 0)).GetComponent<WizardGroundCrackBehavior>().time_before_fade = 6;
                    //current_ground_impact_1 = Instantiate(ground_impact, new Vector2(poi2.transform.position.x, poi2.transform.position.y + 1.7f), Quaternion.Euler(0, 0, 0));
                }
            }
            var attempt = GameManager.Instance.current_camera.GetComponent<CameraShake>();
            if (attempt != null)
            {
                attempt.shakeTime = 0.7f;
            }
        }

        //sprite.color = new Vector4(sprite.color.r, sprite.color.g, sprite.color.b, sprite.color.a + (255f / 37f) / 255f);
        if (sprite.color.a < 1f)
        {
            sprite.color = new Vector4(sprite.color.r, sprite.color.g, sprite.color.b, sprite.color.a + 1 / 37f);
        }

        if (!hit_ground && fall_delay <= 0f)
        {
            sprite.transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + 3);
        }
        else
        {
            time_in_ground += Time.deltaTime;
            if (time_in_ground >= 5f)
            {
                destroy_me();
                return;
            }
        }

        if (hit_ground && sprite.color.b >= 150 / 255f)
        {
            sprite.color = new Vector4(sprite.color.r, sprite.color.g, sprite.color.b - 0.7f / 255f, sprite.color.a);
        }
    }

    public void destroy_me()
    {
        for (int i = 0; i < 50; i++)
        {
            Instantiate(light_particle, transform.position, Quaternion.Euler(0, 0, 0));
        }
        Destroy(current_ground_impact_1);
        Destroy(current_ground_impact_2);
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

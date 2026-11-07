using UnityEngine;

public class TestBoss5WizardIceCrystalBehavior : MonoBehaviour
{
    public TestBoss5Behavior_Wizard the_boss;

    public Rigidbody2D rb;
    public EnemyAttack attack;
    public Animator ani;
    public SpriteRenderer sprite;

    public float speed;
    public float rotation;
    public float life;
    public bool facing_left;

    public float time_to_appear;
    public float time_to_fall;

    public bool destroyed_first_time = false;
    public bool destroyed = false;

    public LayerMask groundLayer;
    public LayerMask platform_layer;
    //public LayerMask player_layer;

    public bool ground_does_not_break;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        sprite.color = new Vector4(1, 1, 1, 0);

        groundLayer = LayerMask.GetMask("Ground");
        platform_layer = LayerMask.GetMask("Platform", "Trigger_Platform");

        life = 6f;
    }

    private void Update()
    {
        if (life > 0f)
        {
            life -= Time.deltaTime;
            if (life < 0f)
            {
                Destroy(gameObject);
            }
        }

        time_to_appear -= Time.deltaTime;
        if (time_to_appear < 0f)
        {
            time_to_appear = 0f;
        }

        time_to_fall -= Time.deltaTime;
        if (time_to_fall < 0f)
        {
            time_to_fall = 0f;
        }

    }
    private void FixedUpdate()
    {
        if (the_boss.is_dead || the_boss.inability_time > 0)
        {
            attack.damage = 0;
            attack.freezeDuration = 0;
        }
        if (attack.wasBlocked || attack.wasHit || attack.wasParried)
        {
            destroyed = true;
        }

        if (destroyed)
        {
            if (!destroyed_first_time)
            {
                destroyed_first_time = true;
                ani.SetBool("is_destroyed", true);
                transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + 180);
                //transform.position = transform.position + transform.up / 2f;
                attack.DisableHitbox();
                Destroy(gameObject, 0.75f);
            }
            rb.linearVelocityX = 0;
            rb.linearVelocityY = 0;
        }
        else
        {
            if (time_to_fall <= 0)
            {
                rb.linearVelocityX = -Mathf.Sin(rotation * Mathf.Deg2Rad) * speed;
                rb.linearVelocityY = Mathf.Cos(rotation * Mathf.Deg2Rad) * speed;
            }
        }

        if (time_to_appear <= 0f)
        {
            sprite.color = new Vector4(1, 1, 1, sprite.color.a + 0.1f);
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (!ground_does_not_break && ((1 << col.gameObject.layer) & groundLayer) != 0)
        {
            destroyed = true;
        }

        if (((1 << col.gameObject.layer) & platform_layer) != 0)
        {
            destroyed = true;
        }
    }
}

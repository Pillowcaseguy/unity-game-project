using UnityEngine;

public class TestBoss5_NinjaThrowableBehavior : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;
    public float lifetime;
    public SpriteRenderer sprite;
    public Animator ani;
    public EnemyAttack attack;

    public bool start_deflection;
    public float deflect_cooldown;
    public float impact_angle;
    public GameObject deflection_effect;
    void Awake()
    {
        attack = GetComponent<EnemyAttack>();
        rb = GetComponent<Rigidbody2D>();
        lifetime = 5f;
    }
    void FixedUpdate()
    {
        /*if (!start_deflection)
        {
            rb.linearVelocityX = -Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.Deg2Rad) * speed;
            rb.linearVelocityY = -Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.Deg2Rad) * speed;
        }
        else
        {
            if (deflect_cooldown <= 0)
            {
                //rb.linearVelocityX = 0;
                //rb.linearVelocityY = 0;
                transform.rotation = Quaternion.Euler(0, 0, 360 - impact_angle);
                /*Debug.Log(new Vector2(-Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.Deg2Rad) * speed, -Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.Deg2Rad) * speed));
                transform.position = new Vector2(transform.position.x, transform.position.y) +
                    new Vector2(-Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.Deg2Rad) * speed, -Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.Deg2Rad) * speed);
                //sprite.transform.rotation = Quaternion.Euler(0,0,sprite.transform.rotation.eulerAngles.z + 30);
                sprite.color = new Vector4(sprite.color.r, sprite.color.g, sprite.color.b, sprite.color.a - 0.3f);
                //deflect_cooldown = 0.2f;
            }
            else
            {
                deflect_cooldown -= Time.deltaTime;
            }
        }*/

        rb.linearVelocityX = -Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.Deg2Rad) * speed;
        rb.linearVelocityY = -Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.Deg2Rad) * speed;

        if (start_deflection)
        {
            transform.rotation = Quaternion.Euler(0, 0, 360 - impact_angle);
            sprite.color = new Vector4(sprite.color.r, sprite.color.g, sprite.color.b, sprite.color.a - 0.05f);
        }
    }
    private void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0 || sprite.color.a <= 0)
        {
            Destroy(gameObject);
        }
    }
}

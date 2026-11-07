using UnityEngine;

public class NinjaSwordWaveBehavior : MonoBehaviour
{
    public Rigidbody2D rb;
    public SpriteRenderer sprite;
    public float speed;
    public float lifetime;
    public Animator ani;
    public EnemyAttack attack;

    public bool hit_something;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        attack = GetComponent<EnemyAttack>();
        lifetime = 5f;
    }
    void FixedUpdate()
    {
        //Debug.Log(transform.rotation.eulerAngles.z);
        if (attack.isDestroyed || hit_something)
        {
            attack.DisableHitbox();
            rb.linearVelocity = new Vector2(0, 0);
            ani.SetBool("is_destroyed", true);
            float zee = transform.rotation.eulerAngles.z;
            if (zee >= 135 && zee <= 270)
            {
                sprite.flipY = true;
            }
            else
            {
                sprite.flipY = false;
            }
        }
        else
        {
            rb.linearVelocityX = Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.Deg2Rad) * speed;
            rb.linearVelocityY = Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.Deg2Rad) * speed;
        }

        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void destroy_self()
    {
        Destroy(gameObject);
    }
}

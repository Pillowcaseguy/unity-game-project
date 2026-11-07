using UnityEngine;

public class WizardRockBehavior : MonoBehaviour
{
    public GameObject fragment;
    public GameObject extra_shot;

    public SpriteRenderer sprite;
    public Rigidbody2D rb;
    public EnemyAttack attack;

    public float lifetime;

    public LayerMask ground_layer;
    public LayerMask other_ground_layer;

    public bool parried_first_time;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        attack = GetComponent<EnemyAttack>();
        ground_layer = LayerMask.GetMask("Ground");
        other_ground_layer = LayerMask.GetMask("BreakableGround");
        lifetime = 6f;
    }
    void FixedUpdate()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            destroy_me();
        }

        if (attack.wasParried && !parried_first_time)
        {
            parried_first_time = true;
            rb.linearVelocity = new Vector2(-rb.linearVelocityX, -rb.linearVelocityY);
        }

        if (rb.linearVelocityX > 0)
        {
            sprite.transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z - 8);
        }
        else if (rb.linearVelocityX < 0)
        {
            sprite.transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + 8);
        }
    }
    public void destroy_me()
    {
        for (int i = 0; i < 30; i++)
        {
            Destroy(Instantiate(fragment, new Vector2(transform.position.x + Random.Range(-0.25f, 0.25f), transform.position.y + 0.1f), Quaternion.Euler(0, 0, 0)), 5f);
        }
        var hi = Instantiate(extra_shot, new Vector2(transform.position.x + 0.05f, -3.8f), Quaternion.Euler(0, 0, 0)).GetComponent<WizardRockExtraShotBehavior>();
        hi.rb.linearVelocityX = 15f;

        var hi2 = Instantiate(extra_shot, new Vector2(transform.position.x - 0.05f, -3.8f), Quaternion.Euler(0, 0, 0)).GetComponent<WizardRockExtraShotBehavior>();
        hi2.rb.linearVelocityX = -15f;

        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & other_ground_layer) != 0)
        {
            rb.linearVelocity = new Vector2(-rb.linearVelocityX, rb.linearVelocityY);
        }

        if (((1 << col.gameObject.layer) & ground_layer) != 0)
        {
            destroy_me();
        }
    }
}

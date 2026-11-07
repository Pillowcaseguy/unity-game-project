using UnityEngine;

public class WizardRockExtraShotBehavior : MonoBehaviour
{
    public GameObject fragment;

    public SpriteRenderer sprite;
    public Rigidbody2D rb;
    public EnemyAttack attack;

    public float lifetime;

    public LayerMask ground_layer;

    public bool parried_first_time;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        attack = GetComponent<EnemyAttack>();
        ground_layer = LayerMask.GetMask("Ground", "BreakableGround");
        lifetime = 6f;
    }
    void FixedUpdate()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            destroy_me();
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
        Destroy(Instantiate(fragment, new Vector2(transform.position.x + (rb.linearVelocityX > 0 ? -0.1f : 0.1f), transform.position.y + 0.1f), Quaternion.Euler(0, 0, 0)), 5f);
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & ground_layer) != 0)
        {
            destroy_me();
        }
    }
}

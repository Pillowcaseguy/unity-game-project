using UnityEngine;

public class SoldierMolotovBehavior : MonoBehaviour
{
    public LayerMask ground_layer;
    public LayerMask breakable_ground_layer;
    public float lifetime;
    public Rigidbody2D rb;
    public SpriteRenderer sprite;
    public LevelManager_TestBoss5 manager;
    public EnemyAttack attack;

    public bool shattered;
    public GameObject molotov_shard;
    public GameObject sticky_burn;

    public bool hit_player_first_time;
    private void Awake()
    {
        ground_layer = LayerMask.GetMask("Ground");
        breakable_ground_layer = LayerMask.GetMask("BreakableGround");
        attack = GetComponent<EnemyAttack>();
        rb = GetComponent<Rigidbody2D>();
        lifetime = 5;
    }
    void Update()
    {
        if (attack.wasParried)
        {
            shatter();
            return;
        }
        if ((attack.wasHit || attack.wasBlocked) && !hit_player_first_time)
        {
            hit_player_first_time = true;
            rb.linearVelocity = new Vector2(rb.linearVelocityX / 2f, rb.linearVelocityY / 2f);
        }
        lifetime -= Time.deltaTime;
        if (lifetime <= 4.6f)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 1f;
        }
        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void FixedUpdate()
    {
        if (rb.linearVelocityX > 0)
        {
            sprite.gameObject.transform.rotation = Quaternion.Euler(0, 0, sprite.gameObject.transform.rotation.eulerAngles.z + 5);
        }
        else
        {
            sprite.gameObject.transform.rotation = Quaternion.Euler(0, 0, sprite.gameObject.transform.rotation.eulerAngles.z - 5);
        }
    }
    public void shatter()
    {
        if (!shattered)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.linearVelocity = Vector2.zero;
            shattered = true;
            for (int x = 0; x < 15; x++)
            {
                Instantiate(molotov_shard, gameObject.transform.position, Quaternion.Euler(0, 0, 0));
                var hi = Instantiate(sticky_burn, new Vector2(transform.position.x, transform.position.y + 0.5f), Quaternion.Euler(0, 0, 0)).GetComponent<SoldierStickyFireBehavior>();
                hi.manager = manager;
            }
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & ground_layer) != 0)
        {
            shatter();
        }

        if (((1 << col.gameObject.layer) & breakable_ground_layer) != 0)
        {
            rb.linearVelocityX = -rb.linearVelocityX;
        }
    }
}

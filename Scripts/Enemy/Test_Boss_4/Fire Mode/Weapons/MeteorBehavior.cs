using UnityEngine;

public class MeteorBehavior : MonoBehaviour
{
    public SpriteRenderer meteor_sprite;
    public SpriteRenderer trail_sprite;
    public Animator meteor_ani;
    public Animator trail_ani;
    public EnemyAttack attack;
    public Rigidbody2D rb;

    public GameObject fire_pool;

    public bool is_destroyed;
    public bool just_destroyed;

    public LayerMask ground_layer;

    public bool falling_rightwards;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attack = GetComponent<EnemyAttack>();
        rb = GetComponent<Rigidbody2D>();
        ground_layer = LayerMask.GetMask("Ground", "BreakableGround");
    }

    // Update is called once per frame
    void Update()
    {
        if (is_destroyed && !just_destroyed)
        {
            just_destroyed = true;
            attack.isDestroyed = true;
            meteor_sprite.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

            rb.linearVelocity = new Vector2(0, 0);
            meteor_sprite.flipX = false;
            trail_sprite.flipX = false;

            Destroy(trail_ani.gameObject);
            Instantiate(fire_pool, new Vector2(transform.position.x, -3f), Quaternion.Euler(0, 0, 0));
            attack.startDestroy();
            meteor_ani.SetBool("is_destroyed", true);
        }
        if (is_destroyed)
        {
            return;
        }
        meteor_sprite.transform.rotation = Quaternion.Euler(0, 0, meteor_sprite.transform.rotation.eulerAngles.z + 5);
    }
    private void FixedUpdate()
    {
        if (is_destroyed)
        {
            return;
        }

        if (falling_rightwards)
        {
            meteor_sprite.flipX = false;
            trail_sprite.flipX = false;
            rb.linearVelocityX = 8;
            rb.linearVelocityY = -8;
        }
        else
        {
            meteor_sprite.flipX = true;
            trail_sprite.flipX = true;
            rb.linearVelocityX = -8;
            rb.linearVelocityY = -8;
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & ground_layer) != 0)
        {
            if (col.GetComponent<MeteorStrikeGround>() != null)
            {
                is_destroyed = true;
            }
        }
    }
}

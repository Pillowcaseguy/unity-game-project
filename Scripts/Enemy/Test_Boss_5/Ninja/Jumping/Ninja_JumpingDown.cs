using UnityEngine;

public class Ninja_JumpingDown : MonoBehaviour
{
    public LevelManager_TestBoss5 manager;
    public EnemyAttack attack;
    public Rigidbody2D rb;
    public LayerMask ground_layer;

    public SpriteRenderer sprite;
    public BoxCollider2D col;

    public bool fine_to_destroy;
    public bool already_knocked_player;

    public float tiny_flip_delay;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
        attack = GetComponent<EnemyAttack>();
        rb = GetComponent<Rigidbody2D>();
        ground_layer = LayerMask.GetMask("Ground");
    }

    void Start()
    {
        if (manager == null)
        {
            manager = FindAnyObjectByType<LevelManager_TestBoss5>();
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!fine_to_destroy)
        {
            if (((1 << col.gameObject.layer) & ground_layer) != 0)
            {
                fine_to_destroy = true;
                rb.linearVelocity = Vector2.zero;
            }
        }
    }
    private void Update()
    {
        if (tiny_flip_delay > 0)
        {
            tiny_flip_delay -= Time.deltaTime;
            if (tiny_flip_delay < 0)
            {
                tiny_flip_delay = 0;
            }
        }

        if (manager != null && attack.wasHit && !already_knocked_player)
        {
            //summon_knocked_player();
        }
        if (manager != null && manager.has_player)
        {
            if (sprite.flipX != manager.player.transform.position.x > transform.position.x && tiny_flip_delay <= 0f)
            {
                sprite.flipX = !sprite.flipX;
                flip_hitbox();
                tiny_flip_delay = 0.05f;
            }
        }
    }
    public void flip_hitbox()
    {
        col.offset = new Vector2(-col.offset.x, col.offset.y);
    }
    public void destroy_self()
    {
        Destroy(gameObject);
    }
}

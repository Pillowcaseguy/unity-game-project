using UnityEngine;

public class Level5KnockedPlayerBehavior : MonoBehaviour
{
    public Rigidbody2D rb;
    public LayerMask ground_layer;
    public LevelManager_TestBoss5 manager;
    public Animator ani;
    public SpriteRenderer sprite;

    public bool do_not_get_up;
    public float lifetime;
    private void Awake()
    {
        lifetime = 6f;
        rb = GetComponent<Rigidbody2D>();
        ground_layer = LayerMask.GetMask("Ground", "BreakableGround");
        ani = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        if (manager == null)
        {
            manager = FindAnyObjectByType<LevelManager_TestBoss5>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (manager != null && manager.has_player)
        {
            manager.player.transform.position = gameObject.transform.position;
        }
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            destroy_self();
        }
    }
    private void OnCollisionStay2D(Collision2D col)
    {
        if (((1 << col.gameObject.layer) & ground_layer) != 0)
        {
            ani.SetBool("hitGround", true);
            if (manager.player_life.player_died || do_not_get_up)
            {
                ani.SetBool("player_is_dead", true);
            }
            rb.linearVelocityX /= 1.2f;
        }
    }
    public void player_punched()
    {
        ani.SetBool("player_punched", true);
    }
    public void destroy_self()
    {
        if (manager != null)
        {
            manager.hide_player(false);
            manager.player_life.movement.rb.bodyType = RigidbodyType2D.Dynamic;
            manager.player_life.movement.sprite_render.flipX = !sprite.flipX;
        }
        Destroy(gameObject);
    }
}

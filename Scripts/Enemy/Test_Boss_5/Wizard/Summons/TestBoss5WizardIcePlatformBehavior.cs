using UnityEngine;

public class TestBoss5WizardIcePlatformBehavior : MonoBehaviour
{
    public LevelManager_TestBoss5 manager;
    public TestBoss5Behavior_Wizard the_boss;

    public Rigidbody2D rb;
    public Animator ani;
    public SpriteRenderer sprite;

    public BoxCollider2D box;
    public EnemyAttack falling_platform;
    public PlatformLogic my_platform_logic;
    public PlatformEffector2D my_platform_effector;
    public float delay_before_visible;
    public bool visible;
    public bool visible_first_time;

    public LayerMask ground_layer;
    public LayerMask player_layer;
    public int attack_layer;

    public GameObject ice_particle;

    public bool debug_start_falling;
    void Awake()
    {
        ground_layer = LayerMask.GetMask("Ground");
        player_layer = LayerMask.GetMask("Player_Damage");
        attack_layer = LayerMask.NameToLayer("Active_Enemy_Attack");
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        sprite.color = new Vector4(1, 1, 1, 0);
    }

    void Update()
    {
        delay_before_visible -= Time.deltaTime;
        if (delay_before_visible <= 0)
        {
            delay_before_visible = 0;
            if (!visible_first_time)
            {
                visible = true;
                visible_first_time = true;
            }
        }
        if (debug_start_falling)
        {
            //ani.SetBool("breaking", true);
            start_falling();
            debug_start_falling = false;
        }
    }
    private void FixedUpdate()
    {
        if (visible && sprite.color.a <= 1f && !ani.GetBool("breaking"))
        {
            sprite.color = new Vector4(1, 1, 1, sprite.color.a + 0.01f);
        }

        if (!ani.GetBool("breaking") && (falling_platform.wasBlocked || falling_platform.wasHit || falling_platform.wasParried))
        {
            for (int i = 0; i < 25; i++)
            {
                var hi = Instantiate(ice_particle, transform.position, Quaternion.Euler(0, 0, 0));
                hi.transform.localScale = new Vector2(0.5f, 0.5f);
            }
            ani.SetBool("breaking", true);
            CancelInvoke("start_actual_fall");
            start_actual_fall();
        }
    }
    public void start_falling()
    {
        Invoke("start_actual_fall", 0.4f);
        gameObject.layer = attack_layer;
        Destroy(my_platform_logic);
        Destroy(my_platform_effector);
        box.isTrigger = true;
        box.usedByEffector = false;
    }
    public void start_actual_fall()
    {
        falling_platform.EnableHitbox();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 1f;
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & ground_layer) != 0)
        {
            if (!ani.GetBool("hit_ground"))
            {
                for (int i = 0; i < 25; i++)
                {
                    var hi = Instantiate(ice_particle, new Vector2(transform.position.x, transform.position.y + 0.5f), Quaternion.Euler(0, 0, 0));
                    hi.transform.localScale = new Vector2(0.5f, 0.5f);
                }
            }
            ani.SetBool("breaking", true);
            ani.SetBool("hit_ground", true);
            rb.bodyType = RigidbodyType2D.Static;
            falling_platform.DisableHitbox();
        }
    }
    public void fade_out()
    {
        sprite.color = new Vector4(1, 1, 1, sprite.color.a - 0.25f);
    }
    public void destroy_self()
    {
        Destroy(gameObject);
    }
}

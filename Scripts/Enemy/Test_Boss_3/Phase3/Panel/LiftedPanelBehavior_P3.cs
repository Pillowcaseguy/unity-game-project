using UnityEngine;

public class LiftedPanelBehavior_P3 : MonoBehaviour
{

    public int phase = 0;

    public Vector2 target_position;
    public Vector2 starting_position;
    public Vector2 difference;

    public GameObject breaking_panel;

    public Rigidbody2D rb;
    public BoxCollider2D box;
    public SpriteRenderer sprite;

    public bool initialized;
    public bool left;
    public bool velocity_unset = true;
    public int rotation_value;

    public int playerLayer;
    public int groundLayer;

    public bool is_destroyed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        rb.rotation = 180;
        playerLayer = LayerMask.GetMask("Player_Damage");
        groundLayer = LayerMask.GetMask("BreakableGround");
    }
    public void initialize(Vector2 start, Vector2 target, bool facing_left)
    {
        starting_position = start;
        target_position = target;
        difference = target - start;
        left = facing_left;
        initialized = true;
    }

    void Update()
    {
        box.size = new Vector2(box.size.x,sprite.size.y);
    }

    private void FixedUpdate()
    {
        if (phase == 1)
        {
            if (velocity_unset)
            {
                if (!left)
                {
                    rb.linearVelocityX = -4;
                    rotation_value = 5;
                }
                else
                {
                    rb.linearVelocityX = 4;
                    rotation_value = -5;
                }
                velocity_unset = false;
            }
            rb.rotation += rotation_value;
        }
    }
    public void step_to_point()
    {
        if (initialized)
        {
            int negative_x = target_position.x - transform.position.x < 0 ? -1 : 1;
            int negative_y = target_position.y - transform.position.y < 0 ? -1 : 1;
            transform.position = new Vector2(transform.position.x + Mathf.Min(Mathf.Abs(difference.x) / 4f,Mathf.Abs(target_position.x - transform.position.x)) * negative_x,
                transform.position.y + Mathf.Min(Mathf.Abs(difference.y) / 4f, Mathf.Abs(target_position.y - transform.position.y)) * negative_y);

            if (left && rb.rotation > 90)
            {
                rb.rotation -= 30;
                if (rb.rotation <= 90)
                    rb.rotation = 90;
            }
            if (!left && rb.rotation < 270)
            {
                rb.rotation += 30;
                if (rb.rotation >= 270)
                    rb.rotation = 270;
            }
        }
    }
    public void go_to_target()
    {
        transform.position = target_position;
    }
    public void increase_phase()
    {
        phase = 1;
    }
    private void OnTriggerStay2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & playerLayer) != 0)
        {
            if (phase == 1)
                destroy_self();
        }

        if (((1 << col.gameObject.layer) & groundLayer) != 0)
        {
            if (phase == 1)
                destroy_self();
        }

    }
    public void destroy_self()
    {
        if (!is_destroyed)
        {
            is_destroyed = true;
            Instantiate(breaking_panel, transform.position, Quaternion.Euler(0, 0, 0));
            Destroy(gameObject);
        }
    }
}

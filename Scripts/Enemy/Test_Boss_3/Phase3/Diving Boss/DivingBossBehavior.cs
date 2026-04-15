using UnityEngine;

public class DivingBossBehavior : MonoBehaviour
{
    public int groundLayer;
    public bool hit_ground;
    public Rigidbody2D rb;
    public int speed = 1;
    public SpriteRenderer sprite;
    public GameObject impact;
    public Animator child_ani;
    public SpriteRenderer child_sprite;
    private void Awake()
    {
        child_ani = GetComponentInChildren<Animator>();
        child_sprite = child_ani.gameObject.GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        hit_ground = false;
        groundLayer = LayerMask.GetMask("Ground");
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        float wangle = transform.rotation.eulerAngles.z % 360;
        if (wangle > 180 && wangle < 270)
        {
            sprite.flipY = true;
            child_sprite.flipY = true;
        }
        else
        {
            sprite.flipY = false;
            child_sprite.flipY = false;
        }

        if (!hit_ground)
        {
            rb.linearVelocityX = transform.right.x * speed;
            rb.linearVelocityY = transform.right.y * speed;
        }
        else
        {
            rb.linearVelocityX = 0;
            rb.linearVelocityY = 0;
        }
    }
    public void finish_dive()
    {
        GameObject hi = Instantiate(impact,new Vector2(transform.position.x,-3f),Quaternion.Euler(0,0,0));
        Destroy(hi, 1);
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & groundLayer) != 0)
        {
            hit_ground = true;
        }
    }
}

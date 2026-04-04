using UnityEngine;

public class DivingBossBehavior : MonoBehaviour
{
    public int groundLayer;
    public bool hit_ground;
    public Rigidbody2D rb;
    public int speed = 1;
    public SpriteRenderer sprite;
    public GameObject impact;
    void Start()
    {
        hit_ground = false;
        groundLayer = LayerMask.GetMask("Ground");
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (transform.rotation.z > 180)
        {
            sprite.flipY = true;
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
        GameObject hi = Instantiate(impact,transform.position,Quaternion.Euler(0,0,0));
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

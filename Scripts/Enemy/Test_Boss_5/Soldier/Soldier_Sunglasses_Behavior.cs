using UnityEngine;

public class Soldier_Sunglasses_Behavior : MonoBehaviour
{
    public LayerMask ground_layer;
    public Animator ani;
    public Rigidbody2D rb;
    public SpriteRenderer sprite;
    public int bounces;
    void Awake()
    {
        ani = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        ground_layer = LayerMask.GetMask("Ground");
        rb.AddForce(new Vector2(0, 300));
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & ground_layer) != 0)
        {
            if (bounces == 0)
            {
                rb.linearVelocityY = 0;
                rb.AddForceY(200);
                bounces += 1;
            }
            else if (bounces == 1)
            {
                rb.linearVelocityY = 0;
                rb.AddForceY(100);
                bounces += 1;
            }
            else
            {
                ani.SetBool("hit_ground", true);
                rb.bodyType = RigidbodyType2D.Static;
            }
        }
    }
}

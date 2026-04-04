using UnityEngine;

public class FallingBossBehavior : MonoBehaviour
{

    public LayerMask groundLayer;
    public Rigidbody2D rb;
    public Animator ani;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        groundLayer = LayerMask.GetMask("Ground");
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & groundLayer) != 0)
        {
            rb.linearVelocityX = 0;
            rb.linearVelocityY = 0;
            rb.bodyType = RigidbodyType2D.Kinematic;
            ani.SetInteger("phase", 1);
        }
    }
}

using UnityEngine;

public class ShellCasingBehavior : MonoBehaviour
{
    public float lifetime = 4.0f;

    public Rigidbody2D rb;
    public bool fly_left;

    LayerMask groundLayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        groundLayer = LayerMask.GetMask("Ground", "BreakableGround");
        float initial_x = fly_left ? 50 : -50;
        rb.AddForceX(initial_x);
    }
    private void FixedUpdate()
    {
        rb.transform.Rotate(0, 0, 20);
    }

    void Update()
    {
        if (lifetime > 0)
        {
            lifetime -= Time.deltaTime;
            if (lifetime <= 0)
            {
                lifetime = 0;
                destroy_self();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {

        if (((1 << col.gameObject.layer) & groundLayer) != 0)
        {
            destroy_self();
        }
    }
    public void destroy_self()
    {
        Destroy(gameObject);
    }
}

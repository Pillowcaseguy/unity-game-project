using UnityEngine;

public class IceParticleBehavior : MonoBehaviour
{
    public float lifetime;
    public bool keep_on_ground_hit;

    public LayerMask ground_layer;
    public Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        lifetime = 2.5f;
        ground_layer = LayerMask.GetMask("Ground", "BreakableGround");
        rb = GetComponent<Rigidbody2D>();

        float x_force = Random.Range(-150f, 150f);
        float y_force = Random.Range(100f, 200f);

        rb.AddForce(new Vector2(x_force, y_force));
    }

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!keep_on_ground_hit && ((1 << col.gameObject.layer) & ground_layer) != 0)
        {
            Destroy(gameObject);
        }
    }
}

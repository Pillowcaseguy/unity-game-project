using UnityEngine;

public class GuardBreakBehavior : MonoBehaviour
{
    public LayerMask ground_layer;
    public float lifetime;
    public Rigidbody2D rb;
    void Awake()
    {
        ground_layer = LayerMask.GetMask("Ground", "BreakableGround");
        lifetime = 3f;
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(Random.Range(-100, 100), 200));
    }
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
        if (((1 << col.gameObject.layer) & ground_layer) != 0)
        {
            Destroy(gameObject);
        }
    }

}

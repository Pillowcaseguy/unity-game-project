using UnityEngine;

public class SoldierMolotovShardBehavior : MonoBehaviour
{
    public LayerMask ground_layer;
    public Rigidbody2D rb;
    public SpriteRenderer sprite;
    void Awake()
    {
        ground_layer = LayerMask.GetMask("Ground", "BreakableGround");
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(Random.Range(-200f, 200f), Random.Range(150f, 250f)));
    }
    private void FixedUpdate()
    {
        if (rb.linearVelocityX > 0)
        {
            sprite.gameObject.transform.rotation = Quaternion.Euler(0, 0, sprite.gameObject.transform.rotation.eulerAngles.z + 10);
        }
        else
        {
            sprite.gameObject.transform.rotation = Quaternion.Euler(0, 0, sprite.gameObject.transform.rotation.eulerAngles.z - 10);
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

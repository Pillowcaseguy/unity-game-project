using UnityEngine;

public class SoldierGrenadePinBehavior : MonoBehaviour
{
    public Rigidbody2D rb;
    public SpriteRenderer sprite;

    public LayerMask ground_layer;
    public float lifetime;
    void Awake()
    {
        lifetime = 5;
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        ground_layer = LayerMask.GetMask("Ground");
    }
    void FixedUpdate()
    {
        float sprite_rotate = rb.linearVelocityX > 0 ? -5 : 5;
        sprite.transform.rotation = Quaternion.Euler(0, 0, sprite.transform.rotation.eulerAngles.z + sprite_rotate);

        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void OnTriggerEnter2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & ground_layer) != 0)
        {
            Destroy(gameObject);
        }
    }
}

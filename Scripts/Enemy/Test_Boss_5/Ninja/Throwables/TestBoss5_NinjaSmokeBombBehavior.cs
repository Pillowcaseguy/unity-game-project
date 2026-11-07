using UnityEngine;

public class TestBoss5_NinjaSmokeBombBehavior : MonoBehaviour
{
    public GameObject smoke;

    public SpriteRenderer sprite;
    public BoxCollider2D box;
    public Rigidbody2D rb;
    public float rotation;
    public float speed;
    public LayerMask ground_layer;
    public LayerMask player_layer;
    public float lifetime;

    void Awake()
    {
        lifetime = 8f;
        rb = GetComponent<Rigidbody2D>();
        ground_layer = LayerMask.GetMask("Ground", "BreakableGround");
        player_layer = LayerMask.GetMask("Player_Damage");
    }
    void FixedUpdate()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }
        rb.linearVelocityX = Mathf.Cos(rotation * Mathf.Deg2Rad) * speed;
        rb.linearVelocityY = Mathf.Sin(rotation * Mathf.Deg2Rad) * speed;
        sprite.transform.rotation = Quaternion.Euler(0, 0, sprite.transform.rotation.eulerAngles.z + 5);
    }
    private void OnTriggerStay2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & ground_layer) != 0)
        {
            Instantiate(smoke, new Vector2(gameObject.transform.position.x, -4.6f), Quaternion.Euler(0, 0, 0));
            Destroy(gameObject);
        }
        else if (((1 << col.gameObject.layer) & player_layer) != 0)
        {
            Instantiate(smoke, transform.position, Quaternion.Euler(0, 0, 0));
            Destroy(gameObject);
        }
    }
}

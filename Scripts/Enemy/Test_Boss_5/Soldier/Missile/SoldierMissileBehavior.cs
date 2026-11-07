using UnityEngine;

public class SoldierMissileBehavior : MonoBehaviour
{
    public LayerMask player_layer;
    public bool has_exploded;
    public GameObject explosion;
    public GameObject explosion_point;
    public Rigidbody2D rb;
    public float lifetime;
    public float rotation;
    public float speed;
    public LevelManager_TestBoss5 manager;
    public float initial_player_x;
    public float initial_player_y;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        lifetime = 3f;
        player_layer = LayerMask.GetMask("Player_Damage");
    }
    void Start()
    {
        transform.rotation = Quaternion.Euler(0, 0, rotation);
        initial_player_x = manager.player.transform.position.x;
        initial_player_y = manager.player.transform.position.y;
    }
    public void explode()
    {
        if (!has_exploded)
        {
            has_exploded = true;
            Destroy(Instantiate(explosion, explosion_point.transform.position, Quaternion.Euler(0, 0, 0)), 0.625f);
            Destroy(gameObject);
        }
    }
    void FixedUpdate()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            if (!has_exploded)
            {
                explode();
            }
        }

        if (has_exploded)
        {
            rb.linearVelocity = new Vector2(0, 0);
        }
        else
        {
            rb.linearVelocityY = -Mathf.Cos(rotation * Mathf.Deg2Rad) * speed;
            rb.linearVelocityX = Mathf.Sin(rotation * Mathf.Deg2Rad) * speed;
        }

    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & player_layer) != 0)
        {
            explode();
        }
    }
}

using UnityEngine;

public class WizardFireballBehavior : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject child_sprite;
    public EnemyAttack my_attack;

    public float lifetime;
    public LayerMask ground_layer;
    public LayerMask player_layer;

    public GameObject fire_leftover;
    public bool destroyed;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        my_attack = GetComponent<EnemyAttack>();
        lifetime = 8f;
        ground_layer = LayerMask.GetMask("Ground");
        player_layer = LayerMask.GetMask("Player_Damage");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float angle = Mathf.Atan2(rb.linearVelocityY, rb.linearVelocityX) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0,0, angle);

        child_sprite.transform.rotation = Quaternion.Euler(0, 0, child_sprite.transform.rotation.eulerAngles.z + 5);

        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }

    }
    private void OnTriggerStay2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & ground_layer) != 0)
        {
            if (!destroyed)
            {
                destroyed = true;
                Destroy(Instantiate(fire_leftover, new Vector2(transform.position.x, transform.position.y + 0.5f), Quaternion.Euler(0,0,0)), 0.5f);
                Destroy(gameObject);
            }
        }
        if (((1 << col.gameObject.layer) & player_layer) != 0)
        {
            if (!destroyed && (my_attack.wasHit || my_attack.wasBlocked))
            {
                destroyed = true;
                Destroy(Instantiate(fire_leftover, new Vector2(transform.position.x, transform.position.y + 0.5f), Quaternion.Euler(0, 0, 0)), 0.5f);
                Destroy(gameObject);
            }
        }
    }
}

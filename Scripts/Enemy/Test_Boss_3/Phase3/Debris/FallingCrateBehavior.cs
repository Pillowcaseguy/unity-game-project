using UnityEngine;

public class FallingCrateBehavior : MonoBehaviour
{
    public bool is_destroyed;
    public int combined_layer;
    public Rigidbody2D rb;
    public SpriteRenderer sprite;
    public Animator ani;
    public EnemyAttack attack;
    void Start()
    {
        combined_layer = LayerMask.GetMask("Player_Damage", "Ground");
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocityY = -20;
        sprite = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
        attack = GetComponent<EnemyAttack>();
    }
    private void OnTriggerStay2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & combined_layer) != 0)
        {
            ani.SetBool("isDestroyed", true);
            attack.DisableHitbox();
        }
    }
    public void destroy_self()
    {
        if (!is_destroyed)
        {
            is_destroyed = true;
            Destroy(gameObject);
        }
    }
    public void step_to_destroy()
    {
        rb.linearVelocity = Vector2.zero;
        rb.rotation += 45;
        transform.localPosition = new Vector2(transform.localPosition.x + 0.3f,transform.localPosition.y + 0.3f);
        sprite.color = new Color(sprite.color.r,sprite.color.g,sprite.color.b,sprite.color.a - 0.1f);
    }
}

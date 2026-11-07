using UnityEngine;

public class Wizard_WizardHatBehavior : MonoBehaviour
{
    public LayerMask ground_layer;
    public LayerMask breakable_ground_layer;
    public Animator ani;
    public Rigidbody2D rb;
    public SpriteRenderer sprite;
    void Awake()
    {
        ani = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        ground_layer = LayerMask.GetMask("Ground");
        breakable_ground_layer = LayerMask.GetMask("BreakableGround");
    }
    private void FixedUpdate()
    {
        if (rb.bodyType != RigidbodyType2D.Static)
        {
            transform.rotation = Quaternion.Euler(0,0,transform.rotation.eulerAngles.z + ((rb.linearVelocityX > 0) ? -8 : 8));
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        if (transform.position.y < -20f)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & ground_layer) != 0)
        {
            rb.bodyType = RigidbodyType2D.Static;
        }
        if (((1 << col.gameObject.layer) & breakable_ground_layer) != 0)
        {
            rb.linearVelocityX = -rb.linearVelocityX;
        }
    }
}

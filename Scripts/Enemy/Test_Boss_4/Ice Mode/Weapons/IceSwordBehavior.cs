using UnityEngine;

public class IceSwordBehavior : MonoBehaviour
{
    public Rigidbody2D rb;
    public SpriteRenderer sprite;

    public float lifetime;
    public float speed;
    public bool initialized;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }
    public void initialize(float life, float sped)
    {
        lifetime = life;
        speed = sped;
        initialized = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (!initialized)
        {
            return;
        }

        lifetime -= Time.deltaTime; 
        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void FixedUpdate()
    {
        if (!initialized)
        {
            rb.linearVelocityX = 0;
            rb.linearVelocityY = 0;
        }
        else
        {
            /*rb.linearVelocityX = Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.Deg2Rad) * speed;
            rb.linearVelocityY = Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.Deg2Rad) * speed;*/
            rb.linearVelocityX = transform.right.x * speed;
            rb.linearVelocityY = transform.right.y * speed;
            //rb.rotation += (speed > 0) ? -10 : 10;
        }
    }
}

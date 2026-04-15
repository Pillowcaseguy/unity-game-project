using UnityEngine;

public class EnergyBallBehavior : MonoBehaviour
{
    public int groundLayer;
    public Rigidbody2D rb;
    public int speed = 15;
    public GameObject impact;
    void Start()
    {
        groundLayer = LayerMask.GetMask("Ground");
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb.linearVelocityX = transform.right.x * speed;
        rb.linearVelocityY = transform.right.y * speed;
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & groundLayer) != 0)
        {
            GameObject thing = Instantiate(impact,new Vector2(transform.position.x,-3f),Quaternion.Euler(0,0,0));
            Destroy(thing,0.5f);
            Destroy(gameObject);
        }
    }
}

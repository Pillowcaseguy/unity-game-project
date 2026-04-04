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
            GameObject thing = Instantiate(impact,transform.position,Quaternion.Euler(0,0,0));
            Destroy(thing,1f);
            Destroy(gameObject);
        }
    }
}

using UnityEngine;

public class PigToy : MonoBehaviour
{

    public float lifetime;
    public LayerMask groundLayer;

    private BoxCollider2D col;
    private EnemyAttack drop;

    public Vector2 origin;
    public float starting_width;
    public float starting_height;
    public float max_distance;

    public bool dropped;

    public RaycastHit2D hit_result;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lifetime = 10.0f;
        groundLayer = LayerMask.GetMask("Ground");

        col = GetComponent<BoxCollider2D>();
        drop = GetComponentInChildren<EnemyAttack>();
        max_distance = 12f;
        dropped = false;

    }

    // Update is called once per frame
    void Update()
    {

        if (lifetime > 0f)
        {
            lifetime -= Time.deltaTime;
        }

        else if (lifetime < 0f)
        {
            Destroy(gameObject);
        }

        starting_width = col.bounds.center.x;
        starting_height = col.bounds.center.y - col.bounds.size.y / 2.0f;
        origin = new Vector2(starting_width, starting_height);

        RaycastHit2D hit_center = Physics2D.Raycast(origin, Vector2.down, max_distance, groundLayer);
        //Debug.DrawRay(origin, Vector2.down * max_distance, Color.green);
        hit_result = hit_center;
        if (!dropped)
        {
            if (hit_result.collider != null && hit_result.distance <= 0.1f)
            {
                dropped = true;
                drop.DisableHitbox();
            }
        }
    }
}

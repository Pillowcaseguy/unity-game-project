using UnityEngine;

public class SmallHotDogShadow : MonoBehaviour
{

    private SmallHotDogBehavior HEHHH;
    private Transform transform;
    private Transform parent_transform;
    private SpriteRenderer sprite;
    private BoxCollider2D col;

    public Vector2 origin;
    public float starting_width;
    public float starting_height;
    public float max_distance = 25f;
    public RaycastHit2D hit_result;

    public bool activated = false;
    public float offset = 0;

    public LayerMask groundLayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HEHHH = GetComponentInParent<SmallHotDogBehavior>();
        transform = GetComponent<Transform>();
        parent_transform = HEHHH.GetComponent<Transform>();
        sprite = GetComponent<SpriteRenderer>();
        col = GetComponentInParent<BoxCollider2D>();
        //transform.SetParent(null);

        groundLayer = LayerMask.GetMask("Ground");

    }

    // Update is called once per frame
    void Update()
    {

        if (parent_transform == null)
        {
            Destroy(gameObject);
            return;
        }

        if (!activated)
        {
            return;
        }

        starting_width = col.bounds.center.x;
        starting_height = col.bounds.center.y - col.bounds.size.y / 2.0f;
        origin = new Vector2(starting_width, starting_height);

        RaycastHit2D hit_center = Physics2D.Raycast(origin, Vector2.down, max_distance, groundLayer);
        //Debug.DrawRay(origin, Vector2.down * max_distance, Color.green);

        if (hit_center.collider == null)
        {
            sprite.enabled = false;
        }

        else
        {
            transform.rotation = Quaternion.Euler(0,0,0);
            sprite.enabled = true;
            GameObject hit_object = hit_center.collider.gameObject;

            float new_scale = Mathf.Abs(hit_center.distance - max_distance) * 0.2f;
            if (new_scale < 0.8f)
            {
                new_scale = 0.8f;
            }
            else if (new_scale > 1.9f)
            {
                new_scale = 2.0f;
            }

            transform.localScale = new Vector2(new_scale, new_scale);
            transform.position = new Vector2(parent_transform.position.x + offset, hit_object.transform.position.y + 0.5f * hit_object.transform.localScale.y);
        }
    }
    public void detach_shadow()
    {
        transform.SetParent(null);
        offset = -0.5f;
    }
    public void hide_shadow()
    {
        sprite.enabled = false;
    }

}

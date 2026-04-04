using UnityEngine;

public class SpikesBehavior : MonoBehaviour
{
    public SpriteRenderer sprite;
    public BoxCollider2D box;
    public float target_width;
    public float start_x;
    public bool facing_left;
    public bool initialized;

    public bool hit_target;
    
    public GameObject end;
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        box = GetComponent<BoxCollider2D>();
    }
    public void initialize(float target,float starting_point,bool left)
    {
        target_width = target;
        start_x = starting_point;
        facing_left = left;
        initialized = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (!initialized)
        {
            return;
        }

        int direction = facing_left ? -1 : 1;
        transform.position = new Vector2(start_x + (sprite.size.x * direction) / 2f, transform.position.y);

        end.transform.localPosition = new Vector2(sprite.size.x * direction / 2f, 0);
        box.size = new Vector2((sprite.size.x + 0.2f), box.size.y);
        if (direction == 1)
        {
            end.GetComponent<SpriteRenderer>().flipX = true;
        }

    }
    public void increase_size()
    {
        if (!initialized)
        {
            return;
        }

        if (sprite.size.x < target_width)
        {
            sprite.size = new Vector2(Mathf.Min(target_width, sprite.size.x + (target_width / 2.0f)), sprite.size.y);
        }
    }

    public void decrease_size()
    {
        if (!initialized)
        {
            return;
        }
        sprite.size = new Vector2(Mathf.Min(target_width, sprite.size.x - (target_width / 2.0f)), sprite.size.y);
    }
    public void destroy()
    {
        Destroy(gameObject);
    }
}

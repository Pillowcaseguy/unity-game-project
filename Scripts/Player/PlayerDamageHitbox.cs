using UnityEngine;

public class PlayerDamageHitbox : MonoBehaviour
{

    private PlayerMovement movement;
    private BoxCollider2D box;

    public float starting_height;
    public float starting_offset;
    public float crouching_height = 0.25f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movement = GetComponentInParent<PlayerMovement>();
        box = GetComponent<BoxCollider2D>();
        starting_height = box.size.y;
        starting_offset = box.offset.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (movement.crouching)
        {
            box.size = new Vector2(box.size.x, 0.25f);
            box.offset = new Vector2(box.offset.x, starting_offset - (starting_height - box.size.y) / 2.0f);
        }
        else
        {
            box.size = new Vector2(box.size.x, starting_height);
            box.offset = new Vector2(box.offset.x, starting_offset);
        }
    }
}

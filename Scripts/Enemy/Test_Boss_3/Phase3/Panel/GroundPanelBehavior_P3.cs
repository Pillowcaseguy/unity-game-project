using UnityEngine;

public class GroundPanelBehavior_P3 : MonoBehaviour
{

    public SpriteRenderer sprite;
    public BoxCollider2D box;

    public GameObject moving_panel;
    public TestBoss3_Phase3Behavior the_boss;

    public bool left;

    public float target_height = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        box = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if (sprite.size.y < target_height)
        {
            sprite.size = new Vector2(sprite.size.x,sprite.size.y + 0.1f);
        }
        box.size = new Vector2(sprite.size.x,sprite.size.y);
        box.offset = new Vector2(0, sprite.size.y / 2f);
    }
    public void destroy()
    {
        if (the_boss != null)
        {
            if (left)
            {
                the_boss.lifted_panel_left = Instantiate(moving_panel, new Vector2(transform.position.x, transform.position.y + sprite.size.y), Quaternion.Euler(0, 0, 0));
                the_boss.lifted_panel_behavior_left = the_boss.lifted_panel_left.GetComponent<LiftedPanelBehavior_P3>();
                the_boss.lifted_panel_behavior_left.initialize(new Vector2(transform.position.x, transform.position.y + sprite.size.y), the_boss.panel_lift_point_right.transform.position, left);
                Destroy(gameObject);
            }
            else
            {
                the_boss.lifted_panel_right = Instantiate(moving_panel, new Vector2(transform.position.x, transform.position.y + sprite.size.y), Quaternion.Euler(0, 0, 0));
                the_boss.lifted_panel_behavior_right = the_boss.lifted_panel_right.GetComponent<LiftedPanelBehavior_P3>();
                the_boss.lifted_panel_behavior_right.initialize(new Vector2 (transform.position.x,transform.position.y + sprite.size.y), the_boss.panel_lift_point_left.transform.position, left);
                Destroy(gameObject);
            }
        }
    }
}

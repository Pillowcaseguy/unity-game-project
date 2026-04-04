using UnityEngine;

public class GroundPanelBehavior : MonoBehaviour
{

    public SpriteRenderer sprite;
    public BoxCollider2D box;

    public GameObject moving_panel;
    public TestBoss3_Phase2Behavior the_boss;

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
            the_boss.lifted_panel = Instantiate(moving_panel, new Vector2(transform.position.x, transform.position.y + sprite.size.y), Quaternion.Euler(0, 0, 0));
            the_boss.lifted_panel_behavior = the_boss.lifted_panel.GetComponent<LiftedPanelBehavior>();
            the_boss.lifted_panel.GetComponent<LiftedPanelBehavior>().initialize(transform.position,the_boss.panel_lift_point.transform.position,the_boss);
            Destroy(gameObject);
        }
    }
}

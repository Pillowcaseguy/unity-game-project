using UnityEngine;

public class FoxmanSpriteBehavior : MonoBehaviour
{

    private FoxmanBehavior FOXMAN;
    private Transform transform;
    private SpriteRenderer sprite;
    private BoxCollider2D box;

    private bool death_rotate = false;
    private float death_delay = 0.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FOXMAN = GetComponentInParent<FoxmanBehavior>();
        transform = GetComponent<Transform>();
        sprite = GetComponent<SpriteRenderer>();
        box = GetComponentInParent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (FOXMAN.death_time > 0)
        {
            if (!death_rotate)
            {
                if (death_delay > 0)
                {
                    death_delay -= Time.deltaTime;
                    if (death_delay <= 0)
                    {
                        death_rotate = true;
                        death_delay = 0;
                        transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                }
            }
        }

        else
        {

            if (FOXMAN.running_left)
            {
                sprite.flipX = false;
                transform.rotation = Quaternion.Euler(0, 0, 340);
                box.offset = new Vector2(-1.64f, 4.7f);
            }
            else
            {
                sprite.flipX = true;
                transform.rotation = Quaternion.Euler(0, 0, 20);
                box.offset = new Vector2(11f, 4.7f);
            }
        }
    }
    public void revive_finished()
    {
        FOXMAN.is_reviving = false;
        death_rotate = false;
        death_delay = 0.5f;
    }

}

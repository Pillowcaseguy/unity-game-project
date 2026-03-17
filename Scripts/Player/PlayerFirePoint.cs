using UnityEngine;

public class PlayerFirePoint : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    private Transform parentTransform;
    private PlayerMovement movement;

    private float offset_x;
    private float offset_y;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponentInParent<SpriteRenderer>();
        movement = GetComponentInParent<PlayerMovement>();
        parentTransform = transform.parent;
        offset_x = (parentTransform.position.x - transform.position.x);
        offset_y = (parentTransform.position.y - transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 player_aim = movement.attemptedMove;

        if (spriteRenderer.flipX)
        {
            transform.position = new Vector2(parentTransform.position.x + offset_x,
            parentTransform.position.y - offset_y);

            if (player_aim.y == 1) // Aiming up
            {
                if (player_aim.x == 0) // Straight
                {
                    transform.position = new Vector2(parentTransform.position.x, parentTransform.position.y - offset_y);
                    transform.rotation = Quaternion.Euler(0, 0, 90);
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, 0, 135); // Backwards Up
                }
            }
            else if (player_aim.y == -1)
            {
                if (player_aim.x == 0)
                {
                    if (!movement.locked)
                    {
                        transform.rotation = Quaternion.Euler(0, 0, 180);
                    }
                    else
                    {
                        transform.position = new Vector2(parentTransform.position.x, parentTransform.position.y - offset_y);
                        transform.rotation = Quaternion.Euler(0, 0, 270);
                    }
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, 0, 225); // Backwards down
                }
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 180); // Backwards straight
            }

        }
        else
        {
            transform.position = new Vector2(parentTransform.position.x - offset_x,
            parentTransform.position.y - offset_y);

            if (player_aim.y == 1) // Aiming up
            {
                if (player_aim.x == 0) // Straight
                {
                    transform.position = new Vector2(parentTransform.position.x, parentTransform.position.y - offset_y);
                    transform.rotation = Quaternion.Euler(0, 0, 90);
                }
                else // x == 1
                {
                    transform.rotation = Quaternion.Euler(0, 0, 45);
                }
            }
            else if (player_aim.y == -1) // Aiming down
            {
                if (player_aim.x == 0) // Straight
                {
                    if (!movement.locked)
                    {
                        transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                    else
                    {
                        transform.position = new Vector2(parentTransform.position.x, parentTransform.position.y - offset_y);
                        transform.rotation = Quaternion.Euler(0, 0, 270);
                    }
                }
                else // x == 1
                {
                    transform.rotation = Quaternion.Euler(0, 0, 315);
                }
            }
            else // No vertical aim
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }

        }

        if (movement.crouching)
        {
            transform.position = new Vector2(transform.position.x,
            parentTransform.position.y - 0.06f * parentTransform.localScale.x);
        }
        else
        {
            transform.position = new Vector2(transform.position.x,
            parentTransform.position.y - 0.02f * parentTransform.localScale.x);
        }

    }
}

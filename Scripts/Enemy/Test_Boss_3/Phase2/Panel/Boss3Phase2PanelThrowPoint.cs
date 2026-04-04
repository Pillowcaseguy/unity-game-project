using UnityEngine;
using UnityEngine.U2D;

public class Boss3Phase2PanelThrowPoint : MonoBehaviour
{
    public bool flipped_once = false;
    public bool unflipped_once = true;
    public SpriteRenderer sprite;
    void Start()
    {
        sprite = GetComponentInParent<SpriteRenderer>();
    }

    public void Flip()
    {
        Vector2 localPos = transform.localPosition;

        if (sprite.flipX)
        {
            if (!flipped_once)
            {
                localPos.x = -localPos.x;
                transform.localPosition = localPos;
                flipped_once = true;
                unflipped_once = false;
            }
        }
        else
        {
            if (!unflipped_once)
            {
                localPos.x = -localPos.x;
                transform.localPosition = localPos;
                flipped_once = false;
                unflipped_once = true;
            }
        }
    }
}

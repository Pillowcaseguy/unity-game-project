using UnityEngine;
public class GrapplingBossShellCasingPoint : MonoBehaviour
{

    private SpriteRenderer sprite;
    public bool flipped_once = false;
    public bool unflipped_once = true;
    public GameObject shell_casing;
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
    public void eject()
    {
        GameObject hi = Instantiate(shell_casing, transform.position, Quaternion.Euler(0, 0, 0));
        bool fly_l = sprite.flipX ? false : true;
        hi.GetComponent<ShellCasingBehavior>().fly_left = fly_l;
    }

}

using UnityEngine;
public class GrapplingBossThrowPoint : MonoBehaviour
{

    private SpriteRenderer sprite;
    public bool flipped_once = false;
    public bool unflipped_once = true;
    public GameObject landmine;
    public GameObject smokebomb;
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
    public void throw_something()
    {
        Instantiate(landmine, transform.position, Quaternion.Euler(0, 0, 0));
    }

    public void drop_smoke()
    {
        int temp = Random.Range(0, 4);
        if (temp == 1)
        {
            Instantiate(smokebomb, transform.position, Quaternion.Euler(0, 0, 0));
        }
    }
}

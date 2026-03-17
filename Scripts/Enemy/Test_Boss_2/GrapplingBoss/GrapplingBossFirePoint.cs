using UnityEngine;
public class GrapplingBossFirePoint : MonoBehaviour
{

    private SpriteRenderer sprite;
    public bool flipped_once = false;
    public bool unflipped_once = true;
    public GameObject bullet;
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
    public void shoot()
    {
        if (sprite.flipX)
        {
            var hi = Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, 330));
            hi.GetComponent<BulletBehavior>().Initialize(10, 5);
        }
        else
        {
            var hi = Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, 210));
            hi.GetComponent<BulletBehavior>().Initialize(10, 5);
        }
    }
}

using UnityEngine;

public class SoldierStickyFireGroundCheck : MonoBehaviour
{
    public SoldierStickyFireBehavior sticky_fire;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & sticky_fire.ground_layer) != 0)
        {
            sticky_fire.hit_ground();
        }
    }
}

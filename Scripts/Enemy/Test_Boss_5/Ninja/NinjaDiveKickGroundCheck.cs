using UnityEngine;

public class NinjaDiveKickGroundCheck : MonoBehaviour
{
    public BoxCollider2D ground_box;
    public TestBoss5Behavior_Ninja boss_behavior;
    public bool touching_ground;

    public LayerMask ground_layer;

    void Awake()
    {
        ground_layer = LayerMask.GetMask("Ground", "BreakableGround");
    }
    private void OnTriggerStay2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & ground_layer) != 0)
        {
            touching_ground = true;
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & ground_layer) != 0)
        {
            touching_ground = false;
        }
    }

}

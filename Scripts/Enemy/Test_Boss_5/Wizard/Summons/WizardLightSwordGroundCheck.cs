using UnityEngine;

public class WizardLightSwordGroundCheck : MonoBehaviour
{
    public WizardLightSwordBehavior light_sword;
    public LayerMask ground_layer;
    private void Awake()
    {
        ground_layer = LayerMask.GetMask("Ground");
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & ground_layer) != 0)
        {
            light_sword.hit_ground = true;
        }
    }
}

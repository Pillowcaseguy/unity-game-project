using UnityEngine;

public class WizardLightCrossGroundCheck : MonoBehaviour
{
    public WizardLightCrossBehavior light_cross;
    public LayerMask ground_layer;
    private void Awake()
    {
        ground_layer = LayerMask.GetMask("Ground");
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & ground_layer) != 0)
        {
            light_cross.hit_ground = true;
        }
    }
}

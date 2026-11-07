using UnityEngine;

public class NinjaSwordWaveGroundCheck : MonoBehaviour
{
    public NinjaSwordWaveBehavior wave;
    public LayerMask ground_layer;
    void Awake()
    {
        ground_layer = LayerMask.GetMask("Ground", "BreakableGround");
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & ground_layer) != 0)
        {
            wave.hit_something = true;
        }
    }
}

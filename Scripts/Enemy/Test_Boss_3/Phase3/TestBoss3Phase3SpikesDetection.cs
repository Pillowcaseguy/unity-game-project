using UnityEngine;

public class TestBoss3Phase3SpikesDetection : MonoBehaviour
{

    public TestBoss3_Phase3Behavior the_boss;
    public int playerLayer;

    void Start()
    {
        playerLayer = LayerMask.GetMask("Player_Damage");
    }
    private void OnTriggerStay2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & playerLayer) != 0)
        {
            the_boss.spikes_attack_hittable = true;
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & playerLayer) != 0)
        {
            the_boss.spikes_attack_hittable = false;
        }
    }
}

using UnityEngine;

public class TestBoss3Phase2HighAttackDetection : MonoBehaviour
{

    public TestBoss3_Phase2Behavior the_boss;
    public int playerLayer;

    void Start()
    {
        playerLayer = LayerMask.GetMask("Player_Damage");
    }
    private void OnTriggerStay2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & playerLayer) != 0)
        {
            the_boss.high_attack_hittable = true;
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & playerLayer) != 0)
        {
            the_boss.high_attack_hittable = false;
        }
    }
}

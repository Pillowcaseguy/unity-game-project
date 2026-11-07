using UnityEngine;

public class NinjaCrossSlashRangeChecker : MonoBehaviour
{
    public LayerMask player_layer;
    public TestBoss5Behavior_Ninja boss;
    void Awake()
    {
        player_layer = LayerMask.GetMask("Player_Damage");
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & player_layer) != 0)
        {
            boss.cross_slash_in_range = true;
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & player_layer) != 0)
        {
            boss.cross_slash_in_range = false;
        }
    }
}

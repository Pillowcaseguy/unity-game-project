using UnityEngine;

public class Soldier_Charge_Detect : MonoBehaviour
{
    public TestBoss5Behavior_Soldier boss;
    public LayerMask player_layer;
    private void Awake()
    {
        player_layer = LayerMask.GetMask("Player_Damage");
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & player_layer) != 0)
        {
            if (!boss.manager.player_life.movement.crouching || boss.player_crouched_too_early)
            {
                boss.player_grabbable = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & player_layer) != 0)
        {
            boss.player_grabbable = false;
        }
    }
}

using UnityEngine;

public class Soldier_Deadly_Charge_Detect : MonoBehaviour
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
            boss.player_deadly_grabbable = true;
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & player_layer) != 0)
        {
            boss.player_deadly_grabbable = false;
        }
    }
}

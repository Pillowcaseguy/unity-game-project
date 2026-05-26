using UnityEngine;

public class TestBoss4SwordDetection : MonoBehaviour
{
    public TestBoss4Behavior the_boss;
    public LayerMask player_layer;
    void Start()
    {
        player_layer = LayerMask.GetMask("Player_Damage");
    }
    public void OnTriggerStay2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & player_layer) != 0)
        {
            the_boss.player_in_sword_range = true;
        }
    }
    public void OnTriggerExit2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & player_layer) != 0)
        {
            the_boss.player_in_sword_range = false;
        }
    }
}

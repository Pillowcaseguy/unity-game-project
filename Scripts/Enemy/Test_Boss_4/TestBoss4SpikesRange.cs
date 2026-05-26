using UnityEngine;

public class TestBoss4SpikesRange : MonoBehaviour
{
    public TestBoss4Behavior boss;
    public LayerMask player_layer;
    void Start()
    {
        player_layer = LayerMask.GetMask("Player_Damage");
    }

    // Update is called once per frame
    /*void Update()
    {
        
    }*/
    public void OnTriggerStay2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & player_layer) != 0)
        {
            boss.player_in_range = true;
        }
    }
    public void OnTriggerExit2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & player_layer) != 0)
        {
            boss.player_in_range = false;
        }
    }
}

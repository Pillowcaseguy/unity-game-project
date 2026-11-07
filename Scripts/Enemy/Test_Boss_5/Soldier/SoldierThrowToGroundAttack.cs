using UnityEngine;

public class SoldierThrowToGroundAttack : MonoBehaviour
{
    public EnemyAttack attack;
    public TestBoss5Behavior_Soldier soldier;
    public bool scan_for_player;
    void Awake()
    {
        attack = GetComponent<EnemyAttack>();
    }
    void Update()
    {
        if (!scan_for_player)
        {
            return;
        }
        if (soldier.current_knocked_player)
        {
            if (soldier.current_knocked_player.ani.GetBool("hitGround"))
            {
                soldier.manager.player_life.Hit_No_Collider(gameObject);
                scan_for_player = false;
            }
        }
    }
}

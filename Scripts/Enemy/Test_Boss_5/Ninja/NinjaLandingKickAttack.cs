using UnityEngine;

public class NinjaLandingKickAttack : MonoBehaviour
{

    public EnemyAttack my_attack;
    public TestBoss5Behavior_Ninja boss_logic;

    public bool knocked_player_summoned;

    void Update()
    {
        if (my_attack.wasHit && !knocked_player_summoned)
        {
            knocked_player_summoned = true;
            boss_logic.summon_knocked_player();
        }
    }
}

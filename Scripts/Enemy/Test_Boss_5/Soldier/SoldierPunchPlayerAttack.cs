using UnityEngine;

public class SoldierPunchPlayerAttack : MonoBehaviour
{
    public EnemyAttack attack;
    public TestBoss5Behavior_Soldier soldier;
    void Awake()
    {
        attack = GetComponent<EnemyAttack>();
    }
    public void attack_player()
    {
        soldier.manager.player_life.Hit_No_Collider(gameObject);
    }
}

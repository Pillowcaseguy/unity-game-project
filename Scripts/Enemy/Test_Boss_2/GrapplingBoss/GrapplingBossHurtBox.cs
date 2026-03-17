using UnityEngine;

public class GrapplingBossHurtBox : MonoBehaviour,EnemyDamageable
{
    public GameObject the_boss;
    public GrapplingBossBehavior boss_behavior;
    public void Hit(PlayerAttack player_attack)
    {
        boss_behavior.Hit(player_attack);
    }

    public void TakeDamage(int damage)
    {
        
    }

    void Awake()
    {
        boss_behavior = the_boss.GetComponent<GrapplingBossBehavior>();
    }
}

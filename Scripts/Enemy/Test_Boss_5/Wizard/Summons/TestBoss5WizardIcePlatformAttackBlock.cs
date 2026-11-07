using UnityEngine;

public class TestBoss5WizardIcePlatformAttackBlock : MonoBehaviour, EnemyDamageable
{
    public void Hit(PlayerAttack player_attack)
    {
        player_attack.isBlock();
    }

    public void TakeDamage(int damage)
    {
        
    }
}

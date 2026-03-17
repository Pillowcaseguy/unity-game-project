using UnityEngine;

public interface EnemyDamageable{
    void TakeDamage(int damage);
    void Hit(PlayerAttack player_attack);

}
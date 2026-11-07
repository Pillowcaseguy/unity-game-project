using UnityEngine;

public class TestBoss4BlockingAttacksHitbox : MonoBehaviour, EnemyDamageable, EnemyMainBodyImplement
{
    public TestBoss4Behavior the_boss;
    public BoxCollider2D col;
    public LayerMask player_attack_layer;
    void Start()
    {
        player_attack_layer = LayerMask.GetMask("Active_Player_Attack");
    }
    public void Hit(PlayerAttack player_attack)
    {
        if (the_boss.is_blocking && the_boss.phase == 0)
        {
            if (player_attack.damage >= 100)
            {
                the_boss.was_blocking = true;
                the_boss.stun_me(3.5f);
                the_boss.block_finished_hitting();
                the_boss.stop_blocking_attacks();
            }
            else
            {
                the_boss.TakeDamage(0);
                the_boss.block_started_hitting();
                player_attack.isBlock();
            }
        }
    }

    public void TakeDamage(int damage)
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (the_boss.phase == 1 && ((1 << col.gameObject.layer) & player_attack_layer) != 0)
        {
            the_boss.block_started_hitting();
        }
    }
}

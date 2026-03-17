using UnityEngine;

public class Kick : MonoBehaviour, EnemyDamageable
{
    public EnemyAttack attack;
    private TestBoss1Behavior boss;
    public bool player_in_range;

    public LayerMask playerLayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attack = GetComponent<EnemyAttack>();
        boss = GetComponent<TestBoss1Behavior>();
        playerLayer = LayerMask.GetMask("Player_Damage");
    }

    public void enable()
    {
        attack.EnableHitbox();
    }

    public void disable()
    {
        attack.DisableHitbox();
    }

    public void TakeDamage(int damage)
    {
        boss.TakeDamage(damage);
    }

    public void Hit(PlayerAttack player_attack)
    {
        player_attack.isHit();
        TakeDamage(player_attack.damage);
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & playerLayer) != 0)
        {
            player_in_range = true;
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & playerLayer) != 0)
        {
            player_in_range = false;
        }
    }
}

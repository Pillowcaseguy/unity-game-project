using UnityEngine;
public class TestBoss4SwordSlashManager : MonoBehaviour
{

    public LevelManager_TestBoss4 level_manager;

    public BoxCollider2D slash_1;
    public BoxCollider2D slash_2;
    public BoxCollider2D slash_3;
    public BoxCollider2D slash_4;
    public BoxCollider2D slash_5;
    public BoxCollider2D slash_6;

    public EnemyAttack attack_1;
    public EnemyAttack attack_2;
    public EnemyAttack attack_3;
    public EnemyAttack attack_4;
    public EnemyAttack attack_5;
    public EnemyAttack attack_6;

    public LayerMask player_layer;
    public LayerMask active_layer;
    public LayerMask inactive_layer;

    public GameObject ice_particle;
    public bool hit_1_exploded = false;
    public bool hit_2_exploded = false;
    public bool hit_3_exploded = false;
    public bool hit_4_exploded = false;
    public bool hit_5_exploded = false;
    public bool hit_6_exploded = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player_layer = LayerMask.GetMask("Player_Damage");
        active_layer = LayerMask.GetMask("Active_Enemy_Attack");
        inactive_layer = LayerMask.GetMask("Inactive_Enemy_Attack");
    }
    private void Update()
    {
        if (!hit_1_exploded && (attack_1.wasBlocked || attack_1.wasHit || attack_1.wasParried))
        {
            explode_particles(50);
            hit_1_exploded = true;
        }

        if (!hit_2_exploded && (attack_2.wasBlocked || attack_2.wasHit || attack_2.wasParried))
        {
            explode_particles(50);
            hit_2_exploded = true;
        }

        if (!hit_3_exploded && (attack_3.wasBlocked || attack_3.wasHit || attack_3.wasParried))
        {
            explode_particles(50);
            hit_3_exploded = true;
        }

        if (!hit_4_exploded && (attack_4.wasBlocked || attack_4.wasHit || attack_4.wasParried))
        {
            explode_particles(50);
            hit_4_exploded = true;
        }

        if (!hit_5_exploded && (attack_5.wasBlocked || attack_5.wasHit || attack_5.wasParried))
        {
            explode_particles(50);
            hit_5_exploded = true;
        }

        if (!hit_6_exploded && (attack_6.wasBlocked || attack_6.wasHit || attack_6.wasParried))
        {
            explode_particles(50);
            hit_6_exploded = true;
        }
    }
    public void activate_hitbox(int x)
    {
        if (x == 0)
        {
            attack_1.EnableHitbox();
        }
        else if (x == 1)
        {
            attack_2.EnableHitbox();
        }
        else if (x == 2)
        {
            attack_3.EnableHitbox();
        }
        else if (x == 3)
        {
            attack_4.EnableHitbox();
        }
        else if (x == 4)
        {
            attack_5.EnableHitbox();
        }
        else if (x == 5)
        {
            attack_6.EnableHitbox();
        }
    }
    public void deactivate_hitbox(int x)
    {
        if (x == 0)
        {
            attack_1.DisableHitbox();
            hit_1_exploded = false;
            attack_1.clearFlags();
            attack_1.disableAttackCycle = false;
        }
        else if (x == 1)
        {
            attack_2.DisableHitbox();
            hit_2_exploded = false;
            attack_2.clearFlags();
            attack_2.disableAttackCycle = false;
        }
        else if (x == 2)
        {
            attack_3.DisableHitbox();
            hit_3_exploded = false;
            attack_3.clearFlags();
            attack_3.disableAttackCycle = false;
        }
        else if (x == 3)
        {
            attack_4.DisableHitbox();
            hit_4_exploded = false;
            attack_4.clearFlags();
            attack_4.disableAttackCycle = false;
        }
        else if (x == 4)
        {
            attack_5.DisableHitbox();
            hit_5_exploded = false;
            attack_5.clearFlags();
            attack_5.disableAttackCycle = false;
        }
        else if (x == 5)
        {
            attack_6.DisableHitbox();
            hit_6_exploded = false;
            attack_6.clearFlags();
            attack_6.disableAttackCycle = false;
        }
    }
    public void explode_particles(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Instantiate(ice_particle, level_manager.player.transform.position, Quaternion.Euler(0, 0, 0));
        }

    }
    /*void OnDrawGizmos()
    {
        if (slash_1.gameObject.layer == LayerMask.NameToLayer("Active_Enemy_Attack"))
        {
            Gizmos.color = Color.red;
            BoxCollider2D col = slash_1;
            Gizmos.DrawWireCube(transform.position + (Vector3)col.offset, col.size);
        }
        else
        {
            Gizmos.color = Color.green;
            BoxCollider2D col = slash_1;
            Gizmos.DrawWireCube(transform.position + (Vector3)col.offset, col.size);
        }

        if (slash_2.gameObject.layer == LayerMask.NameToLayer("Active_Enemy_Attack"))
        {
            Gizmos.color = Color.red;
            BoxCollider2D col = slash_2;
            Gizmos.DrawWireCube(transform.position + (Vector3)col.offset, col.size);
        }
        else
        {
            Gizmos.color = Color.green;
            BoxCollider2D col = slash_2;
            Gizmos.DrawWireCube(transform.position + (Vector3)col.offset, col.size);
        }

        if (slash_3.gameObject.layer == LayerMask.NameToLayer("Active_Enemy_Attack"))
        {
            Gizmos.color = Color.red;
            BoxCollider2D col = slash_3;
            Gizmos.DrawWireCube(transform.position + (Vector3)col.offset, col.size);
        }
        else
        {
            Gizmos.color = Color.green;
            BoxCollider2D col = slash_3;
            Gizmos.DrawWireCube(transform.position + (Vector3)col.offset, col.size);
        }

        if (slash_4.gameObject.layer == LayerMask.NameToLayer("Active_Enemy_Attack"))
        {
            Gizmos.color = Color.red;
            BoxCollider2D col = slash_4;
            Gizmos.DrawWireCube(transform.position + (Vector3)col.offset, col.size);
        }
        else
        {
            Gizmos.color = Color.green;
            BoxCollider2D col = slash_4;
            Gizmos.DrawWireCube(transform.position + (Vector3)col.offset, col.size);
        }

        if (slash_5.gameObject.layer == LayerMask.NameToLayer("Active_Enemy_Attack"))
        {
            Gizmos.color = Color.red;
            BoxCollider2D col = slash_5;
            Gizmos.DrawWireCube(transform.position + (Vector3)col.offset, col.size);
        }
        else
        {
            Gizmos.color = Color.green;
            BoxCollider2D col = slash_5;
            Gizmos.DrawWireCube(transform.position + (Vector3)col.offset, col.size);
        }

        if (slash_6.gameObject.layer == LayerMask.NameToLayer("Active_Enemy_Attack"))
        {
            Gizmos.color = Color.red;
            BoxCollider2D col = slash_6;
            Gizmos.DrawWireCube(transform.position + (Vector3)col.offset, col.size);
        }
        else
        {
            Gizmos.color = Color.green;
            BoxCollider2D col = slash_6;
            Gizmos.DrawWireCube(transform.position + (Vector3)col.offset, col.size);
        }
    }*/
}

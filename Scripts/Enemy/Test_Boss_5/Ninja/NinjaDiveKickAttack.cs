using JetBrains.Annotations;
using UnityEngine;

public class NinjaDiveKickAttack : MonoBehaviour
{
    public BoxCollider2D attack_box;
    public EnemyAttack my_attack;
    public TestBoss5Behavior_Ninja boss_behavior;
    //public bool touching_ground;

    //public LayerMask player_layer;
    //public LayerMask ground_layer;
    void Awake()
    {
        //player_layer = LayerMask.GetMask("Player_Damage");
        //ground_layer = LayerMask.GetMask("Ground", "BreakableGround");
    }

    void FixedUpdate()
    {
        if (my_attack.wasParried)
        {
            boss_behavior.jump_kick_twas_parried = true;
            boss_behavior.ani.SetBool("jump_kicking", false);
            boss_behavior.ani.SetBool("jump_kick_parried", true);
            my_attack.clearFlags();
            my_attack.DisableHitbox();
            my_attack.disableAttackCycle = false;
        }
        if (boss_behavior.dive_kick_ground.touching_ground && boss_behavior.ani.GetBool("jump_kicking"))
        {
            boss_behavior.ani.SetBool("jump_kicking", false);
            boss_behavior.rb.linearVelocityX = 0;
            if (GameManager.Instance.current_camera != null)
            {
                GameManager.Instance.current_camera.GetComponent<CameraShake>().shakeTime += 0.5f;
            }

            if (boss_behavior.health <= boss_behavior.max_health / 2f && !boss_behavior.ani.GetBool("jump_kick_parried"))
            {
                boss_behavior.rb.bodyType = RigidbodyType2D.Dynamic;
                boss_behavior.rb.linearVelocityX = 0;
                boss_behavior.rb.linearVelocityY = 0;
                boss_behavior.ani.SetBool("dive_kick_follow_up", true);
            }
            else
            {
                boss_behavior.end_attack();
            }
            /*if (my_attack.wasBlocked || my_attack.wasHit)
            {
                
            }
            else
            {
                boss_behavior.end_attack();
            }*/
        }
    }
}

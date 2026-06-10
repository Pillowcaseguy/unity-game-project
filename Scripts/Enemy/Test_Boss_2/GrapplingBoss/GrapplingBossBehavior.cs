using UnityEngine;
public class GrapplingBossBehavior : MonoBehaviour, EnemyDamageable, EnemyMainBodyImplement
{

    public Rigidbody2D rb;
    public SpriteRenderer sprite;
    public Animator ani;
    public BoxCollider2D box_main;
    public BoxCollider2D box_child;
    public EnemyAttack attack;

    public int accumulated_damage;
    public int boss_current_hp;

    public GameObject the_boss;
    public TestBoss2Behavior boss_AI;
    public bool initialized = false;
    public bool side_is_right;
    public bool invincible;
    public GameObject jump_target;

    public int swings = 0;
    public float swing_cooldown;

    public LevelManager_TestBoss2 level;
    public GameObject left_high;
    public GameObject left_low;
    public GameObject right_high;
    public GameObject right_low;
    public GameObject grapple_end_point;

    public GameObject throwing_point;
    public GrapplingBossThrowPoint tp;
    public GameObject grapple_hold_point;
    public GrapplingBossGrapplePoint gp;
    public GameObject shooting_point;
    public GrapplingBossFirePoint fp;
    public GameObject shell_casing_point;
    public GrapplingBossShellCasingPoint scp;

    public GameObject grappling_wire;
    public GameObject current_grappling_wire;

    public int current_attack; // 1 = low swing, 2 = gun, 3 = bomb
    public bool done_swinging_completely;

    public int dead_layer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
        box_main = GetComponent<BoxCollider2D>();
        attack = GetComponentInChildren<EnemyAttack>();
        box_child = attack.gameObject.GetComponent<BoxCollider2D>();
        tp = throwing_point.GetComponent<GrapplingBossThrowPoint>();
        gp = grapple_hold_point.GetComponent<GrapplingBossGrapplePoint>();
        fp = shooting_point.GetComponent<GrapplingBossFirePoint>();
        scp = shell_casing_point.GetComponent<GrapplingBossShellCasingPoint>();
        swing_cooldown = 3.0f;
        dead_layer = LayerMask.NameToLayer("Inactive_Enemy_Attack");
    }
    public void initialize(GameObject boss, bool start_right)
    {
        LevelManager_TestBoss2 attempt = FindAnyObjectByType<LevelManager_TestBoss2>();
        if (attempt != null)
        {
            level = attempt;
            initialized = true;
            left_high = level.left_high;
            left_low = level.left_low;
            right_high = level.right_high;
            right_low = level.right_low;
            grapple_end_point = level.grapple_end_point;
        }

        the_boss = boss;
        boss_AI = the_boss.GetComponent<TestBoss2Behavior>();
        boss_current_hp = boss_AI.health;
        invincible = true;
        side_is_right = start_right;
        current_grappling_wire = Instantiate(grappling_wire);
        var temp = current_grappling_wire.GetComponent<GrapplingWireBehavior>();
        temp.sprite.sortingOrder = 2;
        temp.initialize(grapple_hold_point, grapple_end_point.transform.position);
        current_attack = Random.Range(1, 4);
        if (start_right)
        {
            if (current_attack == 1)
            {
                transform.position = right_low.transform.position;
            }
            else
            {
                transform.position = right_high.transform.position;
            }
        }
        else
        {
            if (current_attack == 1)
            {
                transform.position = left_low.transform.position;
            }
            else
            {
                transform.position = left_high.transform.position;
            }
        }

    }
    void Update()
    {
        if (box_child.gameObject.layer == dead_layer)
        {
            box_main.enabled = true;
        }
        else
        {
            box_main.enabled = false;
        }
        if (swing_cooldown > 0)
        {
            swing_cooldown -= Time.deltaTime;
            if (swing_cooldown < 0)
            {
                swing_cooldown = 0;
            }
        }
    }

    void FixedUpdate()
    {
        if (!initialized)
        {
            return;
        }

        if (rb.linearVelocityX == 0 || swing_cooldown > 0)
        {
            ani.SetBool("is_grappling", false);
            ani.SetBool("is_throwing", false);
            ani.SetBool("is_shooting", false);

            invincible = true;
        }
        else
        {
            invincible = false;
        }

        if (rb.linearVelocityX > 0)
        {
            sprite.flipX = true;
            flip_points();
        }
        else
        {
            sprite.flipX = false;
            flip_points();
        }

        if (jump_target == null && !done_swinging_completely)
        {
            if (!side_is_right) // We are on the left, no jump target!
            {
                if (current_attack == 1)
                {
                    transform.position = left_low.transform.position;
                    jump_target = right_low;
                }
                else
                {
                    transform.position = left_high.transform.position;
                    jump_target = right_high;
                }
            }
            else // We are on the right, no jump target!
            {
                if (current_attack == 1)
                {
                    transform.position = right_low.transform.position;
                    jump_target = left_low;
                }
                else
                {
                    transform.position = right_high.transform.position;
                    jump_target = left_high;
                }
            }
        }

        if (jump_target != null)
        {
            if (swing_cooldown <= 0)
            {
                if (side_is_right)
                {
                    if (jump_target.transform.position.x < gameObject.transform.position.x)
                    {
                        if (current_attack == 1)
                        {
                            if (!done_swinging_completely)
                            {
                                rb.linearVelocityX = -8;
                                ani.SetBool("is_grappling", true);
                                ani.SetBool("is_throwing", false);
                                ani.SetBool("is_shooting", false);
                            }
                            else
                            {
                                ani.SetBool("is_grappling", true);
                                ani.SetBool("is_throwing", false);
                                ani.SetBool("is_shooting", false);
                                box_child.enabled = false;
                            }
                        }

                        if (current_attack == 2)
                        {
                            if (!done_swinging_completely)
                            {
                                rb.linearVelocityX = -6;
                                ani.SetBool("is_shooting", true);
                                ani.SetBool("is_grappling", false);
                                ani.SetBool("is_throwing", false);
                            }
                            else
                            {
                                ani.SetBool("is_grappling", true);
                                ani.SetBool("is_shooting", false);
                                ani.SetBool("is_throwing", false);
                            }
                        }

                        if (current_attack == 3)
                        {
                            if (!done_swinging_completely)
                            {
                                rb.linearVelocityX = -6;
                                ani.SetBool("is_throwing", true);
                                ani.SetBool("is_shooting", false);
                                ani.SetBool("is_grappling", false);
                            }
                            else
                            {
                                ani.SetBool("is_throwing", false);
                                ani.SetBool("is_shooting", false);
                                ani.SetBool("is_grappling", true);
                            }
                        }
                    }
                    else
                    {
                        rb.linearVelocityX = 0;
                        side_is_right = false;
                        endOfLine();
                    }
                }
                else
                {
                    if (jump_target.transform.position.x > gameObject.transform.position.x)
                    {
                        if (current_attack == 1)
                        {
                            if (!done_swinging_completely)
                            {
                                rb.linearVelocityX = 8;
                                ani.SetBool("is_throwing", false);
                                ani.SetBool("is_shooting", false);
                                ani.SetBool("is_grappling", true);
                            }
                            else
                            {
                                ani.SetBool("is_throwing", false);
                                ani.SetBool("is_shooting", false);
                                ani.SetBool("is_grappling", true);
                                box_child.enabled = false;
                            }
                        }

                        if (current_attack == 2)
                        {
                            if (!done_swinging_completely)
                            {
                                rb.linearVelocityX = 6;
                                ani.SetBool("is_shooting", true);
                                ani.SetBool("is_grappling", false);
                                ani.SetBool("is_throwing", false);

                            }
                            else
                            {
                                ani.SetBool("is_grappling", true);
                                ani.SetBool("is_shooting", false);
                                ani.SetBool("is_throwing", false);
                            }
                        }

                        if (current_attack == 3)
                        {
                            if (!done_swinging_completely)
                            {
                                rb.linearVelocityX = 6;
                                ani.SetBool("is_throwing", true);
                                ani.SetBool("is_shooting", false);
                                ani.SetBool("is_grappling", false);
                            }
                            else
                            {
                                ani.SetBool("is_throwing", false);
                                ani.SetBool("is_shooting", false);
                                ani.SetBool("is_grappling", true);
                            }
                        }
                    }
                    else
                    {
                        rb.linearVelocityX = 0;
                        side_is_right = true;
                        endOfLine();
                    }
                }
            }
        }
    }

    public void finish_swinging()
    {
        boss_AI.jump_target = null;
    }

    public void Hit(PlayerAttack player_attack)
    {
        if (invincible)
        {
            return;
        }
        else
        {
            player_attack.isHit();
            TakeDamage(player_attack.damage);
        }
    }
    public void TakeDamage(int damage)
    {
        accumulated_damage += damage;
        if (the_boss != null && accumulated_damage > boss_current_hp)
        {
            done_swinging_completely = true;
        }
    }
    public void endOfLine()
    {
        current_attack = current_attack % 3 + 1;
        swing_cooldown = 2.0f;
        swings += 1;
        attack.disableAttackCycle = false;
        attack.EnableHitbox();
        attack.clearFlags();
        jump_target = null;
        if (swings >= 3)
        {
            done_swinging_completely = true;
        }

        if (done_swinging_completely)
        {
            attack.damage = 0;
            attack.stunDuration = 0;
            Destroy(current_grappling_wire);
            Invoke("finish_swinging",1f);
        }
    }
    public void flip_points()
    {
        fp.Flip();
        tp.Flip();
        gp.Flip();
        scp.Flip();
    }
    public void shoot_bullet()
    {
        fp.shoot();
        scp.eject();
    }
    public void throwing()
    {
        tp.throw_something();
    }
    public void drop_smoke()
    {
        tp.drop_smoke();
    }
}

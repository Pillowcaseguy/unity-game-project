using UnityEngine;

public class TestBoss2Behavior : MonoBehaviour, EnemyDamageable, EnemyMainBodyImplement
{

    public bool aiming_gun;
    public bool queued_shot;

    public GameObject sniper_rifle;
    GameObject current_sniper;
    SniperBehavior sb;

    public GameObject dynamite;

    private SpriteRenderer sprite;
    private Animator ani;
    private Rigidbody2D rb;
    private BoxCollider2D box;
    private RuntimeAnimatorController basic_ani_controller;

    private TestBoss2ThrowingPoint throwing_point;

    [SerializeField] private Sprite aiming_up;
    [SerializeField] private Sprite aiming_down;

    public float attackTime = 0f;
    public float moveTime = 0f;

    public int phase;
    public int starting_health = 1000;
    public int health;

    public int last_attack = 0;
    public int attack_count = 0;

    public bool is_attacking;
    public bool is_jumping;
    public bool is_preparing_jump;
    public bool is_throwing;
    public bool getting_gun;
    public bool is_invincible;

    public bool first_jump;

    public GameObject main_building;
    public GameObject left_building;
    public GameObject right_building;

    public SideBuildingBehavior left;
    public SideBuildingBehavior right;

    public GameObject jump_target;

    public GameObject grapple_wire;
    public GameObject current_grapple_wire;

    public float hardcoded_start_x = -0.32f;
    public float hardcoded_start_y = 3f;

    public GameObject assault_rifle;
    GameObject current_AR;
    AssaultRifleBehavior arb;

    public bool AR_just_destroyed;

    public bool is_jump_attacking;
    public GameObject phase2_jump_point;

    public GameObject swinging_boss;
    public GameObject current_swinging_boss;

    public bool is_dead;
    public bool just_died = false;

    /*public AnimationClip getting_sniper_anim;
    public AnimationClip getting_AR_anim;
    public AnimationClip putting_sniper_away;
    public AnimationClip putting_AR_away;*/

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        throwing_point = GetComponentInChildren<TestBoss2ThrowingPoint>();
        basic_ani_controller = ani.runtimeAnimatorController;
        phase = 1;
        health = starting_health;
    }

    // Update is called once per frame
    void Update()
    {

        if (!just_died && is_dead)
        {
            just_died = true;
        }

        if (is_dead)
        {
            if (!ani.enabled)
            {
                ani.enabled = true;
            }
            end_attack();
            ani.SetBool("is_dead", is_dead);
            sprite.sortingOrder = 0;
            transform.position = new Vector2(-0.32f, 3.3f);
            return;
        }

        if (!left)
        {
            SideBuildingBehavior attempt_l = left_building.GetComponent<SideBuildingBehavior>();
            if (attempt_l)
            {
                left = attempt_l;
            }
        }

        if (!right)
        {
            SideBuildingBehavior attempt_r = right_building.GetComponent<SideBuildingBehavior>();
            if (attempt_r)
            {
                right = attempt_r;
            }
        }

        if (ani.runtimeAnimatorController != basic_ani_controller && !is_attacking)
        {
            ani.runtimeAnimatorController = basic_ani_controller;
        }

        AnimatorStateInfo stateInfo = ani.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("hiding"))
        {
            sprite.sortingOrder = -2;
            attackTime += Time.deltaTime;
            is_invincible = true;
        }
        else
        {
            sprite.sortingOrder = 0;
            is_invincible = false;
        }

        if (is_jump_attacking)
        {
            is_invincible = true;
        }

        if (phase == 1)
        {

            if (!is_preparing_jump && !is_attacking && attackTime >= 2)
            {
                bool not_in_middle = left.boss_is_on || right.boss_is_on;
                if (attack_count >= 2 && not_in_middle)
                {
                    prepare_jump();
                }
                else if (attack_count >= 1 && !not_in_middle)
                {
                    prepare_jump();
                }
            }

            if (attackTime > 5.0f && !is_attacking && attack_count < 2)
            {
                is_attacking = true;
                attack_count += 1;
                do_attack();
            }

            if (aiming_gun)
            {
                ani.enabled = false;
                if (current_sniper == null)
                {
                    current_sniper = Instantiate(sniper_rifle, new Vector2(transform.position.x + 0.12f, transform.position.y + 0.1f), Quaternion.Euler(0, 0, 0));
                    sb = current_sniper.GetComponent<SniperBehavior>();
                    sb.instantiate(gameObject);
                }

                if (current_sniper != null)
                {
                    if (!queued_shot)
                    {
                        queued_shot = true;
                        Invoke("take_shot", 3);
                    }

                    float gun_rotation = sb.gameObject.transform.rotation.eulerAngles.z;

                    if (gun_rotation > 270)
                    {
                        sprite.flipX = true;
                        throwing_point.Flip();
                        if (gun_rotation > 315)
                        {
                            sprite.sprite = aiming_up;

                        }
                        else
                        {
                            sprite.sprite = aiming_down;
                        }
                    }

                    else
                    {
                        sprite.flipX = false;
                        throwing_point.Flip();
                        if (gun_rotation < 225)
                        {
                            sprite.sprite = aiming_up;
                        }
                        else
                        {
                            sprite.sprite = aiming_down;
                        }
                    }
                }
            }

            else
            {
                if (!ani.enabled)
                {
                    ani.enabled = true;
                }
            }

            if (left.boss_is_on && left.destroyed)
            {
                end_attack();
                attack_count = 0;
                prepare_jump();
            }

            if (right.boss_is_on && right.destroyed)
            {
                end_attack();
                attack_count = 0;
                prepare_jump();
            }
        }

        if (!left.boss_is_on && !right.boss_is_on && !is_jumping && left.destroyed && right.destroyed && phase == 1 && !is_attacking)
        {
            phase += 1;
            attackTime = 0;
            attack_count = 0;
            last_attack = 0;
            is_jumping = false;
            is_preparing_jump = false;
            left_building.GetComponentInChildren<EnemyActiveBossComponent>().notPrimary();
            right_building.GetComponentInChildren<EnemyActiveBossComponent>().notPrimary();
        }

        if (phase == 2)
        {
            if (is_invincible)
            {
                gameObject.layer = LayerMask.NameToLayer("Inactive_Enemy_Attack");
            }
            else
            {
                gameObject.layer = LayerMask.NameToLayer("Enemy");
            }

            if (attackTime > 4.0f && !is_attacking && !is_jump_attacking)
            {
                is_attacking = true;
                do_attack();
            }

            if (aiming_gun)
            {
                ani.enabled = false;
                if (last_attack == 1)
                {
                    if (current_sniper == null)
                    {
                        current_sniper = Instantiate(sniper_rifle, new Vector2(transform.position.x + 0.12f, transform.position.y + 0.1f), Quaternion.Euler(0, 0, 0));
                        sb = current_sniper.GetComponent<SniperBehavior>();
                        sb.instantiate(gameObject);
                    }

                    if (current_sniper != null)
                    {
                        if (!queued_shot)
                        {
                            queued_shot = true;
                            Invoke("take_shot", 2);
                        }

                        float gun_rotation = sb.gameObject.transform.rotation.eulerAngles.z;

                        if (gun_rotation > 270)
                        {
                            sprite.flipX = true;
                            throwing_point.Flip();
                            if (gun_rotation > 315)
                            {
                                sprite.sprite = aiming_up;

                            }
                            else
                            {
                                sprite.sprite = aiming_down;
                            }
                        }

                        else
                        {
                            sprite.flipX = false;
                            throwing_point.Flip();
                            if (gun_rotation < 225)
                            {
                                sprite.sprite = aiming_up;
                            }
                            else
                            {
                                sprite.sprite = aiming_down;
                            }
                        }
                    }
                }

                else if (last_attack == 2)
                {
                    if (current_AR == null)
                    {
                        current_AR = Instantiate(assault_rifle, new Vector2(transform.position.x + 0.12f, transform.position.y + 0.1f), Quaternion.Euler(0, 0, 0));
                        arb = current_AR.GetComponent<AssaultRifleBehavior>();
                        int rand = Random.Range(0, 2);
                        bool x = (rand == 0) ? true : false;
                        arb.initalize(x,gameObject);
                    }

                    if (current_AR != null)
                    {
                        if (!AR_just_destroyed && arb.started_destruction)
                        {
                            AR_just_destroyed = true;
                            Invoke("end_attack", 1f);
                        }

                        float gun_rotation = arb.gameObject.transform.rotation.eulerAngles.z;

                        if (gun_rotation > 270)
                        {
                            sprite.flipX = true;
                            throwing_point.Flip();
                            if (gun_rotation > 315)
                            {
                                sprite.sprite = aiming_up;

                            }
                            else
                            {
                                sprite.sprite = aiming_down;
                            }
                        }

                        else
                        {
                            sprite.flipX = false;
                            throwing_point.Flip();
                            if (gun_rotation < 225)
                            {
                                sprite.sprite = aiming_up;
                            }
                            else
                            {
                                sprite.sprite = aiming_down;
                            }
                        }
                    }
                }
            }

            else
            {
                if (!ani.enabled)
                {
                    ani.enabled = true;
                }
            }
        }

        ani.SetBool("is_jumping", is_jumping || is_jump_attacking);
        ani.SetBool("is_throwing",is_throwing);
        ani.SetBool("getting_gun",getting_gun);
        ani.SetBool("is_aiming",aiming_gun);
        ani.SetBool("is_grappling",is_preparing_jump);
        ani.SetBool("is_attacking",is_attacking);
        ani.SetInteger("phase", phase);
        ani.SetInteger("last_attack", last_attack);

    }
    void take_shot()
    {
        if (current_sniper)
        {
            sb.took_shot = true;
            Invoke("end_attack",1);
        }
    }
    public void do_attack()
    {
        if (phase == 1)
        {
            if (last_attack != 0)
            {
                last_attack = 0;
                getting_gun = true;
            }

            else if (last_attack == 0)
            {
                last_attack = 1;
                is_throwing = true;
            }
        }

        else if (phase == 2)
        {
            if (last_attack == 0)
            {
                /*AnimatorOverrideController aoc = new AnimatorOverrideController(ani.runtimeAnimatorController);
                aoc["getting_gun"] = getting_sniper_anim;
                aoc["putting_gun_away"] = putting_sniper_away;
                ani.runtimeAnimatorController = aoc;*/

                getting_gun = true;
                last_attack = 1;
            }
            else if (last_attack == 1)
            {
                /*AnimatorOverrideController aoc = new AnimatorOverrideController(ani.runtimeAnimatorController);
                aoc["getting_gun"] = getting_AR_anim;
                aoc["putting_gun_away"] = putting_AR_away;
                ani.runtimeAnimatorController = aoc;*/

                getting_gun = true;
                last_attack = 2;
            }
            else if (last_attack == 2)
            {
                is_throwing = true;
                last_attack = 3;
            }
            else
            {
                P2_jump_attack();
                last_attack = 0;
            }
        }

    }
    public void start_aiming()
    {
        aiming_gun = true;
        getting_gun = false;
    }
    public void end_attack()
    {
        is_attacking = false;
        getting_gun = false;
        if (current_sniper)
        {
            sb.destroy();
        }
        if (current_AR)
        {
            arb.destroy();
        }
        aiming_gun = false;
        is_throwing = false;
        AR_just_destroyed = false;
        attackTime = 0f;
        if (current_swinging_boss)
        {
            var x = current_swinging_boss.GetComponent<GrapplingBossBehavior>();
            Destroy(current_swinging_boss);
            TakeDamage(x.accumulated_damage);
        }
    }
    public void throw_dynamite_precursor()
    {

        int amount = Random.Range(phase, 4); // 1 to 3 (2 to 3 if phase = 2)

        if (left.boss_is_on)
        {
            if (!sprite.flipX)
            {
                sprite.flipX = true;
                throwing_point.Flip();
            }
        }

        else if (right.boss_is_on)
        {
            if (sprite.flipX)
            {
                sprite.flipX = false;
                throwing_point.Flip();
            }
        }

        else
        {
            int rand = Random.Range(0, 2);
            if (rand == 0) // Don't flip
            {
                if (sprite.flipX)
                {
                    sprite.flipX = false;
                    throwing_point.Flip();
                }
            }
            else if (rand == 1) // Yes flip
            {
                if (!sprite.flipX)
                {
                    sprite.flipX = true;
                    throwing_point.Flip();
                }
            }
        }

        int[] positions = new int[amount];
        int[] selected = new int[amount];

        for (int i = 0; i < amount; i++)
        {
            bool repeated = true;
            int chosen = Random.Range(1, 7);
            while (repeated)
            {
                repeated = false;
                for (int j = 0; j < amount; j++)
                {
                    if (selected[j] == chosen)
                    {
                        repeated = true;
                        chosen = Random.Range(1, 7);
                        break;
                    }
                }
            }
            positions[i] = chosen;
            selected[i] = chosen;
        }

        throw_dynamite(positions);

    }
    public void throw_dynamite(int[] positions)
    {
        for (int i = 0; i < positions.Length; i++)
        {
            int x_pos = 0;
            switch (positions[i])
            {
                case 1:
                    if (left.boss_is_on)
                    {
                        x_pos = -30;
                    }
                    else if (right.boss_is_on)
                    {
                        x_pos = -350;
                    }
                    else
                    {
                        if (sprite.flipX)
                            x_pos = -220;
                        else
                            x_pos = -200;
                    }
                    break;
                case 2:
                    if (left.boss_is_on)
                    {
                        x_pos = 10;
                    }
                    else if (right.boss_is_on)
                    {
                        x_pos = -300;
                    }
                    else
                    {
                        if (sprite.flipX)
                            x_pos = -140;
                        else
                            x_pos = -120;
                    }
                    break;
                case 3:
                    if (left.boss_is_on)
                    {
                        x_pos = 100;
                    }
                    else if (right.boss_is_on)
                    {
                        x_pos = -200;
                    }
                    else
                    {
                        if (sprite.flipX)
                            x_pos = -20;
                        else
                            x_pos = 0;
                    }
                    break;
                case 4:
                    if (left.boss_is_on)
                    {
                        x_pos = 200;
                    }
                    else if (right.boss_is_on)
                    {
                        x_pos = -100;
                    }
                    else
                    {
                        if (sprite.flipX)
                            x_pos = 10;
                        else
                            x_pos = 50;
                    }
                    break;
                case 5:
                    if (left.boss_is_on)
                    {
                        x_pos = 300;
                    }
                    else if (right.boss_is_on)
                    {
                        x_pos = -30;
                    }
                    else
                    {
                        if (sprite.flipX)
                            x_pos = 120;
                        else
                            x_pos = 150;
                    }
                    break;
                case 6:
                    if (left.boss_is_on)
                    {
                        x_pos = 350;
                    }
                    else if (right.boss_is_on)
                    {
                        x_pos = 50;
                    }
                    else
                    {
                        if (sprite.flipX)
                            x_pos = 220;
                        else
                            x_pos = 250;
                    }
                    break;
            }

            GameObject boomstick = Instantiate(dynamite, throwing_point.transform.position, throwing_point.transform.rotation);
            DynamiteBehavior db = boomstick.GetComponent<DynamiteBehavior>();
            db.Initialize(x_pos, 100);
        }
    }
    public void prepare_jump()
    {
        if (!is_preparing_jump && !is_jumping)
        {
            attack_count = 0;
            is_preparing_jump = true;
            Invoke("attempt_jump", 1);

            if (left.boss_is_on || right.boss_is_on)
            {
                jump_target = main_building;

                if (left.boss_is_on)
                {
                    sprite.flipX = true;
                    throwing_point.Flip();
                }
                else if (right.boss_is_on)
                {
                    sprite.flipX = false;
                    throwing_point.Flip();
                }

                if (current_grapple_wire == null)
                {
                    current_grapple_wire = Instantiate(grapple_wire);
                    GrapplingWireBehavior gw = current_grapple_wire.GetComponent<GrapplingWireBehavior>();
                    gw.initialize(throwing_point.gameObject, main_building.transform.position);
                }

            }

            else
            {
                int building = Random.Range(0, 2);

                if (left.destroyed && building == 0)
                {
                    building = 1;
                }
                if (right.destroyed && building == 1)
                {
                    building = 0;
                }
                if (right.destroyed && left.destroyed)
                {
                    building = -1;
                    first_jump = false;
                    is_jumping = false;
                }

                if (building == 0)
                {
                    jump_target = left.jumping_point;
                    sprite.flipX = false;
                    throwing_point.Flip();
                    if (current_grapple_wire == null)
                    {
                        current_grapple_wire = Instantiate(grapple_wire);
                        GrapplingWireBehavior gw = current_grapple_wire.GetComponent<GrapplingWireBehavior>();
                        gw.initialize(throwing_point.gameObject, left.jumping_point.transform.position);
                    }
                }
                else if (building == 1)
                {
                    jump_target = right.jumping_point;
                    sprite.flipX = true;
                    throwing_point.Flip();
                    if (current_grapple_wire == null)
                    {
                        current_grapple_wire = Instantiate(grapple_wire);
                        GrapplingWireBehavior gw = current_grapple_wire.GetComponent<GrapplingWireBehavior>();
                        gw.initialize(throwing_point.gameObject, right.jumping_point.transform.position);
                    }
                }
            }
        }
    }
    public void attempt_jump()
    {
        is_preparing_jump = false;
        first_jump = true;
        is_jumping = true;
        if (current_grapple_wire != null)
        {
            Destroy(current_grapple_wire);
        }
    }
    public void P2_jump_attack()
    {
        is_jump_attacking = true;
        jump_target = phase2_jump_point;
    }
    void FixedUpdate()
    {
        if (phase == 1)
        {
            if (jump_target != null && is_jumping)
            {
                Vector2 target_pos = jump_target.gameObject.transform.position;
                if (first_jump)
                {
                    rb.linearVelocityX = target_pos.x - transform.position.x;
                    rb.linearVelocityY = 3;
                    moveTime = 0;
                    first_jump = false;
                }
                else
                {
                    moveTime += Time.deltaTime;
                    if (Mathf.Abs(transform.position.x - target_pos.x) <= 0.1f)
                    {
                        rb.bodyType = RigidbodyType2D.Dynamic;
                        rb.gravityScale = 1.5f;
                        rb.linearVelocityX = 0;
                        if (Mathf.Abs(transform.position.y - target_pos.y) <= 0.1f)
                        {
                            jump_target = null;
                            rb.bodyType = RigidbodyType2D.Kinematic;
                            rb.linearVelocityY = 0;
                            moveTime = 0;
                            is_jumping = false;
                        }
                    }
                    if (moveTime > 3)
                    {
                        transform.position = target_pos;
                        rb.bodyType = RigidbodyType2D.Kinematic;
                        rb.linearVelocityY = 0;
                        jump_target = null;
                        is_jumping = false;
                        moveTime = 0;
                    }
                }
            }
        }

        else if (phase == 2)
        {
            if (is_dead)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.linearVelocityX = 0;
                rb.linearVelocityY = 0;
                return;
            }

            if (jump_target != null && is_jump_attacking)
            {
                Vector2 target_pos = jump_target.gameObject.transform.position;
                if (transform.position.y < target_pos.y)
                {
                    rb.linearVelocityY = 3;
                }
                else
                {
                    rb.linearVelocityY = 0;
                    if (current_swinging_boss == null)
                    {
                        current_swinging_boss = Instantiate(swinging_boss);
                        GrapplingBossBehavior gb = current_swinging_boss.GetComponent<GrapplingBossBehavior>();
                        int side = Random.Range(0, 2);
                        bool right = side == 0 ? true : false;
                        gb.initialize(gameObject, right);
                    }
                }
            }
            else if (is_jump_attacking && jump_target == null)
            {
                if (transform.position.y > hardcoded_start_y)
                {
                    rb.bodyType = RigidbodyType2D.Dynamic;
                }

                else
                {
                    transform.position = new Vector2(transform.position.x,hardcoded_start_y);
                    rb.bodyType = RigidbodyType2D.Kinematic;
                    rb.linearVelocityY = 0;
                    is_jump_attacking = false;
                    end_attack();
                }
            }
        }
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            is_dead = true;
        }
    }
    public void Hit(PlayerAttack player_attack)
    {
        if (phase == 1)
        {
            return;
        }

        else if (phase == 2)
        {
            if (is_invincible)
            {
                return;
            }
            else
            {
                player_attack.isHit();
                TakeDamage(player_attack.damage);
            }
        }
    }
    public void change_me_layer() // For animator
    {
        gameObject.layer = LayerMask.NameToLayer("Inactive_Enemy_Attack");
    }

}

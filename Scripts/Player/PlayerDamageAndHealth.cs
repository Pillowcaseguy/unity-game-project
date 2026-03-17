using System;
using UnityEngine;
public class PlayerDamageAndHealth : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D col;
    private Animator ani;
    private SpriteRenderer sprite_render;
    private CameraShake cameraShake;
    private PlayerMovement movement;
    public PlayerAttacking player_attacking;

    public int maximum_health = 8;
    public int health;
    public float hurtTime = 0f;
    public float parryTime = 0f;
    public float parryCooldown = 0f;
    public float weaponSwitchCooldown = 0f;
    public float invincibilityTime = 0f;
    private float hiddenInvincibilityTime = 0f;
    public float trueInvincibilityTime = 0f;
    public EnemyAttack.AttackType lastHitType = EnemyAttack.AttackType.Normal;

    public bool player_died = false;

    private float dt;

    public float stunTime = 0f;
    public float freezeTime = 0f;
    public float burnTime = 0f;
    public float reverseTime = 0f;
    public float slowTime = 0f;
    public float slowPotency = 0f;
    public float blindTime = 0f;
    public float weaknessTime = 0f;
    public float weaknessPotency = 0f;
    public float curseTime = 0f;

    public bool parryEnabled = true;

    public GameObject parry_effect;

    public GameObject current_burn;
    public GameObject burn_effect;

    public GameObject current_curse;
    public GameObject curse_effect;

    public GameObject current_stun;
    public GameObject stun_effect;

    public GameObject current_reverse;
    public GameObject reverse_effect;

    public GameObject current_weakness;
    public GameObject weakness_effect;

    public GameObject current_slowness;
    public GameObject slowness_effect;

    public LayerMask pitLayer;
    void Awake()
    {
        health = maximum_health;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite_render = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
        ani = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
        player_attacking = GetComponent<PlayerAttacking>();
        burn_effect = Resources.Load<GameObject>("Player_Status_Effects/burning");
        curse_effect = Resources.Load<GameObject>("Player_Status_Effects/curse");
        stun_effect = Resources.Load<GameObject>("Player_Status_Effects/stun");
        parry_effect = Resources.Load<GameObject>("Player_Status_Effects/parry");
        reverse_effect = Resources.Load<GameObject>("Player_Status_Effects/reverse");
        weakness_effect = Resources.Load<GameObject>("Player_Status_Effects/weakness");
        slowness_effect = Resources.Load<GameObject>("Player_Status_Effects/slowness");
    }
    // Update is called once per frame
    private void Update()
    {
        if (!cameraShake && GameManager.Instance != null)
        {
            cameraShake = GameManager.Instance.current_camera.GetComponent<CameraShake>();
        }

        dt = Time.deltaTime;

        if (health <= 0)
        {
            invincibilityTime = 0f;
            hiddenInvincibilityTime = 0f;
            hurtTime = 0f;
            parryTime = 0f;
            player_died = true;
        }

        if (invincibilityTime > 0f) {
            invincibilityTime -= dt;
            if (invincibilityTime < 0f) { invincibilityTime = 0f; }
        }

        if (hiddenInvincibilityTime > 0f)
        {
            hiddenInvincibilityTime -= dt;
            if (hiddenInvincibilityTime < 0f) { hiddenInvincibilityTime = 0f; }
        }

        if (trueInvincibilityTime > 0f)
        {
            trueInvincibilityTime -= dt;
            if (trueInvincibilityTime < 0f) { trueInvincibilityTime = 0f; }
        }

        if (parryTime > 0f) {
            parryTime -= dt;
            if (parryTime < 0f) { parryTime = 0f; }
        }

        if (parryCooldown > 0f)
        {
            parryCooldown -= dt;
            if (parryCooldown < 0f) { parryCooldown = 0f; }
        }

        if (hurtTime > 0f)
        {
            hurtTime -= dt;
            parryTime = 0;
            if (hurtTime < 0f) { hurtTime = 0f; }
        }

        if (weaponSwitchCooldown > 0f)
        {
            weaponSwitchCooldown -= dt;
            if (weaponSwitchCooldown < 0f) { weaponSwitchCooldown = 0f; }
        }

        updateDebuffTimers(dt);

        //Debug.Log(health + " health remaining!");

        ani.SetBool("isHurt", hurtTime > 0f);

    }

    public void Hit_No_Collider(GameObject thing_that_smacked_me)
    {

        if (player_died || trueInvincibilityTime > 0f)
        {
            return;
        }

        EnemyAttack attack = thing_that_smacked_me.GetComponent<EnemyAttack>();

        if (attack == null)
        {
            attack = thing_that_smacked_me.GetComponentInParent<EnemyAttack>();
        }

        if (attack == null) // Who called a hit without an attack?
            return;

        if (trueInvincibilityTime > 0f) // Unconditional blocking of the attack
        {
            Block(attack);
        }

        else if (!attack.cannotBeParried && parryTime > 0f && (invincibilityTime + trueInvincibilityTime) == 0f) // Cannot be invincible to parry
        {
            Instantiate(parry_effect, transform.position, transform.rotation);
            parryTime = 0f;
            Parry(attack);
        }

        else if ((attack.ignoresInvincibility || invincibilityTime == 0f) && hiddenInvincibilityTime == 0f) // Hidden invincibility prevents damage spam
        // But player can still get hurt when invincible + no hidden invincibility if attack ignores it
        {
            Hurt(attack);
            Debuff(attack);
        }

        else if (invincibilityTime > 0f)
        {
            Block(attack);
            if (attack.debuffEvenIfBlocked)
            {
                Debuff(attack);
            }
        }

    }
    public void Hit(Collider2D col)
    {

        if (player_died || trueInvincibilityTime > 0f)
        {
            return;
        }

        EnemyAttack attack = col.gameObject.GetComponent<EnemyAttack>();
        if (attack == null)
        {
            attack = col.gameObject.GetComponentInParent<EnemyAttack>();
        }

        if (trueInvincibilityTime > 0f) // Unconditional blocking of the attack
        {
            Block(attack);
        }

        else if (!attack.cannotBeParried && parryTime > 0f && (invincibilityTime + trueInvincibilityTime) == 0f) // Cannot be invincible to parry
        {
            Instantiate(parry_effect, transform.position, transform.rotation);
            parryTime = 0f;
            Parry(attack);
        }

        else if ((attack.ignoresInvincibility || invincibilityTime == 0f) && hiddenInvincibilityTime == 0f) // Hidden invincibility prevents damage spam
        // But player can still get hurt when invincible + no hidden invincibility if attack ignores it
        {
            Hurt(attack);
            Debuff(attack);
        }

        else if (invincibilityTime > 0f)
        {
            Block(attack);
            if (attack.debuffEvenIfBlocked)
            {
                Debuff(attack);
            }
        }
    }
    private void Hurt(EnemyAttack attack) // Process Attack if taken
    {

        if (player_died || trueInvincibilityTime > 0f)
        {
            return;
        }

        int hurtAmount = attack.damage;
        if (burnTime > 0f) // Burning doubles the damage taken
        {
            hurtAmount *= 2;
        }

        if (hurtAmount > 0)
        {
            hurtTime = 0.5f;
            if (cameraShake) {
                cameraShake.shakeOnHurt(0.5f * Math.Min(3, hurtAmount));
            }
            else
            {
                Debug.Log("YOU FORGOT YOUR SHAKE!");
            }
            invincibilityTime = 2.0f;
            hiddenInvincibilityTime = 0.2f;
            Damage(hurtAmount, attack.attackType);
        }

        attack.isHit();

    }
    private void Damage(int hurtAmount, EnemyAttack.AttackType type = EnemyAttack.AttackType.Normal)
    {

        if (player_died || trueInvincibilityTime > 0f)
        {
            return;
        }

        if (type == EnemyAttack.AttackType.Pit)
        {
            hurtTime = 0.5f;
            if (cameraShake)
            {
                cameraShake.shakeOnHurt(0.5f * Math.Min(3, hurtAmount));
            }
            else
            {
                Debug.Log("YOU FORGOT YOUR SHAKE!");
            }
            invincibilityTime = 2.0f;
            hiddenInvincibilityTime = 0.2f;
        }

        stunTime = 0.0f;

        if (health - hurtAmount < 0)
        {
            health = 0;
        }

        else
        {
            health -= hurtAmount;
        }

        lastHitType = type;

    }
    private void Heal(int healAmount)
    {
        if (health + healAmount > maximum_health)
        {
            health = maximum_health;
        }

        else
        {
            health += healAmount;
        }

    }

    private void Parry(EnemyAttack attack) // Attack is parried
    {
        attack.isParried();
    }

    private void Debuff(EnemyAttack attack) // Check for and inflict debuffs
    {

        if (attack.stunDuration > 0)
        {
            stunTime = (attack.stunDuration < stunTime) ? stunTime : attack.stunDuration;
        }

        if (attack.freezeDuration > 0)
        {
            freezeTime = (attack.freezeDuration < freezeTime) ? freezeTime : attack.freezeDuration;
            if (burnTime > 0)
            {
                burnTime = 0;
                if (current_burn != null)
                {
                    Destroy(current_burn);
                }
            }
        }

        if (attack.burnDuration > 0)
        {
            burnTime = (attack.burnDuration < burnTime) ? burnTime : attack.burnDuration;
            if (freezeTime > 0)
            {
                freezeTime = 0;
            }
            if (current_burn != null)
            {
                Destroy(current_burn);
            }
        }

        if (attack.reverseDuration > 0)
        {
            reverseTime = (attack.reverseDuration < reverseTime) ? reverseTime : attack.reverseDuration;
        }

        if (attack.slowDuration > 0)
        {
            if (slowTime > 0f)
            {
                if (slowPotency <= attack.slowPotency)
                {
                    slowTime = attack.slowDuration;
                    slowPotency = attack.slowPotency;
                }
            }
            else
            {
                slowTime = attack.slowDuration;
                slowPotency = attack.slowPotency;
            }
        }

        if (attack.blindDuration > 0)
        {
            blindTime = (attack.blindDuration < blindTime) ? blindTime : attack.blindDuration;
        }

        if (attack.weaknessDuration > 0)
        {
            if (weaknessTime > 0f)
            {
                if (weaknessPotency <= attack.weaknessPotency)
                {
                    weaknessTime = attack.weaknessDuration;
                    weaknessPotency = attack.weaknessPotency;
                }
            }
            else
            {
                weaknessTime = attack.weaknessDuration;
                weaknessPotency = attack.weaknessPotency;
            }
        }

        if (attack.curseDuration > 0)
        {
            curseTime = (attack.curseDuration < curseTime) ? curseTime : attack.curseDuration;
        }

    }
    private void Block(EnemyAttack attack) // Attack is deflected
    {
        attack.isBlocked();
    }
    public int check_health()
    {
        return health;
    }
    private void updateDebuffTimers(float dt)
    {

        /*float stunTime = 0f;
        float freezeTime = 0f;
        float burnTime = 0f;
        float reverseTime = 0f;
        float slowTime = 0f;
        float slowPotency = 0f;
        float blindTime = 0f;
        float weaknessTime = 0f;
        float weaknessPotency = 0f;
        float curseTime = 0f;*/

        if (stunTime > 0f)
        {

            if (current_stun == null)
            {
                current_stun = Instantiate(stun_effect);
                StunBehavior sb = current_stun.GetComponent<StunBehavior>();
                sb.Initialize(stunTime, gameObject);
            }

            stunTime -= dt;
            if (stunTime < 0f) { stunTime = 0f; }
        }

        if (freezeTime > 0f)
        {
            freezeTime -= dt;
            if (freezeTime < 0f) { freezeTime = 0f; }
        }

        if (burnTime > 0f)
        {
            if (current_burn == null)
            {
                current_burn = Instantiate(burn_effect);
                BurningBehavior bb = current_burn.GetComponent<BurningBehavior>();
                bb.Initialize(burnTime, gameObject);
            }

            burnTime -= dt;
            if (burnTime < 0f) { burnTime = 0f; }
        }

        if (reverseTime > 0f)
        {
            if (current_reverse == null)
            {
                current_reverse = Instantiate(reverse_effect);
                ReverseBehavior rb = current_reverse.GetComponent<ReverseBehavior>();
                rb.Initialize(reverseTime, gameObject);
            }

            reverseTime -= dt;
            if (reverseTime < 0f) { reverseTime = 0f; }
        }

        if (slowTime > 0f)
        {
            if (current_slowness == null)
            {
                current_slowness = Instantiate(slowness_effect);
                SlownessBehavior sb = current_slowness.GetComponent<SlownessBehavior>();
                sb.Initialize(slowTime, gameObject);
            }

            slowTime -= dt;
            if (slowTime < 0f) { slowTime = 0f; }
        }

        if (blindTime > 0f)
        {
            blindTime -= dt;
            if (blindTime < 0f) { blindTime = 0f; }
        }

        if (weaknessTime > 0f)
        {
            if (current_weakness == null)
            {
                current_weakness = Instantiate(weakness_effect);
                WeaknessBehavior wb = current_weakness.GetComponent<WeaknessBehavior>();
                wb.Initialize(weaknessTime, gameObject);
            }

            weaknessTime -= dt;
            if (weaknessTime < 0f) { weaknessTime = 0f; }
        }

        if (curseTime > 0f)
        {

            if (current_curse == null)
            {
                current_curse = Instantiate(curse_effect);
                CurseBehavior cb = current_curse.GetComponent<CurseBehavior>();
                cb.Initialize(curseTime, gameObject);
            }

            else
            {
                CurseBehavior cb = current_curse.GetComponent<CurseBehavior>();
                if (cb.lifetime > 0) {
                    cb.lifetime = curseTime;
                }
            }

            curseTime -= dt;

            if (curseTime < 0f) { curseTime = 0f; }
        }

    }
    private void OnCollisionEnter2D(Collision2D collision) // Check for pits
    {
        if (((1 << collision.gameObject.layer) & pitLayer) != 0)
        {
            if (invincibilityTime == 0f)
            {
                Damage(1, EnemyAttack.AttackType.Pit);
            }
        }

        else {
            return;
        }
    }
}

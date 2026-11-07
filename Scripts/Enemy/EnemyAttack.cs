using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private BoxCollider2D col;
    public Animator ani;
    private SpriteRenderer sprite_render;

    public int active_layer;
    public int inactive_layer;

    public bool ignoresInvincibility = false;
    public bool cannotBeParried = false;
    public bool debuffEvenIfBlocked = false;
    public bool disableAttackCycle = false;
    public bool wasParried = false;
    public bool wasBlocked = false;
    public bool wasHit = false;
    public int damage = 0;
    public float stunDuration = 0f;
    public float freezeDuration = 0f;
    public float burnDuration = 0f;
    public float reverseDuration = 0f;
    public float slowDuration = 0f;
    public float slowPotency = 0f;
    public float blindDuration = 0f;
    public float weaknessDuration = 0f;
    public float weaknessPotency = 0f;
    public float curseDuration = 0f;

    public bool destroyOnHit = false;
    public bool destroyOnParry = false;
    public float destroyTimer = 0f;
    public bool parryDisablesAttack = false;
    public bool blockDisablesAttack = false;
    public bool hitDisablesAttack = false;
    public bool doesNotGrantInvulnerability = false;
    public bool breaksInvulnerability = false;
    public bool isDestroyed = false;

    public enum AttackType { Normal, Fire, Ice, Earth, Bullet, Pit }
    public AttackType attackType = AttackType.Normal;
    public LayerMask playerLayer;

    void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        ani = GetComponent<Animator>();
        sprite_render = GetComponent<SpriteRenderer>();
        active_layer = LayerMask.NameToLayer("Active_Enemy_Attack");
        inactive_layer = LayerMask.NameToLayer("Inactive_Enemy_Attack");
        playerLayer = LayerMask.GetMask("Player_Damage");
    }
    private void OnTriggerStay2D(Collider2D col)
    {
        if (gameObject.layer != active_layer) // If the layer this object is not the active attack layer return
        {
            return;
        }

        if (((1 << col.gameObject.layer) & playerLayer) != 0) // If the collision is hitting the player's damage hitbox layer
        {
            PlayerDamageAndHealth attempt = col.gameObject.GetComponentInParent<PlayerDamageAndHealth>();
            if (attempt != null)
            {
                attempt.Hit(this.col);
            }
        }
    }
    public void isParried()
    {
        if (!cannotBeParried)
        {
            wasParried = true;

            if (parryDisablesAttack)
            {
                disableAttackCycle = true;
                DisableHitbox();
            }
        }

        if (destroyOnHit || destroyOnParry)
        {
            startDestroy();
        }

    }
    public void isBlocked()
    {

        wasBlocked = true;

        if (destroyOnHit)
        {
            startDestroy();
        }

        else if (blockDisablesAttack)
        {
            disableAttackCycle = true;
            DisableHitbox();
        }

    }
    public void isHit()
    {
        wasHit = true;

        if (destroyOnHit)
        {
            startDestroy();
        }

        else if (hitDisablesAttack)
        {
            DisableHitbox();
            disableAttackCycle = true;
        }
    }
    public void EnableHitbox()
    {
        if (!disableAttackCycle)
        {
            gameObject.layer = active_layer;
        }
    }
    public void DisableHitbox()
    {
        gameObject.layer = inactive_layer;
    }
    public void startDestroy()
    {
        isDestroyed = true;
        if (ani != null)
        {
            ani.SetBool("isDestroyed", isDestroyed);
        }
        Destroy(gameObject, destroyTimer);
    }
    public void clearFlags()
    {
        wasParried = false;
        wasBlocked = false;
        wasHit = false;
    }
    public bool anyFlag()
    {
        return (wasParried || wasBlocked || wasHit);
    }
}

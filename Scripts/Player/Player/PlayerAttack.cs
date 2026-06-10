using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D col;
    private Animator ani;
    private SpriteRenderer sprite_render;

    public int active_layer;
    public int inactive_layer;

    public bool ignoresInvincibility = false;
    public int damage = 0;
    public float stunDuration = 0f;
    public float freezeDuration = 0f;
    public float burnDuration = 0f;
    public float blindDuration = 0f;
    /*public float reverseDuration = 0f;
    public float slowDuration = 0f;
    public float slowPotency = 0f;
    public float weaknessDuration = 0f;
    public float weaknessPotency = 0f;
    public float curseDuration = 0f;*/
    public bool debuffEvenIfBlocked = false;

    public bool wasHit = false;
    public bool wasBlock = false;
    public bool hitDisablesAttack = false;
    public bool blockDisablesAttack = false;

    public bool destroyOnHit = false;
    public bool destroyOnBlock = false;
    public float destroyTimer = 0f;
    public bool isDestroyed = false;

    public enum AttackType { Ice, Fire, Gem, Earth, Water, Light, Dark }
    public AttackType attackType = AttackType.Ice;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        ani = GetComponentInChildren<Animator>();
        sprite_render = GetComponent<SpriteRenderer>();
        active_layer = LayerMask.NameToLayer("Player_Attack");
        inactive_layer = LayerMask.NameToLayer("Inactive_Player_Attack");
    }
    public void EnableHitbox()
    {
        gameObject.layer = active_layer;
    }
    public void DisableHitbox()
    {
        gameObject.layer = inactive_layer;
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
        }
    }
    public void isBlock()
    {
        wasBlock = true;

        if (destroyOnBlock)
        {
            startDestroy();
        }

        else if (hitDisablesAttack)
        {
            DisableHitbox();
        }
    }
    public void clearFlags()
    {
        wasBlock = false;
        wasHit = false;
    }
    public void startDestroy()
    {
        isDestroyed = true;
        ani.SetBool("isDestroyed", isDestroyed);
        Destroy(gameObject, destroyTimer);
    }

}

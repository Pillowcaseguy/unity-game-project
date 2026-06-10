using UnityEngine;
using System.Collections.Generic;

public class LesserSuperFireBehavior : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D hit_col;
    public PlayerAttack playerAttack;
    public Animator ani;
    public float speed;
    public float rotation;
    public float life;

    public bool start_attack = false;
    public bool start_attack_first_time = false;

    //public bool reflected_first_time = false;
    //public bool reflected = false;
    private GameObject owner;
    private PlayerDamageAndHealth player_health;

    public LayerMask groundLayer;
    public LayerMask enemyLayer;
    //public LayerMask playerLayer;
    public int active_layer;
    public int inactive_layer;

    public SpriteRenderer sprite;
    public Dictionary<int, int> enemies_hit = new Dictionary<int, int>();
    void Awake()
    {
        playerAttack = GetComponent<PlayerAttack>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        groundLayer = LayerMask.GetMask("Ground");
        enemyLayer = LayerMask.GetMask("Active_Enemy_Attack", "Enemy");
        sprite = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
        //playerLayer = LayerMask.GetMask("Player_Damage");
        active_layer = LayerMask.NameToLayer("Player_Attack");
        inactive_layer = LayerMask.NameToLayer("Inactive_Player_Attack");
        hit_col = GetComponent<BoxCollider2D>();
    }
    public void Initialize(float moveSpeed, float zRotation, float lifetime, GameObject creator, PlayerDamageAndHealth helf)
    {
        speed = moveSpeed;
        rotation = zRotation;
        life = lifetime;
        owner = creator;
        player_health = helf;
    }
    // Update is called once per frame
    private void Update()
    {
        if (life > 0f)
        {
            life -= Time.deltaTime;
            if (life < 0f)
            {
                Destroy(gameObject);
            }
        }
    }
    private void FixedUpdate()
    {
        if (!start_attack_first_time && start_attack)
        {
            start_attack_first_time = true;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            ani.SetBool("is_destroyed", true);
            gameObject.layer = active_layer;
            rb.linearVelocityX = 0;
            rb.linearVelocityY = 0;
            transform.position = new Vector2(transform.position.x,transform.position.y)
                + new Vector2(Mathf.Cos(rotation * Mathf.Deg2Rad), Mathf.Sin(rotation * Mathf.Deg2Rad));
        }
        else if (!start_attack_first_time)
        {
            rb.linearVelocityX = Mathf.Cos(rotation * Mathf.Deg2Rad) * speed;
            rb.linearVelocityY = Mathf.Sin(rotation * Mathf.Deg2Rad) * speed;
        }
    }
    private void OnTriggerStay2D(Collider2D col)
    {
        if (gameObject.layer == inactive_layer)
        {
            return;
        }
        if (((1 << col.gameObject.layer) & enemyLayer) != 0) // If the collision is hitting the enemy's damage hitbox layer
        {
            EnemyDamageable attempt = col.gameObject.GetComponent<EnemyDamageable>();
            EnemyMainBodyImplement attempt2 = col.gameObject.GetComponent<EnemyMainBodyImplement>();
            if (attempt == null || attempt2 == null)
            {
                return;
            }
            else
            {
                if (!enemies_hit.ContainsKey(col.gameObject.GetInstanceID()))
                {
                    enemies_hit.Add(col.gameObject.GetInstanceID(), 1);
                    if (player_health != null && player_health.health > 0)
                    {
                        attempt.Hit(playerAttack);
                    }
                    else
                    {
                        playerAttack.damage = 0;
                        attempt.Hit(playerAttack);
                    }
                }
            }
        }
        /*else
        {
            if (((1 << col.gameObject.layer) & playerLayer) != 0) // If the collision is hitting the player's damage hitbox layer
            {
                col.gameObject.GetComponentInParent<PlayerDamageAndHealth>().Hit(this.col);
            }
        }*/
    }
    public void destroy_me()
    {
        Destroy(gameObject);
    }
}

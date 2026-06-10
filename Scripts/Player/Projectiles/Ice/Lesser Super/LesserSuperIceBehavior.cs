using System.Collections.Generic;
using UnityEngine;

public class LesserSuperIceBehavior : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D col;
    public PlayerAttack playerAttack;
    //public EnemyAttack reflectAttack;
    public float speed;
    public float initial_speed;
    public float rotation;
    public float life;

    public bool inside_hitbox;

    private bool destroyed_first_time = false;
    private bool destroyed = false;

    //public bool reflected_first_time = false;
    //public bool reflected = false;
    private GameObject owner;
    private PlayerDamageAndHealth player_health;

    public LayerMask groundLayer;
    public LayerMask enemyLayer;
    //public LayerMask playerLayer;
    public LayerMask active_layer;
    public LayerMask inactive_layer;

    public GameObject sprite_child;
    public SpriteRenderer sprite;

    public GameObject trail_sprite_child;
    public SpriteRenderer trail_sprite;

    public Dictionary<int, int> enemies_hit = new Dictionary<int, int>();
    public GameObject ice_particle;
    void Awake()
    {
        playerAttack = GetComponent<PlayerAttack>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        groundLayer = LayerMask.GetMask("Ground", "BreakableGround");
        enemyLayer = LayerMask.GetMask("Active_Enemy_Attack", "Enemy");
        //playerLayer = LayerMask.GetMask("Player_Damage");
        active_layer = LayerMask.NameToLayer("Player_Attack");
        inactive_layer = LayerMask.NameToLayer("Inactive_Player_Attack");
        col = GetComponent<BoxCollider2D>();
    }
    public void Initialize(float moveSpeed, float zRotation, float lifetime, GameObject creator, PlayerDamageAndHealth helf)
    {
        speed = moveSpeed;
        initial_speed = speed;
        rotation = zRotation;
        life = lifetime;
        owner = creator;
        player_health = helf;
    }
    // Update is called once per frame
    private void Update()
    {
        /*if (reflected)
        {
            if (!reflected_first_time)
            {
                reflected_first_time = true;
                transform.rotation = Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z + 180);
                rotation = transform.rotation.eulerAngles.z;
            }
        }*/

        /*if (rb.linearVelocityX < 0f)
        {
            sprite.flipX = true;
        }
        else
        {
            sprite.flipX = false;
        }*/
        //Debug.Log("Key: " + enemies_hit.Keys);

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
        if (playerAttack.isDestroyed)// || reflectAttack.isDestroyed)
        {
            destroyed = true;
        }

        if (destroyed)
        {
            if (!destroyed_first_time)
            {
                destroyed_first_time = true;
                //GetComponentInChildren<IceTrailBehavior>().disableSprite();
                playerAttack.DisableHitbox();
                transform.rotation = Quaternion.Euler(0, 0, rotation);
            }

            rb.linearVelocityX = 0;
            rb.linearVelocityY = 0;
        }
        else
        {
            if (inside_hitbox)
            {
                speed = initial_speed / 3f;
            }
            else
            {
                speed = initial_speed;
            }
            rb.linearVelocityX = Mathf.Cos(rotation * Mathf.Deg2Rad) * speed;
            rb.linearVelocityY = Mathf.Sin(rotation * Mathf.Deg2Rad) * speed;
            sprite_child.transform.rotation = Quaternion.Euler(0, 0, sprite_child.transform.rotation.eulerAngles.z - 5);
            //trail_sprite_child.transform.rotation = Quaternion.Euler(0, 0, trail_sprite_child.transform.rotation.eulerAngles.z - 5);
        }
    }
    /*private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Reflective") && ((1 << col.gameObject.layer) & enemyLayer) != 0)
        {
            reflected = true;
        }
        else if (((1 << col.gameObject.layer) & groundLayer) != 0)
        {
            playerAttack.DisableHitbox();
            playerAttack.destroyTimer = 0.5f;
            playerAttack.startDestroy();
        }
    }*/
    private void OnTriggerStay2D(Collider2D col)
    {
        if (gameObject.layer != active_layer) // If the layer this object is on is not the active attack layer return
        {
            return;
        }

        if (((1 << col.gameObject.layer) & enemyLayer) != 0) // If the collision is hitting the enemy's damage hitbox layer
        {
            inside_hitbox = true;
            EnemyDamageable attempt = col.gameObject.GetComponent<EnemyDamageable>();
            EnemyMainBodyImplement attempt2 = col.gameObject.GetComponent<EnemyMainBodyImplement>();
            if (attempt == null || attempt2 == null)
            {
                return;
            }
            else
            {
                /*Debug.Log("BEFORE HITTING");
                foreach (int x in enemies_hit.Keys)
                {
                    Debug.Log("Key: " + x);
                    //Debug.Log("Amount of hits: " + enemies_hit[i]);
                    Debug.Log("Amount of hits: " + enemies_hit[x]);
                }*/

                if (!enemies_hit.ContainsKey(col.gameObject.GetInstanceID()))
                {
                    enemies_hit.Add(col.gameObject.GetInstanceID(), 1);
                }
                else
                {
                    if (enemies_hit[col.gameObject.GetInstanceID()] < 3)
                    {
                        enemies_hit[col.gameObject.GetInstanceID()] += 1;
                    }
                    else
                    {
                        return;
                    }
                }

                int hit_count = 0;
                foreach (int x in enemies_hit.Keys)
                {
                    hit_count += enemies_hit[x];
                }
                int index_count = enemies_hit.Keys.Count;

                playerAttack.damage = 40;
                if (player_health != null && player_health.health > 0 && !(hit_count >= 7 || index_count >= 3))
                {
                    attempt.Hit(playerAttack);
                }
                else if (player_health == null || player_health.health <= 0)
                {
                    playerAttack.damage = 0;
                    attempt.Hit(playerAttack);
                }
                else
                {
                    for (int i = 0; i < 25; i++)
                    {
                        var hi = Instantiate(ice_particle,transform.position, Quaternion.Euler(0,0,0)).GetComponent<IceParticleBehavior>();
                        hi.keep_on_ground_hit = true;
                    }
                    Destroy(gameObject);
                }

                /*Debug.Log("AFTER HITTING");
                foreach (int x in enemies_hit.Keys)
                {
                    Debug.Log("Key: " + x);
                    //Debug.Log("Amount of hits: " + enemies_hit[i]);
                    Debug.Log("Amount of hits: " + enemies_hit[x]);
                }*/
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
    private void OnTriggerExit2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & enemyLayer) != 0)
        {
            inside_hitbox = false;
        }
    }
}

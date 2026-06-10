using UnityEngine;

public class LesserSuperGemBehavior : MonoBehaviour
{
    public PlayerAttack playerAttack;
    public Animator ani;
    public BoxCollider2D box;
    public float life;

    public bool start_attack;

    private GameObject owner;
    private PlayerDamageAndHealth player_health;

    public LayerMask enemyLayer;
    //public int active_layer;
    //public int inactive_layer;

    public SpriteRenderer sprite;
    public GameObject gem_particle;
    void Awake()
    {
        playerAttack = GetComponent<PlayerAttack>();
        box = GetComponent<BoxCollider2D>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyLayer = LayerMask.GetMask("Active_Enemy_Attack", "Enemy");
        sprite = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
        //active_layer = LayerMask.NameToLayer("Player_Attack");
        //inactive_layer = LayerMask.NameToLayer("Inactive_Player_Attack");
    }
    public void initialize(float lifetime, GameObject creator, PlayerDamageAndHealth helf)
    {
        life = lifetime;
        owner = creator;
        player_health = helf;
    }
    // Update is called once per frame
    private void Update()
    {
        life -= Time.deltaTime;
        if (life < 0f)
        {
            Destroy(gameObject);
        }
    }
    /*private void OnTriggerStay2D(Collider2D col)
    {
        /*if (gameObject.layer == inactive_layer)
        {
            return;
        }
        if (!start_attack_first_time && start_attack && ((1 << col.gameObject.layer) & enemyLayer) != 0) // If the collision is hitting the enemy's damage hitbox layer
        {

            
            EnemyDamageable attempt = col.gameObject.GetComponent<EnemyDamageable>();
            EnemyMainBodyImplement attempt2 = col.gameObject.GetComponent<EnemyMainBodyImplement>();

            if (attempt == null || attempt2 == null)
            {
                return;
            }

            if (player_health != null && player_health.health > 0)
            {
                attempt.Hit(playerAttack);
            }
            else
            {
                playerAttack.damage = 0;
                attempt.Hit(playerAttack);
            }

            
            start_attack_first_time = true;
        }
        /*else
        {
            if (((1 << col.gameObject.layer) & playerLayer) != 0) // If the collision is hitting the player's damage hitbox layer
            {
                col.gameObject.GetComponentInParent<PlayerDamageAndHealth>().Hit(this.col);
            }
        }
    }*/
    public void start_the_attack()
    {
        if (start_attack)
            return;
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, new Vector2(box.size.x, box.size.y), 0f, enemyLayer);
        foreach (Collider2D hit in hits)
        {
            EnemyDamageable target = hit.GetComponent<EnemyDamageable>();
            if (target == null) continue;
            EnemyMainBodyImplement further_target = hit.GetComponent<EnemyMainBodyImplement>();
            if (further_target == null) continue;
            if (player_health != null && player_health.health > 0)
            {
                target.Hit(playerAttack);
            }
            else
            {
                playerAttack.damage = 0;
                target.Hit(playerAttack);
            }
        }
        start_attack = true;
        for (int i = 0; i < 25; i++)
        {
            Instantiate(gem_particle, transform.position, Quaternion.Euler(0, 0, 0));
        }
    }
    public void destroy_me()
    {
        Destroy(gameObject, 0.5f);
    }
    public void OnDestroy()
    {
        if (!start_attack)
        {
            start_the_attack();
        }
    }
}

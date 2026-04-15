using UnityEngine;
public class FireBehavior : MonoBehaviour
{
    public SpriteRenderer sprite;
    public PlayerAttack playerAttack;
    private ReflectedFireBehavior reflectedFireBehavior;

    private PlayerAttacking playerAtking;
    private PlayerMovement attackCheck;
    private PlayerDamageAndHealth player_health;
    public GameObject owner;
    public Transform firingPoint;

    public bool reflected_first_time = false;
    public bool reflected = false;

    public LayerMask groundLayer;
    public LayerMask enemyLayer;
    public LayerMask combinedLayer;
    public LayerMask active_layer;
    public LayerMask inactive_layer;

    public float fire_width;

    public Vector2 origin;
    public Vector2 direction;
    public float width;
    public float half_width;
    public float height;
    public float original_height;
    public float half_height;

    public Vector2 top;
    public Vector2 bottom;

    public float maxDistance = 10f;
    public bool hittingSomething;
    void Awake()
    {
        playerAttack = GetComponent<PlayerAttack>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        reflectedFireBehavior = GetComponentInChildren<ReflectedFireBehavior>();

        //origin = transform.position;
        //direction = transform.right;
        width = sprite.bounds.size.x;
        height = sprite.bounds.size.y;
        original_height = sprite.sprite.bounds.size.y * transform.localScale.y;
        half_height = height / 2.0f;

        groundLayer = LayerMask.GetMask("Ground");
        enemyLayer = LayerMask.GetMask("Enemy","Active_Enemy_Attack");
        combinedLayer = groundLayer | enemyLayer;

        active_layer = LayerMask.NameToLayer("Player_Attack");
        inactive_layer = LayerMask.NameToLayer("Inactive_Player_Attack");
    }
    public void Initialize(GameObject creator, Transform startingPoint)
    {
        owner = creator;
        firingPoint = startingPoint;
        playerAtking = owner.GetComponent<PlayerAttacking>();
        attackCheck = owner.GetComponent<PlayerMovement>();
        player_health = owner.GetComponent<PlayerDamageAndHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if (firingPoint == null || owner == null) return;

        transform.position = firingPoint.transform.position;
        transform.rotation = firingPoint.transform.rotation;

        origin = transform.position;
        direction = transform.right;
        top = origin + (Vector2) transform.up * half_height;
        bottom = origin - (Vector2) transform.up * half_height;

        /*Debug.DrawRay(origin, direction * (maxDistance), Color.green); // Center Ray
        Debug.DrawRay(top, direction * (maxDistance), Color.green); // Top Ray
        Debug.DrawRay(bottom, direction * (maxDistance), Color.green); // Bottom Ray*/

        RaycastHit2D hit_center = Physics2D.Raycast(origin, direction, maxDistance, combinedLayer);
        RaycastHit2D hit_top = Physics2D.Raycast(top, direction, maxDistance, combinedLayer);
        RaycastHit2D hit_bottom = Physics2D.Raycast(bottom, direction, maxDistance, combinedLayer);

        float center_d = hit_center.collider != null ? hit_center.distance : maxDistance;
        float top_d = hit_top.collider != null ? hit_top.distance : maxDistance;
        float bottom_d = hit_bottom.collider != null ? hit_bottom.distance : maxDistance;

        RaycastHit2D winner = hit_center;
        fire_width = center_d;

        if (top_d < fire_width)
        {
            winner = hit_top;
            fire_width = top_d;
        }
        if (bottom_d < fire_width)
        {
            winner = hit_bottom;
            fire_width = bottom_d;
        }

        hittingSomething = winner.collider != null;
        sprite.size = new Vector2(fire_width / transform.localScale.x, original_height / transform.localScale.y);

        if (player_health.weaknessTime > 0)
        {
            playerAttack.damage = Mathf.CeilToInt(playerAttack.damage * (1 - player_health.weaknessPotency));
        }
        else
        {
            playerAttack.damage = 5;
        }

        if (hittingSomething)
        {
            reflected = winner.collider.gameObject.CompareTag("Reflective");
        }
        else
        {
            reflected = false;
        }

        if (reflected_first_time && !reflected) // Reflected first time is true, but we are no longer hitting a reflective surface
        {
            reflected_first_time = false;
        }

        if (reflected)
        {
            if (!reflected_first_time)
            {
                reflected_first_time = true;
                gameObject.layer = inactive_layer;
                reflectedFireBehavior.renderSprite(true);
            }
        }

        else
        {
            reflectedFireBehavior.renderSprite(false);
            gameObject.layer = active_layer;
            if (playerAtking.weaponCooldown == 0f)
            {
                playerAtking.fireAttacked();
                EnemyDamageable burn_thanks;
                if (winner.collider != null)
                {
                    burn_thanks = winner.collider.gameObject.GetComponent<EnemyDamageable>();
                    if (burn_thanks != null)
                    {
                        burn_thanks.Hit(playerAttack);
                    }
                }
            }
        }

        if (!attackCheck.isAttacking || playerAtking.disableAttack)
        {
            playerAtking.fireDeactivated();
            Destroy(gameObject);
        }

    }
}

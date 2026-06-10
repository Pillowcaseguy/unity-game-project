using UnityEngine;

public class LightBackgroundBehavior : MonoBehaviour
{
    public SpriteRenderer sprite;
    public LayerMask enemyLayer;
    public LayerMask active_layer;
    public LayerMask inactive_layer;

    public PlayerAttack playerAttack;
    Camera cam;
    public float width;
    public float height;

    public bool attackFired = false;

    void Awake()
    {
        playerAttack = GetComponent<PlayerAttack>();
        sprite = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        enemyLayer = LayerMask.GetMask("Active_Enemy_Attack", "Enemy");
        active_layer = LayerMask.NameToLayer("Player_Attack");
        inactive_layer = LayerMask.NameToLayer("Inactive_Player_Attack");

        cam = Camera.main;
        height = cam.orthographicSize * 2f;
        width = height * cam.aspect;

        Vector2 spriteSize = sprite.sprite.bounds.size;

        transform.localScale = new Vector2(width / spriteSize.x, height / spriteSize.y);
    }
    // Update is called once per frame
    void Update()
    {
        if (sprite.color.a <= 0f)
        {
            Destroy(gameObject);
        }
        transform.position = new Vector2(cam.transform.position.x, cam.transform.position.y);
        if (!attackFired)
        {
            Collider2D[] hits = Physics2D.OverlapBoxAll(cam.transform.position, new Vector2(width, height), 0f, enemyLayer);
            foreach (Collider2D hit in hits)
            {
                EnemyDamageable target = hit.GetComponent<EnemyDamageable>();
                if (target == null) continue;
                EnemyMainBodyImplement further_target = hit.GetComponent<EnemyMainBodyImplement>();
                if (further_target == null) continue;
                target.Hit(playerAttack);
            }
            attackFired = true;
        }
    }
    private void FixedUpdate()
    {
        sprite.color = new Vector4(sprite.color.r, sprite.color.g, sprite.color.b, sprite.color.a - 5 / 255f);
    }
    private void OnDestroy()
    {
        if (!attackFired)
        {
            Collider2D[] hits = Physics2D.OverlapBoxAll(cam.transform.position, new Vector2(width, height), 0f, enemyLayer);
            foreach (Collider2D hit in hits)
            {
                EnemyDamageable target = hit.GetComponent<EnemyDamageable>();
                if (target == null) continue;
                EnemyMainBodyImplement further_target = hit.GetComponent<EnemyMainBodyImplement>();
                if (further_target == null) continue;
                target.Hit(playerAttack);
            }
            attackFired = true;
        }
    }
}

using UnityEngine;

public class IceBackgroundBehavior : MonoBehaviour
{
    public Animator ani;

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
    }
    void Start()
    {
        ani = GetComponent<Animator>();
        enemyLayer = LayerMask.GetMask("Active_Enemy_Attack", "Enemy");
        active_layer = LayerMask.NameToLayer("Player_Attack");
        inactive_layer = LayerMask.NameToLayer("Inactive_Player_Attack");

        cam = Camera.main;
        height = cam.orthographicSize * 2f;
        width = height * cam.aspect;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Vector2 spriteSize = sr.sprite.bounds.size;

        transform.localScale = new Vector2(width / spriteSize.x, height / spriteSize.y);
    }
    // Update is called once per frame
    void Update()
    {
        ani.speed = 1 / Time.timeScale;
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
    /*private void OnTriggerEnter2D(Collider2D col)
    {
        if (gameObject.layer != active_layer) // If the layer this object is on is not the active attack layer return
        {
            return;
        }

        if (((1 << col.gameObject.layer) & enemyLayer) != 0) // If the collision is hitting the enemy's damage hitbox layer
        {
            Debug.Log("hitting u");
            EnemyDamageable attempt = col.gameObject.GetComponent<EnemyDamageable>();
            if (attempt == null)
            {
                return;
            }
            else
            {
                attempt.Hit(playerAttack);
            }
        }
    }*/
}

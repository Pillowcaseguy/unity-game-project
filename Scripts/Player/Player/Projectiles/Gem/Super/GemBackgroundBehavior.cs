using UnityEngine;

public class GemBackgroundBehavior : MonoBehaviour
{
    public Animator ani;

    public LayerMask enemyLayer;
    public LayerMask active_layer;
    public LayerMask inactive_layer;

    public PlayerAttack playerAttack;
    Camera cam;
    CameraShake camera_shake;
    public float width;
    public float height;

    public int attacks_fired;
    CameraShake attempt;

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
        attempt = cam.GetComponent<CameraShake>();
        if (attempt != null)
        {
            camera_shake = attempt;
        }
        height = cam.orthographicSize * 2f;
        width = height * cam.aspect;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Vector2 spriteSize = sr.sprite.bounds.size;

        transform.localScale = new Vector2(width / spriteSize.x, height / spriteSize.y);
        transform.position = new Vector2(cam.transform.position.x, cam.transform.position.y);
    }
    // Update is called once per frame
    void Update()
    {
        ani.speed = 1 / Time.timeScale;
        transform.position = new Vector2(cam.transform.position.x, cam.transform.position.y);
    }
    public void apply_damage()
    {
        if (attacks_fired < 4)
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
            if (camera_shake != null)
            {
                camera_shake.shakeOnHurt(0.5f);
            }
            attacks_fired += 1;
        }
    }
    private void OnDestroy()
    {
        if (attacks_fired < 4)
        {
            for (int i = 0; i < 4 - attacks_fired; i++)
            {
                apply_damage();
            }
        }
    }
}

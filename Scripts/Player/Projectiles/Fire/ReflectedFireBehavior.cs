using UnityEngine;
public class ReflectedFireBehavior : MonoBehaviour
{
    private SpriteRenderer sprite;
    private Transform transform;
    private EnemyAttack reflectAttack;
    private FireBehavior fireAI;
    public Transform firingPoint;

    public LayerMask groundLayer;
    public LayerMask playerLayer;
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

    public float maxAttackTime = 0.2f;
    public float currentAttackTime = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        transform = GetComponent<Transform>();
        reflectAttack = GetComponentInParent<EnemyAttack>();
        fireAI = GetComponentInParent<FireBehavior>();

        //origin = transform.position;
        //direction = transform.right;
        width = sprite.bounds.size.x;
        height = sprite.bounds.size.y;
        original_height = sprite.sprite.bounds.size.y * transform.localScale.y;
        half_height = height / 2.0f;

        groundLayer = LayerMask.GetMask("Ground");
        playerLayer = LayerMask.GetMask("Player_Damage");
        combinedLayer = groundLayer | playerLayer;

        active_layer = LayerMask.GetMask("Active_Enemy_Attack");
        inactive_layer = LayerMask.GetMask("Inactive_Enemy_Attack");
    }

    // Update is called once per frame
    void Update()
    {
        if (currentAttackTime > 0f)
        {
            currentAttackTime -= Time.deltaTime;
            if (currentAttackTime < 0f)
            {
                currentAttackTime = 0f;
            }
        }

        if (sprite.enabled)
        {
            Vector2 beamEndPoint = fireAI.transform.position + (Vector3)(fireAI.direction * fireAI.fire_width);
            Vector2 beamDirection = -fireAI.direction;

            float offset = 0.01f; // Adjust this value as needed
            Vector2 beamStart = beamEndPoint + beamDirection * offset;

            transform.position = beamStart;
            transform.rotation = fireAI.transform.rotation * Quaternion.Euler(0, 0, 180);

            origin = beamStart;
            direction = transform.right;
            top = origin + (Vector2)transform.up * half_height;
            bottom = origin - (Vector2)transform.up * half_height;

            RaycastHit2D hit_center = Physics2D.Raycast(origin, direction, maxDistance, combinedLayer);
            fire_width = hit_center.distance;
            fire_width = (fire_width == 0f) ? maxDistance : fire_width;
            hittingSomething = hit_center.collider == null ? false : true;
            sprite.size = new Vector2(fire_width / transform.lossyScale.x, original_height / transform.localScale.y);

            if (currentAttackTime == 0f)
            {
                if (hittingSomething)
                {
                    PlayerDamageAndHealth hit = hit_center.collider.gameObject.GetComponentInParent<PlayerDamageAndHealth>();

                    if (hit != null)
                    {
                        hit.Hit_No_Collider(gameObject);
                    }
                }

                currentAttackTime = maxAttackTime;

            }

        }

        else
        {
            hittingSomething = false;
        }

    }
    public void renderSprite(bool yes)
    {
        sprite.enabled = yes;
    }

}

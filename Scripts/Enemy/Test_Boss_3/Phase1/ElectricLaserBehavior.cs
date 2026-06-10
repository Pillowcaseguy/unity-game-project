using UnityEngine;

public class ElectricLaserBehavior : MonoBehaviour
{

    public LayerMask groundLayer;
    public LayerMask playerLayer;
    public SpriteRenderer sprite;
    public BoxCollider2D box;
    public EnemyAttack attack;

    public float parry_cooldown;
    public float hit_cooldown;
    public bool not_attacking;
    public GameObject shocked_player;

    public GameObject parent_rotator;
    public ElectricLaserRotationPoint parent_rotator_behavior;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        groundLayer = LayerMask.GetMask("Ground");
        playerLayer = LayerMask.GetMask("Player_Damage");
        sprite = GetComponent<SpriteRenderer>();
        box = GetComponent<BoxCollider2D>();
        attack = GetComponent<EnemyAttack>();
    }
    // Update is called once per frame
    void Update()
    {
        if (parry_cooldown > 0)
        {
            parry_cooldown -= Time.deltaTime;
            if (parry_cooldown < 0)
            {
                parry_cooldown = 0;
            }
            /*sprite.size = new Vector2(0, sprite.size.y);
            box.size = new Vector2(0,box.size.y);
            box.offset = new Vector2(0, box.offset.y);
            return;*/
        }

        if (hit_cooldown > 0)
        {
            hit_cooldown -= Time.deltaTime;
            if (hit_cooldown < 0)
            {
                hit_cooldown = 0;
            }
        }

        if (parent_rotator_behavior.not_attacking && !not_attacking)
        {
            not_attacking = true;
        }

        if (not_attacking)
        {
            sprite.size = new Vector2(0, sprite.size.y);
            box.size = new Vector2(0, box.size.y);
            box.offset = new Vector2(0 / 2, box.offset.y);
            return;
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position,transform.right,99,groundLayer);
        if (hit.collider != null)
        {
            float corrected_dist = hit.distance / transform.lossyScale.x;
            sprite.size = new Vector2(corrected_dist,sprite.size.y);
            box.size = new Vector2(corrected_dist, box.size.y);
            box.offset = new Vector2(corrected_dist / 2, box.offset.y);
        }
        //Debug.DrawRay(transform.position, transform.right * 99, Color.red);
    }
    private void OnTriggerStay2D(Collider2D col)
    {
        if (not_attacking)
        {
            return;
        }
        if (((1 << col.gameObject.layer) & playerLayer) != 0)
        {
            PlayerDamageAndHealth ph = col.gameObject.GetComponentInParent<PlayerDamageAndHealth>();
            if (ph != null)
            {
                if (ph.parryTime > 0)
                {
                    ph.Hit_No_Collider(gameObject);
                    parry_cooldown = 1.5f;
                }
                else if (parry_cooldown <= 0 && hit_cooldown <= 0 && ph.trueInvincibilityTime <= 0)
                {
                    ph.Hit_No_Collider(gameObject);
                    GameObject shock = Instantiate(shocked_player,ph.gameObject.transform.position,Quaternion.Euler(0,0,0));
                    shock.GetComponent<ShockedPlayerBehavior>().initialize(ph.gameObject);
                    Destroy(shock, 0.5f);
                    hit_cooldown = 0.5f;
                }
            }
        }
    }
}

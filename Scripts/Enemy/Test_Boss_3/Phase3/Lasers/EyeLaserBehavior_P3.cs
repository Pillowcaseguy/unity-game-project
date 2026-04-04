using UnityEngine;

public class EyeLaserBehavior_P3 : MonoBehaviour
{

    public BoxCollider2D box;
    public SpriteRenderer sprite;
    public SpriteRenderer parent_sprite;

    public int groundLayer;
    public int playerLayer;

    public bool activated;
    public bool facing_left;

    public bool finished_loop;
    public bool can_hurt_player;

    public bool flipped_once = false;
    public bool unflipped_once = true;

    public EnemyAttack attack;

    public float fixed_x_facing_left;
    public float fixed_x_facing_right;

    public float fixed_y;

    public bool i_wanna_do_damage;

    void Start()
    {
        box = GetComponent<BoxCollider2D>();
        groundLayer = LayerMask.GetMask("Ground","BreakableGround");
        playerLayer = LayerMask.GetMask("Player_Damage");
        sprite = GetComponent<SpriteRenderer>();
        attack = GetComponent<EnemyAttack>();
    }

    void Update()
    {
        if (activated)
        {
            RaycastHit2D test = Physics2D.Raycast(transform.position, -transform.right, 40f, groundLayer);
            //Debug.DrawRay(transform.position, -transform.right * 25f, Color.red);
            if (test.collider != null)
            {
                box.size = new Vector2(test.distance / transform.lossyScale.x, box.size.y);
                box.offset = new Vector2(-1f * test.distance / transform.lossyScale.x / 2f, box.offset.y);
                sprite.size = new Vector2(test.distance / transform.lossyScale.x, sprite.size.y);
            }
            else
            {
                box.size = new Vector2(25f / transform.lossyScale.x, box.size.y);
                box.offset = new Vector2(-25f / transform.lossyScale.x / 2f, box.offset.y);
                sprite.size = new Vector2(25f / transform.lossyScale.x, sprite.size.y);
            }
        }
        else
        {
            sprite.size = new Vector2(0f, sprite.size.y);
            box.size = new Vector2(0f, box.size.y);
            box.offset = new Vector2(0f, box.offset.y);
        }
    }
    private void FixedUpdate()
    {
        if (activated)
        {
            float z = gameObject.transform.rotation.eulerAngles.z;
            if (facing_left)
            {
                if (z > 0)
                {
                    gameObject.transform.Rotate(0, 0, -3);
                    if (gameObject.transform.rotation.eulerAngles.z > 350)
                    {
                        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                }
                else
                {
                    finished_loop = true;
                }
            }
            else
            {
                if (z < 180)
                {
                    gameObject.transform.Rotate(0, 0, 3);
                }
                else
                {
                    finished_loop = true;
                }
            }
        }
    }
    public void Flip()
    {

        if (parent_sprite.flipX)
        {
            if (!flipped_once)
            {
                transform.localPosition = new Vector2(fixed_x_facing_right,fixed_y);
                flipped_once = true;
                unflipped_once = false;
            }
        }
        else
        {
            if (!unflipped_once)
            {
                transform.localPosition = new Vector2(fixed_x_facing_left, fixed_y);
                flipped_once = false;
                unflipped_once = true;
            }
        }
    }
    public void start_rotation()
    {
        attack.clearFlags();
        can_hurt_player = true;
        if (facing_left)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 95);
        }
        else
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 85);
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!i_wanna_do_damage)
        {
            return;
        }
        if (((1 << col.gameObject.layer) & playerLayer) != 0)
        {
            if (can_hurt_player)
            {
                PlayerDamageAndHealth attempt = col.gameObject.GetComponentInParent<PlayerDamageAndHealth>();
                if (attempt != null)
                {
                    attempt.Hit_No_Collider(gameObject);
                }
            }
        }
    }
}

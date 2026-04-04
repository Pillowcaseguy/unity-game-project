using UnityEngine;

public class ElectrifiedGroundFullBehavior : MonoBehaviour
{

    public float hit_cooldown;
    public bool electrified;
    public GameObject shocked_player;
    public Animator ani;
    public EnemyAttack attack;
    public SpriteRenderer sprite;

    public float top;
    public float left;
    public float right;
    public Vector2 origin;

    public int playerLayer;
    public float[] positions;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerLayer = LayerMask.GetMask("Player", "Player_Damage");
        hit_cooldown = 0f;
        ani = GetComponentInChildren<Animator>();
        attack = GetComponent<EnemyAttack>();
        sprite = GetComponent<SpriteRenderer>();

        positions = new float[] { 0.05f, 0.125f, 0.25f, 0.375f, 0.5f, 0.625f, 0.75f, 0.875f, 0.95f };
        top = transform.position.y + transform.localScale.y / 2f;
        left = transform.position.x - transform.localScale.x / 2f;
        right = transform.position.x + transform.localScale.x / 2f;
        origin = new Vector2(left - right, top);

        electrified = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (hit_cooldown > 0f)
        {
            hit_cooldown -= Time.deltaTime;
            if (hit_cooldown <= 0f)
            {
                hit_cooldown = 0f;
            }
        }

        if (hit_cooldown <= 0f && electrified)
        {
            for (int i = 0; i < positions.Length; i++)
            {

                RaycastHit2D temp = Physics2D.Raycast(new Vector2(left - origin.x * positions[i], origin.y), transform.up, 0.05f, playerLayer);
                if (temp.collider != null)
                {
                    PlayerDamageAndHealth attempt = temp.collider.gameObject.GetComponentInParent<PlayerDamageAndHealth>();
                    if (attempt != null)
                    {
                        shock_player(attempt);
                        break;
                    }
                }
            }
        }
    }
    public void shock_player(PlayerDamageAndHealth helf)
    {
        if (helf.invincibilityTime + helf.trueInvincibilityTime <= 0f)
        {
            hit_cooldown = 3f;
            helf.Hit_No_Collider(gameObject);
            GameObject thing = Instantiate(shocked_player, helf.gameObject.transform.position, Quaternion.Euler(0, 0, 0));
            Destroy(thing, 1f);
        }
    }
    public void start_electrify()
    {
        ani.SetBool("start_shocking", true);
        electrified = true;
    }

    public void end_electrify()
    {
        ani.SetBool("start_shocking",false);
        electrified = false;
    }
}

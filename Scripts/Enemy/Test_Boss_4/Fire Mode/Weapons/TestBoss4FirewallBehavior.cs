using UnityEngine;

public class TestBoss4FirewallBehavior : MonoBehaviour, EnemyDamageable
{

    public LayerMask player_layer;
    public int active_layer;
    public int inactive_layer;

    public GameObject the_player;
    public Animator ani;
    public SpriteRenderer sprite;

    public float lifetime;
    public bool already_breaking;
    public bool initialized;

    public bool left_wall;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        ani = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        player_layer = LayerMask.GetMask("Player");
        active_layer = LayerMask.NameToLayer("Active_Enemy_Attack");
        inactive_layer = LayerMask.NameToLayer("Enemy");
    }
    public void initialize(float lif, GameObject player, bool is_left)
    {
        lifetime = lif;
        the_player = player;
        left_wall = is_left;
        initialized = true;
    }
    public void destroyme()
    {
        Destroy(gameObject);
    }
    private void Update()
    {
        if (!initialized)
        {
            return;
        }

        lifetime -= Time.deltaTime;
        if (!already_breaking && lifetime <= 0)
        {
            break_me();
            already_breaking = true;
        }

        if (the_player && left_wall && the_player.transform.position.x < -9)
        {
            the_player.gameObject.transform.position = new Vector2(-8, the_player.gameObject.transform.position.y);
        }
        else if (the_player && !left_wall && the_player.transform.position.x > 9)
        {
            the_player.gameObject.transform.position = new Vector2(8, the_player.gameObject.transform.position.y);
        }
    }
    public void break_me()
    {
        ani.SetBool("is_breaking", true);
        gameObject.layer = inactive_layer;
    }
    public void ARISE()
    {
        gameObject.layer = active_layer;
    }
    public void start_growing()
    {
        ani.SetBool("is_rising", true);
    }
    private void OnCollisionStay2D(Collision2D col)
    {
        if (gameObject.layer == active_layer && ((1 << col.gameObject.layer) & player_layer) != 0)
        {
            var attempt = col.gameObject.GetComponent<PlayerDamageAndHealth>();
            if (attempt != null)
            {
                attempt.Hit_No_Collider(gameObject);
            }
        }
    }
    public void TakeDamage(int damage)
    {
        
    }

    public void Hit(PlayerAttack player_attack)
    {
        player_attack.isBlock();
    }
}

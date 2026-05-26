using UnityEngine;

public class TestBoss4MapAttackBehavior_Ice : MonoBehaviour
{
    public Animator ani;

    public LayerMask playerLayer;
    //public LayerMask active_layer;
    //public LayerMask inactive_layer;

    public EnemyAttack attack;
    Camera cam;
    public float width;
    public float height;

    public bool attackFired = false;

    public GameObject boss;
    public TestBoss4Behavior the_boss;

    public GameObject the_player;

    void Awake()
    {
        attack = GetComponent<EnemyAttack>();
    }
    void Start()
    {
        ani = GetComponent<Animator>();
        playerLayer = LayerMask.GetMask("Player_Damage");
        //active_layer = LayerMask.NameToLayer("Player_Attack");
        //inactive_layer = LayerMask.NameToLayer("Inactive_Player_Attack");

        cam = Camera.main;
        height = cam.orthographicSize * 2f;
        width = height * cam.aspect;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Vector2 spriteSize = sr.sprite.bounds.size;

        transform.localScale = new Vector2(width / spriteSize.x, height / spriteSize.y);
    }
    public void initialize(GameObject bosh, TestBoss4Behavior bosh_b, GameObject playa)
    {
        boss = bosh;
        the_boss = bosh_b;
        the_player = playa;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(cam.transform.position.x, cam.transform.position.y);
    }
    public void do_attack()
    {
        if (boss == null || the_player == null)
            return;
        if (!attackFired)
        {
            if (Mathf.Abs(the_player.transform.position.x - boss.transform.position.x) < 8f)
            {
                var attempt = the_player.GetComponent<PlayerDamageAndHealth>();
                if (attempt != null)
                {
                    attempt.Hit_No_Collider(gameObject);
                }
                attackFired = true;
            }
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

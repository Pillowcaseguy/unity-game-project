using UnityEngine;

public class NinjaSlashBehavior : MonoBehaviour
{
    public Animator ani;
    public EnemyAttack attack;
    public LevelManager_TestBoss5 manager;
    public BoxCollider2D crouch_checker;
    public float chosen_angle;

    //public LayerMask active_layer;
    //public LayerMask player_layer;

    void Awake()
    {
        ani = GetComponent<Animator>();
        attack = GetComponent<EnemyAttack>();
        manager = FindAnyObjectByType<LevelManager_TestBoss5>();
        chosen_angle = Random.Range(10, 80);
        transform.rotation = Quaternion.Euler(0,0,chosen_angle);
        //active_layer = LayerMask.GetMask("Active_Enemy_Attack");
        //player_layer = LayerMask.GetMask("Player_Damage");
        Invoke("begin_attack", 1f);
    }
    public void begin_attack()
    {
        attack.EnableHitbox();
        crouch_checker.GetComponent<EnemyAttack>().EnableHitbox();
        ani.SetBool("start_attack", true);
    }
    public void destroy_self()
    {
        Destroy(gameObject);
    }
    public void FixedUpdate()
    {
        crouch_checker.gameObject.transform.localRotation = Quaternion.Euler(0, 0, chosen_angle * -1f);
        if (manager != null && manager.has_player)
        {
            if (manager.player_life.movement.crouching)
            {
                crouch_checker.enabled = true;
            }
            else
            {
                crouch_checker.enabled = false;
            }
        }
    }
    /*private void OnTriggerStay2D(Collider2D col)
    {
        if (gameObject.layer != active_layer)
        {
            return;
        }

        if (((1 << col.gameObject.layer) & player_layer) != 0)
        {
            Debug.Log("woof");
            PlayerDamageAndHealth attempt = col.gameObject.GetComponentInParent<PlayerDamageAndHealth>();
            if (attempt != null)
            {
                attempt.Hit_No_Collider(gameObject);
            }
        }
    }*/
}

using UnityEngine;

public class SniperBehavior : MonoBehaviour
{

    GameObject level_manager;
    LevelManager_TestBoss2 manager;

    SpriteRenderer sprite;
    SniperFirePoint sf;
    Animator ani;

    public GameObject laser;
    GameObject current_laser;
    GameObject owner;

    public GameObject sniper_bullet;

    TestBoss2Behavior owner_logic;

    public bool initialized = false;

    public bool took_shot = false;
    public bool is_destroyed;
    public bool first_destroyed = false;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sf = GetComponentInChildren<SniperFirePoint>();
        ani = GetComponent<Animator>();
    }
    public void instantiate(GameObject owna)
    {
        owner = owna;
        owner_logic = owner.GetComponent<TestBoss2Behavior>();
        initialized = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (!initialized)
        {
            return;
        }

        if (level_manager == null)
        {
            LevelManager_TestBoss2 attempt = FindAnyObjectByType<LevelManager_TestBoss2>();
            if (attempt != null)
            {
                manager = FindAnyObjectByType<LevelManager_TestBoss2>();
                level_manager = manager.gameObject;
            }
        }

        if (manager != null && manager.has_player && !is_destroyed)
        {
            Vector2 snipe_position = sf.transform.position;
            Vector2 player_position = manager.player.transform.position;

            Vector2 ray_to_player = new Vector2(player_position.x - snipe_position.x, player_position.y - snipe_position.y);
            Debug.DrawLine(snipe_position, player_position, Color.red);
            float calc_rotation = Mathf.Atan2(ray_to_player.y,ray_to_player.x) * Mathf.Rad2Deg;
            //Debug.Log(calc_rotation);
            if (calc_rotation < -90)
            {
                sprite.flipY = true;
            }
            else
            {
                sprite.flipY = false;
            }
            
            transform.rotation = Quaternion.Euler(0, 0, calc_rotation);

        }

        if (owner_logic.aiming_gun && !took_shot)
        {
            if (current_laser == null)
            {
                current_laser = Instantiate(laser, sf.transform.position, Quaternion.Euler(0, 0, 0));
                current_laser.GetComponent<LaserBehavior>().initialize(manager.player, sf);
            }
        }

        if (took_shot && !is_destroyed)
        {
            Destroy(current_laser);
            int bullet_type = Random.Range(0, 4);
            GameObject shot = Instantiate(sniper_bullet,sf.gameObject.transform.position,Quaternion.Euler(0,0,transform.rotation.eulerAngles.z));
            SniperBulletBehavior ss = shot.GetComponent<SniperBulletBehavior>();
            ss.Initialize(18, transform.rotation.eulerAngles.z, 3, bullet_type);
            owner_logic.queued_shot = false;
            is_destroyed = true;
            Destroy(gameObject, 1f);
        }

        if (is_destroyed && !first_destroyed)
        {
            first_destroyed = true;
            ani.SetBool("is_shooting", is_destroyed);
            if (!sprite.flipY)
            {
                transform.Translate((-transform.right + -transform.up) * 0.15f);
                Invoke("move_back",0.5f);
            }
            else
            {
                transform.Translate((transform.right + -transform.up) * 0.15f);
                Invoke("move_back", 0.5f);
            }
        }

    }
    public void destroy()
    {
        Destroy(current_laser);
        owner_logic.aiming_gun = false;
        owner_logic.queued_shot = false;
        is_destroyed = true;
        Destroy(gameObject);
    }
    public void move_back()
    {
        if (!sprite.flipY)
        {
            transform.Translate((transform.right + transform.up) * 0.15f);
        }
        else
        {
            transform.Translate((-transform.right + transform.up) * 0.15f);
        }
    }

}

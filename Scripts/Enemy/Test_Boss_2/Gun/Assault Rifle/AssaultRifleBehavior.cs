using UnityEngine;

public class AssaultRifleBehavior : MonoBehaviour
{

    AssaultRifleFirePoint firePoint;
    SpriteRenderer sprite;
    Animator ani;

    TestBoss2Behavior owner_logic;

    public int starting_direction;
    public GameObject bullet;

    public bool initialized = false;
    public bool sweeping_right;
    public bool shoot_started;

    public bool finished_shooting;
    public bool started_destruction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        firePoint = GetComponentInChildren<AssaultRifleFirePoint>();
        sprite = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
    }
    public void initalize(bool sweep_right_direction,GameObject owner)
    {
        sweeping_right = sweep_right_direction;
        if (sweeping_right)
        {
            transform.rotation = Quaternion.Euler(0, 0, 190);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 350);
        }
        initialized = true;
        owner_logic = owner.GetComponent<TestBoss2Behavior>();
        Invoke("start_shooting", 1f);

    }

    // Update is called once per frame
    void Update()
    {
        if (!initialized)
        {
            return;
        }

        if (!started_destruction && finished_shooting) 
        {
            started_destruction = true;
            shoot_started = false;
            Invoke("destroy",1f);
        }
        else if (started_destruction)
        {
            return;
        }

        float zee = transform.rotation.eulerAngles.z;
        if (zee > 180 && zee < 270)
        {
            sprite.flipY = true;
        }
        else
        {
            sprite.flipY = false;
        }

        ani.SetBool("shooting", shoot_started);
        ani.SetBool("destroyed", started_destruction);

    }
    public void take_shot()
    {
        if (finished_shooting)
        {
            return;
        }

        if (sweeping_right)
        {
            GameObject pew = Instantiate(bullet,firePoint.transform.position,transform.rotation);
            BulletBehavior pewpew = pew.GetComponent<BulletBehavior>();
            pewpew.Initialize(10, 9);
        }

        else
        {
            GameObject pew = Instantiate(bullet, firePoint.transform.position, transform.rotation);
            BulletBehavior pewpew = pew.GetComponent<BulletBehavior>();
            pewpew.Initialize(10, 9);
        }
    }
    public void rotate()
    {
        if (finished_shooting)
        {
            return;
        }

        if (sweeping_right)
        {
            transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + 5);
            if (transform.rotation.eulerAngles.z >= 350)
            {
                finished_shooting = true;
            }
        }

        else
        {
            transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z - 5);
            if (transform.rotation.eulerAngles.z <= 190)
            {
                finished_shooting = true;
            }
        }
    }
    void start_shooting()
    {
        shoot_started = true;
    }
    public void destroy()
    {
        owner_logic.aiming_gun = false;
        Destroy(gameObject);
    }
    void move_back()
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
    void undo_move()
    {
        if (!sprite.flipY)
        {
            transform.Translate((-transform.right + -transform.up) * 0.15f);
        }
        else
        {
            transform.Translate((transform.right + -transform.up) * 0.15f);
        }
    }

}

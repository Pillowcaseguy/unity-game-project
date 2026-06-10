using UnityEngine;

public class SmokeBehavior : MonoBehaviour
{
    Animator ani;
    EnemyAttack attack;

    public float lifetime;
    public bool initialized = false;
    public bool working = false;
    void Awake()
    {
        ani = GetComponent<Animator>();
        attack = GetComponent<EnemyAttack>();
    }
    public void initialize(float life)
    {
        lifetime = life;
        initialized = true;
        working = true;
    }
    void Update()
    {
        if (!initialized)
        {
            return;
        }

        if (lifetime > 0)
        {
            lifetime -= Time.deltaTime;
            if (lifetime < 0)
            {
                lifetime = 0;
                working = false;
            }
        }

        ani.SetBool("is_destroyed", lifetime == 0);

    }
    public void destroy()
    {
        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!working)
        {
            return;
        }
        PlayerDamageAndHealth attempt = collision.gameObject.GetComponent<PlayerDamageAndHealth>();
        if (attempt != null)
        {
            attempt.Hit_No_Collider(gameObject);
        }
    }

}

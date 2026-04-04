using UnityEngine;

public class WiresBehavior : MonoBehaviour
{

    public float hit_cooldown;
    public PlayerMovement current;
    public GameObject shocked_player;
    public Animator ani;
    
    void Start()
    {
        hit_cooldown = 0f;
        ani = GetComponentInChildren<Animator>();
    }

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

        if (current != null)
        {
            current.rb.linearVelocityY = 0;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (hit_cooldown <= 0f)
        {
            PlayerDamageAndHealth attempt = collision.gameObject.GetComponentInParent<PlayerDamageAndHealth>();
            if (attempt != null && attempt.invincibilityTime + attempt.trueInvincibilityTime <= 0f)
            {
                hit_cooldown = 5f;
                attempt.Hit_No_Collider(gameObject);
                GameObject thing = Instantiate(shocked_player, collision.gameObject.transform.position, Quaternion.Euler(0, 0, 0));
                Destroy(thing, 1f);
                current = collision.gameObject.GetComponentInParent<PlayerMovement>();
                Invoke("fix_body_type", 1f);
            }
        }
    }
    public void fix_body_type()
    {
        if (current != null)
        {
            current = null;
        }
    }
}

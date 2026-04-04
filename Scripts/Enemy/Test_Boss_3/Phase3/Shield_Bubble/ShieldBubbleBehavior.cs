using UnityEngine;

public class ShieldBubbleBehavior : MonoBehaviour
{
    public int shield_health;
    public int shield_max_health;
    public bool is_broken = false;
    public Animator ani;
    public SpriteRenderer sprite;
    void Start()
    {
        shield_health = shield_max_health;
        sprite = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
        ani.SetBool("is_shining", true);
    }
    public void damage(int dmg)
    {
        shield_health -= dmg;
        ani.SetBool("is_hit", true);
        if (shield_health <= 0)
        {
            is_broken = true;
            ani.SetBool("is_breaking", true);
        }
    }
    public void destroy()
    {
        Destroy(gameObject);
    }
    public void unhit()
    {
        ani.SetBool("is_hit", false);
    }
    public void unshine()
    {
        ani.SetBool("is_shining", false);
    }
}

using UnityEngine;

public class SmokeBehavior : MonoBehaviour
{
    Animator ani;
    EnemyAttack attack;

    public float lifetime;
    public bool initialized = false;
    void Awake()
    {
        ani = GetComponent<Animator>();
        attack = GetComponent<EnemyAttack>();
    }
    public void initialize(float life)
    {
        lifetime = life;
        initialized = true;
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
                attack.DisableHitbox();
                lifetime = 0;
            }
        }

        ani.SetBool("is_destroyed", lifetime == 0);

    }
    public void destroy()
    {
        Destroy(gameObject);
    }

}

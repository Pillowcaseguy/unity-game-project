using UnityEngine;

public class TestBoss5_NinjaGunpowderBehavior : MonoBehaviour
{
    public EnemyAttack my_attack;
    public SpriteRenderer sprite;
    public Animator ani;
    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        my_attack = GetComponent<EnemyAttack>();
        ani = GetComponent<Animator>();
    }
    public void begin_attack()
    {
        my_attack.EnableHitbox();
    }
    public void destroy_me()
    {
        Destroy(gameObject);
    }
}

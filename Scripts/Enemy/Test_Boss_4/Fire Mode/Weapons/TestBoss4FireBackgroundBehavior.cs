using UnityEngine;

public class TestBoss4FireBackgroundBehavior : MonoBehaviour
{
    public SpriteRenderer sprite;
    public EnemyAttack attack;
    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        attack = GetComponent<EnemyAttack>();
    }
    public void change_scale(float newscale)
    {
        gameObject.transform.localScale = new Vector3(transform.localScale.x, newscale, 1);
        attack.EnableHitbox();
    }
}

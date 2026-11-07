using UnityEngine;

public class WizardGroundBreakBehavior : MonoBehaviour
{
    public BoxCollider2D box;
    public EnemyAttack attack;

    void Awake()
    {
        box = GetComponent<BoxCollider2D>();
        attack = GetComponent<EnemyAttack>();
    }
    public void grow_hitbox()
    {
        box.size = new Vector2(box.size.x + 0.3f, box.size.y);
    }
    public void activate_attack()
    {
        attack.EnableHitbox();
    }
}

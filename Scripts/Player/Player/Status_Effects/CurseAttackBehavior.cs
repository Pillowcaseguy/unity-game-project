using UnityEngine;

public class CurseAttackBehavior : MonoBehaviour
{

    public BoxCollider2D box;
    public EnemyAttack attack;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        box = GetComponent<BoxCollider2D>();
        attack = GetComponent<EnemyAttack>();
    }
    public void activateAttack()
    {
        box.enabled = true;
        attack.startDestroy();
    }
}

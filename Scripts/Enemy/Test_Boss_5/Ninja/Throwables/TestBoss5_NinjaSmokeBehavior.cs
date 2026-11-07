using UnityEngine;

public class TestBoss5_NinjaSmokeBehavior : MonoBehaviour
{
    public EnemyAttack attack;
    void Awake()
    {
        attack = GetComponent<EnemyAttack>();
    }
    public void destroy_me()
    {
        Destroy(gameObject);
    }
}

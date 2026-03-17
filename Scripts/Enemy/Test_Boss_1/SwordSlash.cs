using UnityEngine;

public class SwordSlash : MonoBehaviour
{
    public EnemyAttack attack;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attack = GetComponent<EnemyAttack>();
    }
    public void enable()
    {
        attack.EnableHitbox();
    }
    public void disable()
    {
        attack.DisableHitbox();
    }

}

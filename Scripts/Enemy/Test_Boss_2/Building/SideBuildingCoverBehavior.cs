using UnityEngine;

public class SideBuildingCoverBehavior : MonoBehaviour, EnemyDamageable
{
    public GameObject building;
    public SideBuildingBehavior my_building;
    void Awake()
    {
        my_building = GetComponentInParent<SideBuildingBehavior>();
    }
    public void change_layer()
    {
        building.layer = LayerMask.NameToLayer("Inactive_Enemy_Attack");
    }
    public void Hit(PlayerAttack player_attack)
    {
        my_building.Hit(player_attack);
    }

    public void TakeDamage(int damage)
    {

    }
}

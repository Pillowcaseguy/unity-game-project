using UnityEngine;

public class SideBuildingCoverBehavior : MonoBehaviour
{
    public GameObject building;
    public void change_layer()
    {
        building.layer = LayerMask.NameToLayer("Inactive_Enemy_Attack");
    }
}

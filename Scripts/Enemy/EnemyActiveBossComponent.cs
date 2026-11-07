using UnityEngine;

public class EnemyActiveBossComponent : MonoBehaviour
{
    public bool isPrimaryTarget = true;
    public void notPrimary()
    {
        isPrimaryTarget = false;
    }
    public void isPrimary()
    {
        isPrimaryTarget = true;
    }
}

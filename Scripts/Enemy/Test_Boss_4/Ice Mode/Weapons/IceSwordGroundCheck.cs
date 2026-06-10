using UnityEngine;

public class IceSwordGroundCheck : MonoBehaviour
{
    public IceSwordBehavior main_sword;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & main_sword.ground_layer) != 0)
        {
            main_sword.is_destroyed = true;
        }
    }
}

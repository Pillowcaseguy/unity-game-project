using UnityEngine;

public class SoldierMissileGroundCheck : MonoBehaviour
{
    public SoldierMissileBehavior missile;
    public LayerMask ground_layer;
    public float initial_x;
    public float initial_y;
    void Awake()
    {
        ground_layer = LayerMask.GetMask("Ground");
        initial_x = missile.transform.position.x;
        initial_y = missile.transform.position.y;
    }
    private void OnTriggerStay2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & ground_layer) != 0)
        {
            if (missile.rotation >= 180 && (initial_x >= missile.initial_player_x || initial_y <= missile.initial_player_y))
            {
                missile.explode();
            }
            else if (missile.rotation < 180 && (initial_x <= missile.initial_player_x || initial_y <= missile.initial_player_y))
            {
                missile.explode();
            }
        }
    }
}

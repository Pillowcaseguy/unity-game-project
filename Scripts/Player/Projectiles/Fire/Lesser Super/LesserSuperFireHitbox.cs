using UnityEngine;

public class LesserSuperFireHitbox : MonoBehaviour
{
    public BoxCollider2D hitbox;
    public LesserSuperFireBehavior main_fire;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & main_fire.enemyLayer) != 0)
        {
            if (main_fire != null && main_fire.start_attack == false)
            {
                main_fire.start_attack = true;
            }
        }
        if (((1 << col.gameObject.layer) & main_fire.groundLayer) != 0)
        {
            main_fire.start_attack = true;
        }
    }
}

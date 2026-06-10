using UnityEngine;

public class LesserSuperIceGroundCheck : MonoBehaviour
{
    public LesserSuperIceBehavior icy;

    private void OnTriggerStay2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & icy.groundLayer) != 0)
        {
            int hit_count = 0;
            foreach (int x in icy.enemies_hit.Keys)
            {
                hit_count += icy.enemies_hit[x];
            }
            int index_count = icy.enemies_hit.Keys.Count;

            if (hit_count >= 6 || index_count >= 2)
            {
                for (int i = 0; i < 25; i++)
                {
                    var hi = Instantiate(icy.ice_particle, transform.position, Quaternion.Euler(0, 0, 0)).GetComponent<IceParticleBehavior>();
                    hi.keep_on_ground_hit = true;
                }
                Destroy(icy.gameObject);
            }
        }
    }
}

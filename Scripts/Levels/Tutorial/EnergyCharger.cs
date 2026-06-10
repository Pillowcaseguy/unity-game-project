using UnityEngine;

public class EnergyCharger : MonoBehaviour
{
    public LayerMask player_layer;
    public int energy_charger;
    void Start()
    {
        player_layer = LayerMask.GetMask("Player");
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & player_layer) != 0)
        {
            var attempt = col.GetComponent<PlayerDamageAndHealth>();
            if (attempt != null)
            {
                attempt.AddEnergy(energy_charger);
            }
        }
    }
}

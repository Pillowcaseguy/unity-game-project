using UnityEngine;

public class TestBoss4ThrowingFireballGroundCheck : MonoBehaviour
{
    public TestBoss4ThrowingFireballBehavior main_fireball;
    public LayerMask ground_layer;
    void Awake()
    {
        ground_layer = LayerMask.GetMask("Ground", "BreakableGround");
    }
    public void OnTriggerEnter2D(Collider2D col)
    {
        if (main_fireball.attack.isDestroyed)
        {
            return;
        }

        if (((1 << col.gameObject.layer) & ground_layer) != 0)
        {
            main_fireball.attack.startDestroy();
        }

        /*if (((1 << col.gameObject.layer) & player_layer) != 0)
        {
            PlayerDamageAndHealth attempt = col.gameObject.GetComponent<PlayerDamageAndHealth>();
            if (attempt != null)
            {
                attempt.Hit_No_Collider(gameObject);
            }
        }*/
    }
}

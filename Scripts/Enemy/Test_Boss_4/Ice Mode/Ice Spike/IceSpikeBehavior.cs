using UnityEngine;

public class IceSpikeBehavior : MonoBehaviour
{
    public BoxCollider2D col;
    public Animator ani;

    public LayerMask player_layer;
    public bool attack_hit;
    public bool can_fling_player;
    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        ani = GetComponent<Animator>();
        player_layer = LayerMask.GetMask("Player_Damage");
    }
    public void grow_spike(float f)
    {
        col.size = new Vector2(col.size.x, f);
        col.offset = new Vector2(col.offset.x, f / 2f);
    }
    public void destroy()
    {
        Destroy(gameObject);
    }
    public void OnTriggerStay2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & player_layer) != 0 && !attack_hit)
        {
            Rigidbody2D attempt = col.gameObject.GetComponentInParent<Rigidbody2D>();
            if (attempt != null)
            {
                if (attempt.linearVelocityY < 0)
                {
                    attempt.linearVelocityY = 0;
                }
                //int direction = Random.Range(0, 2);
                //float x_force = 150 * (direction == 0 ? 1 : -1);
                float x_force = 0;
                if (can_fling_player)
                {
                    col.gameObject.transform.parent.position = new Vector2(col.gameObject.transform.parent.position.x, transform.position.y + 3f);
                    attempt.AddForce(new Vector2(x_force, 400));
                }
                attack_hit = true;
            }
        }
    }
}

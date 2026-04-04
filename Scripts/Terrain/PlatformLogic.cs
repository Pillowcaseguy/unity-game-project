using UnityEngine;
using UnityEngine.LowLevelPhysics;

public class PlatformLogic : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D col;
    private int trigger_layer;
    private int platform_layer;

    public LayerMask Feet_Layer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        trigger_layer = LayerMask.NameToLayer("Trigger_Platform");
        platform_layer = LayerMask.NameToLayer("Platform");
    }

    // Update is called once per frame - but it's empty so shut it up
    /*void Update()
    {

    }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //GameObject maybe_player = collision.attachedRigidbody.gameObject;
        //Debug.Log(maybe_player.name.ToString() == "Player");
        //Debug.Log(collision.name);

        if (((1 << collision.gameObject.layer) & Feet_Layer) != 0)
        {
            Rigidbody2D playerRB = collision.attachedRigidbody.GetComponent<Rigidbody2D>();

            if (playerRB.linearVelocityY <= 0f) // If the player is falling or stationary in the y direction
            {
                if (col.isTrigger)
                {
                    col.isTrigger = false;
                    gameObject.layer = platform_layer;
                }

            }

        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        PlayerMovement player = collision.collider.GetComponent<PlayerMovement>();
        if (player != null && player.down_jumping)
        {
            col.isTrigger = true;
            gameObject.layer = trigger_layer;
            collision.collider.attachedRigidbody.linearVelocityY = 0.01f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & Feet_Layer) != 0) // If what exited is on the Feet_Layer
        {
            col.isTrigger = true;
            gameObject.layer = trigger_layer;
        }
    }

}

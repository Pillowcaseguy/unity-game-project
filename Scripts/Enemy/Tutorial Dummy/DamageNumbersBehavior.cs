using TMPro;
using UnityEngine;

public class DamageNumbersBehavior : MonoBehaviour
{
    public LayerMask ground_layer;

    public TextMeshPro the_text;
    public Rigidbody2D rb;
    private void Awake()
    {
        ground_layer = LayerMask.GetMask("Ground", "BreakableGround");
    }
    public void initialize(int number)
    {
        float x_force = Random.Range(-150f, 150f);
        float y_force = Random.Range(100f, 200f);
        rb.AddForce(new Vector2(x_force, y_force));
        the_text.text = number.ToString();
        if (number >= 40)
        {
            the_text.color = Color.yellow;
        }
        if (number >= 100)
        {
            the_text.color = Color.red;
            the_text.fontStyle = FontStyles.Bold;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (((1 << col.gameObject.layer) & ground_layer) != 0)
        {
            Destroy(gameObject);
        }
    }
}

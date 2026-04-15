using UnityEngine;

public class PigToyShadow : MonoBehaviour
{

    private PigToy hwoh;
    private Transform parent_transform;
    private SpriteRenderer sprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hwoh = GetComponentInParent<PigToy>();
        parent_transform = hwoh.GetComponent<Transform>();
        sprite = GetComponent<SpriteRenderer>();
        transform.SetParent(null);
    }

    // Update is called once per frame
    void Update()
    {

        if (parent_transform == null)
        {
            Destroy(gameObject);
            return;
        }

        RaycastHit2D result = hwoh.hit_result;

        if (result.collider == null)
        {
            sprite.enabled = false;
        }

        else
        {
            sprite.enabled = true;
            GameObject hit_object = result.collider.gameObject;
            float new_scale = Mathf.Abs(result.distance - hwoh.max_distance) * 0.4f;
            if (new_scale < 0.5f)
            {
                new_scale = 0.5f;
            }
            else if (new_scale > 1.9f)
            {
                new_scale = 2.0f;
            }

            transform.localScale = new Vector2(new_scale, new_scale);

            transform.position = new Vector2(parent_transform.position.x, hit_object.transform.position.y + 0.5f * hit_object.transform.localScale.y);
        }
    }
}

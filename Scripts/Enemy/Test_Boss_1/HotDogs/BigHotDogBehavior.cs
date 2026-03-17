using UnityEngine;

public class BigHotDogBehavior : MonoBehaviour
{

    public bool has_a_small_hot_dog;

    public float launch_delay = 0f;
    public float last_attack_time = 0f;

    public GameObject smallHotDog;

    public GameObject currentSmallHotDog;
    public SmallHotDogBehavior controls;

    public Sprite single_sprite;
    public Sprite multiple_sprite;

    private SpriteRenderer sprite;
    private Transform transform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (has_a_small_hot_dog)
        {
            last_attack_time += Time.deltaTime;
            sprite.sprite = multiple_sprite;
        }

        else
        {
            sprite.sprite = single_sprite;
        }

        if (has_a_small_hot_dog && last_attack_time > 5.0f)
        {
            has_a_small_hot_dog = false;
            last_attack_time = 0;
            controls.current_phase += 1;
        }

        if (currentSmallHotDog == null)
        {
            currentSmallHotDog = Instantiate(smallHotDog, new Vector2(transform.position.x + 0.8f,transform.position.y + 0.1f), Quaternion.Euler(0,0,0));
            controls = currentSmallHotDog.GetComponent<SmallHotDogBehavior>();
            controls.Initialize(gameObject);
        }

    }
}

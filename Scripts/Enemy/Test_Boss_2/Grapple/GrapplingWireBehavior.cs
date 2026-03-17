using UnityEngine;

public class GrapplingWireBehavior : MonoBehaviour
{

    public bool initialized = false;

    public GameObject owner;
    public SpriteRenderer sprite;
    Vector2 building_target;

    public GameObject grapple_hook;
    public GameObject current_grapple_hook;

    public void initialize(GameObject owna, Vector2 building_position)
    {
        owner = owna;
        //throwing_point = throwPt;
        initialized = true;
        building_target = building_position;
    }
    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if (!initialized)
        {
            return;
        }

        //Vector2 start_position = throwing_point.transform.position;
        Vector2 start_position = owner.transform.position;
        Vector2 end_position = building_target;

        Vector2 ray_to_building = new Vector2(end_position.x - start_position.x, end_position.y - start_position.y);
        float calc_rotation = Mathf.Atan2(ray_to_building.y, ray_to_building.x) * Mathf.Rad2Deg;
        //Debug.Log(calc_rotation);
        float hypotenuse = Mathf.Sqrt(Mathf.Pow(ray_to_building.x, 2) + Mathf.Pow(ray_to_building.y, 2));
        transform.rotation = Quaternion.Euler(0, 0, calc_rotation);
        transform.position = start_position;
        transform.localScale = new Vector2(hypotenuse * 6, 1);

        if (current_grapple_hook == null)
        {
            current_grapple_hook = Instantiate(grapple_hook,end_position,Quaternion.Euler(0,0,calc_rotation - 90));
            GrapplingHookBehavior gh = current_grapple_hook.GetComponent<GrapplingHookBehavior>();
            gh.initialize(gameObject);
        }

    }
}

using UnityEngine;

public class SlownessBehavior : MonoBehaviour
{
    public float lifetime = 0.1f;
    public bool is_destroyed = false;
    public GameObject owner = null;

    public float time_passed;
    /*public Animator ani;
    void Awake()
    {
        ani = gameObject.GetComponent<Animator>();
    }*/
    public void Initialize(float life, GameObject owna)
    {
        lifetime = life;
        owner = owna;
    }

    // Update is called once per frame
    void Update()
    {
        if (owner == null)
        {
            return;
        }

        time_passed += Time.deltaTime / 2;
        float distance_up = time_passed * Mathf.PI;

        if (lifetime > 0f)
        {
            transform.position = new Vector2(owner.transform.position.x + 0.8f, owner.transform.position.y + 1.5f + Mathf.Sin(distance_up) / 4);
            lifetime -= Time.deltaTime;
        }

        else if (lifetime < 0f && !is_destroyed)
        {
            is_destroyed = true;
            //ani.SetBool("is_destroyed", is_destroyed);
        }

        if (is_destroyed)
        {
            Destroy(gameObject, 0.0f);
        }
    }
}

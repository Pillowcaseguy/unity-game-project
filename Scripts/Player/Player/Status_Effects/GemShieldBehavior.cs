using UnityEngine;

public class GemShieldBehavior : MonoBehaviour
{
    public float lifetime;
    public Animator ani;
    public GameObject owner;
    void Awake()
    {
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (owner != null)
        {
            transform.position = new Vector2(owner.transform.position.x, owner.transform.position.y + 1.8f);
        }
        lifetime -= Time.deltaTime;
        if (lifetime <= 1)
        {
            ani.SetBool("start_breaking", true);
        }

        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void initialize(float f, GameObject owna)
    {
        lifetime = f;
        owner = owna;
    }
    public void destroy_me()
    {
        Destroy(gameObject);
    }
}

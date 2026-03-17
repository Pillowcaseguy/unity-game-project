using UnityEngine;

public class CurseBehavior : MonoBehaviour
{
    public float lifetime = 0.1f;
    public bool is_destroyed = false;
    public GameObject owner = null;
    public Transform transform;
    public Animator ani;

    public GameObject curse_attack;
    void Awake()
    {
        transform = gameObject.transform;
        ani = gameObject.GetComponent<Animator>();
    }
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

        if (lifetime > 0f)
        {
            transform.position = new Vector2(owner.transform.position.x + 0.07f,owner.transform.position.y + 0.7f);
            lifetime -= Time.deltaTime;
        }

        else if (lifetime < 0f && !is_destroyed)
        {
            is_destroyed = true;
            ani.SetBool("is_destroyed", is_destroyed);
        }

        if (is_destroyed)
        {
            Destroy(gameObject, 0.5f);
        }
    }
    public void makeCurse()
    {
        Instantiate(curse_attack, transform.position, transform.rotation);
    }

}

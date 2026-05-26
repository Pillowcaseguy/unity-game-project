using UnityEngine;

public class TestBoss4StunEffect : MonoBehaviour
{
    public float lifetime = 0.1f;
    public bool is_destroyed = false;
    public GameObject owner = null;
    public Animator ani;
    void Awake()
    {
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
            transform.position = new Vector2(owner.transform.position.x + 0.3f, owner.transform.position.y + 1.5f);
            lifetime -= Time.deltaTime;
        }

        else if (lifetime < 0f && !is_destroyed)
        {
            is_destroyed = true;
            ani.SetBool("is_destroyed", is_destroyed);
        }

        if (is_destroyed)
        {
            Destroy(gameObject, 0.0f);
        }
    }
}

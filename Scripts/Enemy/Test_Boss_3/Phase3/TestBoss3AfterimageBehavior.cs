using UnityEngine;

public class TestBoss3AfterimageBehavior : MonoBehaviour
{
    public float lifetime;
    void Start()
    {
        lifetime = 1f;
    }
    private void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0f)
        {
            Destroy(gameObject);
        }
    }
}

using UnityEngine;

public class LightSuperImpactBehavior : MonoBehaviour
{
    public Animator ani;
    private void Awake()
    {
        ani = GetComponent<Animator>();
        ani.speed = 1 / Time.timeScale;
    }
    public void destroy_me()
    {
        Destroy(gameObject);
    }
}

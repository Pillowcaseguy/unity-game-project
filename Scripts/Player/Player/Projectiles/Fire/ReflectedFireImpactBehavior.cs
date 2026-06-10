using UnityEngine;
public class ReflectedFireImpactBehavior : MonoBehaviour
{
    private SpriteRenderer sprite;
    private ReflectedFireBehavior fireAI;
    private float offset = 0.6f;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        fireAI = GetComponentInParent<ReflectedFireBehavior>();
    }

    void Update()
    {
        if (fireAI.hittingSomething)
        {
            sprite.enabled = true;

            // Position at the end of the beam along its direction
            Vector2 beamStart = fireAI.transform.position;
            Vector2 beamDirection = fireAI.direction;
            float distanceAlongBeam = fireAI.fire_width - offset;

            transform.position = beamStart + beamDirection * distanceAlongBeam;
            transform.rotation = fireAI.transform.rotation;
        }
        else
        {
            sprite.enabled = false;
        }
    }
}
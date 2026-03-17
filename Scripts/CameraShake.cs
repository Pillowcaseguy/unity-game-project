using UnityEngine;

public class CameraShake : MonoBehaviour
{

    private Transform cameraTransform;
    private float cameraDefaultY = 0.0f;
    public float shakeTime = 0f;
    private float lastShake = 0f;
    private bool negativeY = true;
    private bool isShaking = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (shakeTime > 0f)
        {
            isShaking = true;
            if (lastShake == 0f)
            {
                ShakeCameraVertically();
            }
            shakeTime -= Time.deltaTime;
            if (shakeTime < 0f)
            {
                shakeTime = 0f;
            }
        }

        if (lastShake > 0f)
        {
            lastShake -= Time.deltaTime;
            if (lastShake < 0f)
            {
                lastShake = 0f;
            }
        }

        if (shakeTime == 0f && isShaking == true)
        {
            float currentX = cameraTransform.position.x;
            float currentZ = cameraTransform.position.z;
            cameraTransform.position = new Vector3(currentX,cameraDefaultY,currentZ);
            isShaking = false;
        }

    }
    private void ShakeCameraVertically()
    {
        float currentX = cameraTransform.position.x;
        float currentZ = cameraTransform.position.z;
        lastShake = 0.05f;

        float moveAmount = Mathf.Pow(shakeTime * 2, 2) * 0.25f;
        if (negativeY)
        {
            moveAmount *= -1;
            negativeY = false;
        }
        else
        {
            negativeY = true;
        }

        cameraTransform.position = new Vector3(currentX, moveAmount, currentZ);

    }
    public void shakeOnHurt(float timeToShake)
    {
        shakeTime = timeToShake;
    }
}

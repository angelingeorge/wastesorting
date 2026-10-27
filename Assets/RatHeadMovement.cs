
using UnityEngine;

public class RatHeadMovement : MonoBehaviour
{
    public float tiltAngle = 8f;
    public float tiltSpeed = 2f;

    private Quaternion startRotation;

    void Start()
    {
        startRotation = transform.localRotation;
    }

    void Update()
    {
        float angle = Mathf.Sin(Time.time * tiltSpeed) * tiltAngle;

        transform.localRotation =
            startRotation * Quaternion.Euler(0f, 0f, angle);
    }
}
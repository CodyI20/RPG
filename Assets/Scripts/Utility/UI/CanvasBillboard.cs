using UnityEngine;

public class CanvasBillboard : MonoBehaviour
{
    Transform target;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        // Direction to the target
        Vector3 direction = transform.position - target.position;

        // Zero out the Y component to ensure we only rotate around the Y axis
        direction.y = 0;

        // Check if the direction vector is non-zero to avoid errors
        if (direction != Vector3.zero)
        {
            // Calculate the rotation
            Quaternion rotation = Quaternion.LookRotation(direction);

            // Apply the rotation to the transform
            transform.rotation = rotation;
        }
    }
}

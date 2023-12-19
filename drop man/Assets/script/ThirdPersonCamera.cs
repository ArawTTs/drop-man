using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target; // The target object that the camera will follow
    public float distance = 10f; // The distance between the camera and the target object
    public float height = 5f; // The height of the camera above the target object
    public float rotationDamping = 2f; // The amount of damping to apply to the camera rotation

    // LateUpdate is called once per frame after all Update calls have finished
    void LateUpdate()
    {
        // Calculate the target position for the camera
        Vector3 targetPosition = target.position - (target.forward * distance) + (target.up * height);

        // Set the camera position to the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * rotationDamping);

        // Rotate the camera to look at the target object
        transform.LookAt(target);
    }
}


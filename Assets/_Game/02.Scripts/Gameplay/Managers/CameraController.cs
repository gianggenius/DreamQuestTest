namespace _Game._02.Scripts.Gameplay
{
    using UnityEngine;

    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 10.0f;

        void Update()
        {
            // Get input for camera movement
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput   = Input.GetAxis("Vertical");

            // Calculate movement direction
            Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput);

            // Normalize the direction vector to ensure constant speed in all directions
            if (moveDirection.magnitude > 1.0f)
            {
                moveDirection.Normalize();
            }

            // Move the camera
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
        }
    }

}
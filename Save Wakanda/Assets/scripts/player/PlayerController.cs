using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float gravity = -9.8f;

    private CharacterController controller;
    private Vector3 velocity;

    public float rotationSpeed = 180f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Rotate when pressing side keys (horizontal input) instead of strafing
        if (Mathf.Abs(x) > 0.01f)
        {
            transform.Rotate(0f, x * rotationSpeed * Time.deltaTime, 0f);
            x = 0f; // prevent strafing while turning
        }

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * moveSpeed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
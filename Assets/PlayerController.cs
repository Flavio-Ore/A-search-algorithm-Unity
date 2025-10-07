using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Get input
        float horizontal = Input.GetAxis("Horizontal"); // A/D or Arrow keys
        float vertical = Input.GetAxis("Vertical");     // W/S or Arrow keys

        // Create movement direction
        Vector3 movement = new(horizontal, 0, vertical); // Simplified 'new' expression (IDE0090)

        // Move the player
        controller.Move(movement * Time.deltaTime * moveSpeed); // Re-ordered operands for better performance (UNT0024)
    }
}
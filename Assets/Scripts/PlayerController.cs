using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Transform playerTransform;
    public float moveSpeed = 5f;

    private InputAction moveAction;

    void Start()
    {
        if (playerTransform == null)
        {
            playerTransform = transform;
        }

        moveAction = InputSystem.actions.FindAction("Move");
        moveAction.Enable();
    }

    void FixedUpdate()
    {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();

        if (moveAction.IsPressed() && moveValue != Vector2.zero)
        {
            Vector3 moveDirection = new Vector3(moveValue.x, 0, moveValue.y);
            playerTransform.Translate(moveDirection * Time.deltaTime * moveSpeed);
        }
    }
}

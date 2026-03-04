using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Rigidbody playerRigidbody;

    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    private InputAction _moveAction;

    private void Start()
    {
        _moveAction = InputSystem.actions.FindAction( "Move" );
        _moveAction.Enable();
    }

    private void FixedUpdate()
    {
        var moveValue = _moveAction.ReadValue< Vector2 >();

        if ( !_moveAction.IsPressed() || moveValue == Vector2.zero ) return;

        // Horizontale Bewegung
        float horizontalVelocity = moveValue.x * moveSpeed;

        // Vertikale Bewegung
        if ( moveValue.y > 0 ) // W - nach oben springen
        {
            playerRigidbody.linearVelocity = new Vector3( horizontalVelocity, jumpForce, 0 );
        }
        else if ( moveValue.y < 0 ) // S - nach unten drücken
        {
            playerRigidbody.linearVelocity = new Vector3( horizontalVelocity, -jumpForce, 0 );
        }
        else { playerRigidbody.linearVelocity = new Vector3( horizontalVelocity, playerRigidbody.linearVelocity.y, 0 ); }
    }
}

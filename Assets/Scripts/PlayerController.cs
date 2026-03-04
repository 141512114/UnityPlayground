using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public new Rigidbody rigidbody;

    public float moveSpeed          = 5f;
    public float jumpForce          = 5f;
    public float maxHorizontalSpeed = 3f; // Maximale horizontale Geschwindigkeit
    public float maxVerticalSpeed   = 5f; // Maximale vertikale Geschwindigkeit

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

        // Horizontale Bewegung (x-Achse)
        rigidbody.AddForce( moveValue.x * moveSpeed, 0, 0, ForceMode.Impulse );

        // Vertikale Bewegung (y-Achse)
        if ( moveValue.y > 0 ) // W - nach oben springen
        {
            rigidbody.AddForce( 0, jumpForce, 0, ForceMode.Impulse );
        }
        else if ( moveValue.y < 0 ) // S - nach unten drücken
        {
            rigidbody.AddForce( 0, -jumpForce, 0, ForceMode.Impulse );
        }

        // Velocity auf maximalen Wert begrenzen
        ClampVelocity();
    }

    private void ClampVelocity()
    {
        var velocity = rigidbody.linearVelocity;

        // Horizontale Geschwindigkeit limitieren
        float horizontalSpeed = new Vector2( velocity.x, velocity.z ).magnitude;
        if ( horizontalSpeed > maxHorizontalSpeed )
        {
            float ratio = maxHorizontalSpeed / horizontalSpeed;
            velocity.x *= ratio;
            velocity.z *= ratio;
        }

        // Vertikale Geschwindigkeit limitieren
        velocity.y = Mathf.Clamp( velocity.y, -maxVerticalSpeed, maxVerticalSpeed );

        rigidbody.linearVelocity = velocity;
    }
}

using Attributes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    /// <summary>
    /// Steuert die Bewegung des Spielers basierend auf den Eingaben der WASD-Tasten.
    /// Ermöglicht horizontale Bewegung und vertikales Springen/Drücken, während die Geschwindigkeit begrenzt wird.
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        public new Rigidbody rigidbody;

        [Header( "Einstellungen" )]
        [Label( "Bewegungsgeschwindigkeit" )]
        public float moveSpeed = 30f;

        [Label( "Sprungkraft" )] public float jumpForce = 50f;

        [Label( "Maximale horizontale Geschwindigkeit", "Die maximale Geschwindigkeit, die der Spieler horizontal erreichen kann." )]
        public float maxHorizontalSpeed = 75f;

        [Label( "Maximale vertikale Geschwindigkeit", "Die maximale Geschwindigkeit, die der Spieler vertikal erreichen kann (z.B. beim Springen oder Fallen)." )]
        public float maxVerticalSpeed = 75f;

        private InputAction _moveAction;

        private void Start()
        {
            _moveAction = InputSystem.actions.FindAction( "Move" );
            if ( _moveAction == null )
            {
                Debug.LogError( "Move action not found in Input System." );
                enabled = false;
                return;
            }

            _moveAction.Enable();
        }

        private void FixedUpdate()
        {
            var moveValue = _moveAction.ReadValue< Vector2 >();

            if ( !_moveAction.IsPressed() || moveValue == Vector2.zero ) return;

            // Horizontale Bewegung (x-Achse)
            rigidbody.AddForce( moveValue.x * moveSpeed * Time.fixedDeltaTime, 0, 0, ForceMode.Impulse );

            // Vertikale Bewegung (y-Achse)
            if ( moveValue.y > 0 ) // W - nach oben springen
            {
                rigidbody.AddForce( 0, jumpForce * Time.fixedDeltaTime, 0, ForceMode.Impulse );
            }
            else if ( moveValue.y < 0 ) // S - nach unten drücken
            {
                rigidbody.AddForce( 0, -jumpForce * Time.fixedDeltaTime, 0, ForceMode.Impulse );
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
}

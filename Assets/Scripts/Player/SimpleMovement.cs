using Common;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    /// <summary>
    /// Steuert die Bewegung des Spielers basierend auf den Eingaben der WASD-Tasten.
    /// Ermöglicht horizontale Bewegung, während die Geschwindigkeit begrenzt wird.
    /// </summary>
    [RequireComponent( typeof( PlayerController ) )]
    public class SimpleMovement : AbstractMovement
    {
        private PlayerController playerController;

        private InputAction _moveAction;

        private void Awake()
        {
            playerController = GetComponent<PlayerController>();

            _moveAction = InputSystem.actions.FindAction( "Move" );
            if ( _moveAction == null )
            {
                Debug.LogError( "Move action not found in Input System." );
                enabled = false;
                return;
            }

            _moveAction.Enable();
        }

        public override void Move()
        {
            var moveValue = _moveAction.ReadValue< Vector2 >();

            if ( !_moveAction.IsPressed() || moveValue == Vector2.zero ) return;

            // Horizontale Bewegung (x-Achse)
            playerController.rigidbody.AddForce( moveValue.x * playerController.moveSpeed * Time.fixedDeltaTime, 0, 0, ForceMode.Impulse );
            // Tiefenbewegung (z-Achse)
            playerController.rigidbody.AddForce( 0, 0, moveValue.y * playerController.moveSpeed * Time.fixedDeltaTime, ForceMode.Impulse );

            ClampVelocity();
        }

        protected override void ClampVelocity()
        {
            var velocity = playerController.rigidbody.linearVelocity;

            // Horizontale Geschwindigkeit limitieren
            float horizontalSpeed = new Vector2( velocity.x, velocity.z ).magnitude;
            if ( horizontalSpeed > playerController.maxHorizontalSpeed )
            {
                float ratio = playerController.maxHorizontalSpeed / horizontalSpeed;
                velocity.x *= ratio;
                velocity.z *= ratio;
            }

            // Vertikale Geschwindigkeit limitieren
            velocity.y = Mathf.Clamp( velocity.y, -playerController.maxVerticalSpeed, playerController.maxVerticalSpeed );

            playerController.rigidbody.linearVelocity = velocity;
        }
    }
}

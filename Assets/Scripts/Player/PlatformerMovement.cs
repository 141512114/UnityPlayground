using Common;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// Steuert die Bewegung des Spielers basierend auf den Eingaben der WASD-Tasten.
    /// Ermöglicht horizontale Bewegung und vertikales Springen/Drücken, während
    /// die Geschwindigkeit begrenzt wird. Der Platform-Modus kann aktiviert werden, um vertikale Bewegungen zu ermöglichen.
    /// </summary>
    [RequireComponent( typeof( PlayerController ) )]
    public class PlatformerMovement : AbstractMovement
    {
        public override void Move()
        {
            var moveValue = moveAction.ReadValue< Vector2 >();

            if ( !moveAction.IsPressed() || moveValue == Vector2.zero ) return;

            // Horizontale Bewegung (x-Achse)
            playerController.rigidbody.AddForce( moveValue.x * playerController.moveSpeed * Time.fixedDeltaTime, 0, 0, ForceMode.Impulse );

            // Vertikale Bewegung (y-Achse)
            if ( moveValue.y > 0 ) // W - nach oben springen
            {
                playerController.rigidbody.AddForce( 0, playerController.jumpForce * Time.fixedDeltaTime, 0, ForceMode.Impulse );
            }
            else if ( moveValue.y < 0 ) // S - nach unten drücken
            {
                playerController.rigidbody.AddForce( 0, -playerController.jumpForce * Time.fixedDeltaTime, 0, ForceMode.Impulse );
            }

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

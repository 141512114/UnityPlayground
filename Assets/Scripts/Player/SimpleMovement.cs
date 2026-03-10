using Common;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// Steuert die Bewegung des Spielers basierend auf den Eingaben der WASD-Tasten.
    /// Ermöglicht horizontale Bewegung, während die Geschwindigkeit begrenzt wird.
    /// </summary>
    [RequireComponent( typeof( PlayerController ) )]
    public class SimpleMovement : AbstractMovement
    {
        public override void Move()
        {
            var moveValue = moveAction.ReadValue< Vector2 >();

            if ( !moveAction.IsPressed() || moveValue == Vector2.zero ) return;

            Vector3 forward = playerController.CameraManager.CurrentCameraInstance.transform.forward;
            Vector3 right   = playerController.CameraManager.CurrentCameraInstance.transform.right;
            Vector3 dir     = forward * moveValue.y + right * moveValue.x;

            Vector3 moveVector = playerController.moveSpeed / 10 * Time.fixedDeltaTime * dir;

            playerController.transform.Translate( moveVector, Space.World );
        }

        protected override void ClampVelocity()
        {
            // SimpleMovement begrenzt Geschwindigkeit nicht (kinematische Bewegung)
        }
    }
}

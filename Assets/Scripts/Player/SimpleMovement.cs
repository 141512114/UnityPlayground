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

            Vector3 moveVector = playerController.moveSpeed / 10 * Time.fixedDeltaTime * new Vector3( moveValue.x, 0, moveValue.y ).normalized;

            playerController.transform.Translate( moveVector, Space.World );
        }

        protected override void ClampVelocity()
        {
            throw new System.NotImplementedException();
        }
    }
}

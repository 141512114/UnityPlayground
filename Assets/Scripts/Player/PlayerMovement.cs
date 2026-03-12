using Attributes;
using Camera;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    /// <summary>
    /// Steuert die Bewegung des Spielers basierend auf den Eingaben der WASD-Tasten.
    /// </summary>
    [RequireComponent( typeof( PlayerController ) ), RequireComponent( typeof( CharacterController ) )]
    public class PlayerMovement : MonoBehaviour
    {
        [Header( "Einstellungen" )]
        [Label( "Gravitation" )]
        public float gravity = 9.81f;

        [Label( "Bewegungsgeschwindigkeit" )] public float moveSpeed = 15f;

        [Label( "Sprunghöhe" )] public float jumpHeight = 5f;

        private CharacterController _characterController;
        private PlayerController    _playerController;

        private InputAction _moveAction;
        private InputAction _jumpAction;

        private Vector3 _velocity;

        private void Awake()
        {
            _characterController = GetComponent< CharacterController >();
            _playerController    = GetComponent< PlayerController >();

            // Setze den Mittelpunkt des Spielers auf die Hälfte der Höhe
            _characterController.center = new Vector3( _characterController.center.x, _characterController.height / 2f, _characterController.center.z );

            _moveAction = InputSystem.actions.FindAction( "Move" );
            if ( _moveAction == null )
            {
                Debug.LogError( "Input-Aktion 'Move' nicht gefunden im Input System." );
                enabled = false;
                return;
            }
            
            _jumpAction = InputSystem.actions.FindAction( "Jump" );
            if ( _jumpAction == null )
            {
                Debug.LogError( "Input-Aktion 'Jump' nicht gefunden im Input System." );
                enabled = false;
                return;
            }

            _moveAction.Enable();
            _jumpAction.Enable();
        }

        /// <summary>
        /// Bewegt den Spieler anhand von Tastatureingaben und berücksichtigt ggf. die Kamera in der Orientation des Spielers.
        /// </summary>
        /// <param name="deltaTime">Kann übergeben werden, um Hardware-Abhängigkeit zu aktivieren</param>
        public void Move( float deltaTime = 1f )
        {
            _Jump();
            Vector3 moveVector = _Move();
            
            // Gravitation dem Spieler auswirken
            if ( _characterController.isGrounded && _velocity.y < 0 ) { _velocity.y = -2f; }

            _velocity.y -= gravity * deltaTime;
            moveVector.y = _velocity.y;

            _characterController.Move( moveVector * deltaTime );
        }

        /// <summary>
        /// Separate Methode, welche es dem Spieler erlaubt den Spielcharakter zu bewegen mit den WASD-Tasten.
        /// </summary>
        private Vector3 _Move()
        {
            CameraInstance cameraInstance = _playerController.CameraManager.CurrentCameraInstance;

            var     moveValue  = _moveAction.ReadValue< Vector2 >();
            Vector3 moveVector = Vector3.zero;

            Vector3 forward = Vector3.forward;
            Vector3 right   = Vector3.right;

            if ( cameraInstance )
            {
                forward = cameraInstance.transform.forward;
                right   = cameraInstance.transform.right;

                // Verhindert Bewegung nach oben/unten durch Kamera-Pitch
                forward.y = 0;
                right.y   = 0;

                forward.Normalize();
                right.Normalize();
            }

            if ( moveValue == Vector2.zero ) return moveVector;
            Vector3 dir = forward * moveValue.y + right * moveValue.x;
            moveVector = moveSpeed * dir;

            return moveVector;
        }

        private void _Jump()
        {
            if ( !_jumpAction.triggered || !_characterController.isGrounded )
                return;
            
            _velocity.y = Mathf.Sqrt( jumpHeight * -2f * -gravity );
        }
    }
}

using Attributes;
using Camera;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// Steuert die Bewegung eines Golfballs mit physikbasiertem Rollen und Abbremsen.
    /// </summary>
    [RequireComponent( typeof( Rigidbody ) )]
    public class GolfBallMovement : MonoBehaviour
    {
        private Rigidbody _rigidbody;

        [SerializeField, Label( "Kameramanager" )]
        private CameraManager cameraManager;

        [Header( "Golf-Einstellungen" )]
        [SerializeField, Label( "Schlagkraft", "Die maximale Kraft beim Golfschwung" )]
        private float swingForce = 50f;

        [SerializeField, Label( "Minimale Geschwindigkeit", "Unter dieser Geschwindigkeit stoppt der Ball" )]
        private float minVelocity = 0.1f;

        private bool  _canSwing = true;
        private float _swingPower;
        private bool  _hasTouchedGround;

        private void Awake() { _rigidbody = GetComponent< Rigidbody >(); }

        private void Update()
        {
            if ( _hasTouchedGround )
            {
                // Schlag vorbereiten mit Mausklick halten
                if ( Input.GetMouseButton( 0 ) && _canSwing ) { _swingPower = Mathf.Min( _swingPower + Time.deltaTime * 2f, 1f ); }

                // Schlag ausführen beim Loslassen
                if ( Input.GetMouseButtonUp( 0 ) && _canSwing ) { ExecuteSwing(); }
            }

            // Ball stoppen wenn zu langsam
            if ( _rigidbody.linearVelocity.magnitude < minVelocity ) { _rigidbody.linearVelocity = Vector3.zero; }
        }

        private void ExecuteSwing()
        {
            // Schlag nur in horizontale Richtung (Kamera-Forward)
            Vector3 hitDirection = cameraManager.CurrentCameraInstance.Camera.transform.forward;
            hitDirection.y = 0;
            hitDirection.Normalize();

            _rigidbody.linearVelocity = hitDirection * ( swingForce * _swingPower );

            _canSwing         = false;
            _hasTouchedGround = false;
            _swingPower       = 0f;
        }

        private void OnCollisionStay( Collision collision )
        {
            if ( !collision.gameObject.CompareTag( "Ground" ) ) return;
            _hasTouchedGround = true;
            _canSwing         = true;
        }
    }
}

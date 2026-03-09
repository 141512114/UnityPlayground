using Attributes;
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

        [Header( "Golf-Einstellungen" )]
        [SerializeField, Label( "Schlagkraft", "Die maximale Kraft beim Golfschwung" )]
        private float swingForce = 50f;

        [SerializeField, Label( "Reibungskoeffizient", "Je höher, desto schneller bremst der Ball ab (0-1)" )]
        private float frictionDamping = 0.05f;

        [SerializeField, Label( "Minimale Geschwindigkeit", "Unter dieser Geschwindigkeit stoppt der Ball" )]
        private float minVelocity = 0.1f;

        private bool  _canSwing = true;
        private float _swingPower;

        private void Awake() { _rigidbody = GetComponent< Rigidbody >(); }

        private void Update()
        {
            // Schlag vorbereiten mit Mausklick halten
            if ( Input.GetMouseButton( 0 ) && _canSwing ) { _swingPower = Mathf.Min( _swingPower + Time.deltaTime * 2f, 1f ); }

            // Schlag ausführen beim Loslassen
            if ( Input.GetMouseButtonUp( 0 ) && _canSwing ) { ExecuteSwing(); }

            // Ball stoppen wenn zu langsam
            if ( _rigidbody.linearVelocity.magnitude < minVelocity ) { _rigidbody.linearVelocity = Vector3.zero; }
        }

        private void FixedUpdate()
        {
            // Reibung anwenden
            ApplyFriction();
        }

        private void ExecuteSwing()
        {
            // Schlag nur in horizontale Richtung (Kamera-Forward)
            Vector3 hitDirection = UnityEngine.Camera.main.transform.forward;
            hitDirection.y = 0;
            hitDirection.Normalize();

            _rigidbody.linearVelocity = hitDirection * ( swingForce * _swingPower );

            _canSwing   = false;
            _swingPower = 0f;

            // Nach einer Sekunde wieder schlagen dürfen
            Invoke( nameof( ResetSwing ), 1f );
        }

        private void ApplyFriction()
        {
            Vector3 velocity = _rigidbody.linearVelocity;
            velocity                  *= ( 1f - frictionDamping );
            _rigidbody.linearVelocity =  velocity;
        }

        private void ResetSwing() { _canSwing = true; }
    }
}

using Attributes;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// Steuert die Bewegung des Spielers basierend auf den Eingaben der WASD-Tasten.
    /// Ermöglicht horizontale Bewegung und vertikales Springen/Drücken, während die Geschwindigkeit begrenzt wird.
    /// </summary>
    [RequireComponent( typeof( SimpleMovement ) ), RequireComponent( typeof( PlattformerMovement ) )]
    public class PlayerController : MonoBehaviour
    {
        public new Rigidbody rigidbody;

        [Header( "Einstellungen" )]
        [Label("Plattformer-Modus", "Aktivieren, um vertikale Bewegungen (Springen/Drücken) zu ermöglichen. Deaktivieren für reine horizontale Bewegung.")]
        public bool isPlattformer = false;

        [Label( "Bewegungsgeschwindigkeit" )] public float moveSpeed = 30f;

        [Label( "Sprungkraft" )] public float jumpForce = 50f;

        [Label( "Maximale horizontale Geschwindigkeit", "Die maximale Geschwindigkeit, die der Spieler horizontal erreichen kann." )]
        public float maxHorizontalSpeed = 75f;

        [Label( "Maximale vertikale Geschwindigkeit", "Die maximale Geschwindigkeit, die der Spieler vertikal erreichen kann (z.B. beim Springen oder Fallen)." )]
        public float maxVerticalSpeed = 75f;

        private SimpleMovement _simpleMovement;
        private PlattformerMovement _plattformerMovement;

        private void Awake()
        {
            _simpleMovement = GetComponent<SimpleMovement>();
            _plattformerMovement = GetComponent<PlattformerMovement>();

            if ( _simpleMovement == null && _plattformerMovement == null )
            {
                Debug.LogError( "Neither SimpleMovement nor PlattformerMovement component found on PlayerController GameObject." );
                enabled = false;
                return;
            }
        }

        private void FixedUpdate()
        {
            if ( isPlattformer )
            {
                _plattformerMovement.Move();
            }
            else
            {
                _simpleMovement.Move();
            }
        }
    }
}

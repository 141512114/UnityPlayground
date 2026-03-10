using Attributes;
using Camera;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    /// <summary>
    /// Steuert die Bewegung des Spielers basierend auf den Eingaben der WASD-Tasten.
    /// Ermöglicht horizontale Bewegung und vertikales Springen/Drücken, während die Geschwindigkeit begrenzt wird.
    /// </summary>
    [RequireComponent( typeof( SimpleMovement ) ), RequireComponent( typeof( PlatformerMovement ) )]
    public class PlayerController : MonoBehaviour
    {
        public new Rigidbody rigidbody;

        [SerializeField] private CameraManager _cameraManager;

        public CameraManager CameraManager => _cameraManager;

        [Header( "Einstellungen" )]
        [Label( "Platformer-Modus", "Aktivieren, um vertikale Bewegungen (Springen/Drücken) zu ermöglichen. Deaktivieren für reine horizontale Bewegung." )]
        public bool isPlatformer;

        [Label( "Bewegungsgeschwindigkeit" )] public float moveSpeed = 30f;

        [Label( "Sprungkraft" )] public float jumpForce = 50f;

        [Label( "Maximale horizontale Geschwindigkeit", "Die maximale Geschwindigkeit, die der Spieler horizontal erreichen kann." )]
        public float maxHorizontalSpeed = 75f;

        [Label( "Maximale vertikale Geschwindigkeit", "Die maximale Geschwindigkeit, die der Spieler vertikal erreichen kann (z.B. beim Springen oder Fallen)." )]
        public float maxVerticalSpeed = 75f;

        private SimpleMovement     _simpleMovement;
        private PlatformerMovement _platformerMovement;

        private void Awake()
        {
            _simpleMovement     = GetComponent< SimpleMovement >();
            _platformerMovement = GetComponent< PlatformerMovement >();

            if ( _simpleMovement != null || _platformerMovement != null ) return;
            Debug.LogError( "Neither SimpleMovement nor PlatformerMovement component found on PlayerController GameObject." );
            enabled = false;
        }

        private void FixedUpdate()
        {
            if ( isPlatformer ) { _platformerMovement.Move(); }
            else { _simpleMovement.Move(); }
        }
    }
}

using Attributes;
using UnityEngine;

namespace Camera
{
    /// <summary>
    /// Verwaltet mehrere Kameras in der Szene, ermöglicht das Umschalten zwischen ihnen
    /// und steuert die Kamerabewegung und -rotation basierend auf einem Ziel (z.B. Spieler).
    /// </summary>
    public class CameraManager : MonoBehaviour
    {
        /// <summary>Array der verfügbaren Kamera-Instanzen in der Szene.</summary>
        [SerializeField, Tooltip( "Liste aller Kameras, die von diesem Manager verwaltet werden." )]
        public CameraInstance[] cameras;

        /// <summary>Das Ziel, das die Kamera verfolgen soll (z.B. der Spieler).</summary>
        [SerializeField, Label( "Ziel", "Der Transform, den die Kamera verfolgen soll (typischerweise der Spieler)." )]
        public Transform target;

        private CameraInstance     _currentCameraInstance;
        private int                _currentCameraIndex;
        private UnityEngine.Camera _currentCamera;
        private Transform          _currentCameraTransform;

        private void Start()
        {
            if ( cameras == null || cameras.Length == 0 )
            {
                Debug.LogError( "CameraManager: Keine Kameras zugewiesen. Bitte mindestens eine Kamera hinzufügen.", gameObject );
                enabled = false;
                return;
            }

            if ( target == null ) { Debug.LogWarning( "CameraManager: Kein Ziel (target) zugewiesen. Die Kamera wird sich nicht bewegen.", gameObject ); }

            InitializeCameras();
        }

        private void Update() { HandleCameraSwitching(); }

        private void FixedUpdate()
        {
            FollowTarget();
            LookAtTarget();
        }

        /// <summary>
        /// Initialisiert die Kameras und setzt die aktuelle Kamera.
        /// </summary>
        private void InitializeCameras()
        {
            FindMainCamera();
            SetupCurrentCamera();

            // Deaktiviere alle anderen Kameras außer der aktuellen, um sicherzustellen, dass nur eine Kamera aktiv ist.
            for ( int i = 0; i < cameras.Length; i++ )
            {
                if ( i != _currentCameraIndex ) { cameras[ i ].GetCamera().gameObject.SetActive( false ); }
            }

            SetupInitialCameraPosition();
        }

        private void FindMainCamera()
        {
            _currentCameraIndex = 0;

            for ( int i = 0; i < cameras.Length; i++ )
            {
                if ( !cameras[ i ].IsMainCamera() ) continue;
                _currentCameraIndex = i;
                break;
            }
        }

        /// <summary>
        /// Richtet die aktuelle Kamera ein.
        /// </summary>
        private void SetupCurrentCamera()
        {
            _currentCameraInstance  = cameras[ _currentCameraIndex ];
            _currentCamera          = _currentCameraInstance.GetCamera();
            _currentCameraTransform = _currentCamera.transform;

            _currentCamera.gameObject.SetActive( true );
        }

        private void SetupInitialCameraPosition()
        {
            FollowTarget();
            LookAtTarget();
        }

        /// <summary>
        /// Verwaltet das Umschalten zwischen Kameras.
        /// </summary>
        private void HandleCameraSwitching()
        {
            if ( !Input.GetKeyDown( KeyCode.Tab ) ) return;
            SwitchToNextCamera();
        }

        /// <summary>
        /// Wechselt zur nächsten Kamera in der Liste.
        /// </summary>
        private void SwitchToNextCamera()
        {
            _currentCamera.gameObject.SetActive( false );
            _currentCameraIndex     = ( _currentCameraIndex + 1 ) % cameras.Length;
            _currentCameraInstance  = cameras[ _currentCameraIndex ];
            _currentCamera          = _currentCameraInstance.GetCamera();
            _currentCameraTransform = _currentCamera.transform;
            _currentCamera.gameObject.SetActive( true );

            Debug.Log( $"CameraManager: Kamera gewechselt zu Index {_currentCameraIndex}.", gameObject );
        }

        private void FollowTarget()
        {
            if ( !target ) return;

            transform.position = target.position;

            if ( _currentCameraInstance.IsStatic() ) return;

            float   laziness        = _currentCameraInstance.Laziness;
            Vector3 desiredPosition = new( transform.position.x, transform.position.y, _currentCameraTransform.position.z );

            _currentCameraTransform.position = Vector3.Lerp( _currentCameraTransform.position, desiredPosition, laziness * Time.deltaTime );
        }

        private void LookAtTarget()
        {
            if ( !target || _currentCameraInstance.IsRotationLocked() ) return;
            _currentCameraTransform.LookAt( target );
        }
    }
}

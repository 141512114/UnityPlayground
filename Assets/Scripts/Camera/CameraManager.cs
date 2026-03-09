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
        [Tooltip( "Liste aller Kameras, die von diesem Manager verwaltet werden." )]
        public CameraInstance[] cameras;

        /// <summary>Das Ziel, das die Kamera verfolgen soll (z.B. der Spieler).</summary>
        [Label( "Ziel", "Der Transform, den die Kamera verfolgen soll (typischerweise der Spieler)." )]
        public Transform target;

        [SerializeField, Label( "Vertikaler Abstand", "Der Abstand, den die Kamera vertikal zum Ziel halten soll." )]
        private float verticalDistance = .5f;

        [SerializeField, Label( "Horizontaler Abstand", "Der Abstand, den die Kamera seitlich zum Ziel halten soll." )]
        private float horizontalDistance = -2f;

        private CameraInstance     _currentCameraInstance;
        private int                _currentCameraIndex;
        private UnityEngine.Camera _currentCamera;
        private Transform          _currentCameraTransform;

        private void Awake()
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

        private void LateUpdate()
        {
            FollowTarget();
            LookAtTarget();
        }

        /// <summary>
        /// Initialisiert die Kameras und setzt die aktuelle Kamera.
        /// </summary>
        private void InitializeCameras()
        {
            // Standardmäßig die erste Kamera in der Liste als aktive Kamera setzen, es sei denn, eine andere Kamera ist als Hauptkamera markiert.
            _currentCameraIndex = 0;

            for ( int i = 0; i < cameras.Length; i++ )
            {
                if ( !cameras[ i ].IsMainCamera() ) continue;
                _currentCameraIndex = i;
                break;
            }

            // Aktiviere die aktuelle Kamera und deaktiviere alle anderen Kameras, um sicherzustellen, dass nur eine Kamera aktiv ist.
            _currentCameraInstance = cameras[ _currentCameraIndex ];
            _currentCamera         = _currentCameraInstance?.Camera;
            if ( _currentCameraInstance == null || _currentCamera == null )
            {
                Debug.LogError( "CameraManager: Die aktuelle Kamera-Instanz oder die Kamera-Komponente ist null. Bitte überprüfen Sie die Kamerakonfiguration.", gameObject );
                enabled = false;
                return;
            }

            _currentCameraTransform = _currentCamera.transform;
            _currentCamera.gameObject.SetActive( true );

            // Deaktiviere alle anderen Kameras außer der aktuellen, um sicherzustellen, dass nur eine Kamera aktiv ist.
            for ( int i = 0; i < cameras.Length; i++ )
            {
                if ( i == _currentCameraIndex ) continue;

                var cam = cameras[ i ]?.Camera;
                if ( cam != null ) cam.gameObject.SetActive( false );
            }

            SetupInitialCameraPosition();
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
            _currentCamera          = _currentCameraInstance.Camera;
            _currentCameraTransform = _currentCamera.transform;
            _currentCamera.gameObject.SetActive( true );
        }

        private void FollowTarget()
        {
            if ( !target || !_currentCameraInstance || !_currentCameraTransform )
                return;

            if ( _currentCameraInstance.IsStatic() )
                return;

            transform.position = new Vector3( target.position.x, target.position.y + verticalDistance, target.position.z + horizontalDistance );

            float laziness = _currentCameraInstance.Laziness;
            _currentCameraTransform.position = Vector3.Lerp( _currentCameraTransform.position, transform.position, laziness * Time.deltaTime );
        }

        private void LookAtTarget()
        {
            if ( !target || !_currentCameraTransform || !_currentCameraInstance )
                return;

            if ( _currentCameraInstance.IsRotationLocked() )
                return;

            _currentCameraTransform.LookAt( target );
        }
    }
}

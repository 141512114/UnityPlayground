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

        private int                _currentCameraIndex;
        private UnityEngine.Camera _currentCamera;

        public CameraInstance CurrentCameraInstance { get; private set; }

        private void Awake()
        {
            if ( cameras == null || cameras.Length == 0 )
            {
                Debug.LogError( "CameraManager: Keine Kameras zugewiesen. Bitte mindestens eine Kamera hinzufügen.", gameObject );
                enabled = false;
                return;
            }

            InitializeCameras();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible   = false;
        }

        private void Update()
        {
            if ( Input.GetKeyDown( KeyCode.Escape ) )
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible   = true;
            }

            HandleCameraSwitching();
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
                if ( !cameras[ i ].IsMain() ) continue;
                _currentCameraIndex = i;
                break;
            }

            // Aktiviere die aktuelle Kamera und deaktiviere alle anderen Kameras, um sicherzustellen, dass nur eine Kamera aktiv ist.
            CurrentCameraInstance = cameras[ _currentCameraIndex ];
            _currentCamera        = CurrentCameraInstance?.Camera;
            if ( !CurrentCameraInstance || !_currentCamera )
            {
                Debug.LogError( "CameraManager: Die aktuelle Kamera-Instanz oder die Kamera-Komponente ist null. Bitte überprüfen Sie die Kamerakonfiguration.", gameObject );
                enabled = false;
                return;
            }

            _currentCamera.gameObject.SetActive( true );

            // Deaktiviere alle anderen Kameras außer der aktuellen, um sicherzustellen, dass nur eine Kamera aktiv ist.
            for ( int i = 0; i < cameras.Length; i++ )
            {
                if ( i == _currentCameraIndex ) continue;

                var cam = cameras[ i ]?.Camera;
                if ( cam != null ) cam.gameObject.SetActive( false );
            }
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
            _currentCameraIndex   = ( _currentCameraIndex + 1 ) % cameras.Length;
            CurrentCameraInstance = cameras[ _currentCameraIndex ];
            _currentCamera        = CurrentCameraInstance.Camera;
            _currentCamera.gameObject.SetActive( true );
        }
    }
}

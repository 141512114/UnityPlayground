using UnityEngine;

namespace Camera
{
    /// <summary>
    /// Verwaltet mehrere Kameras in der Szene, ermöglicht das Umschalten zwischen ihnen
    /// und steuert die Kamerabewegung und -rotation basierend auf einem Ziel (z.B. Spieler).
    /// </summary>
    public class CameraManager : MonoBehaviour
    {
        public CameraInstance[] cameras;
        public Transform        target;

        private CameraInstance     _currentCameraInstance;
        private int                _currentCameraIndex;
        private UnityEngine.Camera _currentCamera;

        private void Start()
        {
            if ( cameras.Length == 0 )
            {
                Debug.LogError( "No cameras assigned to CameraManager." );
                enabled = false;
                return;
            }

            // Finde die aktive Kamera (erste oder Hauptkamera)
            _currentCameraIndex = 0;
            for ( int i = 0; i < cameras.Length; i++ )
            {
                if ( !cameras[ i ].IsMainCamera() ) continue;
                _currentCameraIndex = i;
                break;
            }

            // Setze die aktuelle Kamera
            _currentCameraInstance = cameras[ _currentCameraIndex ];
            _currentCamera         = _currentCameraInstance.GetCamera();

            // Aktiviere nur die aktuelle Kamera und deaktiviere die anderen
            for ( int i = 0; i < cameras.Length; i++ ) { cameras[ i ].GetCamera().gameObject.SetActive( i == _currentCameraIndex ); }

            // Initialisiere Kamera-Position
            FollowTarget();
            LookAtTarget();
        }

        private void Update()
        {
            // Wechsle die Kamera mit der Tab-Taste
            if ( !Input.GetKeyDown( KeyCode.Tab ) ) return;

            // Deaktiviere die aktuelle Kamera und aktiviere die nächste
            _currentCamera.gameObject.SetActive( false );
            _currentCameraIndex    = ( _currentCameraIndex + 1 ) % cameras.Length;
            _currentCameraInstance = cameras[ _currentCameraIndex ];
            _currentCamera         = _currentCameraInstance.GetCamera();
            _currentCamera.gameObject.SetActive( true );
        }

        private void FixedUpdate()
        {
            FollowTarget();
            LookAtTarget();
        }

        private void FollowTarget()
        {
            if ( !target ) return;

            transform.position = target.position;

            // Wenn die Kamera statisch ist, soll sie sich nicht bewegen
            if ( _currentCameraInstance.IsStatic() ) return;

            float   laziness        = _currentCameraInstance.Laziness;
            Vector3 desiredPosition = new( transform.position.x, transform.position.y, _currentCamera.transform.position.z );
            // Bewege die Kamera sanft zur gewünschten Position
            _currentCamera.transform.position = Vector3.Lerp( _currentCamera.transform.position, desiredPosition, laziness * Time.deltaTime );
        }

        private void LookAtTarget()
        {
            if ( !target ) return;

            // Wenn die Kamerarotation gesperrt ist, soll sie sich nicht drehen
            if ( _currentCameraInstance.IsRotationLocked() ) return;

            _currentCamera.transform.LookAt( target );
        }
    }
}

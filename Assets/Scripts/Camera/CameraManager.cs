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

        [Header( "Kameraeinstellungen" )]
        [SerializeField, Label( "Vertikaler Abstand", "Der Abstand, den die Kamera vertikal zum Ziel halten soll." )]
        private float verticalDistance = .5f;

        [SerializeField, Label( "Horizontaler Abstand", "Der Abstand, den die Kamera seitlich zum Ziel halten soll." )]
        private float horizontalDistance = -2f;

        [SerializeField, Label( "Minimale Kameradistanz", "Der minimale Abstand, den die Kamera zum Ziel haben darf, um Kollisionen zu vermeiden." )]
        private float minDistance = 1f;

        [Header( "Maussteuerung" )]
        [Label( "Maussensibilität" )]
        public float sensitivity = 3f;

        [Label( "Kamerakollisionsradius", "Der Radius der Kugel, die für die Kollisionsprüfung verwendet wird, um zu verhindern, dass die Kamera durch Wände geht." )]
        public float cameraRadius = 0.3f;

        private int                _currentCameraIndex;
        private UnityEngine.Camera _currentCamera;
        private Transform          _currentCameraTransform;
        
        public CameraInstance CurrentCameraInstance { get; private set; }

        private float   _yaw;
        private float   _pitch;
        private Vector3 _cameraVelocity;

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
            RotateWithMouse();
        }

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
            CurrentCameraInstance = cameras[ _currentCameraIndex ];
            _currentCamera         = CurrentCameraInstance?.Camera;
            if ( CurrentCameraInstance == null || _currentCamera == null )
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
            CurrentCameraInstance  = cameras[ _currentCameraIndex ];
            _currentCamera          = CurrentCameraInstance.Camera;
            _currentCameraTransform = _currentCamera.transform;
            _currentCamera.gameObject.SetActive( true );
        }

        private void FollowTarget()
        {
            if ( !target || !CurrentCameraInstance || !_currentCameraTransform )
                return;

            if ( CurrentCameraInstance.IsStatic() )
                return;

            // Wenn Mausrotation aktiv ist, übernimmt RotateWithMouse die Position
            if ( CurrentCameraInstance.RotateWithMouse() )
                return;

            float smoothY = Mathf.Lerp( _currentCameraTransform.position.y, target.position.y + verticalDistance, 5f * Time.deltaTime );
            transform.position = new Vector3( target.position.x, smoothY, target.position.z + horizontalDistance );

            _currentCameraTransform.position =
                Vector3.SmoothDamp( _currentCameraTransform.position, transform.position, ref _cameraVelocity, CurrentCameraInstance.Laziness * Time.deltaTime );
        }

        private void LookAtTarget()
        {
            if ( !target || !_currentCameraTransform || !CurrentCameraInstance )
                return;

            if ( CurrentCameraInstance.IsRotationLocked() )
                return;

            LookAtSlerp();
        }

        /// <summary>
        /// Slerpt die Rotation der Kamera, um sanft auf das Ziel zu schauen,
        /// basierend auf der "Laziness"-Einstellung der Kamera. Je höher die Laziness, desto langsamer dreht sich die Kamera zum Ziel.
        /// </summary>
        private void LookAtSlerp()
        {
            Quaternion targetRotation = Quaternion.LookRotation( target.position - _currentCameraTransform.position );
            _currentCameraTransform.rotation =
                Quaternion.Slerp( _currentCameraTransform.rotation, targetRotation, CurrentCameraInstance.Laziness * 10 * Time.deltaTime );
        }

        private void RotateWithMouse()
        {
            if ( !CurrentCameraInstance.RotateWithMouse() )
                return;

            float mouseX = Input.GetAxis( "Mouse X" ) * sensitivity;
            float mouseY = Input.GetAxis( "Mouse Y" ) * sensitivity;

            _yaw   += mouseX;
            _pitch -= mouseY;
            _pitch =  Mathf.Clamp( _pitch, -40f, 80f );

            Quaternion rotation = Quaternion.Euler( _pitch, _yaw, 0 );

            Vector3 dir             = rotation * Vector3.back;
            Vector3 desiredPosition = target.position + dir * horizontalDistance;

            HandleCollision( target.position, desiredPosition );

            LookAtSlerp();
        }

        /// <summary>
        /// Überprüft, ob die gewünschte Kameraposition eine Kollision mit der Umgebung verursachen würde,
        /// und korrigiert die Position entsprechend, um zu verhindern, dass die Kamera durch Wände oder andere Hindernisse geht.
        /// </summary>
        /// <param name="targetPos"></param>
        /// <param name="desiredPos"></param>
        private void HandleCollision( Vector3 targetPos, Vector3 desiredPos )
        {
            Vector3 dir  = ( desiredPos - targetPos ).normalized;
            float   dist = Vector3.Distance( targetPos, desiredPos );

            if ( Physics.SphereCast( targetPos, cameraRadius, dir, out RaycastHit hit, dist ) )
            {
                float correctedDist = Mathf.Clamp( hit.distance - cameraRadius, minDistance, Mathf.Abs( horizontalDistance ) );
                _currentCameraTransform.position = targetPos + dir * correctedDist;
                return;
            }

            _currentCameraTransform.position = desiredPos;
        }
    }
}

using Attributes;
using UnityEngine;

namespace Camera
{
    [RequireComponent( typeof( CameraInstance ) )]
    public class CameraController : MonoBehaviour
    {
        /// <summary>Das Ziel, das die Kamera verfolgen soll (z.B. der Spieler).</summary>
        [Label( "Ziel", "Der Transform, den die Kamera verfolgen soll (typischerweise der Spieler)." )]
        public Transform target;

        private CameraInstance _cameraInstance;

        private Vector3 _cameraVelocity;
        private float   _yaw;
        private float   _pitch;

        private void Awake()
        {
            _cameraInstance = GetComponent< CameraInstance >();

            if ( target ) return;
            Debug.LogWarning( "CameraManager: Kein Ziel (target) zugewiesen. Die Kamera wird sich nicht bewegen.", gameObject );
            enabled = false;
        }

        private void Start()
        {
            FollowTarget();
            LookAtTarget();
        }

        private void LateUpdate()
        {
            RotateWithMouse();
            FollowTarget();
            LookAtTarget();
        }

        private void FollowTarget()
        {
            if ( !target )
                return;

            if ( _cameraInstance.IsStatic() || _cameraInstance.CanMouseRotate() )
                return;

            Vector3 desiredPosition
                = new(
                      target.position.x,
                      target.position.y + _cameraInstance.VerticalDistance,
                      target.position.z + _cameraInstance.HorizontalDistance
                     );

            transform.position
                = Vector3.SmoothDamp(
                                     transform.position,
                                     desiredPosition,
                                     ref _cameraVelocity,
                                     _cameraInstance.Laziness
                                    );
        }

        private void LookAtTarget()
        {
            if ( !target )
                return;

            if ( _cameraInstance.IsRotationLocked() )
                return;

            if ( _cameraInstance.CanMouseRotate() && _cameraInstance.IsFirstPerson() )
                return;

            LookAtSlerp();
        }

        /// <summary>
        /// Slerpt die Rotation der Kamera, um sanft auf das Ziel zu schauen,
        /// basierend auf der "Laziness"-Einstellung der Kamera. Je höher die Laziness, desto langsamer dreht sich die Kamera zum Ziel.
        /// </summary>
        private void LookAtSlerp()
        {
            Quaternion targetRotation = _cameraInstance.IsFirstPerson() ? target.rotation : Quaternion.LookRotation( target.position - transform.position );
            transform.rotation = Quaternion.Slerp( transform.rotation, targetRotation, _cameraInstance.Laziness * 10 * Time.deltaTime );
        }

        private void RotateWithMouse()
        {
            if ( !_cameraInstance.CanMouseRotate() )
                return;

            float mouseX = Input.GetAxis( "Mouse X" ) * _cameraInstance.Sensitivity;
            float mouseY = Input.GetAxis( "Mouse Y" ) * _cameraInstance.Sensitivity;

            _yaw   += mouseX;
            _pitch -= mouseY;
            _pitch =  Mathf.Clamp( _pitch, -40f, 80f );

            if ( _cameraInstance.IsFirstPerson() )
            {
                // First Person: nur Rotation

                // Yaw -> Spieler drehen
                target.transform.Rotate( Vector3.up, mouseX );

                // Pitch -> Kamera hoch/runter
                transform.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
            }
            else
            {
                // Third Person: Orbit Kamera
                Quaternion rotation = Quaternion.Euler( _pitch, _yaw, 0 );

                Vector3 dir             = rotation * Vector3.back;
                Vector3 desiredPosition = target.position + dir * _cameraInstance.HorizontalDistance;

                HandleCollision( target.position, desiredPosition );

                LookAtSlerp();
            }
        }

        /// <summary>
        /// Überprüft, ob die gewünschte Kameraposition eine Kollision mit der Umgebung verursachen würde,
        /// und korrigiert die Position entsprechend, um zu verhindern, dass die Kamera durch Wände oder andere Hindernisse geht.
        /// </summary>
        private void HandleCollision( Vector3 targetPos, Vector3 desiredPos )
        {
            Vector3 dir  = ( desiredPos - targetPos ).normalized;
            float   dist = Vector3.Distance( targetPos, desiredPos );

            float   offset      = _cameraInstance.CameraRadius;
            Vector3 rayOrigin   = targetPos + dir * offset;
            float   rayDistance = Mathf.Max( 0f, dist - offset );

            if ( rayDistance > 0f && Physics.SphereCast( rayOrigin, _cameraInstance.CameraRadius, dir, out RaycastHit hit, rayDistance ) )
            {
                float correctedDist = Mathf.Clamp( hit.distance, _cameraInstance.MinDistance, Mathf.Abs( _cameraInstance.HorizontalDistance ) );
                transform.position = targetPos + dir * correctedDist;
                return;
            }

            transform.position = desiredPos;
        }
    }
}

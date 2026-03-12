using Attributes;
using UnityEngine;

namespace Camera
{
    /// <summary>
    /// Repräsentiert eine konkrete Kamera im Spiel, die auf einem CameraProfile basiert und von einem CameraManager verwaltet wird.
    /// </summary>
    [RequireComponent( typeof( UnityEngine.Camera ) )]
    public class CameraInstance : MonoBehaviour
    {
        [Header( "Einstellungen" )]
        [SerializeField, Label( "Hauptkamera" )]
        private bool isMainCamera;

        [SerializeField, Label( "First-Person Kamera" )]
        private bool isFirstPerson;

        [SerializeField, Label( "Mausrotation erlauben" )]
        private bool rotateWithMouse;

        [Header( "Transform-Einstellungen" )]
        [SerializeField, Label( "Position sperren" )]
        private bool lockPosition;

        [SerializeField, Label( "Rotation sperren" )]
        private bool lockRotation;

        [SerializeField, Label( "Trägheit" ), Range( 0f, 1f )]
        private float laziness = .5f;

        [Header( "Abstände" )]
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

        public bool  IsMain()           => isMainCamera;
        public bool  IsFirstPerson()    => isFirstPerson;
        public bool  CanMouseRotate()   => rotateWithMouse;
        public bool  IsStatic()         => lockPosition;
        public bool  IsRotationLocked() => lockRotation;
        public float Laziness           => laziness;
        public float VerticalDistance   => verticalDistance;
        public float HorizontalDistance => horizontalDistance;
        public float MinDistance        => minDistance;
        public float Sensitivity        => sensitivity;
        public float CameraRadius       => cameraRadius;

        private UnityEngine.Camera _camera;

        public UnityEngine.Camera Camera
        {
            get
            {
                if ( !_camera ) _camera = GetComponent< UnityEngine.Camera >();

                return _camera;
            }
        }
    }
}

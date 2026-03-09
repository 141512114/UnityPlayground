using Attributes;
using UnityEngine;

namespace Camera
{
    /// <summary>
    /// Definiert die Eigenschaften und Einstellungen einer Kamera, die von einem CameraManager verwendet werden können, um die Kameralogik zu steuern.
    /// </summary>
    [CreateAssetMenu( fileName = "CameraProfile", menuName = "Camera/Camera Profile" )]
    public class CameraProfile : ScriptableObject
    {
        [SerializeField, Label( "Hauptkamera" )]
        private bool isMainCamera;

        [SerializeField, Label( "Mausrotation erlauben" )]
        private bool rotateWithMouse;

        [SerializeField, Label( "Position sperren" )]
        private bool lockPosition;

        [SerializeField, Label( "Rotation sperren" )]
        private bool lockRotation;

        [SerializeField, Label( "Trägheit" ), Range( 0f, 1f )]
        private float laziness = .5f;

        public bool  IsMain()           => isMainCamera;
        public bool  RotateWithMouse()  => rotateWithMouse;
        public bool  IsStatic()         => lockPosition;
        public bool  IsRotationLocked() => lockRotation;
        public float Laziness           => laziness;
    }
}

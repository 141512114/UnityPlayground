using UnityEngine;

namespace Camera
{
    /// <summary>
    /// Definiert die Eigenschaften und Einstellungen einer Kamera, die von einem CameraManager verwendet werden können, um die Kameralogik zu steuern.
    /// </summary>
    [CreateAssetMenu( fileName = "CameraProfile", menuName = "Camera/Camera Profile" )]
    public class CameraProfile : ScriptableObject
    {
        [SerializeField] private bool isMainCamera;
        [SerializeField] private bool rotateWithMouse;
        [SerializeField] private bool lockPosition;
        [SerializeField] private bool lockRotation;

        [SerializeField] private float laziness = 5f;

        public bool  IsMain()           => isMainCamera;
        public bool  RotateWithMouse()  => rotateWithMouse;
        public bool  IsStatic()         => lockPosition;
        public bool  IsRotationLocked() => lockRotation;
        public float Laziness           => laziness;
    }
}

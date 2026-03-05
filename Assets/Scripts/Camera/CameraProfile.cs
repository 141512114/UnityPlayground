using UnityEngine;

namespace Camera
{
    [CreateAssetMenu( fileName = "CameraProfile", menuName = "Camera/Camera Profile" )]
    public class CameraProfile : ScriptableObject
    {
        [SerializeField] private bool isMainCamera;
        [SerializeField] private bool lockPosition;
        [SerializeField] private bool lockRotation;

        [SerializeField] private float laziness = 5f;

        public bool  IsMain()           => isMainCamera;
        public bool  IsStatic()         => lockPosition;
        public bool  IsRotationLocked() => lockRotation;
        public float Laziness           => laziness;
    }
}

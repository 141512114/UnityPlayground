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
        [SerializeField, Label( "Profil" )] private CameraProfile profile;

        private UnityEngine.Camera _camera;

        public UnityEngine.Camera Camera
        {
            get
            {
                if ( !_camera ) _camera = GetComponent< UnityEngine.Camera >();

                return _camera;
            }
        }

        public bool  IsMainCamera()     => profile && profile.IsMain();
        public bool  RotateWithMouse()  => profile && profile.RotateWithMouse();
        public bool  IsStatic()         => profile && profile.IsStatic();
        public bool  IsRotationLocked() => profile && profile.IsRotationLocked();
        public float Laziness           => profile ? profile.Laziness : 0f;
    }
}

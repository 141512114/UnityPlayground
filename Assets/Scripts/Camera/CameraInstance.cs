using UnityEngine;

namespace Camera
{
    /// <summary>
    /// Repräsentiert eine konkrete Kamera im Spiel, die auf einem CameraProfile basiert und von einem CameraManager verwaltet wird.
    /// </summary>
    [RequireComponent( typeof( UnityEngine.Camera ) )]
    public class CameraInstance : MonoBehaviour
    {
        [SerializeField] private CameraProfile profile;

        private UnityEngine.Camera _camera;

        private void Start()
        {
            _camera = GetComponent< UnityEngine.Camera >();

            if ( profile != null ) return;
            Debug.LogError( $"CameraInstance on {gameObject.name} has no CameraProfile assigned!", this );
            enabled = false;
        }

        public UnityEngine.Camera GetCamera()        => _camera;
        public bool               IsMainCamera()     => profile && profile.IsMain();
        public bool               IsStatic()         => profile && profile.IsStatic();
        public bool               IsRotationLocked() => profile && profile.IsRotationLocked();
        public float              Laziness           => profile ? profile.Laziness : 0f;
    }
}

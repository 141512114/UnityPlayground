using UnityEngine;

public class CameraInstance : MonoBehaviour
{
    [SerializeField] private CameraProfile profile;

    private Camera _camera;

    private void Start()
    {
        _camera = GetComponent<Camera>();
        
        if (profile == null)
        {
            Debug.LogError($"CameraInstance on {gameObject.name} has no CameraProfile assigned!", this);
        }
    }

    public Camera GetCamera() => _camera;
    public bool IsMainCamera() => profile && profile.IsMain();
    public bool IsStatic() => profile && profile.IsStatic();
}

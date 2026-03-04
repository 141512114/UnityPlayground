using UnityEngine;

public class CameraInstance : MonoBehaviour
{
    [SerializeField] private bool isMainCamera;
    [SerializeField] private bool isStatic;

    private Camera _camera;

    private void Start() { _camera = GetComponent< Camera >(); }

    public Camera GetCamera()    { return _camera; }
    public bool   IsMainCamera() { return isMainCamera; }
    public bool   IsStatic()     { return isStatic; }
}

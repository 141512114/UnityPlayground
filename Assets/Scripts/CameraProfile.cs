using UnityEngine;

[CreateAssetMenu( fileName = "CameraProfile", menuName = "Camera/Camera Profile" )]
public class CameraProfile : ScriptableObject
{
    [SerializeField] private bool   isMainCamera;
    [SerializeField] private bool   isStatic;

    public bool   IsMain()    => isMainCamera;
    public bool   IsStatic()  => isStatic;
}

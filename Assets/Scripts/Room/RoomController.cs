using UnityEngine;

public class RoomController : MonoBehaviour
{
    [SerializeField] private RoomProfile roomProfile;

    private void Start()
    {
        if ( roomProfile == null )
        {
            Debug.LogError( "RoomController requires a RoomProfile reference." );
            enabled = false;
            return;
        }
    }
}

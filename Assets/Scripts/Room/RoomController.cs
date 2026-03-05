using UnityEngine;

namespace Room
{
    public class RoomController : MonoBehaviour
    {
        [SerializeField] private RoomProfile roomProfile;

        private void Start()
        {
            if ( roomProfile != null ) return;
            Debug.LogError( "RoomController requires a RoomProfile reference." );
            enabled = false;
        }
    }
}

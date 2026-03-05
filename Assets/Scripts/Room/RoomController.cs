using UnityEngine;

namespace Room
{
    /// <summary>
    /// Verwaltet die Logik und Interaktionen innerhalb eines Raums, basierend auf einem RoomProfile.
    /// </summary>
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

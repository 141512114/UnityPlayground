using Room;
using UnityEngine;

namespace Scene
{
    public class SceneController : MonoBehaviour
    {
        private RoomLoader _loader;

        private void Start()
        {
            LoadRooms();
        }

        /// <summary>
        /// Lädt Räume basierend auf den Einstellungen der RoomLoader-Komponente, die auf demselben GameObject vorhanden sein muss.
        /// </summary>
        private void LoadRooms()
        {
            GetRoomLoader();
            _loader.LoadRooms();
        }
        
        /// <summary>
        /// Sucht nach einem RoomLoader-Komponente auf demselben GameObject und speichert eine Referenz darauf.
        /// </summary>
        private void GetRoomLoader()
        {
            if ( _loader != null ) return;
            _loader = GetComponent< RoomLoader >();
            if ( _loader != null ) return;
            Debug.LogError( "SceneController requires a RoomLoader component on the same GameObject." );
            enabled = false;
        }
    }
}

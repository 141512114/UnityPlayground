using Room;
using UnityEngine;

namespace Scene
{
    /// <summary>
    /// Kontrolliert die Szene, indem er Räume basierend auf den Einstellungen der RoomLoader-Komponente lädt, die auf demselben GameObject vorhanden sein muss.
    /// Stellt sicher, dass die Räume korrekt geladen werden, wenn die Szene gestartet wird.
    /// </summary>
    public class SceneController : MonoBehaviour
    {
        private RoomLoader _loader;

        private void Start() { LoadRooms(); }

        private void Update()
        {
            // Ermöglicht das Neuladen der Räume durch Drücken der R-Taste.
            // Diese Funktion kann sehr ressourcenintensiv sein, wenn viele Räume geladen sind, daher sollte sie mit Vorsicht verwendet werden.
            if ( !Input.GetKeyDown( KeyCode.R ) ) return;
            TrashRooms();
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

        /// <summary>
        /// Zerstört alle Räume, die von der RoomLoader-Komponente geladen wurden.
        /// </summary>
        private void TrashRooms()
        {
            GetRoomLoader();
            foreach ( Transform child in _loader.LoadPoints ) { Destroy( child.GetChild( 0 ).gameObject ); }
        }
    }
}

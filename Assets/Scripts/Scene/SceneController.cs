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

        private int          _chosenRoomIndex;
        private RoomInstance _chosenRoom;

        private void Start() { InitScene(); }

        private void Update()
        {
            // Ermöglicht das Neuladen der Räume durch Drücken der R-Taste.
            // Diese Funktion kann sehr ressourcenintensiv sein, wenn viele Räume geladen sind, daher sollte sie mit Vorsicht verwendet werden.
            if ( !Input.GetKeyDown( KeyCode.R ) ) return;
            InitScene();
        }

        /// <summary>
        /// Initialisiert die Szene, indem er alle Räume zerstört, die von der RoomLoader-Komponente geladen wurden,
        /// und dann neue Räume basierend auf den Einstellungen der RoomLoader-Komponente lädt.
        /// </summary>
        private void InitScene()
        {
            TrashRooms();
            if ( !LoadRooms() ) return;
            ActivateAbnormality();
        }

        /// <summary>
        /// Lädt Räume basierend auf den Einstellungen der RoomLoader-Komponente, die auf demselben GameObject vorhanden sein muss.
        /// </summary>
        private bool LoadRooms()
        {
            // Lade Räume über die RoomLoader-Komponente.
            GetRoomLoader();
            return _loader.LoadRooms();
        }

        /// <summary>
        /// Wählt zufällig einen Raum aus der Liste der geladenen Räume des RoomLoader aus
        /// und aktiviert die Abnormalität in diesem Raum, damit der Spieler sie finden muss.
        /// </summary>
        private void ActivateAbnormality()
        {
            // Wähle zufällig einen Raum aus der Liste der geladenen Räume des RoomLoader aus und speichere die Referenz auf den ausgewählten Raum und seinen Index.
            // Das wird der Raum, welcher die Abnormalität enthält, die der Spieler finden muss.
            ChooseRandomRoom();
            if ( !_chosenRoom ) return;
            _chosenRoom.SetAbnormality( true );
        }

        /// <summary>
        /// Sucht nach einem RoomLoader-Komponente auf demselben GameObject und speichert eine Referenz darauf.
        /// </summary>
        private void GetRoomLoader()
        {
            if ( _loader ) return;
            _loader = GetComponent< RoomLoader >();
            if ( _loader ) return;
            Debug.LogError( "SceneController requires a RoomLoader component on the same GameObject." );
            enabled = false;
        }

        /// <summary>
        /// Zerstört alle Räume, die von der RoomLoader-Komponente geladen wurden.
        /// </summary>
        private void TrashRooms()
        {
            GetRoomLoader();
            if ( _loader.LoadedRooms is not { Count: > 0 } ) return;
            foreach ( Transform child in _loader.LoadPoints ) { Destroy( child.GetChild( 0 ).gameObject ); }

            _loader.LoadedRooms.Clear();
        }

        /// <summary>
        /// Wählt zufällig einen Raum aus der Liste der geladenen Räume des RoomLoader aus und speichert die Referenz auf den ausgewählten Raum und seinen Index.
        /// </summary>
        private void ChooseRandomRoom()
        {
            GetRoomLoader();

            int roomIndex = Random.Range( 0, _loader.LoadedRooms.Count );

            _chosenRoomIndex = roomIndex;
            _chosenRoom      = _loader.LoadedRooms[ _chosenRoomIndex ];
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

namespace Room
{
    /// <summary>
    /// Erlaubt das Laden von Räumen aus RoomDatabases auf vordefinierte Load Points.
    /// </summary>
    public class RoomLoader : MonoBehaviour
    {
        [SerializeField] private List< RoomDatabase > roomDatabases;
        [SerializeField] private List< Transform >    loadPoints;

        public List< Transform > LoadPoints => loadPoints;

        public void LoadRooms()
        {
            if ( roomDatabases == null || roomDatabases.Count == 0 )
            {
                Debug.LogWarning( "RoomLoader: Es gibt keine Raumdatenbank." );
                enabled = false;
                return;
            }

            for ( int databaseIndex = 0; databaseIndex < roomDatabases.Count; databaseIndex++ ) { LoadRoomsFromDatabase( roomDatabases[ databaseIndex ], databaseIndex ); }
        }

        private void LoadRoomsFromDatabase( RoomDatabase database, int databaseIndex )
        {
            if ( database?.Rooms == null || database.Rooms.Count == 0 )
            {
                Debug.LogWarning( $"RoomLoader: Raumdatenbank mit Index {databaseIndex} besitzt keine Räume." );
                return;
            }

            int[] visitedRooms = new int[ database.Rooms.Count ];

            foreach ( Transform loadPoint in loadPoints )
            {
                // Wähle einen zufälligen Raum aus der Datenbank, der noch nicht besucht wurde
                int roomIndex = Random.Range( 0,                                        database.Rooms.Count );
                while ( visitedRooms[ roomIndex ] == 1 ) { roomIndex = Random.Range( 0, database.Rooms.Count ); }

                GameObject roomPrefab = roomDatabases[ databaseIndex ].Rooms[ roomIndex ];

                if ( !roomPrefab ) return;

                // Instanziiere den Raum an der Position und Rotation des Load Points
                Vector3    position = !loadPoint ? Vector3.zero : loadPoint.position;
                Quaternion rotation = !loadPoint ? Quaternion.identity : loadPoint.rotation;

                GameObject room = Instantiate( roomPrefab, position, rotation );
                room.transform.parent = loadPoint;

                visitedRooms[ roomIndex ] = 1;
            }
        }
    }
}

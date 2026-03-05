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

        public void LoadRooms()
        {
            if ( roomDatabases == null || roomDatabases.Count == 0 )
            {
                Debug.LogWarning( "No room databases assigned to RoomLoader." );
                enabled = false;
                return;
            }

            for ( int databaseIndex = 0; databaseIndex < roomDatabases.Count; databaseIndex++ ) { LoadRoomsFromDatabase( roomDatabases[ databaseIndex ], databaseIndex ); }
        }

        private void LoadRoomsFromDatabase( RoomDatabase database, int databaseIndex )
        {
            if ( database?.Rooms == null || database.Rooms.Count == 0 )
            {
                Debug.LogWarning( $"RoomDatabase at index {databaseIndex} has no rooms assigned." );
                return;
            }

            for ( int roomIndex = 0; roomIndex < database.Rooms.Count; roomIndex++ ) { LoadRoom( database.Rooms[ roomIndex ], roomIndex, databaseIndex ); }
        }

        private void LoadRoom( GameObject roomPrefab, int roomIndex, int databaseIndex )
        {
            if ( roomPrefab == null )
            {
                Debug.LogWarning( $"Room prefab at index {roomIndex} in database {databaseIndex} is null." );
                return;
            }

            Transform loadPoint = GetLoadPoint( roomIndex );
            if ( loadPoint == null )
            {
                Debug.LogWarning( $"No load point assigned for room {roomIndex} in database {databaseIndex}. Using world origin." );
                Instantiate( roomPrefab, Vector3.zero, Quaternion.identity );
            }
            else { Instantiate( roomPrefab, loadPoint.position, Quaternion.identity ); }
        }

        private Transform GetLoadPoint( int index ) { return index < loadPoints.Count ? loadPoints[ index ] : null; }
    }
}

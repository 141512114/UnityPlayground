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

            for ( int roomIndex = 0; roomIndex < database.Rooms.Count; roomIndex++ ) { LoadRoom( database.Rooms[ roomIndex ], roomIndex ); }
        }

        private void LoadRoom( GameObject roomPrefab, int roomIndex )
        {
            if ( roomPrefab == null ) return;

            Transform  loadPoint = GetLoadPoint( roomIndex );
            
            Vector3    position  = loadPoint   == null ? Vector3.zero : loadPoint.position;
            Quaternion rotation  = loadPoint   == null ? Quaternion.identity : loadPoint.rotation;
            
            GameObject room = Instantiate( roomPrefab, position, rotation );
            room.transform.parent = loadPoint;
        }

        private Transform GetLoadPoint( int index ) { return index < loadPoints.Count ? loadPoints[ index ] : null; }
    }
}

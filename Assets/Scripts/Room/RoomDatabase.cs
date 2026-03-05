using System.Collections.Generic;
using UnityEngine;

namespace Room
{
    /// <summary>
    /// Dient als Datenbank für Raum-Prefabs, die von einem RoomLoader geladen werden können.
    /// Ermöglicht die zentrale Verwaltung von Raumressourcen und erleichtert die Zuweisung von Räumen zu Load Points im RoomLoader.
    /// </summary>
    [CreateAssetMenu( fileName = "Room Database", menuName = "Room/Room Database" )]
    public class RoomDatabase : ScriptableObject
    {
        [SerializeField] private List< GameObject > rooms;
        public List< GameObject > Rooms      => rooms;
    }
}

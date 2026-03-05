using System.Collections.Generic;
using UnityEngine;

namespace Room
{
    [CreateAssetMenu( fileName = "Room Database", menuName = "Room/Room Database" )]
    public class RoomDatabase : ScriptableObject
    {
        [SerializeField] private List< GameObject > rooms;
        public List< GameObject > Rooms      => rooms;
    }
}

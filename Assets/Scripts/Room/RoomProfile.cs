using Attributes;
using UnityEngine;

namespace Room
{
    /// <summary>
    /// Definiert die Eigenschaften und Einstellungen eines Raums, die von einem RoomController verwendet werden können, um die Raumlogik zu steuern.
    /// </summary>
    [CreateAssetMenu( fileName = "RoomProfile", menuName = "Room/Room Profile" )]
    public class RoomProfile : ScriptableObject
    {
        [Header( "Allgemein" )] public new string name;

        [Label( "Beschreibung" ), TextArea]
        public string description;

        [Header( "Eigenschaften" )]
        [Label( "Raumfarbe" )]
        public Color roomColor = Color.white;

        // Weitere Eigenschaften können hier hinzugefügt werden, z.B.:
        // - Gegner-Typen und -Anzahl
        // - Belohnungen
        // - Musik oder Soundeffekte
    }
}

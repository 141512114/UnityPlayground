using Attributes;
using UnityEngine;

namespace Room
{
    /// <summary>
    /// Verwaltet die Logik und Interaktionen innerhalb eines Raums, basierend auf einem RoomProfile.
    /// </summary>
    public class RoomController : MonoBehaviour
    {
        [SerializeField, Label( "Profil" )] private RoomProfile roomProfile;

        private void Start()
        {
            if ( roomProfile == null )
            {
                Debug.LogError( "RoomController benötigt ein Raumprofil." );
                enabled = false;
                return;
            }

            ApplyColorToRoom();
        }

        /// <summary>
        /// Färbt alle MeshRenderer im Raum mit der Farbe, die im RoomProfile definiert ist.
        /// Dies ist ein einfaches Beispiel dafür, wie das RoomProfile verwendet werden kann, um die visuelle Darstellung des Raums zu steuern.
        /// </summary>
        private void ApplyColorToRoom()
        {
            // Lade alle Kinder des Raums, welche einen MeshRenderer haben, und setze deren Farbe basierend auf dem RoomProfile
            var renderers = GetComponentsInChildren< MeshRenderer >();
            foreach ( MeshRenderer renderer in renderers ) { renderer.material.color = roomProfile.roomColor; }
        }
    }
}

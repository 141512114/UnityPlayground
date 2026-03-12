using UnityEngine;

namespace Room
{
    /// <summary>
    /// Definiert eine Instanz eines Raums, die von einem RoomLoader geladen wird.
    /// Diese Klasse speichert Informationen über den Raum, wie z.B. ob er eine Abnormalität enthält, die der Spieler finden muss.
    /// </summary>
    [RequireComponent( typeof( RoomController ) )]
    public class RoomInstance : MonoBehaviour
    {
        private bool                    _hasAbnormality;
        private AbnormalityInstance[]   _abnormalities;

        private void Awake()
        {
            LoadAbnormalities();
            ActivateAbnormality();
        }

        /// <summary>
        /// Lädt alle AbnormalityInstance-Komponenten, die sich in diesem Raum befinden, und speichert sie in einem Array.
        /// </summary>
        private void LoadAbnormalities()
        {
            _abnormalities = gameObject.GetComponentsInChildren< AbnormalityInstance >( true );
        }

        /// <summary>
        /// Setzt den Wert von _hasAbnormality und aktiviert oder deaktiviert die Abnormalität(en) in diesem Raum basierend auf dem neuen Wert.
        /// </summary>
        public void SetAbnormality( bool value )
        {
            _hasAbnormality = value;
            LoadAbnormalities();
            ActivateAbnormality();
        }

        /// <summary>
        /// Aktiviert oder deaktiviert die Abnormalität(en) in diesem Raum basierend auf dem Wert von _hasAbnormality.
        /// </summary>
        private void ActivateAbnormality()
        {
            if ( _abnormalities is not { Length: > 0 } ) return;

            foreach ( AbnormalityInstance instance in _abnormalities )
            {
                if ( _hasAbnormality ) instance.Show();
                else instance.Hide();
            }
        }
    }
}

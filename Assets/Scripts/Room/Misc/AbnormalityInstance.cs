using Attributes;
using UnityEngine;

namespace Room.Misc
{
    /// <summary>
    /// Erlaubt die Steuerung von Abnormalitäten in einem Raum,
    /// indem er auf Trigger-Ereignisse reagiert und die Sichtbarkeit der Abnormalität basierend auf den Anweisungen des RoomInstance steuert.
    /// </summary>
    public class AbnormalityInstance : MonoBehaviour
    {
        [SerializeField, Label( "Rauminhalt", "Das Wrapper-Element, welches den Rauminhalt enthält." )]
        private GameObject wrapper;

        public void Show() { wrapper.SetActive( true ); }

        public void Hide() { wrapper.SetActive( false ); }
    }
}

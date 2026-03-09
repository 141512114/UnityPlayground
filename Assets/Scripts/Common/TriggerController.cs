using UnityEngine;

namespace Common
{
    /// <summary>
    /// Steuert die Interaktion mit Triggern in einem Raum, z.B. das Öffnen von Türen, wenn der Spieler einen Trigger betritt.
    /// </summary>
    public class TriggerController : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour actioner;

        private IActioner Actioner => actioner as IActioner;

        void OnTriggerEnter( Collider other ) { Debug.Log( "Player entered the door trigger." ); }

        void OnTriggerStay( Collider other )
        {
            if ( other.CompareTag( "Player" ) ) { Actioner?.DoAction(); }
        }
    }
}

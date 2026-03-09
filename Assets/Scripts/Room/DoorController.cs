using Common;
using UnityEngine;

namespace Room
{
    /// <summary>
    /// Steuert die Interaktion mit Triggern in einem Raum, z.B. das Öffnen von Türen.
    /// </summary>
    public class DoorController : MonoBehaviour, IActioner
    {
        public void DoAction()
        {
            if ( Input.GetKeyDown( KeyCode.E ) ) { gameObject.SetActive( false ); }
        }
    }
}

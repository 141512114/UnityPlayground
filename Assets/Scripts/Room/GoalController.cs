using UnityEngine;

namespace Room
{
    public class GoalController : MonoBehaviour
    {
        private void OnTriggerEnter( Collider other )
        {
            if ( !other.CompareTag( "Player" ) ) return;
            Debug.Log( "Ziel erreicht!" );
            // Friere den Spieler ein
            Rigidbody rb = other.GetComponent< Rigidbody >();
            if ( rb != null ) { rb.isKinematic = true; }
        }
    }
}

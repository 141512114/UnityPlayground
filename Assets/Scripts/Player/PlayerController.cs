using Camera;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// Steuert die Bewegung des Spielers basierend auf den Eingaben der WASD-Tasten.
    /// Ermöglicht horizontale Bewegung und vertikales Springen/Drücken, während die Geschwindigkeit begrenzt wird.
    /// </summary>
    [RequireComponent( typeof( PlayerMovement ) )]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private CameraManager cameraManager;

        public CameraManager CameraManager => cameraManager;

        private PlayerMovement _playerMovement;

        private void Awake()
        {
            if ( !CameraManager )
            {
                Debug.LogError( "Kamera-Manager wurde nicht gesetzt!" );
                enabled = false;
                return;
            }
            
            _playerMovement = GetComponent< PlayerMovement >();

            if ( _playerMovement ) return;
            Debug.LogError( "PlayerMovement-Komponente konnte nicht gefunden werden!" );
            enabled = false;
        }

        private void Update() { _playerMovement.Move( Time.deltaTime ); }
    }
}

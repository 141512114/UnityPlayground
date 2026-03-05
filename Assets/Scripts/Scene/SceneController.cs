using UnityEngine;

public class SceneController : MonoBehaviour
{
    private SceneManager _sceneManager;

    private void Awake()
    {
        // Stelle sicher, dass der SceneManager in der Szene vorhanden ist
        if ( SceneManager.Instance == null )
        {
            Debug.LogError( "SceneManager instance not found. Please ensure a SceneManager is present in the scene." );
            enabled = false;
            return;
        }
    }

    private void Start()
    {
        _sceneManager = SceneManager.Instance;
    }
}

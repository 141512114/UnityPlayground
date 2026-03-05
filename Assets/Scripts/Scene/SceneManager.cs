using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public SceneProfile[] sceneProfiles;

    public static SceneManager Instance { get; private set; }

    private void Start()
    {
        Instance = this;

        InitializeScene();
    }

    private void OnDestroy()
    {
        if ( Instance == this )
        {
            Instance = null;
        }
    }

    private void InitializeScene()
    {
        if ( sceneProfiles.Length == 0 )
        {
            Debug.LogWarning( "No scene profiles assigned to SceneManager." );
            enabled = false;
            return;
        }

        for ( int i = 0; i < sceneProfiles.Length; i++ )
        {
            SceneProfile profile = sceneProfiles[ i ];

            if ( profile.Rooms.Count == 0 )
            {
                Debug.LogWarning( $"SceneProfile at index {i} has no rooms assigned." );
                continue;
            }

            for ( int j = 0; j < profile.Rooms.Count; j++ )
            {
                RoomController room = profile.Rooms[ j ];
                var loadpoint = profile.Loadpoints.Count > j ? profile.Loadpoints[ j ] : null;

                // Lade das Zimmer und setze es auf einen Loadpoint
                if ( room != null )
                {
                    if ( loadpoint == null )
                    {
                        Debug.LogWarning( $"No loadpoint assigned for room at index {j} in SceneProfile at index {i}. Using default position." );
                        loadpoint = new GameObject( $"DefaultLoadpoint_{i}_{j}" );
                        loadpoint.transform.position = Vector3.zero;
                    }

                    Instantiate( room.gameObject, loadpoint.transform.position, Quaternion.identity );
                    room.gameObject.SetActive( true );
                }
                else
                {
                    Debug.LogWarning( $"Room at index {j} in SceneProfile at index {i} is null." );
                }
            }
        }
    }
}

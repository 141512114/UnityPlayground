using System.Collections.Generic;
using Room;
using UnityEngine;

namespace Scene
{
    public class SceneManager : MonoBehaviour
    {
        [SerializeField] private List< RoomDatabase > roomDatabases;
        [SerializeField] private List< Transform >    loadPoints;

        private void Start() { InitializeScene(); }

        private void InitializeScene()
        {
            if ( roomDatabases.Count == 0 )
            {
                Debug.LogWarning( "No scene profiles assigned to SceneManager." );
                enabled = false;
                return;
            }

            int sceneIndex = 0;
            foreach ( RoomDatabase profile in roomDatabases )
            {
                if ( profile.Rooms.Count == 0 )
                {
                    Debug.LogWarning( $"SceneProfile at index {sceneIndex} has no rooms assigned." );
                    continue;
                }

                for ( int j = 0; j < profile.Rooms.Count; j++ )
                {
                    GameObject room      = profile.Rooms[ j ];
                    var        loadPoint = loadPoints.Count > j ? loadPoints[ j ] : null;

                    // Lade das Zimmer und setze es auf einen Load point
                    if ( room != null )
                    {
                        if ( loadPoint == null )
                        {
                            Debug.LogWarning( $"No loadpoint assigned for room at index {j} in SceneProfile at index {sceneIndex}. Using default position." );
                            loadPoint = new GameObject( $"LoadPoint_{sceneIndex}_{j}" ).transform;
                        }

                        Instantiate( room, loadPoint.transform.position, Quaternion.identity );
                        room.SetActive( true );
                    }
                    else { Debug.LogWarning( $"Room at index {j} in SceneProfile at index {sceneIndex} is null." ); }
                }

                sceneIndex++;
            }
        }
    }
}

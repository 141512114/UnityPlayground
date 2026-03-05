using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "SceneProfile", menuName = "Scene/Scene Profile" )]
public class SceneProfile : ScriptableObject
{
    [SerializeField] private List<RoomController> rooms;
    [SerializeField] private List<GameObject> loadpoints;

    public List<RoomController> Rooms => rooms;
    public List<GameObject> Loadpoints => loadpoints;
}

using UnityEngine;

[CreateAssetMenu(fileName = "RoomConfig", menuName = "Scriptable Objects/RoomConfig")]
public class RoomConfig : ScriptableObject
{
    public string Name;
    public GameObject Prefab;
}

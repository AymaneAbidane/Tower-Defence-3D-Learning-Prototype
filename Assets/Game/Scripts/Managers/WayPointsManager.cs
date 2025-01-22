using UnityEngine;

public class WayPointsManager : MonoBehaviour
{
    [SerializeField] private Transform[] levelWayPointsArray;

    public Transform[] GetLevelWayPointsArray() => levelWayPointsArray;

}

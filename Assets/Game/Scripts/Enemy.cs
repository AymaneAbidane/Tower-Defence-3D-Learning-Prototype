using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{
    [SerializeField] private float turiningSpeed = 10f;

    [SerializeField] private Transform[] waypointsArray;
    [SerializeField] private NavMeshAgent ownAgent;

    private int wayPointIndex;

    private void Awake()
    {
        ownAgent.updateRotation = false;
        ownAgent.avoidancePriority = Mathf.RoundToInt(ownAgent.speed * 10f);//setting priority based on speed
    }

    private void Start()
    {
        waypointsArray = FindAnyObjectByType<WayPointsManager>().GetLevelWayPointsArray();
    }

    private void Update()
    {
        FaceTarget(ownAgent.steeringTarget);
        if (ownAgent.remainingDistance < 0.5f)
        {
            if (wayPointIndex < waypointsArray.Length)
            {
                ownAgent.SetDestination(GetNextWayPoint());
            }
        }
    }

    private void FaceTarget(Vector3 target)
    {
        Vector3 directionToTarget = target - transform.position;
        directionToTarget.y = 0;//ingonre y axis in rotation
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turiningSpeed * Time.deltaTime);
    }

    private Vector3 GetNextWayPoint()
    {
        if (wayPointIndex >= waypointsArray.Length)
        {
            wayPointIndex = 0;
            //return transform.position;
        }
        Vector3 targetPosition = waypointsArray[wayPointIndex].position;
        wayPointIndex++;
        return targetPosition;
    }
}

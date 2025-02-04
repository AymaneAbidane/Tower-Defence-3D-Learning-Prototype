using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour, IDamagable
{

    [SerializeField] private NavMeshAgent ownAgent;

    [SerializeField] private int health = 4;
    [SerializeField] private Transform centerPoint;

    [Header("Movment")]
    [SerializeField] private float turiningSpeed = 10f;
    [SerializeField] private Transform[] waypointsArray;

    [Space]

    private float totalDistance;

    private int wayPointIndex;


    private void Awake()
    {
        ownAgent.updateRotation = false;
        ownAgent.avoidancePriority = Mathf.RoundToInt(ownAgent.speed * 10f);//setting priority based on speed
    }

    private void Start()
    {
        waypointsArray = FindAnyObjectByType<WayPointsManager>().GetLevelWayPointsArray();
        CalculateTotalDistance();
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
        // If the current waypoint index is greater than or equal to the length of the waypoints array,
        // reset the waypoint index to 0 to start from the beginning.
        if (wayPointIndex >= waypointsArray.Length)
        {
            wayPointIndex = 0;
        }

        // Get the position of the next waypoint.
        Vector3 targetPosition = waypointsArray[wayPointIndex].position;

        // If the waypoint index is greater than 0, calculate the distance between the current waypoint
        // and the previous waypoint, and subtract it from the total distance.
        if (wayPointIndex > 0)
        {
            float distance = Vector3.Distance(waypointsArray[wayPointIndex].position, waypointsArray[wayPointIndex - 1].position);
            totalDistance -= distance;
        }

        // Increment the waypoint index to move to the next waypoint.
        wayPointIndex++;

        // Return the position of the next waypoint.
        return targetPosition;
    }
    private void CalculateTotalDistance()
    {
        for (int i = 0; i < waypointsArray.Length - 1; i++)
        {
            float distance = Vector3.Distance(waypointsArray[i].position, waypointsArray[i + 1].position);

            totalDistance += distance;
        }
    }


    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
    public float DistanceToFinishline() => totalDistance + ownAgent.remainingDistance;
    public Vector3 GetCenterPointPosition() => centerPoint.position;
}

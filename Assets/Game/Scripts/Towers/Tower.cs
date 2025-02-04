using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    public Enemy currentTarget;

    [SerializeField] protected float attaCooldown;
    protected float lastTimeAttacked;

    [Header("Tower Setup")]
    [SerializeField] protected Transform towerHeadTransform;
    [SerializeField] protected Transform towerBuilding;
    [SerializeField] protected float rotationSpeed;

    [SerializeField] protected float attackRange = 1.5f;

    [SerializeField] protected LayerMask enemyLayer;

    private bool canRotate;

    private void Start()
    {
        SetCanRotate(true);
    }

    protected virtual void Update()
    {
        if (currentTarget == null)
        {
            currentTarget = FindRandomEnemyWithingRange();
            return;
        }

        if (CanAttack())
        {
            AttackTarget();
        }

        if (currentTarget != null && Vector3.Distance(currentTarget.GetCenterPointPosition(), towerBuilding.position) > attackRange)
        {
            currentTarget = null;
        }

        RotateTowardsEnemy();
    }

    protected virtual void AttackTarget()
    {
        Debug.Log("Attacking Performered At :" + Time.time);
    }

    protected virtual bool CanAttack()
    {
        // Check if the current time is greater than the last attack time plus the attack cooldown
        if (Time.time > lastTimeAttacked + attaCooldown)
        {
            // Update the last attack time to the current time
            lastTimeAttacked = Time.time;
            return true;
        }
        return false;
    }

    protected virtual Enemy FindRandomEnemyWithingRange()
    {
        // Create a list to store possible enemy targets within range
        List<Enemy> possibleTrgets = new();

        // Get all colliders within the attack range that belong to the enemy layer
        Collider[] enemyColliders = Physics.OverlapSphere(towerBuilding.position, attackRange, enemyLayer);

        // Iterate through each collider to find enemies
        foreach (var enemyTarget in enemyColliders)
        {
            // Try to get the Enemy component from the collider
            if (enemyTarget.TryGetComponent<Enemy>(out Enemy enemy))
            {
                // Add the enemy to the list of possible targets
                possibleTrgets.Add(enemy);
            }
        }

        Enemy newTarget = GetMostAdvancedEnemy(possibleTrgets);
        if (newTarget != null)
        {
            return newTarget;
        }
        return null;
    }

    private Enemy GetMostAdvancedEnemy(List<Enemy> possibleTrgets)
    {
        // Initialize variables to find the most advanced enemy (closest to the finish line)
        Enemy mostAdvancedEnemy = null;
        float minRemainingDIstance = float.MaxValue;

        // Iterate through the list of possible targets
        foreach (Enemy enemy in possibleTrgets)
        {
            // Calculate the remaining distance for the enemy to reach the finish line
            float remainingDistance = enemy.DistanceToFinishline();

            // If this enemy has a shorter remaining distance, update the most advanced enemy
            if (remainingDistance < minRemainingDIstance)
            {
                minRemainingDIstance = remainingDistance;
                mostAdvancedEnemy = enemy;
            }
        }

        return mostAdvancedEnemy;
    }

    protected virtual void RotateTowardsEnemy()
    {
        if (canRotate == false) return;

        if (currentTarget == null)
        {
            return;
        }

        // Calculate the direction vector from the tower head to the enemy
        Vector3 directionToEnemy = DirectionToEnemy(towerHeadTransform);

        // Calculate the target rotation based on the direction vector
        Quaternion targetRotation = Quaternion.LookRotation(directionToEnemy);

        // Interpolate between the current rotation and the target rotation
        Vector3 rotation = Quaternion.Lerp(towerHeadTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime).eulerAngles;

        // Apply the interpolated rotation to the tower head
        towerHeadTransform.rotation = Quaternion.Euler(rotation);
    }

    protected Vector3 DirectionToEnemy(Transform startPoint)
    {
        if (currentTarget != null)
        {
            return (currentTarget.GetCenterPointPosition() - startPoint.position).normalized;
        }
        else
        {
            return Vector3.zero;
        }
    }

    public void SetCanRotate(bool canRotate)
    {
        this.canRotate = canRotate;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(towerBuilding.position, attackRange);
    }
}

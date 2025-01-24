using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    public Transform currentTarget;

    [SerializeField] protected float attaCooldown;
    protected float lastTimeAttacked;

    [Header("Tower Setup")]
    [SerializeField] protected Transform towerHeadTransform;
    [SerializeField] protected Transform towerBuilding;
    [SerializeField] protected float rotationSpeed;

    [SerializeField] protected float attackRange = 1.5f;

    [SerializeField] protected LayerMask enemyLayer;
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

        if (currentTarget != null && Vector3.Distance(currentTarget.position, towerBuilding.position) > attackRange)
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

    protected virtual Transform FindRandomEnemyWithingRange()
    {
        List<Transform> possibleTrgets = new();
        Collider[] enemyColliders = Physics.OverlapSphere(towerBuilding.position, attackRange, enemyLayer);

        foreach (var enemy in enemyColliders)
        {
            possibleTrgets.Add(enemy.transform);
        }

        if (possibleTrgets.Count == 0)
        {
            return null;
        }
        else
        {
            return possibleTrgets[Random.Range(0, possibleTrgets.Count)];
        }
    }

    protected virtual void RotateTowardsEnemy()
    {
        if (currentTarget == null)
        {
            return;
        }

        // Calculate the direction vector from the tower head to the enemy
        Vector3 directionToEnemy = currentTarget.position - towerHeadTransform.position;

        // Calculate the target rotation based on the direction vector
        Quaternion targetRotation = Quaternion.LookRotation(directionToEnemy);

        // Interpolate between the current rotation and the target rotation
        Vector3 rotation = Quaternion.Lerp(towerHeadTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime).eulerAngles;

        // Apply the interpolated rotation to the tower head
        towerHeadTransform.rotation = Quaternion.Euler(rotation);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(towerBuilding.position, attackRange);
    }
}

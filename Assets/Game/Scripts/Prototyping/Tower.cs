using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private Transform towerHead;
    private EnemyCreator enemyCreator;
    private Transform enemy;

    [Header("Attack details")]
    [SerializeField] private float attackRange = 3;
    [SerializeField] private float attackCooldown;
    private float lastTimeAttacked;

    [Header("Bullet details")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 3;

    private void Awake()
    {
        enemyCreator = FindFirstObjectByType<EnemyCreator>();
    }


    // Update is called once per frame
    void Update()
    {
        if (enemy == null)
        {
            enemy = FindClosestEnemy();
            return;
        }

        if (Vector3.Distance(enemy.position, towerHead.position) < attackRange)
        {
            towerHead.LookAt(enemy);

            if (ReadyToAttack())
            {
                CreateBullet();
            }
        }

    }

    private Transform FindClosestEnemy()
    {
        float closestDistance = float.MaxValue;
        Transform closestEnemy = null;

        foreach (Transform enemy in enemyCreator.EnemyList())
        {
            float distance = Vector3.Distance(enemy.position, transform.position);

            if (distance < closestDistance && distance <= attackRange)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy != null)
            enemyCreator.EnemyList().Remove(closestEnemy);

        return closestEnemy;
    }

    private void FindRandomEnemy()
    {
        if (enemyCreator.EnemyList().Count <= 0)
            return;

        int randomIndex = Random.Range(0, enemyCreator.EnemyList().Count);
        enemy = enemyCreator.EnemyList()[randomIndex];
        enemyCreator.EnemyList().RemoveAt(randomIndex);
    }

    private void CreateBullet()
    {
        GameObject newBullet = Instantiate(bulletPrefab, towerHead.position, Quaternion.identity);
        newBullet.GetComponent<Rigidbody>().linearVelocity = (enemy.position - towerHead.position).normalized * bulletSpeed;
    }

    private bool ReadyToAttack()
    {
        if (Time.time > lastTimeAttacked + attackCooldown)
        {
            lastTimeAttacked = Time.time;
            return true;
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}

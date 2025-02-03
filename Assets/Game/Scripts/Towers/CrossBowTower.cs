using UnityEngine;

public class CrossBowTower : Tower
{
    [Header("Cross Bow Details")]
    [SerializeField] private int damage = 1;
    [SerializeField] private Transform gunPoint;
    [SerializeField] private CrossbowVisuals crossbowVisuals;

    protected override void AttackTarget()
    {
        Vector3 directionToEnemy = DirectionToEnemy(gunPoint);

        if (Physics.Raycast(gunPoint.position, directionToEnemy, out RaycastHit hitInfo, Mathf.Infinity))
        {
            crossbowVisuals.PlayAttackLaserFX(gunPoint.position, hitInfo.point);
            crossbowVisuals.PlayReloadFx(attaCooldown);

            if (hitInfo.transform.TryGetComponent<IDamagable>(out IDamagable damagable))
            {
                damagable.TakeDamage(damage);
            }
        }
    }
}

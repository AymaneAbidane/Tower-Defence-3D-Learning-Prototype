using UnityEngine;

public class CrossBowTower : Tower
{
    [Header("Cross Bow Details")]
    [SerializeField] private Transform gunPoint;
    [SerializeField] private CrossbowVisuals crossbowVisuals;

    protected override void AttackTarget()
    {
        Vector3 directionToEnemy = DirectionToEnemy(gunPoint);

        if (Physics.Raycast(gunPoint.position, directionToEnemy, out RaycastHit hitInfo, Mathf.Infinity))
        {
            Debug.DrawLine(gunPoint.position, hitInfo.point);
            Debug.Log("Hit Enemy: " + hitInfo.collider.gameObject.name);
            crossbowVisuals.PlayAttackLaserFX(gunPoint.position, hitInfo.point);
            crossbowVisuals.PlayReloadFx(attaCooldown);
        }
    }
}

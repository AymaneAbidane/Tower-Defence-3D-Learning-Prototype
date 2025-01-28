using System.Collections;
using UnityEngine;

public class CrossbowVisuals : MonoBehaviour
{
    [SerializeField] private CrossBowTower myTower;
    [SerializeField] private LineRenderer attackVisuals;
    [SerializeField] private float attackVisualsDuration = 0.1f;

    public void PlayAttackLaserFX(Vector3 startPoint, Vector3 endPoint)
    {
        StartCoroutine(COR_ShhotLaser(startPoint, endPoint));
    }

    private IEnumerator COR_ShhotLaser(Vector3 startPoint, Vector3 endPoint)
    {
        myTower.SetCanRotate(false);
        attackVisuals.enabled = true;

        attackVisuals.SetPosition(0, startPoint);
        attackVisuals.SetPosition(1, endPoint);

        yield return new WaitForSeconds(attackVisualsDuration);
        attackVisuals.enabled = false;
        myTower.SetCanRotate(true);
    }
}

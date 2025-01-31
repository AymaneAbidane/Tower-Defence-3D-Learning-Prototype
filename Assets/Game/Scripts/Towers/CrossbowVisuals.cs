using System.Collections;
using UnityEngine;

public class CrossbowVisuals : MonoBehaviour
{
    [SerializeField] private CrossBowTower myTower;
    [SerializeField] private LineRenderer attackVisuals;
    [SerializeField] private float attackVisualsDuration = 0.1f;

    [Header("Glowing Visuals")]
    [SerializeField] private MeshRenderer meshRenderer;
    [Space]
    [SerializeField] private float maxIntesity = 150f;
    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;

    [Header("Font Strings")]
    [SerializeField] private LineRenderer frontStringLeft;
    [SerializeField] private LineRenderer frontStringRight;

    [Space]

    [SerializeField] private Transform frontStartPointLeft;
    [SerializeField] private Transform frontStartPointRight;
    [SerializeField] private Transform frontEndPointLeft;
    [SerializeField] private Transform frontEndPointRight;


    [Header("Back Strings")]
    [SerializeField] private LineRenderer backStringLeft;
    [SerializeField] private LineRenderer backStringRight;

    [Space]

    [SerializeField] private Transform backStartPointLeft;
    [SerializeField] private Transform backStartPointRight;
    [SerializeField] private Transform backEndPointLeft;
    [SerializeField] private Transform backEndPointRight;

    private Material material;
    private float currentIntesity;

    private void Awake()
    {
        // Create a new material instance from the mesh renderer's material
        material = new(meshRenderer.material);
        // Assign the new material instance to the mesh renderer
        meshRenderer.material = material;
        // Start the emission change coroutine with a duration of 1 second
        StartCoroutine(COR_ChangeEmission(1));
    }

    /// <summary>
    /// Plays the attack laser visual effect from the start point to the end point.
    /// </summary>
    /// <param name="startPoint">The starting point of the laser.</param>
    /// <param name="endPoint">The ending point of the laser.</param>
    public void PlayAttackLaserFX(Vector3 startPoint, Vector3 endPoint)
    {
        StartCoroutine(COR_ShootLaser(startPoint, endPoint));
    }

    /// <summary>
    /// Coroutine to handle the shooting laser visual effect.
    /// </summary>
    /// <param name="startPoint">The starting point of the laser.</param>
    /// <param name="endPoint">The ending point of the laser.</param>
    /// <returns>An IEnumerator to be used with StartCoroutine.</returns>
    private IEnumerator COR_ShootLaser(Vector3 startPoint, Vector3 endPoint)
    {
        // Disable tower rotation during the laser effect
        myTower.SetCanRotate(false);
        // Enable the line renderer for the laser effect
        attackVisuals.enabled = true;

        // Set the positions of the line renderer to create the laser effect
        attackVisuals.SetPosition(0, startPoint);
        attackVisuals.SetPosition(1, endPoint);

        // Wait for the duration of the laser effect
        yield return new WaitForSeconds(attackVisualsDuration);
        // Disable the line renderer after the effect
        attackVisuals.enabled = false;
        // Re-enable tower rotation
        myTower.SetCanRotate(true);
    }

    private void Update()
    {
        // Update the emission color of the material each frame
        UpdateEmissionColor();

        // Update the front strings visuals each frame
        UpdateStingsVisuals(frontStringLeft, frontStartPointLeft, frontEndPointLeft);
        UpdateStingsVisuals(frontStringRight, frontStartPointRight, frontEndPointRight);
        UpdateStingsVisuals(backStringLeft, backStartPointLeft, backEndPointLeft);
        UpdateStingsVisuals(backStringRight, backStartPointRight, backEndPointRight);
    }

    /// <summary>
    /// Updates the emission color of the material based on the current intensity.
    /// </summary>
    private void UpdateEmissionColor()
    {
        // Interpolate the emission color between the start and end colors based on the current intensity
        Color emissionCOlor = Color.Lerp(startColor, endColor, currentIntesity / maxIntesity);
        // Adjust the emission color for gamma correction
        emissionCOlor *= Mathf.GammaToLinearSpace(currentIntesity);
        // Set the emission color of the material
        material.SetColor("_EmissionColor", emissionCOlor);
    }

    /// <summary>
    /// Coroutine to gradually change the emission intensity of the material over a specified duration.
    /// </summary>
    /// <param name="duration">The duration over which the emission intensity changes.</param>
    /// <returns>An IEnumerator to be used with StartCoroutine.</returns>
    private IEnumerator COR_ChangeEmission(float duration)
    {
        // Record the start time of the coroutine
        float startTime = Time.time;

        // Initial intensity of the emission
        float startIntensity = 0;

        // Loop until the specified duration has passed
        while (Time.time < startTime + duration)
        {
            // Calculate the delta time as a fraction of the total duration
            float delta = (Time.time - startTime) / duration;

            // Interpolate the current intensity from the start intensity to the maximum intensity
            currentIntesity = Mathf.Lerp(startIntensity, maxIntesity, delta);

            // Wait for the next frame
            yield return null;
        }

        // Ensure the final intensity is set to the maximum intensity
        currentIntesity = maxIntesity;
    }

    /// <summary>
    /// Plays the reload visual effect by changing the emission intensity over a specified duration.
    /// </summary>
    /// <param name="duration">The duration of the reload effect.</param>
    public void PlayReloadFx(float duration)
    {
        StartCoroutine(COR_ChangeEmission(duration / 2));
    }


    private void UpdateStingsVisuals(LineRenderer lineRenderer, Transform startPoint, Transform endPoints)
    {
        lineRenderer.SetPosition(0, startPoint.position);
        lineRenderer.SetPosition(1, endPoints.position);
    }
}

using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private Vector3 rotationVector;
    [SerializeField] private float rotationSpeed;

    void Update()
    {
        float rotationSpeed = this.rotationSpeed * 100f;
        transform.Rotate(rotationVector * rotationSpeed * Time.deltaTime);
    }
}

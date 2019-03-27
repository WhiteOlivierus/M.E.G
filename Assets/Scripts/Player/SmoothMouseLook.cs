using UnityEngine;

public class SmoothMouseLook : MonoBehaviour
{
    public float lookSensitivity = 5;
    public float lookSmoothnes = 0.1f;
    public Vector2 angle;
    Vector2 Rotation;
    Vector2 currentRotation;
    Vector2 RotationV;

    void Update()
    {
        Rotation.y += Input.GetAxis("Mouse X") * lookSensitivity;
        Rotation.x -= Input.GetAxis("Mouse Y") * lookSensitivity;
        Rotation.x = Mathf.Clamp(Rotation.x, angle.x, angle.y);
        currentRotation.x = Mathf.SmoothDamp(currentRotation.x, Rotation.x, ref RotationV.x, lookSmoothnes);
        currentRotation.y = Mathf.SmoothDamp(currentRotation.y, Rotation.y, ref RotationV.y, lookSmoothnes);
        transform.rotation = Quaternion.Euler(Rotation.x, Rotation.y, 0);
    }
}
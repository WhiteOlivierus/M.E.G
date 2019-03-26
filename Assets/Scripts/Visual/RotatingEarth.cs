using UnityEngine;

public class RotatingEarth : MonoBehaviour
{
    [SerializeField] private float speed = 1f;

    void Update()
    {
        transform.Rotate(transform.up, speed * Time.deltaTime);
    }
}

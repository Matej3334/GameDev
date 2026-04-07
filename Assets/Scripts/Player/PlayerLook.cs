using UnityEngine;
using UnityEngine.Rendering;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private Transform cameraRoot;
    private float xRotation = 0f;


    public float xSensitivity = 30f;
    public float ySensitivity = 30f;

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        camera.transform.position = cameraRoot.position;
        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        camera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
    }
}

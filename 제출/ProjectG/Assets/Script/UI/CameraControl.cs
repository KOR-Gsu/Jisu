using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private Vector3 offset = new Vector3(0, 1.0f, -1.0f);

    public Transform target;

    [SerializeField] private float lookSensitivity;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float currentZoom;
    [SerializeField] private int minZoom;
    [SerializeField] private int maxZoom;
    [SerializeField] private float minPositionY;
    [SerializeField] private float maxPositionY;

    private void Start()
    {
        InputManager.instance.mouseAction -= OnMouse1Pressed;
        InputManager.instance.mouseAction += OnMouse1Pressed;
    }

    private void LateUpdate()
    {
        CameraZoom();
        ClampPositionY();
    }
    private void CameraRotation()
    {
        float rotX = Input.GetAxis("Mouse Y") * lookSensitivity;
        float rotY = Input.GetAxis("Mouse X") * lookSensitivity;

        transform.RotateAround(target.position, Vector3.right, rotX);
        transform.RotateAround(target.position, Vector3.up, rotY);

        offset = transform.position - target.position;
        offset.Normalize();
    }

    private void CameraZoom()
    {
        currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

        LayerMask checkLayer = LayerMask.GetMask("Ground") | LayerMask.GetMask("Terrain") | LayerMask.GetMask("Shop") | LayerMask.GetMask("Enemy");
        if (Physics.Raycast(target.transform.position, offset, out RaycastHit hit, maxZoom, checkLayer))
        {
            float newZoom = (hit.point - target.transform.position).magnitude * 0.5f;
            currentZoom = Mathf.Lerp(currentZoom, newZoom, Time.deltaTime * zoomSpeed);
        }
    }

    private void ClampPositionY()
    {
        Vector3 newPos = target.position + offset * currentZoom;

        if (newPos.y < minPositionY * (currentZoom/ maxZoom))
            newPos.y = minPositionY * (currentZoom / maxZoom);
        if (newPos.y > maxPositionY * (currentZoom / maxZoom))
            newPos.y = maxPositionY * (currentZoom / maxZoom);

        transform.position = newPos;
        transform.LookAt(target);
    }

    private void OnMouse1Pressed(Define.Mouse mouse, Define.MouseEvent evt)
    {
        if (mouse != Define.Mouse.Mouse_1 || evt != Define.MouseEvent.Press)
            return;

        CameraRotation();
    }
}

using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform follow;
    public float maxDistance = 5f;
    public float sensitivity = 2f;
    public float smoothSpeed = 10f;
    public float radio = 0.5f;

    private Vector3 currentRotation;
    private float currentDistance;
    private float targetDistance;

    void Start()
    {

        currentDistance = maxDistance;
        targetDistance = maxDistance;

    }

    void Update()
    {

        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = -Input.GetAxis("Mouse Y") * sensitivity;


        currentRotation.x += mouseX;
        currentRotation.y -= mouseY;


        currentRotation.y = Mathf.Clamp(currentRotation.y, -90f, 90f);


        Quaternion rotation = Quaternion.Euler(currentRotation.y, currentRotation.x, 0);
        Vector3 direccion = rotation * Vector3.back;


        RaycastHit hit;
        if (Physics.SphereCast(follow.position, radio, direccion, out hit, maxDistance))
        {
            targetDistance = hit.distance;
        }
        else
        {
            targetDistance = maxDistance;
        }

        currentDistance = Mathf.Lerp(currentDistance, targetDistance, Time.deltaTime * smoothSpeed);

        transform.position = (follow.position + transform.right * 0.5f) + direccion * currentDistance;
        transform.LookAt(follow);
    }
}

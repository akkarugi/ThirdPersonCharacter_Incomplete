using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public float disappearTime = 5f;
    public Vector3 forceMin = new Vector3(-1, -1, 50);
    public Vector3 forceMax = new Vector3(1, 1, 100);
    public LayerMask layers;
    public float collisionForceMultiplier = 2f;
    public float radius = .1f;
    [HideInInspector]
    public Rigidbody rb;

    public GameObject BulletHole;

    Vector3 lastPos;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddRelativeForce(new Vector3(Random.Range(forceMin.x, forceMax.x), Random.Range(forceMin.y, forceMax.y), Random.Range(forceMin.z, forceMax.z)));
        Destroy(gameObject, disappearTime);
        lastPos = transform.position;
        transform.parent = null;
    }

    private void FixedUpdate()
    {
        Vector3 dir = transform.position - lastPos;

        Debug.DrawRay(lastPos, dir, Color.blue, disappearTime);

        RaycastHit hit;

        if (Physics.SphereCast(lastPos, radius, dir, out hit, dir.magnitude, layers))
        {
            Hitted(hit);
        }

        lastPos = transform.position;
    }

    void Hitted(RaycastHit hit)
    {
        GameObject tmp= Instantiate(BulletHole, hit.point, Quaternion.LookRotation(hit.normal));
        
        if (hit.rigidbody)
        {
            hit.rigidbody.AddForceAtPosition(rb.velocity * rb.mass * collisionForceMultiplier, this.transform.position);
            tmp.transform.parent = hit.transform;
        }
        Destroy(gameObject);
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericGun : MonoBehaviour
{
    [Header("Weapon")]
    public int clipMax = 30;
    public int clipCurrent = 30;
    public bool automatic = true;
    [Min(1f/60f)]
    public float fireRate = 0.1f;
    public float reloadTime = 0.5f;
    [Header("Firing")]
    public UnityEvent onFire;
    public Transform firePoint;
    public GameObject bullet;
    [Header("Animation")]
    public float positionRecover;
    public float rotationRecover;
    public Vector3 knockbackPosition;
    public Vector3 knockbackRotation;
    Vector3 originalPosition;
    Quaternion originalRotation;

    public float maxDistance = 4f;
   
    public Transform mirilla;
    float currTimer = 0;
    float contador = 0;
    private void Start()
    {
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
    }

    void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, positionRecover * Time.deltaTime);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, originalRotation, rotationRecover * Time.deltaTime);

        //RAYCAST
        RaycastHit hit;

        Debug.DrawRay(firePoint.position, firePoint.forward * maxDistance, Color.red);

        if (Physics.Raycast(firePoint.position, firePoint.forward,out hit ,maxDistance))
        {
            mirilla.position = Camera.main.WorldToScreenPoint(hit.point);

        }
        if (automatic)
        {
            if (Input.GetButton("Fire1"))
            {

                currTimer += Time.deltaTime;
                if (currTimer >= fireRate)
                {
                    if (clipCurrent > 0)
                    {
                        Fire();
                        currTimer = 0;
                        clipCurrent--;
                    }
                }
            }
        }
        if (!automatic)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (clipCurrent > 0)
                {
                    Fire();
                   clipCurrent--;
                }
            }
        }

        if (clipCurrent <= 0)
        {
            contador += Time.deltaTime;
            if (contador >= reloadTime)
            {
                clipCurrent = clipMax;
                contador = 0;
            }
        }

    }
    public void Fire()
    {
        Destroy(Instantiate(bullet, firePoint.position, firePoint.rotation), 10);
        onFire.Invoke();
        StartCoroutine(Knockback_Corutine());
    }
    IEnumerator Knockback_Corutine()
    {
        yield return null;
        transform.localPosition -= new Vector3(Random.Range(-knockbackPosition.x, knockbackPosition.x), Random.Range(0, knockbackPosition.y), Random.Range(-knockbackPosition.z, -knockbackPosition.z * .5f));
        transform.localEulerAngles -= new Vector3(Random.Range(knockbackRotation.x * 0.5f, knockbackRotation.x), Random.Range(-knockbackRotation.y, knockbackRotation.y), Random.Range(-knockbackRotation.z, knockbackRotation.z));
    }
}

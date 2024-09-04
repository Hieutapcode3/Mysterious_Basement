using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRotate : MonoBehaviour
{
    public static GunRotate Instance;

    public float angle;

    public GameObject Charactor;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Update()
    {
        RotateGun();
    }

    private void RotateGun()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = mousePos - transform.position;
        angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = rotation;

        if(transform.eulerAngles.z > 90 && transform.eulerAngles.z < 270 )
        {
            transform.localScale = new Vector3(transform.localScale.x, -Mathf.Abs(transform.localScale.y), transform.localScale.z);
            if(Charactor != null) 
                Charactor.transform.localEulerAngles = new Vector3(0, 0, 90);
        }
        else
        {
            if (Charactor != null)
                Charactor.transform.localEulerAngles = new Vector3(0, 0, -90f);
            transform.localScale = new Vector3(transform.localScale.x, Mathf.Abs(transform.localScale.y), transform.localScale.z);
        }


    }
}

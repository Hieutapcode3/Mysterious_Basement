using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunItems : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (gameObject.name == "M4Item")
            {
                GunController.instance.hasM4 = true;
            }
            else
                GunController.instance.hasShotGun = true;
            Destroy(gameObject);
        }
    }
}

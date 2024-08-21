using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCheck : MonoBehaviour
{
    public bool hasKey;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player inside");
            if(!hasKey)
            {
                anim.SetInteger("Check", 1);
                StartCoroutine(WaitForDoorCheck());
            }
            else
            {
                anim.SetInteger("Check", 2);
            }
        }
    }
    IEnumerator WaitForDoorCheck()
    {
        yield return new WaitForSeconds(1f);
        anim.SetInteger("Check", 0);
    }
}

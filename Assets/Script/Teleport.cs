using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] private Transform teleportToTransform;
    private PlayerMoveMent player;
    private bool hasTeleported = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.GetComponent<PlayerMoveMent>();
            if (player != null && !hasTeleported)
            {
                DissolveEffect dissolve = collision.GetComponent<DissolveEffect>();
                if (dissolve != null)
                {
                    hasTeleported = true;
                    
                    teleportToTransform.GetComponent<Teleport>().hasTeleported = true;
                    StartCoroutine(TeleportWithDissolve(dissolve));
                }
            }
        }
    }

    IEnumerator TeleportWithDissolve(DissolveEffect dissolve)
    {
        dissolve.StartDissolve();
        yield return new WaitForSeconds(0.5f);
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        yield return new WaitForSeconds(0.5f);
        player.TeleportTo(teleportToTransform);
        yield return new WaitForSeconds(0.5f);
        dissolve.StopDissolve();
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        yield return new WaitForSeconds(2f);
        hasTeleported = false;
        teleportToTransform.GetComponent<Teleport>().hasTeleported = false;


    }
}

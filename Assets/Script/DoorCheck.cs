using System.Collections;
using UnityEngine;
using static KeyCheck;
using Pathfinding;

public class DoorCheck : MonoBehaviour
{
    public ColorType doorColor; 
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

            PlayerInven playerInventory = collision.GetComponent<PlayerInven>();

            if (playerInventory.currentKey.HasValue && playerInventory.currentKey.Value == doorColor)
            {
                anim.SetInteger("Check", 2);  
                playerInventory.UseKey(); 
            }
            else
            {
                anim.SetInteger("Check", 1);  
                StartCoroutine(WaitForDoorCheck());
            }
        }
    }

    IEnumerator WaitForDoorCheck()
    {
        yield return new WaitForSeconds(1f);
        anim.SetInteger("Check", 0);  
    }
    public void RescanMap()
    {
        AstarPath.active.Scan();
    }
}

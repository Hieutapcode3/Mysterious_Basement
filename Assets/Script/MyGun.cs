using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyGun : MonoBehaviour
{
    [SerializeField] private Sprite pistolSprite;
    [SerializeField] private Sprite shotgunSprite;
    [SerializeField] private Sprite M4Sprite;
    [SerializeField] private Image spriteImage;
    private GunController gunController;

    private void Start()
    {
        spriteImage.sprite = pistolSprite; 
        gunController = FindObjectOfType<GunController>();
    }

    private void Update()
    {
        if (gunController.gunType == GunController.GunType.Pistol)
            spriteImage.sprite = pistolSprite;
        else if (gunController.gunType == GunController.GunType.Shotgun)
            spriteImage.sprite = shotgunSprite;
        else
            spriteImage.sprite = M4Sprite;
    }
}

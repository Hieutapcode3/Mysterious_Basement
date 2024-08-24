using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static KeyCheck;

public class PlayerInven : MonoBehaviour
{
    public ColorType? currentKey = null; 

    public void PickupKey(ColorType keyColor)
    {
        currentKey = keyColor;
    }

    public void UseKey()
    {
        currentKey = null;
    }
}

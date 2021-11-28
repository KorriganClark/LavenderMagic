using Lavender;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavenderAttackFeild : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var comp =other.GetComponent<LavenderCharacterControl>();
        if(comp != null)
        {
            comp.canMove = false;
            comp.CameraMoveWithMonsterPos();
        }
    }
}

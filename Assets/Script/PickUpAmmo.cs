using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpAmmo : MonoBehaviour
{
    public Reloading reloadScript;
    

    void OnTriggerEnter(Collider Other)
    {
        if(Other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Collide");
            reloadScript.Invoke("RefillAmmo", reloadScript.refillSpeed);
            Destroy(this.gameObject);
        }
    }
}

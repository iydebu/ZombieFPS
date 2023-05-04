using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Reloading : MonoBehaviour
{
    public int maxAmmo = 24;
    int currentMaxAmmo;
    public int ammoCapacity = 24;
    public int currentAmmo;


    public float reloadSpeed  = 1f;
    public float refillSpeed = 1.5f;

    public Text ammoText;
    public Text maxAmmoText;

    public bool needReload;
    public bool refilling;

    public GameObject reloadAnim;
    // Start is called before the first frame update
    void Start()
    {
        currentAmmo = ammoCapacity;
        currentMaxAmmo = maxAmmo;
        reloadAnim.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(currentAmmo <= 0)
        { needReload = true; }

        if(currentMaxAmmo > 0 && currentAmmo < ammoCapacity)
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(Reload());
            }
        }
        ammoText.text = currentAmmo.ToString();
        maxAmmoText.text =  currentMaxAmmo.ToString();
    }

    IEnumerator Reload()
    {
        Debug.Log("Reload");
        reloadAnim.SetActive(true);
        yield return new WaitForSeconds(reloadSpeed);
        needReload = false;

        if((ammoCapacity - currentAmmo) <= currentMaxAmmo)
        {
            currentMaxAmmo -= (ammoCapacity - currentAmmo);
            currentAmmo += (ammoCapacity - currentAmmo);
        }
        else
        {
            currentAmmo += currentMaxAmmo;
            currentMaxAmmo = 0;
        }

        reloadAnim.SetActive(false);
    }

    public void RefillAmmo()
    {
        Debug.Log("Refill");
        refilling = true;
        currentMaxAmmo += maxAmmo;
    }

    public void UseAmmo()
    {
        currentAmmo--;
    }
}

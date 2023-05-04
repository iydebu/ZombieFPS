using System.Collections;
using UnityEngine;

public class ShootScript : MonoBehaviour
{
    [Header("Effects")]
    public GameObject BloodEffect;
    public GameObject WallEffect;

    [Header("Bullet Settings")]
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletForce = 1000f;
    public float fireRate = 1f;
    
    [Header("Recoil Settings")]
    public float verticalRecoil = 4f;
    public float horizontalRecoil = 4f;

    [Header("Animator")]
    public Animator animator;

    private float nextFire = 0f;
    private bool isAiming;
    private bool canShoot = true;

    [Header("Aim")]
    public float aimSensitivity;
    public float lookSensitivity;
    public mouseLook mouseLook;


    [Header("GameObjects")]
    public Camera Camera;
    public GameObject aimUI;

    [Header("Reload Script")]
    public Reloading reloadScript;


    private void Awake()
    {
        mouseLook.mouseSensitivity = lookSensitivity;
    }
    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire && canShoot)
        {
            nextFire = Time.time + fireRate;
            if(reloadScript.needReload == false)
            {
                Shoot();
                reloadScript.UseAmmo();
                //count_Reload.Count();
                StartCoroutine(PlayShootingAnimation());
            }
        }

        if (Input.GetButtonDown("Fire2"))
        {
            StartCoroutine(PlayAimingAnimation());
        }
    }

    private void Shoot()
    {
        Quaternion originalRotation = bulletSpawn.rotation;
        ApplyRecoil();
        SoundManager.Sfx.playARSound();
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.AddForce(bulletSpawn.forward * bulletForce);
        Destroy(bullet, 3f);
        bullet.AddComponent<BulletCollision>();

        bulletSpawn.rotation = originalRotation;
    }

    private void ApplyRecoil()
    {
        if (!isAiming)
        {
            float randomVertical = Random.Range(-verticalRecoil, verticalRecoil);
            float randomHorizontal = Random.Range(-horizontalRecoil, horizontalRecoil);
            bulletSpawn.Rotate(randomVertical, randomHorizontal, 0);
        }
    }

    IEnumerator PlayAimingAnimation()
    {
        canShoot = false;
        isAiming = !isAiming;
        if (isAiming)
        {
            aimUI.SetActive(false);
            mouseLook.mouseSensitivity = aimSensitivity;
            Camera.fieldOfView = 40;  //zoom
        }
        else 
        {
            aimUI.SetActive(true);
            mouseLook.mouseSensitivity = lookSensitivity;
            Camera.fieldOfView = 60; //zoom
        }
        animator.SetBool("isAiming", isAiming);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        canShoot = true;
    }

    IEnumerator PlayShootingAnimation()
    {
        animator.SetBool("isShooting", true);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        animator.SetBool("isShooting", false);
    }
}

public class BulletCollision : MonoBehaviour
{
    private ShootScript shootScript;

    private void Start()
    {
        shootScript = FindObjectOfType<ShootScript>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            SpawnEffect(collision, shootScript.BloodEffect);
        }
        else
        {
            SpawnEffect(collision, shootScript.WallEffect);
        }

        Destroy(gameObject);
    }


    private void SpawnEffect(Collision collision, GameObject effect)
    {
        ContactPoint contact = collision.GetContact(0);
        Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, contact.normal);
        Vector3 position = contact.point;
        Instantiate(effect, position, rotation);
    }
}
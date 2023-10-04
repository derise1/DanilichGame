using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random=UnityEngine.Random;

public class GunController : MonoBehaviour
{
    
    [Header("Gun Settings")]
    public float fireRate = 0.1f;
    public int clipSize = 30;
    public int reservedAmmoCapacity = 270;
    public int damage;
    public float distance = 50f;
    public bool dieEnemy;

    [Header("Muzzle Flash")]
    public ParticleSystem muzzle;

    [Header("Ammo UI")]
    public Text currentAmmoText;
    public Text ammoReversedText;

    [Header("Aiming")]
    public Vector3 normalPosition;
    public Vector3 aimingPosition;
    public float aimSmoothing = 10;
    public GameObject chrosshair;
    public GameObject chrosshairDefault;
    public GameObject bloodEffect;
    public GameObject hitEffect;
    private float fieldOfView = 73f;
    public float fovSmooth = 0.08f;
    public Camera mainCam;

    [Header("Mouse Settings")]
    public Transform playerBody;
    public float mouseSensitivity = 1;
    public float weaponSwayAmount = -1;
    Vector2 _currentRotation;
    public Pause isPause;

    [Header("Recoil Settings")]
    public bool randomizeRecoil;
    public Vector2 randomRecoilConstraints;
    public Vector2[] recoilPattern;

    [Header("Animations")]
    public Animator animationsGun;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip fireSound;
    public AudioClip reloadSound;
    public AudioClip noAmmoSound;

    bool _canShoot;
    int _currentAmmoClip;
    int _ammoInReverse;

    private void Start() {
        mainCam.fieldOfView = fieldOfView;
        _currentAmmoClip = clipSize;
        _ammoInReverse = reservedAmmoCapacity;
        _canShoot = true;
        Cursor.lockState = CursorLockMode.Locked;
        chrosshair.SetActive(false);
        animationsGun = GetComponent<Animator>();
    }

    private void Update() {
        if(!isPause.PauseGame){
            DetermineAim();
            DetermineRotation();
            currentAmmoText.text = "" + _currentAmmoClip;
            if(Input.GetMouseButton(0) && _canShoot && _currentAmmoClip > 0){
                _canShoot = false;
                _currentAmmoClip--;
                StartCoroutine(ShootGun());
                audioSource.PlayOneShot(fireSound, 0.2f);
            }

            if(Input.GetKeyDown("mouse 0") && _currentAmmoClip == 0){
                audioSource.PlayOneShot(noAmmoSound, 0.2f);
            }

            else if(Input.GetKeyDown(KeyCode.R) && _currentAmmoClip < clipSize && _ammoInReverse > 0){
                
                int amountNeeded = clipSize - _currentAmmoClip;
                currentAmmoText.text = "" + _currentAmmoClip;
                audioSource.PlayOneShot(reloadSound, 0.4f);
                StartCoroutine(ReloadAnimations());
                if(amountNeeded >= _ammoInReverse){
                    _currentAmmoClip += _ammoInReverse;
                    _ammoInReverse = 0;
                    ammoReversedText.text = "/" + _ammoInReverse;
                }
                else{
                    _currentAmmoClip = clipSize;
                    _ammoInReverse -= amountNeeded;
                    ammoReversedText.text = "/" + _ammoInReverse;
                }
            }
        }
    }
    
    IEnumerator ReloadAnimations(){
        animationsGun.SetBool("reversedAmmo", true);
        _canShoot = false;
        yield return new WaitForSeconds(1.7f);
        animationsGun.SetBool("reversedAmmo", false);
        _canShoot = true;
    }

    void DetermineRotation()
    {
        Vector2 mouseAxis = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        mouseAxis *=mouseSensitivity;
        _currentRotation += mouseAxis;

        _currentRotation.y = Mathf.Clamp(_currentRotation.y, -90, 90);

        transform.localPosition += (Vector3)mouseAxis * weaponSwayAmount / 1000;

        transform.root.localRotation = Quaternion.AngleAxis(_currentRotation.x, Vector3.up);
        transform.parent.localRotation = Quaternion.AngleAxis(-_currentRotation.y, Vector3.right);
    }

    public void DetermineAim(){
        Vector3 target = normalPosition;
        if(Input.GetMouseButton(1)) {
            mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, 53f, fovSmooth);
            target = aimingPosition;
            chrosshairDefault.SetActive(false);
            chrosshair.SetActive(true);
        }else{
            mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, 73f, fovSmooth);
            chrosshair.SetActive(false);
            chrosshairDefault.SetActive(true);
        }

        Vector3 desiredposition = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * aimSmoothing);

        transform.localPosition = desiredposition;
    }

    void DetermineRecoil(){
        transform.localPosition -= Vector3.forward * 0.03f;

        if(randomizeRecoil){
            float xRecoil = Random.Range(-randomRecoilConstraints.x, randomRecoilConstraints.x);
            float yRecoil = Random.Range(-randomRecoilConstraints.y, randomRecoilConstraints.y);

            Vector2 recoilD = new Vector2(xRecoil, yRecoil);

            _currentRotation += recoilD;
        }else{
            int currenStep = clipSize + 1 - _currentAmmoClip;
            currenStep = Mathf.Clamp(currenStep, 0, recoilPattern.Length - 1);

            _currentRotation += recoilPattern[currenStep];
        }
    }

    IEnumerator ShootGun(){
        DetermineRecoil();
        RayCastEnemy();
        muzzle.Play();
        animationsGun.SetBool("fire", true);
        yield return new WaitForSeconds(fireRate);
        animationsGun.SetBool("fire", false);
        _canShoot = true;
    }

    void RayCastEnemy(){
        RaycastHit hit;
        if(Physics.Raycast(transform.parent.position, transform.parent.forward, out hit, distance)){
            if(hit.transform.tag == "Enemy"){
                try{
                    GameObject impact = Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(impact, 2f);
                    Debug.Log("Hit an enemy");
                    damage = Random.Range(19, 25);
                    hit.collider.GetComponent<HealthAIEnemy>().TakeDamage(damage);
                }catch{}
            }else{
                GameObject impactHit = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactHit, 2f);
            }
        }
    }
}
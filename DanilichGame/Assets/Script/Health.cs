using System.Net.Mime;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random=UnityEngine.Random;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    public float healthAmount = 100;
    public float healing;
    public Pause pause;

    [Header("Health UI")]
    public Text currentHealth;
    public Image healthImage;
    private Animation anim;

    [Header("Audio Settings")]
    public AudioClip hurtSound;

    private void Start(){
        anim = gameObject.GetComponent<Animation>();
    }

    private void Update()
    {
        if(healthAmount <= 0){
            pause.Die();
        }
        if(healthAmount < 20){
            anim.Play("healthAnimations");
        }
    }

    public void TakeDamage(int damage){
        healthAmount -= damage;
        anim.Play("bloody");
        //GetComponent<AudioSource>().PlayOneShot(hurtSound, 0.4f);
        if(healthAmount >= 0){
            currentHealth.text = "" + healthAmount + "%";
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "heartUp"){
            anim.Play("healthUp");
            if(healthAmount > 0){
                healthAmount += healing;
                if(healthAmount > 100){
                    healthAmount = 100;
                }
                currentHealth.text = "" + healthAmount + "%";
            }
            other.gameObject.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthAIEnemy : MonoBehaviour
{
    [Header("Health Settings")]
    public float healthAmount;
    public float healthMax = 100;

    [Header("UI")]
    public Slider slider;
    public GameObject healthBar;

    [Header("Animations")]
    public Animator animationsEnemy;

    [Header("Audio Settings")]
    public AudioClip hurtSound;

    private void Start(){
        animationsEnemy = GetComponent<Animator>();
        healthAmount=healthMax;
        slider.value = CalculateHealth();
    }

    private void Update(){
        slider.value = CalculateHealth();

        if(healthAmount < healthMax){
            healthBar.SetActive(true);
        }
        if(healthAmount <= 0){
            healthBar.SetActive(false);
            if (gameObject.GetComponent<EnemyAI>()){
                gameObject.GetComponent<EnemyAI>().die = true;
            }
            animationsEnemy.SetBool("isDead", true);
            Destroy(gameObject, 3.5f);
            healthAmount = 0;
            slider.value = 0;
        }
    }

    public void TakeDamage(int damage){
        healthAmount -= damage;
        if(healthAmount > 0){
            animationsEnemy.SetTrigger("isHit");
            GetComponent<AudioSource>().PlayOneShot(hurtSound, 0.3f);
                    
            if (gameObject.GetComponent<EnemyAI>())
                gameObject.GetComponent<EnemyAI>().overrideAttack = true;
        }
    }

    float CalculateHealth(){
        return healthAmount / healthMax;
    }
}

using System;
using System.Net.NetworkInformation;
using System.Threading;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("CharasterSettings")]
    public CharacterController controller;
    public float speed = 4f;
    public float runSpeed = 6f;
    public float sitSpeed = 2f;
    public bool isRunning;

    [Header("GroundSettings")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public float gravity = -9.81f;
    public float jumpHeight = 1f;
    public LayerMask groundMask;
    bool isGrounded;
    Vector3 velocity;

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip audioClipWalking;
    public AudioClip audioClipRunning;

    private void Start(){    
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClipWalking;
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0){
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)){
            if(Input.GetKey(KeyCode.LeftShift)){
                controller.Move(move * runSpeed * Time.deltaTime);
                isRunning = true;
            }
            else if(Input.GetKey(KeyCode.LeftControl)){
                controller.Move(move * sitSpeed * Time.deltaTime);
                isRunning = false;
            }
            else{
                controller.Move(move * speed * Time.deltaTime);
                isRunning = false;
            }
            PlayFootstepSounds();
        }
        else if (!(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
            audioSource.Pause();

            
        if(Input.GetButtonDown("Jump") && isGrounded){
            velocity.y = (float)Math.Sqrt(jumpHeight * -1.5f * gravity);
        }

        if(Input.GetKey(KeyCode.LeftControl)){
            controller.height = 1.3f;
        }else{
            controller.height = 2f;
        }


        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    private void PlayFootstepSounds()
    {
        if (isGrounded)
        {
            audioSource.clip = isRunning ? audioClipRunning : audioClipWalking;
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else if (audioSource.isPlaying)
                audioSource.Pause();
    }
}

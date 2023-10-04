using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationBody : MonoBehaviour
{
    public Animator body;

    void Start()
    {
        body = GetComponent<Animator>();
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)){
            if(Input.GetKey(KeyCode.LeftShift)){
                body.SetBool("run", true);
            }
            else if(Input.GetKey(KeyCode.LeftControl)){

            }
            else{
                body.SetBool("run", true);
            }
        }

        if (!(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))){
            body.SetBool("run", false);
        }
        
        if(Input.GetButtonDown("Jump")){
            body.SetTrigger("jump");
            body.SetBool("run", false);
        }

        if(Input.GetKey(KeyCode.LeftControl)){
            body.SetBool("sitDown", true);
            body.SetBool("sitDownWalk", false);
        }else{
            body.SetBool("sitDown", false);
        }
        
        if(Input.GetKey(KeyCode.W)  || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)){
            if(Input.GetKey(KeyCode.LeftControl)){
                body.SetBool("sitDownWalk", true);
            }else{
                body.SetBool("sitDownWalk", false);
            }
        }else{
            body.SetBool("sitDownWalk", false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heartUpAnimations : MonoBehaviour
{
    private Animation anim;
    void Start()
    {
        anim = gameObject.GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.Play("heartAnimation");
    }
}

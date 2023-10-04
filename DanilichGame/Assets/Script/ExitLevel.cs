using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitLevel : MonoBehaviour
{
    public Pause pause;

    void OnTriggerEnter(Collider col){
        if(col.gameObject.tag == "Player"){
            pause.LevelEnd();
        }
    }
}
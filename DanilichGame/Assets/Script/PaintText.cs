using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaintText : MonoBehaviour
{
    private string text;
    void Start()
    {
       text = GetComponent<Text>().text;
       GetComponent<Text>().text = "";
       StartCoroutine(TextCoroutine());
    }

    IEnumerator TextCoroutine(){
        foreach(char abc in text){
            GetComponent<Text>().text += abc;
            yield return new WaitForSeconds(0.05f);
        }
    }
}

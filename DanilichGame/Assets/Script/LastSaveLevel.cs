using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LastSaveLevel : MonoBehaviour
{
    void Start()
    {
        PlayerPrefs.SetInt("Last_Level", SceneManager.GetActiveScene().buildIndex);
    }
}

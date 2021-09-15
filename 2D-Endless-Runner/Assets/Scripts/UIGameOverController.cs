using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIGameOverController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
{
        if (Input.GetKeyDown(KeyCode.Space))
     {
       SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);   
     }
}
}

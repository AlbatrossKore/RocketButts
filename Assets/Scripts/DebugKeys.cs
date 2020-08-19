using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugKeys : MonoBehaviour
{
    #if UNITY_EDITOR
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         if (Input.GetKey(KeyCode.C))
        {
            // Stop collisions
            // Call UI Element to let user know debug key has been activated
        }
        else if (Input.GetKey(KeyCode.L))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); 
            // Call UI Element to let user know debug key has been activated 
        }
    }
 
    void OnCollisionEnter(Collision other)
    {
        
    }
    #endif
}

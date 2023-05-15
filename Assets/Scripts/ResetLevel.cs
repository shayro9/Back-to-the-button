using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetLevel : MonoBehaviour
{
    bool Fire1axisInUse = false;
    bool Fire2axisInUse = false;
    int SceneToLoad;
    private void Start()
    {
        SceneToLoad = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Reset Level") != 0 && !Fire1axisInUse)
        {
            Time.timeScale = 1;
            GetComponent<GetItem>().StartCoroutine("ResetLevel",SceneToLoad);
            Fire1axisInUse = true;
        }
        //reset the flag
        if (Input.GetAxis("Reset Level") == 0)
        {
            Fire1axisInUse = false;
        }

        if (Input.GetKeyDown(KeyCode.P) && !Fire2axisInUse)
        {
            Time.timeScale = 1;
            GetComponent<GetItem>().StartCoroutine("LoadLevel", 0);
            Fire2axisInUse = true;
        }
        //reset the flag
        if (Input.GetKeyUp(KeyCode.P))
        {
            Fire2axisInUse = false;
        }
    }
}

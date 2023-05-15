using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetris : MonoBehaviour
{
    public GameObject[] doors_to_open;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(allDone())
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject p = transform.GetChild(i).gameObject;
                p.GetComponents<MovingPlatform>()[1].always_on = true;
            }
            foreach(GameObject d in doors_to_open)
                d.GetComponent<Door>().keys_needed = 0;
        }
    }

    public bool allDone()
    {
        bool temp = true;
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject p = transform.GetChild(i).gameObject;
            if (!p.GetComponent<MovingPlatform>().done)
            {
                temp = false;
            }
        }
        return temp;
    }
}

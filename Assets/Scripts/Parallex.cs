using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallex : MonoBehaviour
{
    Vector2 start_pos;
    GameObject cam;
    public float parallex_effect;
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Camera");
        start_pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float distx = cam.transform.position.x * parallex_effect;
        float disty = cam.transform.position.y * parallex_effect;

        transform.position = new Vector3(start_pos.x + distx, start_pos.y + disty);
    }
}

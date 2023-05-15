using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class SecretDoor : MonoBehaviour
{
    public GameObject button;
    private bool currently_Open = false;
    public bool object_inside;
    public bool open_door;
    Color invisible;
    private void Start()
    {
        invisible = gameObject.GetComponent<SpriteShapeRenderer>().color;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10 || collision.gameObject.layer == 16 || collision.gameObject.layer == 15)
            object_inside = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10 || collision.gameObject.layer == 16 || collision.gameObject.layer == 15)
        {
            Debug.Log(collision.name);
            object_inside = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (button.GetComponent<Button>().is_pressed)
            open_door = true;
        else
            open_door = false;
                

        if (!open_door)
            open_door = object_inside;

        if (open_door)
        {
            invisible.a = 0;
            gameObject.GetComponent<SpriteShapeRenderer>().color = invisible;
            gameObject.GetComponent<Collider2D>().isTrigger = true;
            gameObject.layer = 17;
        }
        else
        {
            invisible.a = 1;
            gameObject.GetComponent<SpriteShapeRenderer>().color = invisible;
            gameObject.GetComponent<Collider2D>().isTrigger = false;
            gameObject.layer = 14;
        }

        if (currently_Open != open_door)
        {
            if (open_door)
            {
                GameObject.Find("AudioSource").GetComponent<AudioController>().PlayEffect("Open_Door");
            }
            else
            {
                GameObject.Find("AudioSource").GetComponent<AudioController>().PlayEffect("Close_Door");
            }
            currently_Open = open_door;
        }

    }
}

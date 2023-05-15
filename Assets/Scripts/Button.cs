using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public bool is_pressed;
    public bool left_btn;

    private void Start()
    {
        left_btn = true;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (left_btn)
        {
            if (collision.gameObject.tag.Equals("Player"))
            {
                left_btn = false;
                Invoke("Press", Time.deltaTime * 2);
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            left_btn = false;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        left_btn = true;
        Invoke("UnPress", Time.deltaTime * 2);
    }

    public void UnPress()
    {
        if (left_btn && is_pressed)
        {
            is_pressed = false;
            gameObject.GetComponent<Animator>().SetBool("Press", false);
            GameObject.Find("AudioSource").GetComponent<AudioController>().PlayEffect("Button");
        }
    }
    public void Press()
    {
        if (!left_btn)
        {
            is_pressed = true;
            gameObject.GetComponent<Animator>().SetBool("Press", true);

            GameObject.Find("AudioSource").GetComponent<AudioController>().PlayEffect("Button");
        }
    }
}

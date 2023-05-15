using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Door : MonoBehaviour
{
    public enum LockType { Button, Key, Both, Either };
    public LockType lock_type;
    public int keys_needed;
    [Serializable]
    public struct Butn
    {
        public GameObject objct;
        public bool opens;
    }
    public bool need_all_buttons;
    public Butn[] buttons;
    private Animator anim;
    private bool currently_Open = false;
    public bool object_inside;
    public bool open_door;

    private void Start()
    {
        anim = GetComponent<Animator>();
        if (lock_type != LockType.Button)
        {
            if(keys_needed != 0)
                anim.SetBool("Key", true);
        }
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
            object_inside = false;
        }
    }
    // Update is called once per frame
    void Update()
    {        
        if(need_all_buttons)
            open_door = true;
        else
            open_door = false;

        switch(lock_type)
        {
            case LockType.Button:
                foreach (Butn b in buttons)
                {
                    if (need_all_buttons)
                    {
                        if (b.opens && !b.objct.GetComponent<Button>().is_pressed)
                        {
                            open_door = false;
                            break;
                        }
                        else
                            open_door = true;
                    }
                    else if (b.opens && b.objct.GetComponent<Button>().is_pressed)
                        open_door = true;
                    if (!b.opens && b.objct.GetComponent<Button>().is_pressed)
                    {
                        open_door = false;
                        break;
                    }
                }
                break;
            case LockType.Key:
                if (keys_needed == 0)
                {
                    open_door = true;
                }
                else
                    open_door = false;
                break;
            case LockType.Both:
                if (keys_needed == 0)
                {
                    open_door = true;
                    foreach (Butn b in buttons)
                    {
                        if (need_all_buttons)
                        {
                            if (b.opens && !b.objct.GetComponent<Button>().is_pressed)
                            {
                                open_door = false;
                                break;
                            }
                            else
                                open_door = true;
                        }
                        else if (b.opens && b.objct.GetComponent<Button>().is_pressed)
                            open_door = true;
                        if (!b.opens && b.objct.GetComponent<Button>().is_pressed)
                        {
                            open_door = false;
                            break;
                        }
                    }
                }
                else
                    open_door = false;
                break;
            case LockType.Either:
                if (keys_needed == 0)
                {
                    open_door = true;
                    break;
                }
                else
                    open_door = false;
                foreach (Butn b in buttons)
                {
                    if (need_all_buttons)
                    {
                        if (b.opens && !b.objct.GetComponent<Button>().is_pressed)
                        {
                            open_door = false;
                            break;
                        }
                        else
                            open_door = true;
                    }
                    else if (b.opens && b.objct.GetComponent<Button>().is_pressed)
                        open_door = true;
                    if (!b.opens && b.objct.GetComponent<Button>().is_pressed)
                    {
                        open_door = false;
                        break;
                    }
                }
                break;
        }

        if (!open_door)
            open_door = object_inside;

        if (open_door)
        {
            gameObject.GetComponent<Collider2D>().isTrigger = true;
            gameObject.layer = 17;
        }
        else
        {
            gameObject.GetComponent<Collider2D>().isTrigger = false;
            gameObject.layer = 14;
        }

        if (currently_Open != open_door)
        {
            if (open_door)
            {
                anim.SetTrigger("Open");
                GameObject.Find("AudioSource").GetComponent<AudioController>().PlayEffect("Open_Door");
            }
            else
            {
                anim.SetTrigger("Close");
                GameObject.Find("AudioSource").GetComponent<AudioController>().PlayEffect("Close_Door");
            }
            currently_Open = open_door;
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (lock_type != LockType.Button )
        {
            if (collision.gameObject.tag.Equals("Player"))
            {
                if (collision.gameObject.GetComponent<Inventory>().isItemInInventory("Key"))
                {
                    int keys_on_player = collision.gameObject.GetComponent<Inventory>().getItemAmount("Key");
                    if (keys_needed != 0)
                    {
                        if (keys_on_player >= keys_needed)
                        {
                            collision.gameObject.GetComponent<Inventory>().RemoveItem("Key", keys_needed);
                            if (lock_type == LockType.Either)
                                lock_type = LockType.Key;
                            keys_needed = 0;
                        }
                        else
                        {
                            collision.gameObject.GetComponent<Inventory>().RemoveItem("Key", keys_on_player);
                            keys_needed -= keys_on_player;
                        }
                    }
                }
            }
        }
    }
}

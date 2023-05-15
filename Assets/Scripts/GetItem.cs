using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GetItem : MonoBehaviour
{
    [SerializeField]
    public Item item_props;
    public enum Action { Collect, Next_Level, Specific_Level}
    public int Level_index;
    public Action action;

    private void Start()
    {
        GameObject.Find("AudioSource").GetComponent<AudioController>().PlayEffect("Open_Door");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Equals("Player"))
        {
            switch(action)
            {
                case Action.Collect:
                    collision.GetComponent<Inventory>().AddItem(item_props.name,item_props.amount);
                    Destroy(gameObject.transform.parent.gameObject);
                    break;
                case Action.Next_Level:
                    LoadNextLevel();
                    break;
                case Action.Specific_Level:
                    StartCoroutine(LoadLevel(Level_index));
                    break;
            }
        }
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }
    IEnumerator LoadLevel(int level_index)
    {
        GameObject.Find("AudioSource").GetComponent<AudioController>().PlayEffect("Close_Door");
        GetComponentInChildren<Animator>().SetTrigger("Load_Level");

        yield return new WaitForSecondsRealtime(1);

        SceneManager.LoadScene(level_index);
    }
    public IEnumerator ResetLevel(int level_index)
    {
        GameObject.Find("AudioSource").GetComponent<AudioController>().PlayEffect("Close_Door");
        GetComponentInChildren<Animator>().SetTrigger("Load_Level");

        yield return new WaitForSecondsRealtime(0.25f);

        SceneManager.LoadScene(level_index);
    }
}

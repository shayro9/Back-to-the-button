using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuWithoutUI : MonoBehaviour
{
    int pointer_index = 0;
    public GameObject titles;
    public GameObject pointer;

    public float pointer_space = 1;

    public string title_name;

    private void Start()
    {
        title_name = titles.transform.GetChild(pointer_index).GetComponent<TextMesh>().text;
        MovePointer(pointer_index, true);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (pointer_index > 0)
            {
                pointer_index--;
                MovePointer(pointer_index, true);
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (pointer_index < titles.transform.childCount-1)
            {
                pointer_index++;
                MovePointer(pointer_index,false);
            }
        }
        if(Input.GetKeyDown(KeyCode.Return))
        {
            Load(title_name);
        }
    }
    public void MovePointer(int index,bool up)
    {
        title_name = titles.transform.GetChild(pointer_index).GetComponent<TextMesh>().text;
        Vector2 title_pos = titles.transform.GetChild(index).transform.position;
        if(up)
            pointer.transform.position = new Vector3(title_pos.x - pointer_space, title_pos.y + 0.1f);
        else
            pointer.transform.position = new Vector3(title_pos.x - pointer_space, title_pos.y + 0.1f);
    }
    public void Load(string scene)
    {
        switch(scene)
        {
            case "START":
                StartCoroutine(LoadLevel(1));
                break;
            case "BONUS LEVELS":
                StartCoroutine(LoadLevel(16));
                break;
            case "CREDITS":
                StartCoroutine(LoadLevel(15));
                break;
            case "OPTIONS":
                StartCoroutine(LoadLevel(23));
                break;
            case "QUIT":
                Application.Quit();
                break;
            case "Back":
                StartCoroutine(LoadLevel(0));
                break;
            default:
                try
                {
                    StartCoroutine(LoadLevel(17+pointer_index));
                }
                finally
                {
                }
                break;
        }
    }

    public IEnumerator LoadLevel(int level_index)
    {
        GetComponentInChildren<Animator>().SetTrigger("Load_Level");

        yield return new WaitForSecondsRealtime(1);

        SceneManager.LoadScene(level_index);
    }
}

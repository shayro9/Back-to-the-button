using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeMenu : MonoBehaviour
{
    int pointer_index = 9;
    public GameObject lines;
    public GameObject pointer;

    private void Start()
    {
        MovePointer(pointer_index, true);
    }
    // Update is called once per frame
    void Update()
    {
        if (GetComponent<MainMenuWithoutUI>().title_name == "VOLUME")
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (pointer_index < lines.transform.childCount - 1)
                {
                    pointer_index++;
                    MovePointer(pointer_index, true);
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (pointer_index > -1)
                {
                    pointer_index--;
                    MovePointer(pointer_index, false);
                }
            }
        }
    }
    public void MovePointer(int index,bool right)
    {
        if (index > -1)
        {
            if (right)
            {
                GameObject last_l = lines.transform.GetChild(index).gameObject;
                last_l.GetComponent<SpriteRenderer>().color = Color.green;
            }
            else
            {
                GameObject last_l = lines.transform.GetChild(index + 1).gameObject;
                last_l.GetComponent<SpriteRenderer>().color = Color.black;
            }
            GameObject l = lines.transform.GetChild(index).gameObject;
            Vector2 line_pos = l.transform.position;
            pointer.transform.position = new Vector3(line_pos.x, line_pos.y - 1.5f);
        }
        else
        {
            GameObject last_l = lines.transform.GetChild(index + 1).gameObject;
            last_l.GetComponent<SpriteRenderer>().color = Color.black;
        }
        ChangeVolume(index);
    }
    public void ChangeVolume(int pointer_index)
    {
        AudioListener.volume = (pointer_index + 1) * 0.1f;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> items;

    GameObject grid_panel;
    GameObject key_prefab;

    private void Awake()
    {
        items = new List<Item>();
        key_prefab = (GameObject)Resources.LoadAll("KeyUI")[0];
        if(GameObject.Find("Keys_UI") != null)
            grid_panel = GameObject.Find("Keys_UI").transform.GetChild(0).gameObject;
    }

    public void AddItem(string name, int amount)
    {
        bool item_exists = false;
        if (items.Count > 0)
        {
            foreach (Item i in items)
            {
                if (i.item_name == name)
                {
                    i.amount++;
                    item_exists = true;
                    Quaternion q = new Quaternion(0, 0, 0, 0);
                    GameObject new_key = Instantiate(key_prefab, grid_panel.transform.position, q, grid_panel.transform);
                    GameObject.Find("AudioSource").GetComponent<AudioController>().PlayEffect("Key");
                }
            }
        }
        if (!item_exists)
        {
            Item temp = (Item)ScriptableObject.CreateInstance(typeof(Item));
            temp.item_name = name;
            temp.amount = amount;
            items.Add(temp);

            Quaternion q = new Quaternion(0, 0, 0, 0);
            GameObject new_key = Instantiate(key_prefab, grid_panel.transform.position, q, grid_panel.transform);
            GameObject.Find("AudioSource").GetComponent<AudioController>().PlayEffect("Key");
        }
    }
    public void RemoveItem(string name, int amount)
    {
        foreach (Item i in items)
        {
            if (i.item_name == name)
            {
                if (i.amount > amount)
                    i.amount -= amount;
                else
                {
                    items.Remove(i);
                    break;
                }
            }
        }
        for(int i = 0; i<amount;i++)
        {
            Destroy(grid_panel.transform.GetChild(i).gameObject);
        }
    }
    public bool isItemInInventory(string name)
    {
        foreach (Item i in items)
        {
            if (i.item_name == name)
            {
                return true;
            }
        }
        return false;
    }

    public int getItemAmount(string name)
    {
        return items.Find(i => i.item_name == name).amount;
    }
}

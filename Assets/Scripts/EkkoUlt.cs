using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class EkkoUlt : MonoBehaviour
{
    public GameObject shadow;
    public Sprite[] Shadow_sprites;

    public float time_window;
    public int return_speed;
    public GameObject pp_effect;
    public GameObject ui_effect_canvas;

    Vector2 current_pos;
    Queue<Vector2> past_pos;

    SpriteRenderer SpriteRenderer;
    Sprite current_anim;
    Queue<Sprite> past_anim;

    float current_localscale;
    Queue<float> past_localScale;

    float t = 0;
    LayerMask mask;

    Stack<Vector2> recall;
    bool restart_recall = false;

    bool Fire1axisInUse = false;

    public float cooldown = 0;
    private float cooldownCounter = 0;
    private bool onCooldown = false;

    // Start is called before the first frame update
    void Start()
    {
        Shadow_sprites = Resources.LoadAll<Sprite>("Ghost");
        mask = LayerMask.GetMask("Door", "Platform");
        SpriteRenderer = shadow.GetComponent<SpriteRenderer>();
        past_anim = new Queue<Sprite>();
        past_pos = new Queue<Vector2>();
        recall = new Stack<Vector2>();
        past_localScale = new Queue<float>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Fire1") != 0 && !Fire1axisInUse && !onCooldown)
        {
            ReturnToShadow();
            Fire1axisInUse = true;
        }
        //reset the flag
        if (Input.GetAxis("Fire1") == 0)
        {
            Fire1axisInUse = false;
        }

        current_pos = gameObject.transform.position;
        past_pos.Enqueue(current_pos);

        current_anim = gameObject.GetComponent<SpriteRenderer>().sprite;
        past_anim.Enqueue(current_anim);

        current_localscale = gameObject.transform.localScale.x;
        past_localScale.Enqueue(current_localscale);

        Invoke("SetShadowPos", time_window);
        //StartCoroutine("SetShadowPos");

        if (onCooldown)
        {
            cooldownCounter -= Time.deltaTime;
        }
        if (cooldownCounter <= 0)
        {
            onCooldown = false;
        }

        if (recall.Count > 0)
        {
            //GetComponent<Jump>().canDoubleJump = false;
            GetComponent<Jump>().grounded_remmber = 0;

            Time.timeScale = 0.075f;
            t += Time.deltaTime/0.25f;
            if(pp_effect.GetComponent<Volume>().weight < 1)
                pp_effect.GetComponent<Volume>().weight += Mathf.Lerp(0, 1, t);

            ui_effect_canvas.SetActive(true);
            ui_effect_canvas.transform.GetChild(0).GetComponent<Animator>().SetTrigger("VHS");

            gameObject.GetComponentInChildren<TrailRenderer>().emitting = false;
            transform.position = recall.Pop();
            restart_recall = true;
        }
        else
        {
            if (restart_recall)
            {
                GetComponent<Jump>().canDoubleJump = false;
                GetComponent<Jump>().grounded_remmber = 0;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;

                var door_touching = Physics2D.OverlapCircleAll(transform.position, 0.05f, mask);
                if (door_touching.Length != 0)
                {
                    ShadowBugControl(door_touching[0]);
                }

                ui_effect_canvas.SetActive(false);

                t = 0;
                pp_effect.GetComponent<Volume>().weight = 0;
                Time.timeScale = 1;
                gameObject.GetComponentInChildren<TrailRenderer>().emitting = true;
                restart_recall = false;
            }
        }
    }

    public void SetShadowPos()
    {
        shadow.transform.position = past_pos.Dequeue();
        int sprite_num = int.Parse(past_anim.Dequeue().name.Substring(7));
        SpriteRenderer.sprite = Shadow_sprites[sprite_num];
        shadow.transform.localScale = new Vector3(past_localScale.Dequeue(), 1, 1);
    }

    /*public IEnumerator SetShadowPos()
    {
        yield return new WaitForSecondsRealtime(time_window);
        if (Time.timeScale != 0)
        {
            shadow.transform.position = past_pos.Dequeue();
            SpriteRenderer.sprite = past_anim.Dequeue();
            shadow.transform.localScale = past_localScale.Dequeue();
        }
    }*/

    public void ReturnToShadow()
    {
        cooldownCounter = cooldown;
        onCooldown = true;

        Vector2[] q = past_pos.ToArray();
        for (int i = 0; i < q.Length; i += return_speed)
        {
            recall.Push(q[i]);
        }
        GameObject.Find("AudioSource").GetComponent<AudioController>().PlayEffect("Rewind");
    }

    public void ShadowBugControl(Collider2D door)
    {
        transform.position = door.transform.Find("Popik").transform.position;
    }
}

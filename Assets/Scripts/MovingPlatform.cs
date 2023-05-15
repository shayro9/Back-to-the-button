using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public enum MoveDiraction { Horizontal,Vertical};
    public MoveDiraction diraction;
    public float distance;
    public GameObject button;
    public bool always_on;
    public bool retracting = true;
    public bool repeating = true;
    public float speed = 3;
    public float reverse_speed = 3;
    public bool stop_at_the_end;

    [HideInInspector]
    public bool done = false;
    bool emergency_stop;
    bool activate_platform;
    public bool reverse = false;
    Vector2 starting_pos;
    LayerMask mask;
    Animator animator;
    ParticleSystem particle;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        particle = GetComponentInChildren<ParticleSystem>();
        starting_pos = transform.position;
        mask = LayerMask.GetMask("Button", "Door");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!emergency_stop)
        {
            if (!always_on)
            {
                if (button != null)
                {
                    if (button.GetComponent<Button>().is_pressed)
                        activate_platform = true;
                    else
                        activate_platform = false;
                }
            }
            else
                activate_platform = true;
        }
        else
            activate_platform = false;

        if (done && retracting && stop_at_the_end)
        {
            retracting = false;
        }
        if (animator != null)
            animator.SetBool("LiftActive", activate_platform);
        if (particle != null)
        {
            ParticleSystem.EmissionModule emission = particle.emission;
            if (activate_platform)
            {
                if(particle.emission.enabled == false)
                    particle.Clear();
                emission.enabled = true;
            }
            else
            {
                emission.enabled = false;
            }
        }

        switch (diraction)
        {
            case MoveDiraction.Horizontal:
                if (activate_platform)
                {
                    if (reverse)
                    {
                        if (transform.position.x > starting_pos.x || transform.position.x > starting_pos.x + distance)
                            gameObject.transform.position = new Vector2(transform.position.x - reverse_speed * Time.deltaTime, transform.position.y);
                        else if (!stop_at_the_end)
                        {
                            reverse = false;
                        }
                        else
                            done = true;
                    }
                    else
                    {
                        if (transform.position.x < starting_pos.x + distance)
                            gameObject.transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);
                        else if (repeating)
                        {
                            reverse = true;
                        }
                        else
                            done = true;
                    }
                }
                else
                    if (retracting)
                    {
                        if (!checkRear())
                        {
                            if (transform.position.x > starting_pos.x)
                            {
                                gameObject.transform.position = new Vector2(transform.position.x - reverse_speed * Time.deltaTime, transform.position.y);
                                done = false;
                                if (transform.position.x <= starting_pos.x)
                                    emergency_stop = false;
                            }
                        }
                        else
                            emergency_stop = false;
                    }
                break;
            case MoveDiraction.Vertical:
                if (activate_platform)
                {
                    if (reverse)
                    {
                        if (transform.position.y > starting_pos.y || transform.position.y > starting_pos.y + distance)
                            gameObject.transform.position = new Vector2(transform.position.x, transform.position.y - reverse_speed * Time.deltaTime);
                        else if (!stop_at_the_end)
                        {
                            reverse = false;
                        }
                        else
                            done = true;
                    }
                    else
                    {
                        if (transform.position.y < starting_pos.y + distance)
                            gameObject.transform.position = new Vector2(transform.position.x, transform.position.y + speed * Time.deltaTime);
                        else if (repeating)
                        {
                            reverse = true;
                        }
                        else
                            done = true;
                    }
                }
                else
                    if (retracting)
                    {
                        if (transform.position.y > starting_pos.y)
                        {
                            gameObject.transform.position = new Vector2(transform.position.x, transform.position.y - reverse_speed * Time.deltaTime);
                            done = false;
                        }
                    }
                break;
        }       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!collision.gameObject.tag.Equals("Player") && !collision.gameObject.tag.Equals("Key"))
        {
            emergency_stop = true;
        }
    }

    public bool checkRear()
    {
        float len = gameObject.GetComponent<Collider2D>().bounds.extents.x + 0.15f;
        Debug.DrawRay(transform.position, Vector2.left * Mathf.Sign(distance) * len, Color.green);
        Collider2D obs = Physics2D.Raycast(transform.position, Vector2.left * Mathf.Sign(distance), len,mask).collider;
        if(obs != null)
            return !obs.isTrigger;
        return false;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (diraction == MoveDiraction.Horizontal)
        {
            if (collision.gameObject.layer == 10 || collision.gameObject.layer == 15)
                collision.transform.parent = gameObject.transform;
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (diraction == MoveDiraction.Horizontal)
        {
            if (collision.gameObject.layer == 10 || collision.gameObject.layer == 15)
                collision.transform.parent = null;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public Transform rayStart;
    public GameObject crystalEffect;

    private Rigidbody body;
    private bool walkingRight = true;
    private Animator animator;
    private GameManager manager;
    private float multiplier = 2f;

    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        manager = FindObjectOfType<GameManager>();

        float y = 2f;

        for (int i = 0; i < 40 / 5; i++)
        {
            y *= 1.05f;
        }

        print(y);
    }

    private void FixedUpdate()
    {
        if (!manager.IsGameStarted())
        {
            return;
        }
        else
        {
            animator.SetTrigger("gameStarted");
        }

        body.transform.position = transform.position + transform.forward * multiplier * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(manager.IsGameStarted() && Input.GetKeyDown(KeyCode.Space))
        {
            Switch();
        }

        RaycastHit hit;

        if (!Physics.Raycast(rayStart.position, -transform.up, out hit, Mathf.Infinity) && transform.position.y < 0)
        {
            animator.SetTrigger("isFalling");
            
        }

        if (transform.position.y < -2f)
        {
            manager.EndGame();
        }
    }

    private void Switch()
    {
        walkingRight = !walkingRight;

        if(walkingRight)
        {
            transform.rotation = Quaternion.Euler(0f, 45f, 0f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, -45f, 0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Crystal")
        {
            manager.IncreaseScore();

            GameObject g = Instantiate(crystalEffect, rayStart.transform.position, Quaternion.identity);
            Destroy(g, 2f);
            Destroy(other.gameObject);

            if(manager.GetScore() % 5 == 0)
            {
                multiplier *= 1.05f;
            }
        }
    }
}

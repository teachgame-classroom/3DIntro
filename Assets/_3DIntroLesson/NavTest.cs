using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class NavTest : MonoBehaviour
{
    GameObject player;
    NavMeshAgent nav;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!nav.enabled) return;

        if (Input.GetKeyDown(KeyCode.K))
        {
            anim.SetTrigger("Dead");
            nav.isStopped = true;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            nav.isStopped = !nav.isStopped;
        }

        nav.SetDestination(player.transform.position);

        if(nav.isStopped == true || nav.remainingDistance < nav.stoppingDistance)
        {
            anim.SetBool("Move", false);
        }
        else
        {
            anim.SetBool("Move", true);
        }
    }

    public void Die()
    {
        anim.SetTrigger("Dead");
        nav.isStopped = true;
    }

    void StartSinking()
    {
        nav.enabled = false;
        GetComponent<Collider>().enabled = false;
        Destroy(gameObject, 2f);
    }
}

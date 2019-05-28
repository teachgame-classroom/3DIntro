using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicTest : MonoBehaviour
{
    public bool useFixedUpdate;
    public float force;
    Rigidbody body;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!useFixedUpdate) body.AddForce(Vector3.forward * force);

        Debug.Log("Fixed:" + useFixedUpdate + "," + body.velocity);
    }

    void FixedUpdate()
    {
        if(useFixedUpdate) body.AddForce(Vector3.forward * force);
    }
}

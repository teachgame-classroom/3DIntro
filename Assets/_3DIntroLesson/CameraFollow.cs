using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform followTarget;
    public float smooth = 5;

    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - followTarget.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetCamPos = followTarget.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smooth * Time.deltaTime);
    }
}

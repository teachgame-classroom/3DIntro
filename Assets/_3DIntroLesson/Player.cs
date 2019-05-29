using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum MoveType { WASD, MouseClick }

public class Player : MonoBehaviour
{
    public MoveType moveType;
    public int loopCount = 100;
    public float moveSpeed = 6;

    private float rayLength = 40;
    private int floorMask;

    private Rigidbody body;
    private Vector3 movement = Vector3.zero;
    private Vector3 xzPlaneVec = new Vector3(1, 0, 1);

    private NavMeshAgent nav;

    private Transform shotPos;
    private Light light;
    private LineRenderer line;

    private float fireInterval = 0.1f;
    private float lastFireTime;
    private int shootTimerHandler;


    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
        floorMask = LayerMask.GetMask("Floor");

        shotPos = transform.Find("GunBarrelEnd");

        light = GetComponentInChildren<Light>(true);
        line = GetComponentInChildren<LineRenderer>(true);
    }

    void Update()
    {
        if(moveType == MoveType.MouseClick)
        {
            Turning(false);

            if(Input.GetMouseButtonDown(1))
            {
                MouseClickMove();
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (moveType == MoveType.WASD)
        {
            Move();
            Turning();
        }

        if(Input.GetMouseButton(0))
        {
            Shoot();
        }

    }

    void Shoot()
    {
        if (Time.realtimeSinceStartup - lastFireTime < fireInterval) return;

        lastFireTime = Time.realtimeSinceStartup;

        light.enabled = true;
        line.enabled = true;

        RaycastHit hit;

        if(Physics.Raycast(shotPos.position, shotPos.forward, out hit, 60))
        {
            NavTest target = hit.transform.GetComponent<NavTest>();

            if(target)
            {
                target.Die();
            }
            line.SetPosition(1, transform.InverseTransformPoint(hit.point));
        }
        else
        {
            line.SetPosition(1, transform.InverseTransformPoint(shotPos.position + shotPos.forward * 20));
        }

        Scheduler.instance.Schedule(fireInterval / 5, false, TurnOffGunEffect);
    }

    void MouseClickMove()
    {
        // Create a ray from the mouse cursor on screen in the direction of the camera.
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Create a RaycastHit variable to store information about what was hit by the ray.
        RaycastHit floorHit;

        // Perform the raycast and if it hits something on the floor layer...
        if (Physics.Raycast(camRay, out floorHit, rayLength, floorMask))
        {
            nav.SetDestination(floorHit.point);

            Vector3 playerToMouse = floorHit.point - transform.position;

            playerToMouse.y = 0f;

            Quaternion newRotatation = Quaternion.LookRotation(playerToMouse);

            transform.rotation = newRotatation;
        }
    }

    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 cam_forward = Vector3.Scale(Camera.main.transform.forward, xzPlaneVec).normalized;
        Vector3 cam_right = Camera.main.transform.right;
        movement = (cam_forward * v + cam_right * h).normalized;

        body.MovePosition(transform.position + movement * moveSpeed * Time.deltaTime);
    }

    void Turning(bool useRigidBody = true)
    {
        // Create a ray from the mouse cursor on screen in the direction of the camera.
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Create a RaycastHit variable to store information about what was hit by the ray.
        RaycastHit floorHit;

        // Perform the raycast and if it hits something on the floor layer...
        if (Physics.Raycast(camRay, out floorHit, rayLength, floorMask))
        {
            // Create a vector from the player to the point on the floor the raycast from the mouse hit.
            Vector3 playerToMouse = floorHit.point - transform.position;

            // Ensure the vector is entirely along the floor plane.
            playerToMouse.y = 0f;

            // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
            Quaternion newRotatation = Quaternion.LookRotation(playerToMouse);
            if(useRigidBody)
            {
                // Set the player's rotation to this new rotation.
                body.MoveRotation(newRotatation);
            }
            else
            {
                transform.rotation = newRotatation;
            }
        }
    }
    
    void TurnOffGunEffect()
    {
        light.enabled = false;
        line.enabled = false;
    }
}

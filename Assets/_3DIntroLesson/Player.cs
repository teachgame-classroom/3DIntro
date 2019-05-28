using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int loopCount = 100;
    public float moveSpeed = 6;

    private float rayLength = 20;
    private int floorMask;

    private Rigidbody body;
    private Vector3 movement = Vector3.zero;
    private Vector3 xzPlaneVec = new Vector3(1, 0, 1);



    private int update_count;
    private int fixedupdate_count;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        floorMask = LayerMask.GetMask("Floor");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        update_count++;
        //Debug.Log("Update Count:" + update_count + ",Realtime:" + Time.realtimeSinceStartup + "," + "Delta Time:" + Time.deltaTime);

        Move();
        Turning();
        //body.AddForce(movement * 20);
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

    void Turning()
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

            // Set the player's rotation to this new rotation.
            body.MoveRotation(newRotatation);
        }

}
}

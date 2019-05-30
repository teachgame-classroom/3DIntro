using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform followTarget;
    public Transform lookAtTarget;

    public bool lookAt;

    public float smooth = 5;

    public float mouseSensitivity_X = 10;
    public float mouseSensitivity_Y = 2;

    private Transform armX;
    private Transform armY;
    private Transform armZ;
    private Transform camTrans;
    private Camera cam;

    private float fov = 60;
    private float yaw_root;
    private float yaw;
    private float pitch;

    private Quaternion targetRotYaw_root;
    private Quaternion targetRotYaw;
    private Quaternion targetRotPitch;

    // Start is called before the first frame update
    void Start()
    {
        armX = transform.Find("Rig_ArmX");
        armY = armX.Find("Rig_ArmY");
        armZ = armY.Find("Rig_ArmZ");
        cam = GetComponentInChildren<Camera>();
        camTrans = cam.transform;

        SetFollowTarget(followTarget);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            float newYaw = Mathf.Repeat(yaw + 30, 360);
            SetYawAngle(newYaw);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            float newPitch = Mathf.Repeat(pitch + 30, 360);
            SetPitchAngle(newPitch);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            float newYaw = Mathf.Repeat(yaw - 30, 360);
            SetYawAngle(newYaw);
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            float newPitch = Mathf.Repeat(pitch - 30, 360);
            SetPitchAngle(newPitch);
        }

        if(!lookAt)
        {
            MoveCameraByMouse();
            camTrans.localRotation = Quaternion.Lerp(camTrans.localRotation, targetRotPitch, smooth * Time.deltaTime);
            armX.localRotation = Quaternion.Lerp(armX.localRotation, targetRotYaw, smooth * Time.deltaTime);
        }
        else
        {
            LookAt(lookAtTarget);
            //camTrans.rotation = Quaternion.Lerp(camTrans.rotation, targetRotPitch, smooth * Time.deltaTime);
        }

        transform.position = Vector3.Lerp(transform.position, followTarget.position, smooth * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotYaw_root, smooth * Time.deltaTime);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, smooth * Time.deltaTime);
    }

    public void SetFollowTarget(Transform target)
    {
        followTarget = target;

        targetRotYaw_root = Quaternion.LookRotation(followTarget.forward);

        yaw_root = targetRotYaw_root.eulerAngles.y;

        ICameraFollowable followable = followTarget.GetComponent<ICameraFollowable>();

        if(followable != null)
        {
            followable.cameraFollow = this;
        }
    }

    public void MoveCameraByMouse()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        Debug.Log(mouseX + "," + mouseY);
        float deltaYaw = mouseX * mouseSensitivity_X;
        float deltaPitch = -mouseY * mouseSensitivity_Y;

        SetRootYawAngle(yaw_root + deltaYaw);
        SetPitchAngle(pitch + deltaPitch);

        fov = 60;
    }

    public void StartLookAt(float duration = 2)
    {

        lookAt = true;
        Scheduler.instance.Schedule(duration, false, () => { lookAt = false; });
    }

    public void LookAt(Transform target)
    {
        Vector3 lookDirection = target.position - camTrans.position;
        Vector3 lookDirection_Yaw = Vector3.Scale(new Vector3(1, 0, 1), lookDirection);
        Vector3 lookDirection_Pitch = Vector3.Scale(new Vector3(0, 1, 1), lookDirection);

        Quaternion lookRotationYaw = Quaternion.LookRotation(lookDirection_Yaw);
        Quaternion lookRotationPitch = Quaternion.LookRotation(lookDirection_Pitch);

        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);

        targetRotYaw_root = lookRotationYaw;
        targetRotPitch = lookRotation;

        SetYawAngle(0);

        fov = 30;
    }

    public void SetTargetAngle(float yaw, float pitch)
    {
        SetYawAngle(yaw);
        SetPitchAngle(pitch);
    }

    public void SetYawAngle(float yaw)
    {
        this.yaw = yaw;
        targetRotYaw = Quaternion.Euler(0, this.yaw, 0);
    }

    public void SetPitchAngle(float pitch)
    {
        this.pitch = pitch;
        targetRotPitch = Quaternion.Euler(this.pitch, 0, 0);
    }

    public void SetRootYawAngle(float yaw)
    {
        this.yaw_root = yaw;
        targetRotYaw_root = Quaternion.Euler(0, this.yaw_root, 0);
    }
}

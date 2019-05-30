using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArmDirection { X, Y, Z }

public class SpringArm : MonoBehaviour
{
    public ArmDirection armDirection;
    public float relaxLength = 5;

    private Transform startPosTrans;
    private float length;

    private int layerMask;



    // Start is called before the first frame update
    void Start()
    {
        startPosTrans = transform.parent;
        layerMask = LayerMask.GetMask(new string[] { "Floor", "Obstacle" });
    }

    // Update is called once per frame
    void Update()
    {
        length = CalculateLength(relaxLength);
        transform.position = Vector3.Lerp(transform.position, GetEndPos(), 5 * Time.deltaTime);
    }

    void OnDrawGizmos()
    {
        Vector3 endPos = GetEndPos();

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(startPosTrans.position, 0.2f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(endPos, 0.3f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(startPosTrans.position, endPos);
    }

    float CalculateLength(float maxLength)
    {
        RaycastHit hit;
        Vector3 direction = GetArmDirection();

        if (Physics.Raycast(startPosTrans.position, direction, out hit, Mathf.Abs(maxLength), layerMask, QueryTriggerInteraction.Ignore))
        {
            return hit.distance;
        }
        else
        {
            return Mathf.Abs(maxLength);
        }
    }

    public Vector3 GetEndPos()
    {
        return startPosTrans.position + GetArmDirection() * length;
    }

    public Vector3 GetArmDirection()
    {
        float sign = Mathf.Sign(relaxLength);
        Vector3 ret;

        switch(armDirection)
        {
            case ArmDirection.X:
                ret = startPosTrans.right;
                break;
            case ArmDirection.Y:
                ret = startPosTrans.up;
                break;
            case ArmDirection.Z:
                ret = startPosTrans.forward;
                break;
            default:
                ret = Vector3.zero;
                break;
        }

        return ret * sign;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ProceduralAnimationController : MonoBehaviour
{
    [Header("Root")]
    [SerializeField] Transform RootBone;
    [SerializeField] Transform COM;
    [SerializeField] Transform LNextStep;
    [SerializeField] Transform RNextStep;
    Vector3 centerCOM;
    Vector3 nextStepLPos;
    Vector3 nextStepRPos;
    Vector3 centerOfFoot;

    [Header("Foot")]
    [SerializeField] Transform LFoot;
    [SerializeField] Transform RFoot;
    [SerializeField] bool LIsFornt;

    [Header("Move")]
    [SerializeField] float moveSpeed;
    [SerializeField] float nextStepDis;
    [SerializeField] float rotationSpeed;

    [SerializeField] LayerMask groundMask;

    [Header("Cam")]
    [SerializeField] Transform camObj;
    Vector3 moveDirection;

    Rigidbody rootRB;

    [Header("Anim")]
    [SerializeField] float moveLegUpSpeed;
    [SerializeField] float moveLegForwardSpeed;
    [SerializeField] float stepHeight;
    [SerializeField] float stepDuration;

    private void Awake()
    {
        rootRB = RootBone.GetComponent<Rigidbody>();
    }


    void Update()
    {
        HandleMovement();

        HandleAnimation();

        HandleRotation();
    }

    void HandleMovement()
    {
        Vector2 movementInput = PlayerManager.Instance.movementInput;
        float inputVer = movementInput.y;
        float inputHor = movementInput.x;
        moveDirection = camObj.forward * inputVer;
        moveDirection = moveDirection + camObj.right * inputHor;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if (movementInput.magnitude > 0)
        {
            rootRB.AddForce(moveDirection * moveSpeed, ForceMode.Impulse);
        }

    }

    void HandleRotation()
    {
        Vector2 movementInput = PlayerManager.Instance.movementInput;
        Vector3 targetDir = Vector3.zero;
        float inputVer = movementInput.y;
        float inputHor = movementInput.x;
        targetDir = camObj.forward * inputVer;
        targetDir = targetDir + camObj.right * inputHor;
        targetDir.Normalize();
        targetDir.y = 0;

        if (targetDir == Vector3.zero)
            targetDir = COM.forward;

        Quaternion targetRot = Quaternion.LookRotation(targetDir);
        Quaternion playerRot = Quaternion.Slerp(COM.rotation, targetRot, rotationSpeed * Time.deltaTime);
        //Quaternion LFootRot = Quaternion.Slerp(LFoot.rotation, targetRot, rotationSpeed * Time.deltaTime);
        //Quaternion RFootRot = Quaternion.Slerp(RFoot.rotation, targetRot, rotationSpeed * Time.deltaTime);

        COM.rotation = playerRot;
        //LFoot.rotation = LFootRot;
        //RFoot.rotation = RFootRot;

    }

    void HandleAnimation()
    {
        if (Physics.Raycast(RootBone.transform.position, Vector3.down,
            out RaycastHit groundHit, Mathf.Infinity, groundMask))
        {
            centerCOM = groundHit.point;
        }

        if (Physics.Raycast(LNextStep.position, Vector3.down,
            out RaycastHit LNextHit, Mathf.Infinity, groundMask))
        {
            nextStepLPos = LNextHit.point;
        }

        if (Physics.Raycast(RNextStep.position, Vector3.down,
           out RaycastHit RNextHit, Mathf.Infinity, groundMask))
        {
            nextStepRPos = RNextHit.point;
        }

        Vector3 LFootForward = LFoot.TransformDirection(COM.forward);
        Vector3 dirLtoR = RFoot.position - LFoot.position;
        LIsFornt = Vector3.Dot(LFootForward, dirLtoR) < 0;

        centerOfFoot = (LFoot.position + RFoot.position) / 2;

        float comAndCenterFootDis = Vector3.Distance(centerCOM, centerOfFoot);
        float LfootAndCOMDis = Vector3.Distance(LFoot.position, centerCOM);
        float RfootAndCOMDis = Vector3.Distance(RFoot.position, centerCOM);

        if (comAndCenterFootDis >= nextStepDis /*|| LfootAndCOMDis >= nextStepDis ||*/
           /* RfootAndCOMDis >= nextStepDis*/)
        {
            if (LIsFornt)
            {
                RFoot.position = nextStepRPos;
                //StartCoroutine(moveLeg(RFoot, nextStepRPos));
            }
            else
            {
                LFoot.position = nextStepLPos;
                //StartCoroutine(moveLeg(LFoot, nextStepLPos));
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(centerOfFoot, 0.1f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(centerCOM, 0.1f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(nextStepLPos, 0.1f);
        Gizmos.DrawSphere(nextStepRPos, 0.1f);

    }

    //IEnumerator moveLeg(Transform foot, Vector3 targetPos)
    //{
    //    Vector3 startPos = foot.position;

    //    Vector3 centerPos = (foot.position + targetPos) / 2;

    //    centerPos.y = targetPos.y + stepHeight;

    //    float timeElapsed = 0;

    //    do
    //    {
    //        timeElapsed += Time.deltaTime;
    //        float normalizedTime = timeElapsed / stepDuration;

    //        foot.position =
    //          Vector3.Lerp(
    //            Vector3.Lerp(startPos, centerPos, normalizedTime),
    //            Vector3.Lerp(centerPos, targetPos, normalizedTime),
    //            normalizedTime
    //          );

    //        yield return null;
    //    }
    //    while (timeElapsed < stepDuration);
    //}

}

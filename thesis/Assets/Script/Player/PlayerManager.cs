using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Auto_Singleton<PlayerManager>
{
    public BaseState currentState;

    public BalanceState balance = new BalanceState();
    public DropState drop = new DropState();
    public OnAirState onAir = new OnAirState();
    public OnDragState onDrag = new OnDragState();

    [HideInInspector] public Vector2 movementInput;
    [HideInInspector] public ProceduralAnimationController pac;

    public float hardSpring;
    public float softSpring;

    [Header("Arm")]
    public List<Rigidbody> armRB = new List<Rigidbody>();
    public List<ConfigurableJoint> armJoint = new List<ConfigurableJoint>();
    [Space(10f)]

    [Header("Leg")]
    public List<Rigidbody> legRB = new List<Rigidbody>();
    public List<ConfigurableJoint> legJoint = new List<ConfigurableJoint>();
    [Space(10f)]

    [Header("Body")]
    public Rigidbody rootRB;
    public ConfigurableJoint rootJoint;
    public List<Rigidbody> bodyRB = new List<Rigidbody>();
    public List<ConfigurableJoint> bodyJoint = new List<ConfigurableJoint>();


    private void Awake()
    {
        pac = GetComponent<ProceduralAnimationController>();
        SwitchState(balance);
    }

    private void Update()
    {
        currentState.UpdateState(transform.gameObject);


    }

    public void SwitchState(BaseState state)
    {
        currentState = state;
        currentState.EnterState(transform.gameObject);
    }

    public void SetupJointSpringWeight(float root, float body, float arm, float leg)
    {
        foreach (var joint in legJoint)
        {
            JointDrive jointDrive = new JointDrive();
            jointDrive.positionSpring = leg;
            jointDrive.maximumForce = leg;

            joint.angularXDrive = jointDrive;
            joint.angularYZDrive = jointDrive;
        }

        foreach (var joint in armJoint)
        {
            JointDrive jointDrive = new JointDrive();
            jointDrive.positionSpring = arm;
            jointDrive.maximumForce = arm;

            joint.angularXDrive = jointDrive;
            joint.angularYZDrive = jointDrive;
        }

        foreach (var joint in bodyJoint)
        {
            JointDrive jointDrive = new JointDrive();
            jointDrive.positionSpring = body;
            jointDrive.maximumForce = body;

            joint.angularXDrive = jointDrive;
            joint.angularYZDrive = jointDrive;
        }

        JointDrive rootJointDrive = new JointDrive();
        rootJointDrive.positionSpring = root;
        rootJointDrive.maximumForce = root;

        rootJoint.angularXDrive = rootJointDrive;
        rootJoint.angularYZDrive = rootJointDrive;

    }

}

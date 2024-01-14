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

    public Vector2 movementInput;

    private void Awake()
    {
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

}

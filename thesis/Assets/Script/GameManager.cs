using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Auto_Singleton<GameManager>
{
    [Header("===== Grid =====")]
    public int gridWidth;
    public int gridHeight;
    public float gridSize;
    public Vector3 gridOrigin;
    [HideInInspector] GridController gridController;

    public BaseState currentGameState;

    public StandbyPhase standbyPhase = new StandbyPhase();
    public BattlePhase battlePhase = new BattlePhase();
    public EndPhase endPhase = new EndPhase();

    private void Awake()
    {
        SwitchState(standbyPhase);

        gridController = GetComponent<GridController>();
        gridController.GenerateGrid(gridWidth, gridHeight, gridSize, gridOrigin);

    }

    void Update()
    {
        currentGameState.UpdateState(transform.gameObject);
    }

    public void SwitchState(BaseState state)
    {
        currentGameState = state;
        currentGameState.EnterState(transform.gameObject);
    }

}

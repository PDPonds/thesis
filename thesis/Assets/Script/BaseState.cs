using UnityEngine;

public abstract class BaseState
{
    public abstract void EnterState(GameObject obj);
    public abstract void UpdateState(GameObject obj);

}

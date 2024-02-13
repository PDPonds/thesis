using UnityEngine;

public abstract class BaseState
{
    public abstract void EnterState(GameObject go);
    public abstract void UpdateState(GameObject go);

    public abstract void FixedUpdateState(GameObject go);

}

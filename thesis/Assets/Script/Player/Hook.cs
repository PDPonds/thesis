using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    public Transform target;
    public LineRenderer line;
    public Material lineMat;
    public AnimationCurve curve;

    private void Start()
    {
        GenerateLine();
        Destroy(gameObject, 1f);
    }

    private void Update()
    {
        if (target != null)
        {
            transform.position =
                Vector3.MoveTowards(transform.position,
                target.position, PlayerManager.Instance.hookSpeed);
        }

        line.SetPosition(0, transform.position);
        line.SetPosition(1, PlayerManager.Instance.transform.position);

    }

    void GenerateLine()
    {
        GameObject lineObj = new GameObject("HookLine");
        LineRenderer lineHook = lineObj.AddComponent<LineRenderer>();
        lineHook.material = lineMat;
        lineHook.widthCurve = curve;
        lineHook.sortingOrder = 3;
        line = lineHook;
        Destroy(line, 1f);

    }

    public void DestroyHook()
    {
        Destroy(gameObject);
        Destroy(line.gameObject);
    }

}

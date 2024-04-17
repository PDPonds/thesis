using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneAfterEndCutScene : MonoBehaviour
{
    public float duration;

    float cur;

    private void Start()
    {
        cur = duration;
    }

    private void Update()
    {
        cur -= Time.deltaTime;
        if (cur < 0)
        {
            SceneManager.LoadScene(0);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<PlayerManager>(out PlayerManager manager))
            {
                if (PlayerManager.passTutorial == false)
                {
                    TutorialManager.instance.ShowTutorial();
                    Destroy(gameObject);

                }
            }
        }
    }
}

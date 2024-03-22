using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextScrolling : MonoBehaviour
{
    public GameObject TargetText;
    public GameObject NewTargetText;
    public GameObject StartPoint;
    public GameObject EndPoint;

    private RectTransform rectTransform;

    [SerializeField] private bool isVertical;

    public float scrollingSpeed;

    void Start()
    {
        rectTransform = TargetText.GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {
        if (isVertical)
        {
            // Move text vertically
            TargetText.transform.Translate(Vector3.up * scrollingSpeed * Time.deltaTime);
            NewTargetText.transform.Translate(Vector3.up * scrollingSpeed * Time.deltaTime);

            // Reset text if it exceeds the endpoint
            if (TargetText.transform.position.y >= EndPoint.transform.position.y)
            {
                TargetText.transform.position = StartPoint.transform.position;
            }
            if (NewTargetText.transform.position.y >= EndPoint.transform.position.y)
            {
                NewTargetText.transform.position = StartPoint.transform.position;
            }
        }
        else
        {
            // Move text horizontally
            TargetText.transform.Translate(Vector3.right * scrollingSpeed * Time.deltaTime);
            NewTargetText.transform.Translate(Vector3.right * scrollingSpeed * Time.deltaTime);

            // Reset text if it exceeds the endpoint
            if (TargetText.transform.position.x >= EndPoint.transform.position.x)
            {
                TargetText.transform.position = StartPoint.transform.position;
            }
            if (NewTargetText.transform.position.x >= EndPoint.transform.position.x)
            {
                NewTargetText.transform.position = StartPoint.transform.position;
            }
        }
    }
}

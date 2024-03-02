using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    float length, startPos , startY;
    public GameObject target;
    public float parallaxXEffect , parallaxYEffect;

    void Start()
    {
        startPos = transform.position.x;
        startY = transform.position.y;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void FixedUpdate()
    {
        float temp = (target.transform.position.x * (1 - parallaxXEffect));
        float dist = (target.transform.position.x * parallaxXEffect);
        float yPos = (target.transform.position.y * parallaxYEffect);

        transform.position = new Vector3(startPos + dist, startY - yPos, transform.position.z);
        if (temp > startPos + length) startPos += length;
        else if (temp < startPos - length) startPos -= length;
    }
}

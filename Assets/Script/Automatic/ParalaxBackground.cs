using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxBackground : MonoBehaviour
{
    private GameObject camera;

    [SerializeField] private float parallaxEffect;

    private float length;
    private float xPosition;
    void Start()
    {
        //Find camera
        camera = GameObject.Find("Main Camera");
        //take the total length of the background layer
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        //take the position x of the background layer
        xPosition = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToMove = camera.transform.position.x * parallaxEffect;
        float distanceMoved = camera.transform.position.x * (1 - parallaxEffect);
        transform.position = new Vector3(xPosition+ distanceToMove, transform.position.y);
        if(distanceMoved > length + xPosition) {
            xPosition = xPosition + length;
        }
    }
}

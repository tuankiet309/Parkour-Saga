using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    [SerializeField] private Vector3 nextPartPosition;
    [SerializeField] private Transform player;
    [SerializeField] Transform[] levelPart;
    [SerializeField] private float distanceToGenerate;
    [SerializeField] private float distanceToDelete;
    
    void Update()
    {
        GenratePlatform();
    }

    private void GenratePlatform()
    {
        while (Vector2.Distance(player.position, nextPartPosition) < distanceToGenerate)
        {
            Transform part = levelPart[Random.Range(0, levelPart.Length)];
            Vector2 newPosition = new Vector2(nextPartPosition.x - part.Find("StartPoint").position.x, 0);
            Transform newPart = Instantiate(part, newPosition, transform.rotation, transform);
            nextPartPosition = newPart.Find("EndPoint").position;
        }
    }
}

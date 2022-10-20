using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] int numberOfItems;
    [SerializeField] Transform parent;

    public float[] spawnPositions;
    [SerializeField] private float y = 1.5f;
    private float count;
    public float timeUntilSpawn;
    private Vector3 spawnPosition;

    private void Update()
    {
       
        if (count < timeUntilSpawn + 0.2f)
        {
            count += Time.deltaTime;
        }
        else if(count >= timeUntilSpawn)
        {
            RandomizePosition();
            Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
            count = 0;
           
        }
    }


    private void Start()
    {
        for (int i = 0; i < numberOfItems; i++)
        {
            var randomNum = Random.Range(0, 2);
            var xPos = GameManager.Instance.LanePositions[randomNum].x;
            Vector3 position = new Vector3(xPos, 1.5f, Random.Range(0f, 10f));
            Instantiate(objectToSpawn, position, Quaternion.identity, parent);
        }
    }

    void RandomizePosition()
    {
        int i = UnityEngine.Random.Range(0, spawnPositions.Length);
        spawnPosition = new Vector3(spawnPositions[i], 1.5f, 100);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class GroundGenerator : MonoBehaviour
{
    public Transform StartPoint;
    public RoadBehaviour RoadPrefab;
    public int TilesToSpawnAtStart;
    public List<RoadBehaviour> SpawnedRoad;
    public Camera MainCamera;

    public int refuelSpawnChance;
    [SerializeField] public bool moving;
    [SerializeField] public List<GameObject> activeFuelStations;
    public bool CanSpawnObject = true;
    public bool CanSpawnProps = true;
    public Rigidbody Rigidbody;
    public RoadBehaviour.TileType LastSpawnedObstacleType;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 spawnPosition = StartPoint.position;

        for (int i = 0; i < TilesToSpawnAtStart; i++)
        {
            spawnPosition.z += RoadPrefab.MeshRenderer.bounds.extents.z;
            
            var newRoad = Instantiate(RoadPrefab, spawnPosition, Quaternion.identity);
            
            newRoad.name = "Road " + (i + 1);
            
            newRoad.Id = i;

            spawnPosition = newRoad.EndPoint.position;
            
            newRoad.transform.SetParent(transform);

            if (i % 2 == 0 && i != 0)
            {
                if (CanSpawnObject)
                {
                    //Debug.Log("spawning starting obstacle every 2 tiles");
                    
                    newRoad.SpawnObject(newRoad, false);
                }
            }
            
            newRoad.SpawnPropSet();

            SpawnedRoad.Add(newRoad);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (MainCamera.transform.position.z > SpawnedRoad[0].EndPoint.position.z - 2f)
        {
            SpawnNewTile();
        }
    }

    private void FixedUpdate()
    {
        if (moving)
        {
            MoveMap();
        }
    }

    private void MoveMap()
    {
        Vector3 newPos = new Vector3(0, 0, -transform.forward.z * Time.deltaTime * (GameManager.Instance.Speed * GameManager.Instance.SpeedMultiplier) * Time.timeScale);
        
        transform.Translate(newPos, Space.World);
    }

    private void SpawnNewTile()
    {
        var tempRoad = SpawnedRoad[0];

        tempRoad.HasSpawn = false;
        
        tempRoad.HasProps = false;

        if (SpawnedRoad[SpawnedRoad.Count - 1].Type == RoadBehaviour.TileType.Refuel)
        {
            //Debug.Log(SpawnedRoad[SpawnedRoad.Count - 1].name + " was a refuel tile so we won't spawn obstacles on " + tempRoad.name);
            
            CanSpawnObject = false;
            
            tempRoad.Type = RoadBehaviour.TileType.None;
        }
        else if (SpawnedRoad[SpawnedRoad.Count - 1].Type == RoadBehaviour.TileType.JumpingObstacle)
        {
            CanSpawnObject = false;
            
            tempRoad.Type = RoadBehaviour.TileType.None;
            
            //Debug.Log("can't spawn obstacle because " + SpawnedRoad[SpawnedRoad.Count - 1].name + " was jumping " + tempRoad.name);
        }
        else if (SpawnedRoad[SpawnedRoad.Count - 1].Type != RoadBehaviour.TileType.Refuel && 
                 SpawnedRoad[SpawnedRoad.Count - 1].Type != RoadBehaviour.TileType.JumpingObstacle && 
                 tempRoad.Id % 2 != 0)
        {
            CanSpawnObject = true;
            
            //Debug.Log("can spawn obstacle because id is not a multiple of 2 and " + SpawnedRoad[SpawnedRoad.Count - 1].name + " wasn't refuel or jumping " + tempRoad.name);
        }
        
        SpawnedRoad.RemoveAt(0);
        
        tempRoad.ResetSpawns();

        var newPos = new Vector3(SpawnedRoad[SpawnedRoad.Count - 1].EndPoint.position.x,
            SpawnedRoad[SpawnedRoad.Count - 1].EndPoint.position.y,
            SpawnedRoad[SpawnedRoad.Count - 1].EndPoint.position.z + RoadPrefab.MeshRenderer.bounds.extents.z);

        tempRoad.transform.position = newPos;

        var num = Random.Range(1, 100);

        var refuel = false;

        if (num <= refuelSpawnChance && GameManager.Instance.GroundGenerator.activeFuelStations.Count == 0)
        {
            refuel = true;
        }
        else
        {
            refuel = false;
        }

        if (!tempRoad.HasProps && CanSpawnProps)
        {
            //Debug.Log("spawning props on " + tempRoad.name);
            
            tempRoad.ResetProps();
            
            tempRoad.SpawnPropSet();
        }

        if (!tempRoad.HasSpawn && CanSpawnObject)
        {
            //Debug.Log("spawning object refuel " + refuel + " " + tempRoad.name);
            
            tempRoad.ResetSpawns();
            
            tempRoad.SpawnObject(tempRoad, refuel);
        }
        else
        {
            //Debug.Log(tempRoad.name + " already has a spawn");
        }

        //Debug.Log(refuel);

        SpawnedRoad.Add(tempRoad);
    }
}

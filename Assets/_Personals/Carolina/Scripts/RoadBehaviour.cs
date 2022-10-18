using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using Random = UnityEngine.Random;

public class RoadBehaviour : MonoBehaviour
{
    public Transform StartPoint;
    public Transform EndPoint;
    public List<ObstacleParentBehaviour> Obstacles;
    public List<RefuelStationBehaviour> RefuelingStations;
    public List<GameObject> ScoreTriggers;
    public MeshRenderer MeshRenderer;
    public GameObject RefuelingStation;
    public GameObject DropletSpawnpoint;
    public bool HasSpawn;
    public int Id;
    public GameObject SpawnsParent;
    
    public enum TileType
    {
        None,
        Obstacle,
        JumpingObstacle,
        Refuel
    }

    public TileType Type;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("Gather References")]
    private void OnValidate()
    {
        Obstacles = SpawnsParent.GetComponentsInChildren<ObstacleParentBehaviour>(true).ToList();
        ScoreTriggers = FindTagObjects(SpawnsParent, "ScoreTrigger");
        RefuelingStations = SpawnsParent.GetComponentsInChildren<RefuelStationBehaviour>(true).ToList();
    }

    /*private void Awake()
    {
        Obstacles = SpawnsParent.GetComponentsInChildren<ObstacleParentBehaviour>(true).ToList();
        ScoreTriggers = FindTagObjects(SpawnsParent, "ScoreTrigger");
        RefuelingStations = SpawnsParent.GetComponentsInChildren<RefuelStationBehaviour>(true).ToList();
    }*/
    
    public List<GameObject> FindTagObjects(GameObject parent, string tag)
    {
        List<GameObject> objs = new List<GameObject>();
        
        foreach (Transform t in parent.GetComponentsInChildren<Transform>(true))
        {
            if (t.tag == tag)
            {
                objs.Add(t.gameObject);
            }
        }
        
        return objs;
    }

    public GameObject SpawnObject(RoadBehaviour road, bool refuel)
    {
        GameManager.Instance.GroundGenerator.CanSpawnObject = false;
        
        var count = 0;
        
        if (refuel)
        {
            foreach (var station in RefuelingStations)
            {
                //Debug.Log("deactivating stations " + road.name);
                station.gameObject.SetActive(false);
            }

            if (GameManager.Instance.GroundGenerator.activeFuelStations.Count >= 1) return null;
            
            GameObject newStation = null;

            var num = Random.Range(0, RefuelingStations.Count);

            newStation = RefuelingStations[num].gameObject;

            newStation.SetActive(true);

            RefuelingStation = newStation;

            //GameManager.Instance.DropManager.SetDropletSpawnpoint(newStation.GetComponent<RefuelStationBehaviour>().DropLetSpawnPoint.transform);
            //StartCoroutine(GameManager.Instance.DropManager.spawnDroplets(newStation.GetComponent<RefuelStationBehaviour>().DropLetSpawnPoint.transform));

            GameManager.Instance.GroundGenerator.activeFuelStations.Add(newStation);

            //GameManager.Instance.GroundGenerator.CanSpawnObject = true;
                
            //Debug.Log("spawning fuel station " + road.name);

            count++;

            //Debug.Log(count + " fuel stations spawned");
                
            road.SetScoreTriggers(true);

            HasSpawn = true;
                
            road.Type = TileType.Refuel;
            
            Debug.Log("setting " + road.name + "'s type to refuel");
            
            GameManager.Instance.GroundGenerator.CanSpawnObject = true;

            return newStation;
        }
        else
        {
            foreach (var obstacle in Obstacles)
            {
                Debug.Log("deactivating obstacles " + road.name);
                obstacle.gameObject.SetActive(false);
            }

            var num = Random.Range(0, Obstacles.Count);

            var newObstacle = Obstacles[num];

            while (newObstacle.Type == GameManager.Instance.GroundGenerator.LastSpawnedObstacleType)
            {
                num = Random.Range(0, Obstacles.Count);

                newObstacle = Obstacles[num];

                break;
            }

            newObstacle.gameObject.SetActive(true);
            
            GameManager.Instance.GroundGenerator.CanSpawnObject = true;
            
            Debug.Log("spawning obstacle " + newObstacle + " on " + road.name);
            
            HasSpawn = true;

            road.Type = (TileType)
                Array.IndexOf(Enum.GetValues(newObstacle.Type.GetType()), newObstacle.Type);

            GameManager.Instance.GroundGenerator.LastSpawnedObstacleType = road.Type;
            
            Debug.Log("setting " + road.name + "'s type to " + road.Type);
            
            GameManager.Instance.GroundGenerator.CanSpawnObject = true;

            return newObstacle.gameObject;
        }

    }
    
    public void ResetSpawns()
    {
        foreach (var station in RefuelingStations)
        {
            //Debug.Log("deactivating stations " + gameObject.name);
            station.gameObject.SetActive(false);
        }

        foreach (var obstacle in Obstacles)
        {
            //Debug.Log("deactivating obstacles " + gameObject.name);
            obstacle.gameObject.SetActive(false);
        }

        SetScoreTriggers(true);

        Type = TileType.None;
    }

    public void SetScoreTriggers(bool value)
    {
        foreach (var trigger in ScoreTriggers)
        {
            trigger.SetActive(value);
        }
    }
}

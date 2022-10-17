using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropManager : MonoBehaviour
{
    public int dropletsInPlay;
    public GameObject dropletPrefab;
    public Vector3 nextDropPos;
    public List<GameObject> inactiveDroplets = new List<GameObject>();


    private void Start()
    {
        StartCoroutine(InstantiateStartDroplets());
    }

    IEnumerator InstantiateStartDroplets()
    {
        for (int i = 0; i < dropletsInPlay; i++)
        {
            var dropLet = Instantiate(dropletPrefab, new Vector3(0, -0.21f, 0), Quaternion.identity);
            dropLet.transform.parent = GameObject.Find("Water").transform;
            yield return new WaitForSeconds(.01f);
        }
    }

    public IEnumerator spawnDroplets(Transform transform)
    {    
        while(transform.gameObject.activeInHierarchy)
        {
                for (int i = 0; i < inactiveDroplets.Count; i++)
                {
                    inactiveDroplets[i].transform.position = transform.position;
                    inactiveDroplets[i].SetActive(true);
                    inactiveDroplets[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    inactiveDroplets.RemoveAt(i);
                    yield return new WaitForSeconds(.005f);               
                }
        }
        //Debug.Log("No transform");
    }

    public void SetDropletSpawnpoint(Transform transform)
    {
        for (int i = 0; i < inactiveDroplets.Count; i++)
        {
            inactiveDroplets[i].transform.parent = transform;
            inactiveDroplets[i].transform.localPosition = new Vector3(0, 0, 0);
        }
    }

    public void MoveDropletsToResevoir()
    {
        for (int i = 0; i < inactiveDroplets.Count; i++)
        {
            inactiveDroplets[i].transform.parent = GameObject.Find("Water").transform;
        }
    }
}

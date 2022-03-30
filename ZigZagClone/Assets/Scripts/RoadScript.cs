using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadScript : MonoBehaviour
{
    public GameObject roadPartPrefab;
    public Vector3 lastPos;

    private int roadCount;
    private float offset = 0.707f;
    private const float MIN_DISTANCE = 4f;

    private List<GameObject> roads = new(12);

    public void StartBuilding()
    {
        InvokeRepeating(nameof(CreateNewRoad), 1f, 0.2f);
    }

    private void Awake()
    {
        // Create the first bit of the world.
        for (int i = 0; i < 12  ; i++)
        {
            CreateNewRoad(i == 0);
        }
    }

    private void CreateNewRoad()
    {
        CreateNewRoad(false);
    }

    private void CreateNewRoad(bool firstPiece)
    {
        int chance = Random.Range(0, 2);
        if(firstPiece || chance == 0)
        {
            CreateNewRoad(new Vector3(lastPos.x + offset, lastPos.y, lastPos.z + offset));
        }
        else
        {
            CreateNewRoad(new Vector3(lastPos.x - offset, lastPos.y, lastPos.z + offset));
        }
    }

    private void CreateNewRoad(Vector3 spawnPos)
    {
        CharacterController character = FindObjectOfType<CharacterController>();

        if(Vector3.Distance(spawnPos, character.transform.position) > 15f)
        {
            Debug.Log("Too far, returning");
            return;
        }

        GameObject g = Instantiate(roadPartPrefab, spawnPos, transform.rotation);
        g.transform.parent = transform;

        lastPos = g.transform.position;

        roadCount++;

        if (roadCount % 5 == 0)
        {
            g.transform.GetChild(0).gameObject.SetActive(true);
        }

        roads.Add(g);

        GameObject furthestRoadPiece = roads[0];

        if (Vector3.Distance(FindObjectOfType<CharacterController>().transform.position, furthestRoadPiece.transform.position) > MIN_DISTANCE)
        {
            Debug.Log("Road destroyed");
            Destroy(furthestRoadPiece);
            roads.RemoveAt(0);
            Debug.Log(roads.Count);
        }
    }
}

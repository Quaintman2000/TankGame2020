using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGen : MonoBehaviour
{
    public int rows;
    public int columns;
    private float roomWidth = 50f;
    private float roomHeight = 50f;
    public GameObject[] gridPrefabs;
    public Room[,] grid;
    public int mapSeed;
    public int currentRow;
    public int currentColumn;
    public enum MapType
    {
        Seeded,
        Random,
        MapOfTheDay
    }

    public MapType mapType = MapType.Random;

    public int DateToInt(DateTime dateToUse)
    {
        return dateToUse.Year + dateToUse.Day + 
            dateToUse.Hour + dateToUse.Minute + 
            dateToUse.Second + dateToUse.Millisecond;
    }
    public GameObject RandomRoomPrefab()
    {
        return gridPrefabs[UnityEngine.Random.Range(0, gridPrefabs.Length)];
    }

    public void GenerateGrid()
    {
        //UnityEngine.Random.seed = mapSeed;
        UnityEngine.Random.InitState(mapSeed);
        //  for (int currentRow; cur)
        //start with a an empty grid
        grid = new Room[columns, rows];
        //for each grid row
        for ( currentRow = 0; currentRow < rows; currentRow++)
        {
            for ( currentColumn = 0; currentColumn < columns; currentColumn++)
            {
                Debug.Log("Making column " + currentColumn + " and row " + currentRow);
                //figure out the location
                float xPosition = roomWidth * currentColumn;
                float zPosition = roomHeight * currentRow;
                Vector3 newPosition = new Vector3(xPosition, 0, zPosition);

                //create a new grid at the appropriate location
                GameObject tempRoomObj = Instantiate(RandomRoomPrefab(), newPosition, Quaternion.identity) as GameObject;

                //set the room's parent
                tempRoomObj.transform.parent = this.transform;

                //name the room
                tempRoomObj.name = "Room_" + currentColumn + "," + currentRow;

                Room tempRoom = tempRoomObj.GetComponent<Room>();
                if(rows == 1)
                {

                }
                else if (currentRow == 0)
                {
                    tempRoom.doorNorth.SetActive(false);
                }
                else if (currentRow == rows - 1)
                {
                    tempRoom.doorSouth.SetActive(false);
                }
                else
                {
                    tempRoom.doorNorth.SetActive(false);
                    tempRoom.doorSouth.SetActive(false);
                }
                if(columns == 1)
                {

                }
                else if (currentColumn == 0)
                {
                    tempRoom.doorWest.SetActive(false);
                }
                else if (currentColumn == rows - 1)
                {
                    tempRoom.doorEast.SetActive(false);
                }
                else
                {
                    tempRoom.doorEast.SetActive(false);
                    tempRoom.doorWest.SetActive(false);
                }
                grid[currentColumn, currentRow] = tempRoom;
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        switch (mapType)
        {
            case MapType.MapOfTheDay:
                mapSeed = DateToInt(DateTime.Now.Date);
                break;
            case MapType.Random:
                mapSeed = DateToInt(DateTime.Now);
                break;
            case MapType.Seeded:

                break;
            default:
                Debug.LogWarning("[MapGen] Map type not implemented");
                break;
        }
        GenerateGrid();
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

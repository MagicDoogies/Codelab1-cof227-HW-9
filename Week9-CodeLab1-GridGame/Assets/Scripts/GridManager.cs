﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width; //initializing width
    public int height; //initializing height

    public GameObject cube; //initializing the cube as a game object

    GameObject[,] grid; //this is a 2d array for the grid

    public static GridManager instance; //this allows the gridmanager to be accessed from other scripts

    public GridItem selected; //this is initializing which cube we are selecting so we can swap

    void Awake() //this is the singleton for the game object this script is attached to
    {
        if(instance == null){ //if theres no instance of this script
            instance = this; //now there is
            DontDestroyOnLoad(gameObject); //please dont destroy my game object
        } else {
            Destroy(gameObject); //otherwise if there is a duplicate, it is a fake, please destroy it on sight seargent
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        grid = new GameObject[width, height]; // creates a new grid as a GameObject 

        GameObject gridHolder = new GameObject("Grid Holder"); // lets you set what prefab is holding the grid

        for (int x = 0; x < width; x++){
            for (int y = 0; y < height; y++) // nested for loops that iterate through each grid position
            {
                grid[x, y] = Instantiate<GameObject>(cube); // instantiate a cube at each iteration
                grid[x, y].transform.position = new Vector3(x, y, 0); // sets where the cube is instantiated

                grid[x, y].transform.parent = gridHolder.transform; // setting the transform to the gridHolder's transform
                grid[x, y].GetComponent<GridItem>().SetPos(x, y); // putting it there in that position
            }
        }

        Camera.main.transform.position = new Vector3(width / 2, height / 2, -10); // placing the camera in the center and above.
    }

    public void Swap(GridItem newItem) // this is the swapping function. it gets called in GridItem.cs
    {
        int tempX = newItem.gridX; // the temp variable gets replaced by the new one.
        int tempY = newItem.gridY; // the temp variable gets replaced by the new one.

        // THIS MOVES THE FIRST CUBE'S POSITION TO THE SECOND CUBE'S POSITION
        newItem.SetPos(selected.gridX, selected.gridY); // set the position to the position of the selected cube
        newItem.transform.position = new Vector2(selected.gridX, selected.gridY); // set position on the XY position of selected cube
        grid[tempX, tempY] = newItem.gameObject; // setting temp and newItem to be equal to each other

        // THIS MOVES THE SECOND CUBE TO THE FIRST CUBE'S POSITION
        selected.SetPos(tempX, tempY); // set the position of the selected cube to the position of the temp cube.
        selected.transform.position = new Vector2(tempX, tempY); // set position on the XY position of the temp cube. 
        grid[tempX, tempY] = selected.gameObject; // setting temp and selected to be equal to each other

        // These are to reset the selection states after the swap is over.
        selected.transform.localScale = new Vector3(.75f, .75f, .75f); // resets the size of the cube back to smaller again.
        selected = null; // resets the selection back to null after the swap is over.
    }
}

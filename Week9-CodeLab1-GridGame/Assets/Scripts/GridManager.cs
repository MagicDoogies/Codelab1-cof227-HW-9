using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    public int width; //initializing width
    public int height; //initializing height

    
    public float timer = 30;  //start timer at 30 seconds
    public Text infoTime;

    public GameObject cube; //initializing the cube as a game object

    public GameObject imagecube; //initializes the cubes we are using for user to copy

    GridItem[,] grid; //this is a 2d array for the grid

    ImageItem[,] grid2; //this is the 2d array for the grid that should be matched

    public static GridManager instance; //this allows the gridmanager to be accessed from other scripts

    public GridItem selected; //this is initializing which cube we are selecting so we can swap

    public static int CorrectCubes; //this is how many cubes are correct. when it reaches 16 we're good


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

        CorrectCubes = 0; // start off with CorrectCubes being 0
        
        grid = new GridItem[width, height]; // creates a new grid as a GameObject 

        GameObject gridHolder = new GameObject("Grid Holder"); // lets you set what prefab is holding the grid

        for (int x = 0; x < width; x++){
            for (int y = 0; y < height; y++) // nested for loops that iterate through each grid position
            {
                grid[x, y] = Instantiate<GameObject>(cube).GetComponent<GridItem>(); // instantiate a cube at each iteration
                grid[x, y].transform.position = new Vector3(x, y, 0); // sets where the cube is instantiated

                grid[x, y].transform.parent = gridHolder.transform; // setting the transform to the gridHolder's transform
                grid[x, y].GetComponent<GridItem>().SetPos(x, y); // putting it there in that position
            }
        }

        grid2 = new ImageItem[width, height]; // creates a new grid as a GameObject 

        GameObject gridHolder2 = new GameObject("Grid Holder2"); // lets you set what prefab is holding the grid

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++) // nested for loops that iterate through each grid position
            {
                grid2[x, y] = Instantiate<GameObject>(imagecube).GetComponent<ImageItem>(); // instantiate a cube at each iteration
                grid2[x, y].transform.position = new Vector3(x, y + 5, 0); // sets where the cube is instantiated

                grid2[x, y].transform.parent = gridHolder2.transform; // setting the transform to the gridHolder's transform
                grid2[x, y].GetComponent<ImageItem>().SetPos(x, y + 5); // putting it there in that position
            }
        }

        Camera.main.transform.position = new Vector3(width / 2, height, -10); // placing the camera in the center and above.
    }

    public void Swap(GridItem newItem) // this is the swapping function. it gets called in GridItem.cs
    {
        int tempX = newItem.gridX; // the temp variable gets replaced by the new one.
        int tempY = newItem.gridY; // the temp variable gets replaced by the new one.

        // THIS MOVES THE FIRST CUBE'S POSITION TO THE SECOND CUBE'S POSITION
        newItem.SetPos(selected.gridX, selected.gridY); // set the position to the position of the selected cube
        newItem.transform.position = new Vector2(selected.gridX, selected.gridY); // set position on the XY position of selected cube
        grid[selected.gridX, selected.gridY] = newItem;

        // THIS MOVES THE SECOND CUBE TO THE FIRST CUBE'S POSITION
        selected.SetPos(tempX, tempY); // set the position of the selected cube to the position of the temp cube.
        selected.transform.position = new Vector2(tempX, tempY); // set position on the XY position of the temp cube. 
        grid[tempX, tempY] = selected; // setting temp and selected to be equal to each other

        // These are to reset the selection states after the swap is over.
        selected.transform.localScale = new Vector3(.75f, .75f, .75f); // resets the size of the cube back to smaller again.
        selected = null; // resets the selection back to null after the swap is over.


        // CHECK the 2 grids
        CorrectCubes = 0; // start off with CorrectCubes being 0

        // for loop that checks each position of the grid to see if it's correct
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {

                // compare the 2 grids to each other. if they're equal, increment the CorrectCubes variable
                if (grid[x, y].color == grid2[x, y].color)
                {
                    Debug.Log("Found: " + x + ", " + y + " grid1 - " + grid[x, y].color + " grid 2 - " + grid2[x, y].color);
                    CorrectCubes += 1;
                }
                else
                {
                    Debug.Log("NOT found: " + x + ", " + y + " grid1 - " + grid[x, y].color + " grid 2 - " + grid2[x, y].color);
                }
            }
        }
        

        Debug.Log("CorrectCubes: " + CorrectCubes);
        

        // if all 16 cubes are correct
        if (CorrectCubes == 16)
        {
            print("YOU ARE SMART. WIN");
        }





    }

    void Update()
    {
        timer -= Time.deltaTime;
        infoTime.text = "Time: " + (int)timer;
    }
}

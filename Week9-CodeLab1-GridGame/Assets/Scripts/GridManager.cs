using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GridManager : MonoBehaviour
{
    public int width; //initializing width
    public int height; //initializing height

    public float timer = 5;  //Changed timer to be a countdown for game restart.
    public Text infoTime;

    public Text youwin;

    public GameObject cube; //initializing the cube as a game object

    public GameObject imagecube; //initializes the cubes we are using for user to copy

    GridItem[,] matchGrid; //this is a 2d array for the grid

    ImageItem[,] baseGrid; //this is the 2d array for the grid that should be matched

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
        
        matchGrid = new GridItem[width, height]; // creates a new grid as a GameObject 

        GameObject gridHolder = new GameObject("Grid Holder"); // lets you set what prefab is holding the grid

        for (int x = 0; x < width; x++){
            for (int y = 0; y < height; y++) // nested for loops that iterate through each grid position
            {
                matchGrid[x, y] = Instantiate<GameObject>(cube).GetComponent<GridItem>(); // instantiate a cube at each iteration
                matchGrid[x, y].transform.position = new Vector3(x, y, 0); // sets where the cube is instantiated

                matchGrid[x, y].transform.parent = gridHolder.transform; // setting the transform to the gridHolder's transform
                matchGrid[x, y].GetComponent<GridItem>().SetPos(x, y); // putting it there in that position
            }
        }

        baseGrid = new ImageItem[width, height]; // creates a new grid as a GameObject 

        GameObject gridHolder2 = new GameObject("Grid Holder2"); // lets you set what prefab is holding the grid

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++) // nested for loops that iterate through each grid position
            {
                baseGrid[x, y] = Instantiate<GameObject>(imagecube).GetComponent<ImageItem>(); // instantiate a cube at each iteration
                baseGrid[x, y].transform.position = new Vector3(x, y + 5, 0); // sets where the cube is instantiated

                baseGrid[x, y].transform.parent = gridHolder2.transform; // setting the transform to the gridHolder's transform
                baseGrid[x, y].GetComponent<ImageItem>().SetPos(x, y + 5); // putting it there in that position
            }
        }

        for (int i = 0; i < 5; i++) {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);

            int x2 = Random.Range(0, width);
            int y2 = Random.Range(0, height);

            baseGrid[x, y] = baseGrid[x2, y2];
            baseGrid[x, y].transform.position = new Vector3(x, y + 5, 0);
            baseGrid[x, y].GetComponent<ImageItem>().SetPos(x, y + 5);

            baseGrid[x2, y2] = baseGrid[x, y];
            baseGrid[x2, y2].transform.position = new Vector3(x2, y2 + 5, 0);
            baseGrid[x2, y2].GetComponent<ImageItem>().SetPos(x, y + 5);
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
        matchGrid[selected.gridX, selected.gridY] = newItem;

        // THIS MOVES THE SECOND CUBE TO THE FIRST CUBE'S POSITION
        selected.SetPos(tempX, tempY); // set the position of the selected cube to the position of the temp cube.
        selected.transform.position = new Vector2(tempX, tempY); // set position on the XY position of the temp cube. 
        matchGrid[tempX, tempY] = selected; // setting temp and selected to be equal to each other

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
                if (matchGrid[x, y].color == baseGrid[x, y].color)
                {
                    //Debug.Log("Found: " + x + ", " + y + " grid1 - " + grid[x, y].color + " grid 2 - " + grid2[x, y].color);
                    CorrectCubes += 1;
                }
               // else
               // {
               //     Debug.Log("NOT found: " + x + ", " + y + " grid1 - " + grid[x, y].color + " grid 2 - " + grid2[x, y].color);
               // }
            }
        }
        

        Debug.Log("CorrectCubes: " + CorrectCubes);
        

        // if all 16 cubes are correct
        if (CorrectCubes == 16)
        {
            print("YOU ARE SMART. WIN");
            youwin.GetComponent<Text>().enabled = true;
        }
    }

    void Update()
    {
        if(CorrectCubes == 16)
        {
            timer = timer;
        }
        else
        {
            timer -= Time.deltaTime;
            infoTime.text = "Time: " + (int)timer;

            if (timer <= 0)
            {
                timer = 0;
            }
        }
        
        if(CorrectCubes == 16)
        {
            timer -= Time.deltaTime;
            Debug.Log("I'm counting down now" + timer);
        }
        
        if (timer <= 0)//if the timer hits zero.
        {
            Destroy(gameObject);
            SceneManager.LoadScene("SampleScene");//Reload the scene. (Basically the game auto restarts.
        }   
    }
}

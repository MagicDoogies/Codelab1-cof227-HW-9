using System.Collections;
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
        grid = new GameObject[width, height];

        GameObject gridHolder = new GameObject("Grid Holder");

        for (int x = 0; x < width; x++){
            for (int y = 0; y < height; y++)
            {
                grid[x, y] = Instantiate<GameObject>(cube);
                grid[x, y].transform.position = 
                    new Vector3(x, y, 0);

                grid[x, y].transform.parent = gridHolder.transform;
                grid[x, y].GetComponent<GridItem>().SetPos(x, y);
            }
        }

        Camera.main.transform.position = 
            new Vector3(width / 2, height / 2, -10);
    }

    public void Swap(GridItem newItem)
    {
        int tempX = newItem.gridX;
        int tempY = newItem.gridY;

        newItem.SetPos(selected.gridX, selected.gridY);
        newItem.transform.position =
                    new Vector2(selected.gridX, selected.gridY);
        grid[tempX, tempY] = newItem.gameObject;

        selected.SetPos(tempX, tempY);
        selected.transform.position =
                    new Vector2(tempX, tempY);
        grid[tempX, tempY] = selected.gameObject;

        selected.transform.localScale = new Vector3(.75f, .75f, .75f);
        selected = null;
    }
}

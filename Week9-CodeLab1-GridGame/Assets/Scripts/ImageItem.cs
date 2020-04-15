using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageItem : MonoBehaviour
{
    public Material[] materials;  //materials array to be applied on the cube
    public int gridX, gridY;      //initializes where 3d mesh of cube is on the grid

    void Start()
    {
        GetComponent<MeshRenderer>().material = materials[gridX];  //randomly assigns a material within the material array
    }

    // Start is called before the first frame update
    public void SetPos(int x, int y)
    {
        gridX = x;         //sets the position of x on the grid
        gridY = y;         //sets the position of y on the grid

        name = "X: " + x + " Y: " + y;//it prints the co-ordinates of the originally selected square.
    }
}

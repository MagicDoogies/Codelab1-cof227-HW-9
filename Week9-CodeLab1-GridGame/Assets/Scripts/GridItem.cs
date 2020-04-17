using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridItem : MonoBehaviour
{
    public Material[] materials;  //materials array to be applied on the cube
    public int gridX, gridY;      //initializes where 3d mesh of cube is on the grid

    public int color;   // 

    void Start()
    {
        color = 3 - gridX; 
        GetComponent<MeshRenderer>().material = materials[color]; //Chooses which material to grab based don the position of the X-axis.
        
    }

    // Start is called before the first frame update
    public void SetPos(int x, int y)
    {
        gridX = x;         //sets the position of x on the grid
        gridY = y;         //sets the position of y on the grid

        name = "X: " + x + " Y: " + y;//it prints the co-ordinates of the originally selected square.
    }

    void OnMouseDown() //custom fucntion for when the player clicks down on the mouse.
    {
        if(GridManager.instance.selected == null){ //if there is no Grid Manager script
            GridManager.instance.selected = this;//it makes one and activates the 'Swap(GridItem) function under the GridManager script.
            transform.localScale = new Vector3(1, 1, 1);//The selected cube scales by 1 in X,Y,Z
        } else { //else if you already clicked on the cube.
            GridManager.instance.Swap(this);//activates the function in the GridManager script that swaps the positions of the selected cubes.
        }

        //print(name); //prints out the co-ordinates of whatever is in line 22.
    }
}

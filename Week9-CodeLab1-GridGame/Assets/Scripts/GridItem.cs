using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridItem : MonoBehaviour
{
    public Material[] materials;  //materials array to be applied on the cube
    public int gridX, gridY;      //makes variable for x & y public within editor to determine grid size

    void Start()
    {
        GetComponent<MeshRenderer>().material =            //gets the applied material on the mesh
            materials[Random.Range(0, materials.Length)];  //randomly assigns a material within the material array
    }

    // Start is called before the first frame update
    public void SetPos(int x, int y)
    {
        gridX = x;         //sets the length of x on the grid
        gridY = y;         //sets the length of y on the grid

        name = "X: " + x + " Y: " + y;
    }

    void OnMouseDown()
    {
        if(GridManager.instance.selected == null){
            GridManager.instance.selected = this;
            transform.localScale = new Vector3(1, 1, 1);
        } else {
            GridManager.instance.Swap(this);
        }

        print(name);
    }
}

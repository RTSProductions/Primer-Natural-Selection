using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewGizmos : MonoBehaviour
{
    [Range (0.01f, 10)]
    public float raidus = 1f;

    public ViewType viewType;

    public ViewColor viewColor;

   

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnDrawGizmos()
    {
        if (viewColor == ViewColor.Black)
        {
            Gizmos.color = Color.black;
        }
        if (viewColor == ViewColor.Blue)
        {
            Gizmos.color = Color.blue;
        }
        if (viewColor == ViewColor.Green)
        {
            Gizmos.color = Color.green;
        }
        if (viewColor == ViewColor.Grey)
        {
            Gizmos.color = Color.grey;
        }
        if (viewColor == ViewColor.Gray)
        {
            Gizmos.color = Color.gray;
        }
        if (viewColor == ViewColor.Red)
        {
            Gizmos.color = Color.red;
        }
        if (viewColor == ViewColor.White)
        {
            Gizmos.color = Color.white;
        }
        if (viewColor == ViewColor.Clear)
        {
            Gizmos.color = Color.clear;
        }
        if (viewColor == ViewColor.Cyan)
        {
            Gizmos.color = Color.cyan;
        }
        if (viewColor == ViewColor.Magenta)
        {
            Gizmos.color = Color.magenta;
        }

        if (viewType == ViewType.Cube)
        {
            Gizmos.DrawWireCube(this.transform.position, new Vector3(raidus,raidus,raidus));
        }
        if (viewType == ViewType.Sphere)
        {
            Gizmos.DrawWireSphere(this.transform.position, raidus);
        }
        
    }
}
public enum ViewType
{
    Sphere = (1 << 0),
    Cube = (1 << 1),
}
public enum ViewColor 
{
    Red = (1 << 0),
    Green = (1 << 1),
    Blue = (1 << 2),
    Black = (1 << 3),
    Grey = (1 << 4),
    Gray = (1 << 5),
    Clear = (1 << 6),
    Cyan = (1 << 7),
    Magenta = (1 << 8),
    White = (1 << 9)

}

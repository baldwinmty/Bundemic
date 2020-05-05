using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject tile;
    public int MapLength;
    public int MapWidth;

    // Start is called before the first frame update
    void Start()
    {
        createLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void createLevel()
    {
        
        for(int z = 0; z < MapLength; z++)
        {
            for(int x = 0; x < MapWidth; x++)
            {
                GameObject newTile = Instantiate(tile);
                newTile.transform.position = new Vector3(-MapLength/2 + 0.5f + x, 0, -MapWidth / 2 + 0.5f + z);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    private GameObject Obstacle;
    private GameObject newObstacle;
    // Start is called before the first frame update
    void Start()
    {
        Obstacle = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<ObstacleUI>().currentObstacle;
    }


    private void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Obstacle = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<ObstacleUI>().currentObstacle;
            placeObstacle();
        }
    }

    private void placeObstacle()
    {
        Instantiate(Obstacle, transform.position + new Vector3(0,1,0), Quaternion.identity);
    }
}

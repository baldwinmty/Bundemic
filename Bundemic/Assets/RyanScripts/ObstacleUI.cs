using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleUI : MonoBehaviour
{
    
    public GameObject currentObstacle;
    public void ChangeActiveObstacle(GameObject ActiveObstacle)
    {
        currentObstacle = ActiveObstacle;
    }
}

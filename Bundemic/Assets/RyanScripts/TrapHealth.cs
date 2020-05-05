using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TrapHealth : MonoBehaviour
{
    public bool DegradingHealth = true;
    float currentTime = 0f;
    public float startingTime = 10f;
    public int health = 100;
    public int Damage = 10;
    public GameObject Injured;
    public GameObject Destroyed;

    // Start is called before the first frame update
    void Awake()
    {
        currentTime = startingTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(DegradingHealth)
        {
            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
                if (currentTime <= 0)
                {
                    health -= Damage;
                    currentTime = startingTime;
                    testHealth();
                }
            }
        }
    }

    public void testHealth()
    {
        if (health <= 50)
        {
            if (health <= 25)
            {
                if (health <= 0)
                {
                    Destroy(gameObject);
                }

                if (gameObject != Destroyed)
                {
                    Instantiate(Destroyed, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
            }

            if (gameObject != Injured && gameObject != Destroyed)
            {
                Instantiate(Injured, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}

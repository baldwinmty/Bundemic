using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnyAI : MonoBehaviour
{
    public bool healthyBunny, vaccinatedBunny;
    public bool carrotSpotted;
    public bool trapped;

    [HideInInspector]
    public float stepTimer, idleTimer, biteTimer, carrotTimer;
    public float startStepTimer, startIdleTimer, startBiteTimer, startCarrotTimer;

    //GameObject[] carrots = new GameObject[0];
    //GameObject[] fence = new GameObject[0];

    public List<GameObject> carrots = new List<GameObject>();
    public List<GameObject> fence = new List<GameObject>();

    Rigidbody rb;

    public IDecision currentDecision;
    IDecision BunnyAi;


    private void Start()
    {
        vaccinatedBunny = false;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        BunnyAi = new AmITrapped(new BiteFence(this), new DoISeeCarrot(new AmINearCarrot(new EatCarrot(this), new GoToCarrot(this), this), new KeepWalking(this), this), this);

        currentDecision = BunnyAi;

        if (currentDecision != null)
        {
            currentDecision = currentDecision.MakeDecision();
        }
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Fence") // may change later
        {
            fence.Add(other.gameObject);
            //add fence into array
            trapped = true;
        }
        if (other.gameObject.name == "carrot") // may change later
        {
            fence.Add(other.gameObject);

            //add fence into carrot

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Fence") // may change later
        {
            fence.Remove(other.gameObject);
            //add fence into array
            trapped = true;
        }
        if (other.gameObject.name == "carrot") // may change later
        {
            fence.Remove(other.gameObject);

            //add fence into carrot

        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Normal Rabbit" && !healthyBunny && collision.gameObject.GetComponent<BunnyAI>().vaccinatedBunny) // when this sick bunny makes contact with another healthy bunny
        {
            collision.gameObject.GetComponent<BunnyAI>().healthyBunny = false;

        }
    }
}

public interface IDecision
{
    IDecision MakeDecision();
}

public class AmITrapped : IDecision
{
    bool value;
    IDecision trueBranch;
    IDecision falseBranch;

    public AmITrapped(IDecision trueBranch, IDecision falseBranch, BunnyAI bunny)
    {
        if (bunny.trapped) //collider dependant
        {
            value = true;
            this.trueBranch = trueBranch;
        }
        else
        {
            value = false;
            this.falseBranch = falseBranch;
        }
    }

    public IDecision MakeDecision()
    {
        if (value == true)
        {
            return trueBranch;
        }
        else if (value == false)
        {
            return falseBranch;
        }
        return null;
    }
}

public class DoISeeCarrot : IDecision
{
    bool value;
    IDecision trueBranch;
    IDecision falseBranch;

    public DoISeeCarrot(IDecision trueBranch, IDecision falseBranch, BunnyAI bunny)
    {
        if (bunny.carrotSpotted) //collider dependant
        {
            value = true;
            this.trueBranch = trueBranch;
        }
        else
        {
            value = false;
            this.falseBranch = falseBranch;
        }
    }

    public IDecision MakeDecision()
    {
        if (value == true)
        {
            return trueBranch;
        }
        else if (value == false)
        {
            return falseBranch;
        }
        return null;
    }
}

public class AmINearCarrot : IDecision
{
    bool value;
    IDecision trueBranch;
    IDecision falseBranch;

    public AmINearCarrot(IDecision trueBranch, IDecision falseBranch, BunnyAI bunny)
    {
        if (bunny.carrotSpotted) //check the distance between all carrots and player/collider dependant
        {
            value = true;
            this.trueBranch = trueBranch;
        }
        else
        {
            value = false;
            this.falseBranch = falseBranch;
        }
    }

    public IDecision MakeDecision()
    {
        if (value == true)
        {
            return trueBranch;
        }
        else if (value == false)
        {
            return falseBranch;
        }
        return null;
    }
}

public class BiteFence : IDecision
{
    BunnyAI bunny;
    public BiteFence(BunnyAI _bunny)
    {
        bunny = _bunny;
    }

    public IDecision MakeDecision()
    {
        if(bunny.biteTimer <= 0)
        {
            // fence takes damage
            bunny.biteTimer = bunny.startBiteTimer;
        }
        else
        {
            bunny.biteTimer -= Time.deltaTime;
        }
        return null;
    }
}

public class GoToCarrot : IDecision
{
    BunnyAI bunny;
    public GoToCarrot(BunnyAI _bunny)
    {
        bunny = _bunny;
    }

    public IDecision MakeDecision()
    {
        //move to carrot

        return null;
    }
}

public class EatCarrot : IDecision
{
    BunnyAI bunny;
    public EatCarrot(BunnyAI _bunny)
    {
        bunny = _bunny;
    }

    public IDecision MakeDecision()
    {
        //eat carrot with a timer
        if(bunny.carrotTimer <= 0)
        {
            // carrot takes damage
            bunny.carrotTimer = bunny.startCarrotTimer;
        }
        else
        {
            bunny.carrotTimer -= Time.deltaTime;
        }
        return null;
    }
}

public class KeepWalking : IDecision
{
    BunnyAI bunny;
    public KeepWalking(BunnyAI _bunny)
    {
        bunny = _bunny;
    }

    public IDecision MakeDecision()
    {
        if(bunny.idleTimer <= 0)
        {
            //changes its target position of travel here
            if(bunny.stepTimer <= 0)
            {
                bunny.idleTimer = bunny.startIdleTimer;
                bunny.stepTimer = bunny.startStepTimer;
            }
            else
            {
                bunny.stepTimer -= Time.deltaTime;
                //make the bunny move around here
                
            }
        }
        else
        {
            bunny.idleTimer -= Time.deltaTime;
        }

        return null;
    }
}
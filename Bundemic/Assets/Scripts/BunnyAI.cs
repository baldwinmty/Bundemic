using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnyAI : MonoBehaviour
{
    Rigidbody rb;
    public Animator animator;
    public bool healthyBunny, vaccinatedBunny;
    public bool carrotSpotted;
    public bool trapped;
    public AudioSource hop;
    public AudioClip sneeze;
    public ParticleSystem infect;

    public float stepTimer, idleTimer, biteTimer, carrotTimer;
    public float startStepTimer, startIdleTimer, startBiteTimer, startCarrotTimer;

    public List<GameObject> carrots = new List<GameObject>();
    public GameObject fence;

    public int moveDirection;
    public int newDirection;
    public float moveSpeed;
    public Vector3 movement;
    public Vector3 movementSave;

    public IDecision currentDecision;
    IDecision BunnyAi;


    private void Start()
    {
        hop = GetComponent<AudioSource>();
        animator.GetComponent<Animator>();
        animator.SetBool("Healthy", healthyBunny);
        vaccinatedBunny = false;
        rb = GetComponent<Rigidbody>();
        biteTimer = startBiteTimer;
        stepTimer = startStepTimer;
        idleTimer = startIdleTimer;
        carrotTimer = startCarrotTimer;
        BunnyAi = new DidIMeetFence(new BiteFence(this), new DoISeeCarrot(new AmINearCarrot(new EatCarrot(this), new GoToCarrot(this), this), new KeepWalking(this), this), this);
    }

    private void Update()
    {
        currentDecision = BunnyAi;

        while (currentDecision != null)
        {
            currentDecision = currentDecision.MakeDecision();
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        if (stepTimer > 0 || idleTimer > 0)
        {
            animator.SetBool("Jumping", false);
            animator.SetBool("Idling", true);
        }
        if (stepTimer < 0 || idleTimer < 0)
        {
            animator.SetBool("Jumping", true);
            animator.SetBool("Idling", false);
        }
        if(!healthyBunny)
        {
            animator.SetBool("Healthy", false);
        }
        if (idleTimer < 0)
        {
            animator.SetBool("GettingInfected", false);
        }
        if (stepTimer == startStepTimer && idleTimer > 0 && idleTimer <0.017)
        { 
            hop.Play(); 
        }
    }

    void MovementSwitch(Collision collision)
    {
        Vector3 v = collision.gameObject.GetComponent<BunnyAI>().movement;

        movement = Vector3.zero;

        movement = v;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Carrot")) // may change later
        {
            carrots.Add(other.gameObject);
            carrotSpotted = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Carrot")) // may change later
        {
            carrots.Remove(other.gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bunny"))
        {
            MovementSwitch(collision);
        }
        // when this sick bunny makes contact with another healthy bunny
        if(collision.gameObject.CompareTag("Bunny") && !healthyBunny && !collision.gameObject.GetComponent<BunnyAI>().vaccinatedBunny && collision.gameObject.GetComponent<BunnyAI>().healthyBunny)
        {
            collision.gameObject.GetComponent<BunnyAI>().idleTimer = 2;
            collision.gameObject.GetComponent<BunnyAI>().healthyBunny = false;
            collision.gameObject.GetComponent <Animator>().SetBool("GettingInfected", true);
            infect.Play();
            AudioSource.PlayClipAtPoint(sneeze, transform.position);
        }

        if(collision.gameObject.CompareTag("Fence")) // may change later
        {
            fence = collision.gameObject;
            trapped = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Fence")) // may change later
        {
            fence = null;
            trapped = false;
        }
    }
}

public interface IDecision
{
    IDecision MakeDecision();
}

public class DidIMeetFence : IDecision
{
    bool value;
    IDecision trueBranch;
    IDecision falseBranch;

    public DidIMeetFence(IDecision trueBranch, IDecision falseBranch, BunnyAI bunny)
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

        float minDistance = Mathf.Infinity;
        float newDist = 0;
        int index = 0;

        for (int i = 0; i < bunny.carrots.Capacity; i++)
        {
            newDist = Vector3.Distance(bunny.transform.position, bunny.carrots[i].gameObject.transform.position); // looks for the distance betw bunny and all carrots

            if (newDist < minDistance)
            {
                minDistance = newDist;
                index = i;
            }
        }

        if (bunny.carrots.Count > 0)
        {
            if (Vector3.Distance(bunny.transform.position, bunny.carrots[index].gameObject.transform.position) < 1.5f) //check the distance between all carrots and player/collider dependant
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
        if (bunny.biteTimer <= 0)
        {
            // fence takes damage
            bunny.movement = Vector3.zero;
            bunny.fence.GetComponent<TrapHealth>().health--;
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
        float minDistance = Mathf.Infinity;
        float newDist = 0;
        int index = 0;

        for (int i = 0; i < bunny.carrots.Capacity; i++)
        {
            newDist = Vector3.Distance(bunny.transform.position, bunny.carrots[i].gameObject.transform.position); // looks for the distance betw bunny and all carrots

            if (newDist < minDistance)
            {
                minDistance = newDist;
                index = i;
            }
        }
        //move to carrot
        //checks coordinates to see if the carrots z is higher than its own z. same thing with x.

        bunny.movement = Vector3.zero;

        if (bunny.transform.position.z < bunny.carrots[index].gameObject.transform.position.z)
        {
            bunny.movement.z = 1; //move up
            bunny.animator.SetInteger("Direction", 0);
        }
        else if(bunny.transform.position.z > bunny.carrots[index].gameObject.transform.position.z)
        {
            bunny.movement.z = -1; //move down
            bunny.animator.SetInteger("Direction", 1);
        }
        else if (bunny.transform.position.x < bunny.carrots[index].gameObject.transform.position.x)
        {
            bunny.movement.x = 1; //move right
            bunny.animator.SetInteger("Direction", 2);
        }
        else if (bunny.transform.position.x > bunny.carrots[index].gameObject.transform.position.x)
        {
            bunny.movement.x = -1; //move left
            bunny.animator.SetInteger("Direction", 3);
        }

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
        float minDistance = Mathf.Infinity;
        float newDist = 0;
        int index = 0;

        for (int i = 0; i < bunny.carrots.Capacity; i++)
        {
            newDist = Vector3.Distance(bunny.transform.position, bunny.carrots[i].gameObject.transform.position); // looks for the distance betw bunny and all carrots

            if (newDist < minDistance)
            {
                minDistance = newDist;
                index = i;
            }
        }


        if(bunny.carrotTimer <= 0)
        {
            // carrot takes damage

            bunny.carrots[index].GetComponent<TrapHealth>().health--;

            bunny.movement = Vector3.zero;

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
        //bunny.moveDirection = Random.Range(0, 4);

        if (bunny.idleTimer <= 0)
        {
           
            //movement needs to be present becuase timer would've reseted by now
            bunny.movement = bunny.movementSave;
            //changes its target position of travel here
            if (bunny.stepTimer <= 0)//if bunny is done Idling/ If bunny 
            {
                
                //movement is at 0 rn
                bunny.idleTimer = bunny.startIdleTimer;
                bunny.stepTimer = bunny.startStepTimer;

                bunny.newDirection = bunny.moveDirection;
                while (bunny.newDirection == bunny.moveDirection)
                {
                    bunny.moveDirection = Random.Range(0, 4);
                    
                }

                bunny.movement = Vector3.zero;

                if (bunny.moveDirection == 0) // move up
                {
                    bunny.movement.z = 1;
                    bunny.animator.SetInteger("Direction", 0);

                }
                else if (bunny.moveDirection == 1)
                {
                    bunny.movement.z = -1; // move down
                    bunny.animator.SetInteger("Direction", 1);

                }
                else if (bunny.moveDirection == 2)
                {
                    bunny.movement.x = 1; // move right
                    bunny.animator.SetInteger("Direction", 2);

                }
                else if (bunny.moveDirection == 3)
                {
                    bunny.movement.x = -1; // move left
                    bunny.animator.SetInteger("Direction", 3);

                }

                bunny.movementSave = bunny.movement;
            }
            else // continues to move in said direction
            {
                bunny.stepTimer -= Time.deltaTime;
            }
        }
        else // stand still/Idle
        {
            bunny.idleTimer -= Time.deltaTime;
            bunny.movement = Vector3.zero;
            // it would have to go back to 
        }
        return null;
    }
    
}
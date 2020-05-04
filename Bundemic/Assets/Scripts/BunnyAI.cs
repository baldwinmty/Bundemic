using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnyAI : MonoBehaviour
{
    public bool healthyBunny;
    public bool carrotSpotted;
    public bool trapped;

    public IDecision currentDecision;
    IDecision BunnyAi;


    private void Start()
    {
        BunnyAi = new AmIInfected(new SwitchScript(this), new AmITrapped(new BiteFence(this), new DoISeeCarrot(new AmINearCarrot(new EatCarrot(this), new GoToCarrot(this), this), new KeepWalking(this), this), this), this);

        currentDecision = BunnyAi;

        if (currentDecision != null)
        {
            currentDecision = currentDecision.MakeDecision();
        }
    }
}

public interface IDecision
{
    IDecision MakeDecision();
}

public class AmIInfected : IDecision
{
    bool value;
    IDecision trueBranch;
    IDecision falseBranch;

    public AmIInfected(IDecision trueBranch, IDecision falseBranch, BunnyAI bunny)
    {
        if (bunny.healthyBunny)
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

public class AmITrapped : IDecision
{
    bool value;
    IDecision trueBranch;
    IDecision falseBranch;

    public AmITrapped(IDecision trueBranch, IDecision falseBranch, BunnyAI bunny)
    {
        if (bunny.trapped)
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
        if (bunny.carrotSpotted)
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
        if (bunny.carrotSpotted)
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

public class SwitchScript : IDecision
{
    BunnyAI bunny;
    public SwitchScript(BunnyAI _bunny)
    {
        bunny = _bunny;
    }

    public IDecision MakeDecision()
    {


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


        return null;
    }
}




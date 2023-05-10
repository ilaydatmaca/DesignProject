

public class BoosterCell : Singleton<BoosterCell>
{
    public Cell targetCell;

    public bool isHammerActive;
    public bool isDiscoActive;
    public bool isRocketActive;

    public Booster hammerBooster;
    public Booster discoBooster;
    public Booster rocketBooster;

    public float waitTime = 5f;

    public void CallHammer()
    {
        isHammerActive = true;
        hammerBooster.SetStateParticle(true);
        Invoke("SetDisableOthers", waitTime);
    }
    public void CallRocket()
    {
        isRocketActive = true;
        rocketBooster.SetStateParticle(true);
        Invoke("SetDisableOthers", waitTime);
    }
    
    public void CallDisco()
    {
        isDiscoActive = true;
        discoBooster.SetStateParticle(true);
        Invoke("SetDisableOthers", waitTime);
    }

    void SetDisableOthers()
    {
        isHammerActive = false;
        isDiscoActive = false;
        isRocketActive = false;
        targetCell = null;
        
        hammerBooster.SetStateParticle(false);
        rocketBooster.SetStateParticle(false);
        discoBooster.SetStateParticle(false);

    }

    public void CallEvent()
    {
        if (isHammerActive)
        {
            hammerBooster.RemoveOneGamePiece();
        }
        else if (isDiscoActive)
        {
            discoBooster.DropColorBomb();
        }
        else if (isRocketActive)
        {
            rocketBooster.RocketBooster();
        }
        SetDisableOthers();
    }
}

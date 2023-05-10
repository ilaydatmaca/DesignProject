

public class BoosterCell : Singleton<BoosterCell>
{
    public Cell targetCell;

    public bool isHammerActive;
    public bool isDiscoActive;
    public bool isRocketActive;

    public Booster hammerBooster;
    public Booster discoBooster;
    public Booster rocketBooster;

    public void CallHammer()
    {
        isHammerActive = true;
    }

    public void CallRocket()
    {
        isRocketActive = true;
    }
    
    public void CallDisco()
    {
        isDiscoActive = true;
    }

    void SetDisableOthers()
    {
        isHammerActive = false;
        isDiscoActive = false;
        isRocketActive = false;

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
        targetCell = null;
    }
}

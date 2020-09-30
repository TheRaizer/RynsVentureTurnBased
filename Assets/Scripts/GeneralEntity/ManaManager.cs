[System.Serializable]
public class ManaManager : AmountManager
{
    public ManaManager(int _maxMana) : base(_maxMana)
    {

    }

    public bool CanUse(int amount)
    {
        if(CurrentAmount - amount < 0)
        {
            return false;
        }
        return true;
    }
}

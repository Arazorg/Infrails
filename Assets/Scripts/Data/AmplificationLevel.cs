[System.Serializable]
public class AmplificationLevel
{
    public string Name;
    public int Level;

    public AmplificationLevel(string name)
    {
        Name = name;
        Level = 0;
    }

    public void IncrementLevel()
    {
        float maxAvailableLevel = 3;
        if(Level + 1 <= maxAvailableLevel)
            Level++;
    }
}

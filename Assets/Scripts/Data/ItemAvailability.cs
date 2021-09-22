[System.Serializable]
public class ItemAvailability
{
    public string Name;
    public bool IsAvailable;

    public ItemAvailability(string name, bool isAvailable)
    {
        Name = name;
        IsAvailable = isAvailable;
    }
}

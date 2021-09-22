public interface ISaveSystem
{
    void Save(PlayerProgressData data);

    PlayerProgressData Load();
}

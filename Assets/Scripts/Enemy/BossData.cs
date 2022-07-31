using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/Standart Boss", fileName = "New Boss")]
public class BossData : EnemyData
{
    [SerializeField] private AudioClip _spawnAudioClip;

    public AudioClip SpawnAudioClip => _spawnAudioClip;
}

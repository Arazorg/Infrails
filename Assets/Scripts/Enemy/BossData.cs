using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/Standart Boss", fileName = "New Boss")]
public class BossData : AttackingEnemyData
{
    [SerializeField] private Sprite _bossPortraitSprite;
    [SerializeField] private AudioClip _spawnAudioClip;

    public Sprite BossPortraitSprite => _bossPortraitSprite;

    public AudioClip SpawnAudioClip => _spawnAudioClip;
}

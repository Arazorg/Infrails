using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial Items/Standart Tutorial Item", 
    fileName = "New Tutorial Item")]
public class TutorialItem : ScriptableObject
{
    [SerializeField] private string _phraseKey;
    [SerializeField] private Sprite _phraseSprite;
    [SerializeField] private Vector2 _phraseImageSize;
    [SerializeField] private float _showTime;

    public string PhraseKey => _phraseKey;

    public Sprite PhraseSprite => _phraseSprite;

    public Vector2 PhraseImageSize => _phraseImageSize;

    public float ShowTime => _showTime;
}

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SelectableCharacter : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private CharacterData _data;
    [SerializeField] private GameObject _teleportaionPrefab;
    [SerializeField] private ChooseCharacterEvent _chooseCharacterEvent;
    [SerializeField] private Vector3 _teleportationEffectOffset;
    [SerializeField] private AudioClip _teleportationEffectClip;

    private Transform _startParent;
    private Vector3 _startPosition;
    private Vector3 _startScale;
    private bool _isClickable;

    public CharacterData Data => _data;

    public bool IsClickable { get => _isClickable; set => _isClickable = value; }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_isClickable)
            _chooseCharacterEvent.Invoke(this);
    }

    public void SetCharacterVisiblity(bool isState)
    {
        bool startSpriteRendererState = true;
        foreach (var spriteRenderer in GetComponentsInChildren<SpriteRenderer>())
        {
            if(!spriteRenderer.enabled)
            {
                spriteRenderer.enabled = isState;
                startSpriteRendererState = false;
            }           
        }

        if(isState && !startSpriteRendererState)
            SpawnTeleportationEffect();
    }

    public void Teleport(bool toTrolley, Transform teleportPoint = null)
    {
        if (toTrolley)
        {
            _isClickable = false;
            transform.parent = teleportPoint;
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
        }
        else
        {
            _isClickable = true;
            transform.parent = _startParent;
            transform.localPosition = _startPosition;
            transform.localScale = _startScale;
        }

        SpawnTeleportationEffect();
    }

    public void SpawnTeleportationEffect()
    {
        AudioManager.Instance.PlayEffect(_teleportationEffectClip);
        var teleportationEffect = Instantiate(_teleportaionPrefab, transform.position + _teleportationEffectOffset, Quaternion.identity);
        teleportationEffect.GetComponent<Animator>().runtimeAnimatorController = _data.TeleportationAnimatorController;
        Destroy(teleportationEffect, 0.5f);
    }

    private void Start()
    {
        _startParent = transform.parent;
        _startPosition = transform.localPosition;
        _startScale = transform.localScale;
        _isClickable = false;
    }
}

[System.Serializable]
public class ChooseCharacterEvent : UnityEvent<SelectableCharacter> { }

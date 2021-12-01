using UnityEngine;

public class LootboxUI : MonoBehaviour
{
    private const string AnimatorOpenKey = "isOpen";

    private AnimationsUI _animationsUI;
    private Animator _animator;

    public delegate void AnimationEnd();

    public event AnimationEnd OnAnimationEnd;

    public void Init(LootboxData data)
    {
        _animator.runtimeAnimatorController = data.AnimatorController;
    }

    public void Open()
    {
        _animator.SetBool(AnimatorOpenKey, true);
    }

    public void ResetLootbox()
    {
        _animationsUI.HideImmediate();
        _animator.SetBool(AnimatorOpenKey, false);
    }

    public void ShowLoot()
    {
        OnAnimationEnd?.Invoke();
    }

    private void Start()
    {
        _animationsUI = GetComponent<AnimationsUI>();
        _animator = GetComponent<Animator>();
    }
}
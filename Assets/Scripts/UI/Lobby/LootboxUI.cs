using UnityEngine;

public class LootboxUI : MonoBehaviour
{
    private const string AnimatorOpenKey = "isOpen";

    private AnimationsUI _animationsUI;
    private Animator _animator;
    private LootboxData _lootboxData;
    private GameShopProductData _productData;

    public delegate void AnimationEnd();

    public event AnimationEnd OnAnimationEnd;

    public LootboxData LootboxData => _lootboxData;

    public GameShopProductData ProductData => _productData;

    public void Init(LootboxData data)
    {
        _lootboxData = data;
        _animator.runtimeAnimatorController = data.AnimatorController;
    }

    public void Init(GameShopProductData data)
    {
        _productData = data;
        _animator.runtimeAnimatorController = data.AnimatorController;
    }

    public void Show()
    {
        _animationsUI.Show();
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
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AnimationsUI))]
public class BossHealthBarUI : MonoBehaviour
{
    [SerializeField] private Image _enemyPortrait;
    [SerializeField] private Image _barImage;
    [SerializeField] private TextMeshProUGUI _healthText;

    private AnimationsUI _animationsUI;
    private int _maxHealth;
    private int _currentHealth;

    public void Init(Boss boss, BossData bossData)
    {
        _enemyPortrait.sprite = bossData.BossPortraitSprite;
        _maxHealth = bossData.MaxHealth;
        _currentHealth = _maxHealth;
        _animationsUI.Show();
        _animationsUI.IsShowOnStart = true;
        boss.OnEnemyDamage += SetHealth;
    }

    public void SetHealth(int damage)
    {
        _currentHealth -= damage;
        _healthText.text = $"{_currentHealth}/{_maxHealth}";
        _barImage.fillAmount =  _currentHealth / (float)_maxHealth;
    }

    private void Start()
    {
        _animationsUI = GetComponent<AnimationsUI>();
    }
}
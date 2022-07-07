using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingButtonUI : MonoBehaviour
{
    [SerializeField] private CooldownIndicator _cooldownIndicator;
    [SerializeField] private AnimationsUI _moneyImage;
    [SerializeField] private int _health;
    [SerializeField] private int _price;
    [SerializeField] private float _cooldownTime;

    private Character _character;

    public void Init(Character character)
    {
        _character = character;
    }

    public void Healing()
    {
        if (_character.Money >= _price)
        {
            _character.AddMoney(-_price);
            _character.Heal(_health);
            _cooldownIndicator.SetFinishTime(Time.time + _cooldownTime);
            StartCoroutine(Cooldown());
        }
    }

    private IEnumerator Cooldown()
    {
        _moneyImage.IsShowOnStart = false;
        _moneyImage.Hide();
        yield return new WaitForSeconds(_cooldownTime);
        _moneyImage.Show();
        _moneyImage.IsShowOnStart = true;
    }
}
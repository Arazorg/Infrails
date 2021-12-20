using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CounterDPS : Enemy, IDebuffVisitor
{
    [SerializeField] private EnemyData _enemyData;
    [SerializeField] private Element _type;

    private EnemyDebuffs _enemyDebuffs;
    private TextMeshPro _damagePopUpText;
    private int _currentDamage;
    private float _startTime;

    public override void Init(EnemyData data, Transform spawnPoint, Character character)
    {
        Data = data;
        OnInit();
    }

    public override void BulletHit(PlayerBullet bullet)
    {
        if (!_damagePopUpText.gameObject.activeSelf)
            _startTime = Time.time;

        int damageWithResistance = _type.GetDamageWithResistance(bullet.Damage, bullet.ElementType);
        GetDamage(damageWithResistance);
        bullet.Accept(Transform, this);
        ShowDpsText(damageWithResistance);

    }

    public void StartBleeding()
    {
        _enemyDebuffs.StartBleeding();
    }

    public void StartStunning()
    {

    }

    protected override void Death(bool isDeathWithEffect = false)
    {

    }

    private void Start()
    {
        _enemyDebuffs = GetComponent<EnemyDebuffs>();
        _damagePopUpText = GetComponentInChildren<TextMeshPro>();
        Init(_enemyData, null, null);
    }

    private void ShowDpsText(int damageWithResistance)
    {
        _damagePopUpText.gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(HideDpsText());
        _currentDamage += damageWithResistance;
        var dps = (_currentDamage / (Time.time - _startTime));
        _damagePopUpText.text = "Урон в секунду составляет:\n" + GetTwoDecimalNumberPlaces(dps.ToString());
    }

    private string GetTwoDecimalNumberPlaces(string text)
    {
        return text;
    }

    private IEnumerator HideDpsText()
    {
        yield return new WaitForSeconds(2f);
        _damagePopUpText.gameObject.SetActive(false);
    }
}

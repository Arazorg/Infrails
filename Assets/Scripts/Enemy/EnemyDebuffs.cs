using System.Collections;
using UnityEngine;

public class EnemyDebuffs : MonoBehaviour
{
    [SerializeField] private GameObject _dizzinesEffect;

    private Enemy _enemy;
    private IEnemyStateSwitcher _enemyStateSwitcher;
    private Coroutine _stunningCoroutine;
    private Coroutine _bleedingCoroutine;
    private float _stunningFinishTime;
    private float _bleedingFinishTime;

    public void StartStunning(IEnemyStateSwitcher enemyStateSwitcher)
    {
        _enemyStateSwitcher = enemyStateSwitcher;
        float stunningDuration = 2f;
        if (_stunningCoroutine == null)
        {
            _dizzinesEffect.SetActive(true);
            _stunningFinishTime = Time.time + stunningDuration;
            _stunningCoroutine = StartCoroutine(Stunning());
        }
        else
        {
            _stunningFinishTime = Time.time + stunningDuration;
        }
    }

    public void StartBleeding()
    {
        float bleedingDuration = 3f;
        if (_bleedingCoroutine == null)
        {
            _bleedingFinishTime = Time.time + bleedingDuration;
            _bleedingCoroutine = StartCoroutine(Bleeding());
        }
        else
        {
            _bleedingFinishTime = Time.time + bleedingDuration;
        }
    }

    private void Start()
    {
        _enemy = GetComponent<Enemy>();
        SetDizzinesEffect();
    }

    private void SetDizzinesEffect()
    {
        _dizzinesEffect.transform.localPosition = (_enemy.Data as AttackingEnemyData).DizzinesOffset;
        var dizzinesSpriteRenderer = _dizzinesEffect.GetComponent<SpriteRenderer>();
        dizzinesSpriteRenderer.size /= transform.localScale.x;
    }

    private IEnumerator Stunning()
    {
        while (Time.time < _stunningFinishTime)
            yield return null;

        _enemyStateSwitcher.SwitchState<EnemyMovementState>();
        _dizzinesEffect.SetActive(false);
        _stunningCoroutine = null;
    }

    private IEnumerator Bleeding()
    {
        while (Time.time < _bleedingFinishTime)
        {
            float timeBetweenDamage = 1f;
            int partOfHealth = 10;
            int minDamage = 1;

            yield return new WaitForSeconds(timeBetweenDamage);
            if(_enemy.Health / partOfHealth < minDamage)
                _enemy.GetDamage(minDamage);
            else
                _enemy.GetDamage(_enemy.Health / partOfHealth);
        }

        _bleedingCoroutine = null;
    }
}

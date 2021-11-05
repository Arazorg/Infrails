using UnityEngine;

public class Coin : MonoBehaviour
{
    private const float Speed = 40;

    private Transform _character;

    public void Init(Transform character)
    {
        _character = character;
    }

    private void Update()
    {
        if (_character != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, _character.position, Speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, _character.position) < 0.01f)
            {
                _character.GetComponent<Character>().AddMoney(1);
                Destroy(gameObject);
            }
        }
    }
}

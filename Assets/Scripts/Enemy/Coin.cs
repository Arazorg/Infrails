using UnityEngine;

public class Coin : MonoBehaviour
{
    private Transform _character;

    public void Init(Transform character)
    {
        _character = character;
    }

    private void Update()
    {
        if (_character != null)
        {
            float distanceForAddCoin = 0.01f;
            float speed = 40f;
            int numberOfCoins = 1;
            transform.position = Vector2.MoveTowards(transform.position, _character.position, speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, _character.position) < distanceForAddCoin)
            {
                _character.GetComponent<Character>().AddMoney(numberOfCoins);
                Destroy(gameObject);
            }
        }
    }
}

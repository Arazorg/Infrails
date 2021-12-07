using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameShopStarUI : MonoBehaviour
{
    [SerializeField] private Sprite _filledStar;

    public void FillStar()
    {
        GetComponent<Image>().sprite = _filledStar;
    }
}

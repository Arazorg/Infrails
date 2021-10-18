using System.Collections;
using UnityEngine;

public class LevelInfoUI : MonoBehaviour
{
    [SerializeField] private float _showingTime;

    public void Show()
    {
        StartCoroutine(Showing());
    }

    public void Hide()
    {
        StopAllCoroutines();
        GetComponent<AnimationsUI>().SetTransparency(0);
    }

    private IEnumerator Showing()
    {
        float startDelay = 0.5f;
        yield return new WaitForSeconds(startDelay);
        GetComponent<AnimationsUI>().SetTransparency(1);       
        yield return new WaitForSecondsRealtime(_showingTime);  
        GetComponent<AnimationsUI>().SetTransparency(0);
    }
}

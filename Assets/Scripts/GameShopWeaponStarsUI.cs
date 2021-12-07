using System.Collections;
using UnityEngine;

public class GameShopWeaponStarsUI : MonoBehaviour
{
    public void Show(int starNumber)
    {
        string startAnimatorKey = "Start";
        foreach (var starAnimator in GetComponentsInChildren<Animator>())
            starAnimator.Play(startAnimatorKey);

        GetComponent<AnimationsUI>().Show();
        StartCoroutine(FillStars(starNumber));
    }

    private IEnumerator FillStars(int starNumber)
    {
        float startDelay = 0.33f;
        yield return new WaitForSecondsRealtime(startDelay);

        float fillDelay = 0.3f;
        int starsCounter = 0;
        foreach (var starAnimator in GetComponentsInChildren<Animator>())
        {
            string appearenceAnimatorKey = "Appearence";
            starAnimator.Play(appearenceAnimatorKey);
            starsCounter++;
            if (starsCounter == starNumber)
                break;

            yield return new WaitForSecondsRealtime(fillDelay);
        }
    }
}

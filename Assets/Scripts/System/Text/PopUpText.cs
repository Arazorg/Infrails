using System.Collections;
using TMPro;
using UnityEngine;

public class PopUpText : MonoBehaviour
{
    private string DISABLE_ANIMATION_NAME = "TextFloat";

    public void InitPhrase(string phraseKey, Color color, float disableDelay = 0.25f)
    {
        GetComponentInChildren<LocalizedText>().SetLocalization(phraseKey);
        GetComponentInChildren<TextMeshPro>().color = color;
        StartCoroutine(DisableText(disableDelay));
    }

    public IEnumerator DisableText(float disableDelay)
    {
        yield return new WaitForSeconds(disableDelay);
        GetComponentInChildren<Animator>().Play(DISABLE_ANIMATION_NAME);
        yield break;
    }

}

using UnityEngine;
using UnityEngine.UI;

public class LoadOnAnimationEnd : MonoBehaviour
{
    public void OnAnimationEnd()
    {
        GetComponent<Image>().enabled = false;
        Loader.Load(Loader.Scene.Lobby);
    }
}

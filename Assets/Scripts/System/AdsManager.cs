using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsListener
{
    public static AdsManager Instance;

    private const string RewardedVideoId = "rewardedVideo";
    private const string VideoId = "video";
#if UNITY_IOS
    private const string GameId = "4101012";
#elif UNITY_ANDROID
    private const string GameId = "4101013";
#endif

    [SerializeField] private bool _isTestMode;

    private bool _isAdsReady;

    public delegate void SkipAd();
    public event SkipAd OnSkipAd;

    public delegate void FinishAd();
    public event FinishAd OnFinishAd;

    public delegate void AdsIsReady();
    public event AdsIsReady OnAdsIsReady;

    public void ShowRewardedVideo()
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show(VideoId);
        }
    }

    public void OnUnityAdsReady(string placementId)
    {
        OnAdsIsReady?.Invoke();
    }

    public void OnUnityAdsDidError(string message)
    {
        
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        switch (showResult)
        {
            case ShowResult.Failed:
                OnSkipAd?.Invoke();
                break;
            case ShowResult.Finished:
                OnFinishAd?.Invoke();
                break;
            case ShowResult.Skipped:
                OnSkipAd?.Invoke();
                break;
        }
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        InitAds();
    }

    private void InitAds()
    {
        if (!_isAdsReady)
        {
            Advertisement.AddListener(this);
            Advertisement.Initialize(GameId, _isTestMode);
            _isAdsReady = true;
        }
    }
}

using GoogleMobileAds.Api;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
    private BannerView bannerView;
    private RewardedAd rewardedAd;
    private InterstitialAd interstitialAd;

    private InitializationStatus initStatues;

    private void Start()
    {
        MobileAds.Initialize(initStatues => Debug.Log("Ads Initialized"));
    }

    #region BannerAd

    private string GetBannerUnitID()
    {
        

        return "unused";
    }

    public void LoadBanner()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
        }

        bannerView = new BannerView(GetBannerUnitID(), AdSize.Banner, AdPosition.Bottom);

        AdRequest request = new AdRequest();

        bannerView.LoadAd(request);
    }

    public void ShowBanner()
    {
        if (bannerView == null) 
        {
            Debug.LogWarning("Banner No Disponible");
            return;
        }

        bannerView.Show();
    }

    public void HideBanner()
    {
        if (bannerView == null)
        {
            return;
        }

        bannerView.Hide();
    }

    public void DestroyBanner()
    {
        if (bannerView == null) return;

        bannerView.Destroy();
        bannerView = null;
    }

    #endregion

    //private string GetRewarded()
    //{

    //}

    public void LoadRewarded()
    {

    }
}

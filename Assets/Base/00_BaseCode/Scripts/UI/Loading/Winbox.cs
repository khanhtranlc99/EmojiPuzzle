using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Winbox : BaseBox
{
    public static Winbox _instance;
    public static Winbox Setup()
    {
        if (_instance == null)
        {
            _instance = Instantiate(Resources.Load<Winbox>(PathPrefabs.WIN_BOX));
            _instance.Init();
        }
        _instance.InitState();
        return _instance;
    }

    public Button nextButton;
    public GiftBar giftBar;

    public List<GameObject> lsObj;
    public Transform parent;

    public void Init()
    {
        nextButton.onClick.AddListener(delegate { HandleNext(); });
        nextButton.transform.localScale = Vector3.zero;
        GameController.Instance.AnalyticsController.WinLevel(UseProfile.CurrentLevel);

        UseProfile.WinStreak += 1;
        GameController.Instance.musicManager.PlayWinSound();
        var temp = Instantiate(lsObj[GamePlayController.Instance.playerContain.levelData.idEmoji]);
        temp.transform.SetParent(parent,false);
    }
    public void InitState()
    {

        giftBar.Init(this, delegate { HandleScaleBtn(); });
        UseProfile.CurrentLevel += 1;
        if (UseProfile.CurrentLevel >= 500)
        {
            UseProfile.CurrentLevel = 500;
        }
    



    }

    private void HandleScaleBtn()
    {
        nextButton.transform.DOScale(new Vector3(1, 1, 1), 0.5f);
    }
    public void HandleNext()
    {
        GameController.Instance.musicManager.PlayClickSound();



        GameController.Instance.admobAds.ShowInterstitialAd(  actionIniterClose: () => { Next(); } );
        void Next()
        {

            Close();
         
            Initiate.Fade("GamePlay", Color.black, 2f);

        }
    }
    private void HandleReward()
    {
        GameController.Instance.musicManager.PlayClickSound();
        GameController.Instance.admobAds.ShowRewardedAd(
                   actionReward: () =>
                   {
                       Close();
                       //GameController.Instance.admobAds.HandleHideMerec();

                       List<GiftRewardShow> giftRewardShows = new List<GiftRewardShow>();
                       giftRewardShows.Add(new GiftRewardShow() { amount = 1, type = GiftType.Coin });
                       PopupRewardBase.Setup(false).Show(giftRewardShows, delegate {
                           PopupRewardBase.Setup(false).Close();
                           Initiate.Fade("GamePlay", Color.black, 2f);
                       });

                   },
                   actionNotLoadedVideo: () =>
                   {

                   },
                 
                   ActionWatchVideo.WinBox_Claim_Coin );
    }
    private void OnDestroy()
    {

    }

}

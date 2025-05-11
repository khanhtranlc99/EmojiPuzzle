using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System;
using MoreMountains.NiceVibrations;
using UnityEngine.Events;

public class GameScene : BaseScene
{
 
    public Text tvLevel;
    public Button settinBtn;
    public Button btnClean;
    public Button btnSkip;
    public Button resetBtn;
    public Transform canvas;

    public List<Sprite> lsSpriteFlag;
    public Image iconFlag;
 
    public void Init(LevelData levelData)
    {

        tvLevel.text = "Level " + UseProfile.CurrentLevel;
        iconFlag.sprite = lsSpriteFlag[UseProfile.CurrentLevel - 1];
        settinBtn.onClick.AddListener(delegate { SettingBox.Setup(false).Show(); });
        btnClean.onClick.AddListener(delegate { HandleClean(); });
        btnSkip.onClick.AddListener(delegate { HandleSkip();  });
    
        resetBtn.onClick.AddListener(delegate { HandleReset(); });
    }
    public void HandleClean()
    {
        GameController.Instance.musicManager.PlayClickSound();
        GamePlayController.Instance.playerContain.emojiController.HandleClean();
    }
    private void HandleReset()
    {
        GameController.Instance.musicManager.PlayClickSound();



        GameController.Instance.admobAds.ShowInterstitialAd(actionIniterClose: () => { Next(); });
        void Next()
        {

          
            Initiate.Fade("GamePlay", Color.black, 2f);

        }
    }    
    public void HandleSkip()
    {
        GameController.Instance.musicManager.PlayClickSound();
        GameController.Instance.admobAds.ShowRewardedAd(
                   actionReward: () =>
                   {

                       UseProfile.CurrentLevel += 1;
                       if (UseProfile.CurrentLevel >= 50)
                       {
                           UseProfile.CurrentLevel = 50;
                       }
                       Initiate.Fade("GamePlay", Color.black, 2f);


                   },
                   actionNotLoadedVideo: () =>
                   {

                   },

                   ActionWatchVideo.Skip_level);
    }


    public override void OnEscapeWhenStackBoxEmpty()
    {
     
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;
public class GiftBar : MonoBehaviour
{
     public Image fillAmount;
     public TMP_Text tvBar;
      public levelChest levelChest;
      float tempPercent; 
      Winbox  paramWinbox;
     public void Init(Winbox winbox,Action CallBack)
     {

       
        paramWinbox = winbox;
        UseProfile.LevelOfLevelChest += 1;
        UseProfile.CurrentLevelOfLevelChest += 1;

        levelChest = GameController.Instance.dataContain.levelChestData.CurrentLevelChest;
        tvBar.text =  UseProfile.CurrentLevelOfLevelChest + "/" + GameController.Instance.dataContain.levelChestData.TargetChest;
      

        tempPercent =  (float)UseProfile.CurrentLevelOfLevelChest / GameController.Instance.dataContain.levelChestData.TargetChest ;


        fillAmount.DOFillAmount(tempPercent,0.5f).SetDelay(0.75f).OnComplete(delegate{

           if(fillAmount.fillAmount >=1)
           {
            
                UseProfile.CurrentLevelOfLevelChest = 0;
                HandleShowGift();
                 
           }
           else
           {
              CallBack?.Invoke();
           }
  
        });
        
        Debug.LogError(" tempPercent_" + tempPercent);
     }
     
     private void HandleShowGift()
     {  
        GameController.Instance.dataContain.giftDatabase.Claim(levelChest.giftType, levelChest.amount);
        List<GiftRewardShow> giftRewardShows = new List<GiftRewardShow>();
        giftRewardShows.Add(new GiftRewardShow() { amount =  levelChest.amount, type = levelChest.giftType });
        PopupRewardBase.Setup(false).Show(giftRewardShows, delegate {

           paramWinbox.HandleNext();
         
         });

     }


}

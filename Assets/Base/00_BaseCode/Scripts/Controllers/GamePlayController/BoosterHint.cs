using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
public class BoosterHint : MonoBehaviour
{
    public Button btnHint_Booster;
    public Text tvNum;
    public GameObject objNum;
    public GameObject objAds;
    public GameObject lockIcon;   
    public bool wasUseHint_Booster;
 
    public LineRenderer lineRenderer;
     
    public void Init()
    {
        
        wasUseHint_Booster = false;   
        if (UseProfile.CurrentLevel >= 3)//3
        {     
            lockIcon.gameObject.SetActive(false);
            HandleUnlock();     
        }
        else
        {
            lockIcon.gameObject.SetActive(true);
            objNum.SetActive(false);
               objAds.SetActive(false);
            HandleLock();       
        }


        void HandleUnlock()
        {
            btnHint_Booster.onClick.AddListener(HandleHint_Booster);
            if (UseProfile.HintBooster > 0)
            {
                objNum.SetActive(true);
                tvNum.text = UseProfile.HintBooster.ToString();
                objAds.SetActive(false);
            }
            else
            {
                objNum.SetActive(false);
                tvNum.gameObject.SetActive(false);
                objAds.SetActive(true);
         
            }
            EventDispatcher.EventDispatcher.Instance.RegisterListener(EventID.CHANGE_HINT_BOOSTER, ChangeText);
        }
        void HandleLock()
        {
          
            
            btnHint_Booster.onClick.AddListener(HandleLockBtn);
        }
        GamePlayController.Instance.tutHint.StartTut();
 
    }

    public void HandleLockBtn()
    {
        GameController.Instance.musicManager.PlayClickSound();
        GameController.Instance.moneyEffectController.SpawnEffectText_FlyUp
                              (
                              btnHint_Booster.transform.position,
                              "Unlock at level 3",
                              Color.white,
                              isSpawnItemPlayer: true
                              );
    }


    public void HandleHint_Booster()
    {
        GameController.Instance.musicManager.PlayClickSound();
        if (UseProfile.HintBooster >= 1)
        {      
            UseProfile.HintBooster -= 1;         
            wasUseHint_Booster = true;
            btnHint_Booster.interactable = false;
            HandleLine();
            GamePlayController.Instance.tutHint.NextTut();
        }
        else
        {
         GameController.Instance.admobAds.ShowRewardedAd(
                     actionReward: () =>
                     {
                         UseProfile.HintBooster += 3;                        
                         List<GiftRewardShow> giftRewardShows = new List<GiftRewardShow>();
                         giftRewardShows.Add(new GiftRewardShow() { amount = 3, type = GiftType.HintBooster });
                         PopupRewardBase.Setup(false).Show(giftRewardShows, delegate { });
                     },
                     actionNotLoadedVideo: () =>
                     {
                         GameController.Instance.moneyEffectController.SpawnEffectText_FlyUp
                          (
                         
                          btnHint_Booster.transform.position,
                          "No video at the moment!",
                          Color.white,
                          isSpawnItemPlayer: true
                          );
                     },
                   
                       ActionWatchVideo.Hint );
        }


    }

 private void HandleLine()
{
    var temp = GamePlayController.Instance.playerContain.levelData.lsEmoji;
    lineRenderer.positionCount = temp.Count;
    
    // Ẩn line ban đầu bằng cách set width = 0
    lineRenderer.startWidth = 0;
    lineRenderer.endWidth = 0;
    
    // Bắt đầu animation
    StartCoroutine(AnimateLine(temp));
}

private IEnumerator AnimateLine(List<EmojiBase> blocks)
{
    float animationDuration = 0.3f; // Thời gian di chuyển giữa 2 điểm
    float lineWidth = 0.1f; // Độ rộng của line
    
    for(int i = 0; i < blocks.Count - 1; i++)
    {
        Vector3 startPos = new Vector3(blocks[i].transform.position.x, blocks[i].transform.position.y, -1);
        Vector3 endPos = new Vector3(blocks[i + 1].transform.position.x, blocks[i + 1].transform.position.y, -1);
        
        // Reset lại positions cho đoạn line hiện tại
        lineRenderer.positionCount = i + 2; // Chỉ hiển thị đến điểm đang vẽ
        
        // Set vị trí các điểm đã vẽ xong
        for(int j = 0; j <= i; j++)
        {
            lineRenderer.SetPosition(j, new Vector3(blocks[j].transform.position.x, blocks[j].transform.position.y, -1));
        }
        
        // Set độ rộng của line
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        
        float elapsedTime = 0f;
        while(elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animationDuration;
            
            // Di chuyển điểm cuối của đoạn hiện tại
            Vector3 currentPos = Vector3.Lerp(startPos, endPos, t);
            lineRenderer.SetPosition(i + 1, currentPos);
            
            yield return null;
        }
        
        // Đảm bảo điểm cuối cùng đúng vị trí
        lineRenderer.SetPosition(i + 1, endPos);
        
        yield return new WaitForSeconds(0.1f); // Delay nhỏ giữa các đoạn
    }
}

    public void ChangeText(object param)
    {
        tvNum.text = UseProfile.HintBooster.ToString();
        if (UseProfile.HintBooster > 0)
        {
            objNum.SetActive(true);
            tvNum.gameObject.SetActive(true);
            tvNum.text = UseProfile.HintBooster.ToString();
            objAds.SetActive(false);
        }
        else
        {
            objNum.SetActive(false);
            tvNum.gameObject.SetActive(false);
            objAds.SetActive(true);
 
        }
      
    }
    public void OnDestroy()
    {
        EventDispatcher.EventDispatcher.Instance.RemoveListener(EventID.CHANGE_HINT_BOOSTER, ChangeText);
    }

 
}

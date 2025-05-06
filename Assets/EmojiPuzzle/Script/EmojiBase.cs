using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MoreMountains.NiceVibrations;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
public enum BlockType
{
    Active,
    Horizontal,
    Vertical,
    Random
}
public abstract class EmojiBase : MonoBehaviour
{
    public BlockType blockType;
    public GameObject emojiCurrent;
    public GameObject bgEmoji;
 

    public int valueNumber;
    public int valueActive;

    public Vector3 firstPost;
    public int row;
    public int col;
    public AudioClip OpenSfx;
    public AudioClip CloseSfx;

    public List<EmojiId> lsEmoji;
    public GameObject GetEmojiCurrent(int id)
    {
        foreach(var item in lsEmoji)
        {
            if(item.id == id)
            {
                return item.obj;
            }
        }
        return lsEmoji[0].obj;
    }

    [Button]
    public void HandleRemove()
    {
        
        foreach (var item in lsEmoji)
        {
            if(item.obj != emojiCurrent)
            {
                DestroyImmediate(item.obj);
            }
        }    

        for(int i = lsEmoji.Count -1 ; i >= 0 ; i --)
        {
            if (lsEmoji[i].obj == null)
            {
                lsEmoji.Remove(lsEmoji[i]);
            }
        }
    }    

    public abstract void SetFirstPost();

    public abstract void Init(int idEmoji,Data param, int row, int col);

    public abstract void HandleFill(int param);

    public abstract void HandleOff();

    public void HandleUnlock()
    {
        //if (!tvCount.gameObject.activeSelf)
        //{
        //    tvCount.color = new Color32(0, 0, 0, 0);
        //    tvCount.gameObject.transform.localScale = new Vector3(0, 0, 0);
        //    tvCount.gameObject.SetActive(true);
        //    tvCount.text = valueNumber.ToString();
        //    Sequence seq = DOTween.Sequence();
        //    seq.Join(tvCount.gameObject.transform.DOScale(1.5f, 0.5f));
        //    seq.Join(tvCount.DOColor(Color.white, 0.5f));
        //    seq.Join(tvCount.gameObject.transform.DOScale(1, 0.5f));

        //}

    }

    public void HandleShake()
    {
        this.transform.DOKill();
        this.transform.DOShakePosition(0.5f, 0.1f, 10, 10, false, true).onComplete = () =>
        {
            this.transform.position = firstPost;
        };
        MMVibrationManager.Haptic(HapticTypes.Warning);
    }
    protected void OnMouseEnter()
    {
        if (GamePlayController.Instance.stateGame == StateGame.Playing)
        {
            GamePlayController.Instance.playerContain.emojiController.HandleBlock(this);
       //     GamePlayController.Instance.gameScene.HandleSubTrackMove();
        }

    }

    public void HandlePlayOpenSound()
    {
      //  GameController.Instance.musicManager.PlayOneShot(OpenSfx);
        MMVibrationManager.Haptic(HapticTypes.HeavyImpact);

    }

    public void HandlePlayCloseSound()
    {
      //  GameController.Instance.musicManager.PlayOneShot(CloseSfx);
        MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
    }

    public void OnDestroy()
    {
        this.transform.DOKill();
        this.emojiCurrent.transform.DOKill();
    }
}
[System.Serializable]
public class EmojiId
{
    public int id;
    public GameObject obj;
}
 
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
 

public class EmojiNormal : EmojiBase
{

    
    public override void Init(int idEmoji, Data param, int row, int col)
    { 
        this.row = row;
        this.col = col;
        valueNumber = param.valueNumber;
        valueActive = param.isActive;
        emojiCurrent = GetEmojiCurrent(idEmoji);
        emojiCurrent.SetActive(true);
        emojiCurrent.transform.localScale = Vector3.zero;
    }

    public override void HandleFill(int param)
    {
        bgEmoji.gameObject.SetActive(false);
        emojiCurrent.transform.DOScale(Vector3.one, 0.4f).SetEase(Ease.OutBack);
    }

    public override void HandleOff()
    {
        bgEmoji.gameObject.SetActive(true);
        emojiCurrent.transform.DOScale(Vector3.zero, 0.4f).SetEase(Ease.OutBack);
    }

    public override void SetFirstPost()
    {
        firstPost = this.transform.position;
    }
}
 
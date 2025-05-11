using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutHint_Step_1 : TutorialBase
{
    public GameObject currentHand;

    public override bool IsCanEndTut()
    {
        if (currentHand != null)
        {
            Destroy(currentHand.gameObject);
        }

        return base.IsCanShowTut();

    }

    public override void StartTut()
    {
        Debug.LogError("TutHint_Step_1");
        if (UseProfile.CurrentLevel == 3)
        {
           if(currentHand != null)
            {
                return;
            }                
            currentHand = SimplePool2.Spawn(handTut);
            currentHand.transform.parent = GamePlayController.Instance.playerContain.boosterHint.btnHint_Booster.transform;
            currentHand.transform.localScale = new Vector3(1, 1, 1);
            currentHand.transform.localEulerAngles = new Vector3(0, 0, 120);
            currentHand.transform.position = new Vector3(post.x +0.5f, post.y + 0.7f, post.z);
        
        }
    }
    Vector3 post
    {
        get
        {
            return GamePlayController.Instance.playerContain.boosterHint.btnHint_Booster.transform.position;
        }
    }    


    protected override void SetNameTut()
    {
     
    }
}

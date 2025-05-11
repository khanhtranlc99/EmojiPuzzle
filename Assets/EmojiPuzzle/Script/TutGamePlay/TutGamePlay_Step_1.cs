using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TutGamePlay_Step_1 : TutorialBase
{
  public GameObject currentHand;
  [SerializeField] LineRenderer lineRenderer;
  
 
    public override bool IsCanEndTut()
    {
        if(currentHand != null)
        {
            Destroy(currentHand.gameObject);
 
        }

     return true; 
    }

    public GameObject tempBlock_1
    {
        get
        {
            return GamePlayController.Instance.playerContain.levelData.GetEmojiBaseById(9).gameObject;
        }
    }
       public GameObject tempBlock_3
    {
        get
        {
            return GamePlayController.Instance.playerContain.levelData.GetEmojiBaseById(3).gameObject;
        }
    }
       public GameObject tempBlock_5
    {
        get
        {
            return GamePlayController.Instance.playerContain.levelData.GetEmojiBaseById(5).gameObject;
        }
    }
       public GameObject tempBlock_7
    {
        get
        {
            return GamePlayController.Instance.playerContain.levelData.GetEmojiBaseById(7).gameObject;
        }
    }
       public GameObject tempBlock_8
    {
        get
        {
            return GamePlayController.Instance.playerContain.levelData.GetEmojiBaseById(8).gameObject;
        }
    }
       public GameObject tempBlock_9
    {
        get
        {
            return GamePlayController.Instance.playerContain.levelData.GetEmojiBaseById(1).gameObject;
        }
    }

    public override void StartTut()
    {
     
        if (UseProfile.CurrentLevel == 1)
        {
            if (currentHand != null)
            {
                return;
            }
     
        
        currentHand = SimplePool2.Spawn(handTut);
        DOTween.defaultEaseType = Ease.Linear;
        HandleLoopHand();



          void HandleLoopHand()
          {
             
         
            
              currentHand.transform.position = tempBlock_1.transform.position;
              currentHand.GetComponent<SpriteRenderer>().color = new Color32(0,0,0,0);
 
      

              currentHand.GetComponent<SpriteRenderer>().DOColor(new Color32(255,255,255,255), 0.5f).OnComplete(delegate{
               currentHand.transform.DOMove(new Vector3(tempBlock_3.transform.position.x,tempBlock_3.transform.position.y,-1), 1) .OnComplete(delegate{
                    
               currentHand.transform.DOMove(new Vector3(tempBlock_5.transform.position.x,tempBlock_5.transform.position.y,-1), 1) .OnComplete(delegate{
                  currentHand.transform.DOMove(new Vector3(tempBlock_7.transform.position.x,tempBlock_7.transform.position.y,-1),1) .OnComplete(delegate{
                  currentHand.transform.DOMove(new Vector3(tempBlock_8.transform.position.x,tempBlock_8.transform.position.y,-1), 1) .OnComplete(delegate{
                      currentHand.transform.DOMove(new Vector3(tempBlock_9.transform.position.x,tempBlock_9.transform.position.y,-1), 1) .OnComplete(delegate{
                   currentHand.GetComponent<SpriteRenderer>().DOColor(new Color32(0,0,0,0), 0.5f).OnComplete(delegate{

                              lineRenderer.positionCount = 0;
                        HandleLoopHand();

                   });
               });
               });
               });
               });
               });
          });
          }
        }
        
       
    }
 



    public void DeleteHand()
    {
        if (currentHand != null)
        {   
            currentHand.transform.DOKill();
            Destroy(currentHand.gameObject);

        }

    }

    protected override void SetNameTut()
    {
     
    }
    public override void OnEndTut()
    {
      
    }
}

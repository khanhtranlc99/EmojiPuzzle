using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EmojiController : MonoBehaviour
{
    public int target;
     public bool isEnter;
     public int currentValue;
     public EmojiBase currentBlock;
     public List<EmojiBase> listBlockFill;
     public LineRenderer lineRenderer;
     
     public bool CanFill(EmojiBase blockParam)
     {
        // Kiểm tra cùng cột và cách 1 hàng (trên hoặc dưới)
        bool isVerticalNeighbor = currentBlock.col == blockParam.col && 
                             Mathf.Abs(currentBlock.row - blockParam.row) == 1;

    // Kiểm tra cùng hàng và cách 1 cột (trái hoặc phải)
    bool isHorizontalNeighbor = currentBlock.row == blockParam.row && 
                               Mathf.Abs(currentBlock.col - blockParam.col) == 1;

    // Trả về true nếu block nằm kề theo chiều dọc hoặc ngang
    return isVerticalNeighbor || isHorizontalNeighbor;
}
      
     public bool isWin
     {
        get
        {
            if(listBlockFill.Count == target)
            {
                return true;
            }
            return false;
        }
     }
     
     [SerializeField] GameObject vfxGrass;
        [SerializeField] GameObject vfxRock;
     public void Init(LevelData param)
     {
        currentValue = 1;
        target = param.lsEmoji.Count;
        SimplePool2.Preload(vfxGrass,30);
          SimplePool2.Preload(vfxRock,30);
     }
     public void HandleBlock(EmojiBase block)
     {
        // GamePlayController.Instance.tutGamePlay.NextTut();
         
         //GamePlayController.Instance.tutUndoBooster.StartTut();
        if(currentBlock == null)
        {
            if(block.valueActive != 0 )
            {
                if(block.valueNumber == currentValue)
                {
                    currentBlock = block;
                    currentBlock.HandleFill(currentValue);
                    if(!listBlockFill.Contains(block))
                    {
                        listBlockFill.Add(block);
                      var tempvfx = SimplePool2.Spawn(vfxGrass);
                      tempvfx.transform.position = block.transform.position;
                      block.HandlePlayOpenSound();
                    }
                }
                else
                {
                    block.HandleShake();
                }
            }
            else
            {
                currentBlock = block;
                currentBlock.HandleFill(currentValue);
               if(!listBlockFill.Contains(block))
                    {
                        listBlockFill.Add(block);
                          block.HandlePlayOpenSound();
                      var tempvfx = SimplePool2.Spawn(vfxGrass);
                      tempvfx.transform.position = block.transform.position;
                    }
            }
        }   
        else
        { 
          
            if(block != currentBlock)
            {
                if(!listBlockFill.Contains(block))
                {
                    if(CanFill(block))
                    {
                        if(block.valueActive != 0 )
                        {
                            if(block.valueNumber - 1 == currentValue)
                            {
                                currentBlock = block;
                                currentValue++;
                                currentBlock.HandleFill(currentValue);
                               if(!listBlockFill.Contains(block))
                                {
                                   listBlockFill.Add(block);
                                     block.HandlePlayOpenSound();
                                   var tempvfx = SimplePool2.Spawn(vfxGrass);
                                   tempvfx.transform.position = block.transform.position;
                                }
                            }
                            else
                            {
                                block.HandleShake();
                            }
                        }
                        else
                        {
                            currentBlock = block;
                            currentValue++; 
                            currentBlock.HandleFill(currentValue);
                            if(!listBlockFill.Contains(block))
                    {
                        listBlockFill.Add(block);
                          block.HandlePlayOpenSound();
                        var tempvfx = SimplePool2.Spawn(vfxGrass);
                      tempvfx.transform.position = block.transform.position;
                    }
                        }
                    }
                    else 
                    {
                        block.HandleShake();
                    }
                }
                else
                {
                     
                    // Tìm index của block được click trong list
                    int clickedIndex = listBlockFill.IndexOf(block);
                    
                    // Xử lý các block được thêm vào sau block được click
                    for(int i = listBlockFill.Count - 1; i > clickedIndex; i--)
                    {

                        EmojiBase blockToRemove = listBlockFill[i];
                        blockToRemove.HandlePlayCloseSound();
                        blockToRemove.HandleOff();
                        listBlockFill.RemoveAt(i);
                    }

                    // Cập nhật currentBlock và currentValue
                    currentBlock = block;
                    currentValue = clickedIndex + 1;
             if (listBlockFill.Count == 1  )
              {
                       listBlockFill[0].HandlePlayCloseSound();
                       listBlockFill[0].HandleOff();
                       currentBlock = null;
                        listBlockFill.Clear();
              }
                 
                }
            }
            else
            {
                block.HandleShake();
            }
        }
        
        if(isWin)
        {
            if(GamePlayController.Instance.stateGame == StateGame.Playing)
            {
                GamePlayController.Instance.stateGame = StateGame.Win;
                Debug.LogError("Win");
                //GamePlayController.Instance.playerContain.levelData.HandleEffectWin(delegate{
                Winbox.Setup().Show();
                //});
            
            }
          
        }
        lineRenderer.positionCount = listBlockFill.Count;
        for (int i= 0; i < listBlockFill.Count; i ++)
        {
            lineRenderer.SetPosition(i, new Vector3(listBlockFill[i].transform.position.x, listBlockFill[i].transform.position.y, 1) );
        }    
    }
    public void HandleRemoveLastBlock()
    {
 
       listBlockFill[listBlockFill.Count - 1].HandleOff();
       listBlockFill.Remove(listBlockFill[listBlockFill.Count - 1]);
                 
    }
      
    public void HandleExit(EmojiBase block)
    {
     
         if(listBlockFill.Count == 1 && listBlockFill[0] == block )
        {
          listBlockFill[0].HandleOff();
          currentBlock = null;
          listBlockFill.Clear();
        }
    }
    private void OnMouseEnter()
    {

        if (listBlockFill.Count == 1  )
        {
            listBlockFill[0].HandlePlayCloseSound();
            listBlockFill[0].HandleOff();
            currentBlock = null;
            listBlockFill.Clear();
        }
    }
    public void HandleClean()
    {
        if(listBlockFill.Count > 0)
        {
            foreach(var item in listBlockFill)
            {
                item.HandlePlayCloseSound();
                item.HandleOff();
            }
              currentBlock = null;
              listBlockFill.Clear();
              currentValue = 1;
        }
    }
     
    public void HandleSpawnRockVfx(EmojiBase block)
    {
          var tempvfx = SimplePool2.Spawn(vfxRock);
         tempvfx.transform.position = block.transform.position;
    }
     public void HandleSpawnGrassVfx(EmojiBase block)
    {
          var tempvfx = SimplePool2.Spawn(vfxGrass);
         tempvfx.transform.position = block.transform.position;
    }
}

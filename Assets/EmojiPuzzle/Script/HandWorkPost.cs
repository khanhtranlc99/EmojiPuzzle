using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class HandWorkPost : MonoBehaviour
{
    Vector3 firstPost;
    void Start()
    {
        firstPost = this.transform.position;
        HandleMove();
    }

    private void HandleMove()
    {
        this.transform.DOMove(new Vector3(firstPost.x+0.5f, firstPost.y+0.5f , firstPost.z), 0.5f).OnComplete(delegate {

            this.transform.DOMove(new Vector3(firstPost.x  , firstPost.y , firstPost.z), 0.5f).OnComplete(delegate {

                HandleMove();
            });
        });
    }
    private void OnDestroy()
    {
        this.transform.DOKill();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    private int hightlightMask;
    private int defaultMask;
    private int clickHighlightMask;
    private GameObject oldTarget;
    private GameObject oldClickTarget;
    public PlayerManager playerManager;
    
    public Camera playerCamera;
    private void Awake()
    {
        hightlightMask = LayerMask.NameToLayer("Highlight");
        defaultMask = LayerMask.NameToLayer("Default");
        clickHighlightMask = LayerMask.NameToLayer("HighlightClicked");
    }

    void LateUpdate()
    {
        RaycastHit hit;
        Debug.DrawRay(playerCamera.ScreenPointToRay(Input.mousePosition).origin,playerCamera.ScreenPointToRay(Input.mousePosition).direction*1000f,Color.red);
        if(Physics.Raycast(playerCamera.ScreenPointToRay(Input.mousePosition).origin,playerCamera.ScreenPointToRay(Input.mousePosition).direction,out hit,Mathf.Infinity))
        {
            GameObject target = hit.collider.gameObject;
            if (target.CompareTag("Targetable"))
            {
                if (target.layer == defaultMask)
                {
                    if(oldTarget!=null && oldTarget!=target && oldTarget.layer!=clickHighlightMask)
                        oldTarget.layer = defaultMask;
                    oldTarget = target;
                    target.layer = hightlightMask;
                }
                else if (target.layer == clickHighlightMask)
                {
                    if(oldTarget!=null && oldTarget!=target)
                        oldTarget.layer = defaultMask;
                }
            }
            else
            {
                if(oldTarget!=null && oldTarget!=target && oldTarget.layer!=clickHighlightMask)
                    oldTarget.layer = defaultMask;
            }
        }
        else
        {
            if (oldTarget != null && oldTarget.layer!=clickHighlightMask)
            {
                oldTarget.layer = defaultMask;
            }
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            if(Physics.Raycast(playerCamera.ScreenPointToRay(Input.mousePosition).origin,playerCamera.ScreenPointToRay(Input.mousePosition).direction,out hit,Mathf.Infinity))
            {
                GameObject target = hit.collider.gameObject;
                if (target.CompareTag("Targetable"))
                {
                    if(oldClickTarget!=null && oldClickTarget!=target)
                        oldClickTarget.layer = defaultMask;
                    oldClickTarget = target;
                    target.layer = clickHighlightMask;
                    PlayerManager test= target.GetComponentInParent<PlayerManager>();
                    playerManager.targetId = (test == null)?-1:test.id;
                    if(playerManager.targetId==-1)
                        playerManager.targetId = 999;
                }
            }
        }else if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(oldClickTarget!=null)
            {
                oldClickTarget.layer = defaultMask;
                oldClickTarget = null;
                playerManager.targetId = -1;
                
                if(Physics.Raycast(playerCamera.ScreenPointToRay(Input.mousePosition).origin,playerCamera.ScreenPointToRay(Input.mousePosition).direction,out hit,Mathf.Infinity))
                {
                    GameObject target = hit.collider.gameObject;
                    if (target.CompareTag("Targetable"))
                    {
                        if (target.layer == defaultMask)
                        {
                            if(oldTarget!=null && oldTarget!=target && oldTarget.layer!=clickHighlightMask)
                                oldTarget.layer = defaultMask;
                            oldTarget = target;
                            target.layer = hightlightMask;
                        }
                        else if (target.layer == clickHighlightMask)
                        {
                            if(oldTarget!=null && oldTarget!=target)
                                oldTarget.layer = defaultMask;
                        }
                    }
                    else
                    {
                        if(oldTarget!=null && oldTarget!=target && oldTarget.layer!=clickHighlightMask)
                            oldTarget.layer = defaultMask;
                    }
                }
                else
                {
                    if (oldTarget != null && oldTarget.layer!=clickHighlightMask)
                    {
                        oldTarget.layer = defaultMask;
                    }
                }
            }
        }
    }
}

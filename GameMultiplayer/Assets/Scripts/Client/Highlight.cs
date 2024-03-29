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
    public static Highlight instance;

    public Camera playerCamera;
    private void Awake()
    {
        hightlightMask = LayerMask.NameToLayer("Highlight");
        defaultMask = LayerMask.NameToLayer("Default");
        clickHighlightMask = LayerMask.NameToLayer("HighlightClicked");
        if(instance == null)
            instance = this;
    }
    RaycastHit hit;
    private void ChangeGameObjectLayer(GameObject gameObject,int layer)
    {
        gameObject.layer = layer;
        foreach (Transform child in gameObject.transform)
            child.gameObject.layer = layer;
    }
    
    private GameObject GetTransformParent(GameObject gameObject)
    {
        if (gameObject.transform.parent!=null && gameObject.transform.parent.gameObject.CompareTag("Targetable"))
            return gameObject.transform.parent.gameObject;
        return gameObject;
    }

    void Update()
    {
        ClientSend.PlayerTargetId(playerManager.targetId);
    }

    void LateUpdate()
    {
        bool isPointerOverUIElement = UITest.instance.IsPointerOverUIElement();
        //Debug.DrawRay(playerCamera.ScreenPointToRay(Input.mousePosition).origin,playerCamera.ScreenPointToRay(Input.mousePosition).direction*1000f,Color.red);
        if(!Input.GetMouseButton(1) && Physics.Raycast(playerCamera.ScreenPointToRay(Input.mousePosition).origin,playerCamera.ScreenPointToRay(Input.mousePosition).direction,out hit,Mathf.Infinity) && !isPointerOverUIElement)
        {
            GameObject target = GetTransformParent(hit.collider.gameObject);
            if (target.CompareTag("Targetable"))
            {
                if (target.layer == defaultMask)
                {
                    if(oldTarget!=null && oldTarget!=target && oldTarget.layer!=clickHighlightMask)
                        ChangeGameObjectLayer(oldTarget,defaultMask);
                    oldTarget = target;
                    ChangeGameObjectLayer(target, hightlightMask);
                }
                else if (target.layer == clickHighlightMask)
                {
                    if(oldTarget!=null && oldTarget!=target)
                        ChangeGameObjectLayer(oldTarget, defaultMask);
                }
            }
            else
            {
                if(oldTarget!=null && oldTarget!=target && oldTarget.layer!=clickHighlightMask)
                    ChangeGameObjectLayer(oldTarget, defaultMask);
            }
        }
        else
        {
            if (oldTarget != null && oldTarget.layer!=clickHighlightMask)
                ChangeGameObjectLayer(oldTarget, defaultMask);
        }
        
        if (Input.GetMouseButtonDown(0) && !isPointerOverUIElement)
        {
            if(Physics.Raycast(playerCamera.ScreenPointToRay(Input.mousePosition).origin,playerCamera.ScreenPointToRay(Input.mousePosition).direction,out hit,Mathf.Infinity))
            {
                GameObject target = GetTransformParent(hit.collider.gameObject);
                if (target.CompareTag("Targetable"))
                {
                    if(oldClickTarget!=null && oldClickTarget!=target)
                        ChangeGameObjectLayer(oldClickTarget, defaultMask);
                    oldClickTarget = target;
                    ChangeGameObjectLayer(target,clickHighlightMask);
                    PlayerManager test= target.GetComponentInParent<PlayerManager>();
                    playerManager.targetId = (test == null)?-1:test.id;
                    if(playerManager.targetId==-1)
                        playerManager.targetId = 999;
                }
            }
        }
    }

    public void EscapePressed()
    {
        if (oldClickTarget != null)
        {
            ChangeGameObjectLayer(oldClickTarget, defaultMask);
            oldClickTarget = null;
            playerManager.targetId = -1;

            if (Physics.Raycast(playerCamera.ScreenPointToRay(Input.mousePosition).origin,
                    playerCamera.ScreenPointToRay(Input.mousePosition).direction, out hit, Mathf.Infinity))
            {
                GameObject target = GetTransformParent(hit.collider.gameObject);
                if (target.CompareTag("Targetable"))
                {
                    if (target.layer == defaultMask)
                    {
                        if (oldTarget != null && oldTarget != target && oldTarget.layer != clickHighlightMask)
                            ChangeGameObjectLayer(oldTarget, defaultMask);
                        oldTarget = target;
                        ChangeGameObjectLayer(target, hightlightMask);
                    }
                    else if (target.layer == clickHighlightMask)
                    {
                        if (oldTarget != null && oldTarget != target)
                            ChangeGameObjectLayer(oldTarget, defaultMask);
                    }
                }
                else
                {
                    if (oldTarget != null && oldTarget != target && oldTarget.layer != clickHighlightMask)
                        ChangeGameObjectLayer(oldTarget, defaultMask);
                }
            }
            else
            {
                if (oldTarget != null && oldTarget.layer != clickHighlightMask)
                    ChangeGameObjectLayer(oldTarget, defaultMask);
            }
        }else
            PauseMenu.instance.EscapePressed();
    }
}
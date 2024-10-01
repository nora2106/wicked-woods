using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class UsableObject : MonoBehaviour, ISetup
{
    protected Text displayText;
    public string usableItemID;
    public bool locked;
    public string id;
    public string lockedText;
    protected GameManager gm;

    public void Setup()
    {
        gm = GameManager.Instance;
        if(locked && gm.gameData.unlockedObjs.Contains(id))
        {
            locked = false;
        }
    }

    private void OnMouseDown() {
        if(!EventSystem.current.IsPointerOverGameObject())
        {
            Action();
        }
    }

    public abstract void Action();

    public void Unlock() {
        OpenAnimation();
        locked = false;
        gm.gameData.unlockedObjs.Add(id);
    }

    public abstract void OpenAnimation();
}

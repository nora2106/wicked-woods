using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class UsableObject : MonoBehaviour, ISetup
{
    public bool locked;
    public string id;
    protected GameManager gm;

    public void Setup()
    {
        gm = GameManager.Instance;
        if(locked && gm.save.data.unlockedObjs.Contains(id))
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
        if(id != null)
        {
            gm.save.data.unlockedObjs.Add(id);
        }

        if(gameObject.GetComponent<ItemInteraction>())
        {
            Destroy(gameObject.GetComponent<ItemInteraction>());
        }
    }

    public abstract void OpenAnimation();
}

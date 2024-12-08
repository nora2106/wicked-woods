using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.EventSystems;

public abstract class UsableObject : MonoBehaviour, ISetup
{
    public bool locked;
    public string id;
    public string objName;
    public LocalizedString localizedName;
    public LocalizedString lockedText;

    protected GameManager gm;

    protected void Start()
    {
        localizedName.StringChanged += UpdateName;
    }

    public void Setup()
    {
        gm = GameManager.Instance;
        if(locked && gm.save.data.unlockedObjs.Contains(id))
        {
            locked = false;
        }
    }

    private void OnMouseDown() {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (!locked)
            {
                Action();
            }
            else if (!lockedText.IsEmpty)
            {
                var dict = new Dictionary<string, object>
            {
                { "object", objName }
            };
                lockedText.Arguments = new object[] { dict };
                gm.SetText(lockedText.GetLocalizedString());
            }
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

    protected void UpdateName(string val)
    {
        objName = val;
    }
}

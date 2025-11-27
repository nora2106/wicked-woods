using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.EventSystems;

// any object that can be interacted with on click
// can be locked
public abstract class UsableObject : MonoBehaviour, ISetup
{
    public bool locked;
    public LocalizedString localizedName;
    public LocalizedString lockedText;
    public Vector2 targetPos;
    [HideInInspector] public string id;
    [HideInInspector] public string objName;
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
                gm.QueueInteraction(new SetTextCommand(lockedText.GetLocalizedString()));
            }
        }
    }

    public abstract void Action();

    public void UnlockAction() {
        gm.QueueInteraction(new ActionCommand(Unlock));
    }

    public void Unlock()
    {
        locked = false;
        if (id != null)
        {
            gm.save.data.unlockedObjs.Add(id);
        }

        // TODO maybe delete only specific interaction?
        //if (gameObject.GetComponent<ItemInteraction>())
        //{
        //    Destroy(gameObject.GetComponent<ItemInteraction>());
        //}
        Action();
    }

    protected void UpdateName(string val)
    {
        objName = val;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;

[System.Serializable]
public class InventoryItem : MonoBehaviour
{
    public ItemData data;
    public Inventory inventory;
    public bool selected = false;
    private Vector2 pos;
    private GameObject player;
    private GameObject usableObj;
    private GameObject combineObj;
    private GameObject newSlot;
    private GameObject cursorHandler;
    public string id;
    public string displayName;
    private GameManager gm;

    public void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = data.sprite;
        gameObject.GetComponent<SpriteRenderer>().size = new Vector2(20, 20);
        player = GameObject.FindWithTag("Player");
        cursorHandler = GameObject.FindWithTag("Cursor");
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        id = data.id;
        gm = GameManager.Instance;
        data.displayName.StringChanged += setName;
    }

    public void Update()
    {

        if (combineObj){
            if (Input.GetMouseButtonDown(0))
            {
                print(displayName);
               if (combineObj.GetComponent<InventoryItem>().id == data.combineWith)
               {
                    Reset();
                    RemoveItem();
                    Destroy(combineObj);
                    inventory.addItem(data.newItem);
               }

                else
                {
                    gm.SetText("Ich kann " + displayName + " nicht mit " + combineObj.name + " kombinieren.");
                    transform.position = pos;
                }
            }
        }

        if (newSlot && selected && Input.GetMouseButtonDown(0))
        {
            if (newSlot.transform.childCount == 0)
            {
                selected = false;
                transform.parent = newSlot.transform;
                newSlot = null;
                transform.localPosition = Vector2.zero;
            }
            else if (newSlot.transform == transform.parent)
            {
                selected = false;
                newSlot = null;
                transform.localPosition = Vector2.zero;
            }
        }

        if (selected)
        {
            gm.selectedItemID = data.id;
            gm.itemSelected = true;
            player.GetComponent<MoveCharacter>().canMove = false;
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePosition;

            if(Input.GetMouseButtonDown(1)) {
                Reset();
            }
        }
    }

    private void Reset()
    {
        selected = false;
        gm.itemSelected = false;
        transform.localPosition = Vector2.zero;
        player.GetComponent<MoveCharacter>().canMove = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(selected)
        {
            if (collision.gameObject.GetComponent<InventoryItem>())
            {
                cursorHandler.GetComponent<CursorIcon>().SetUse();
                // translation missing
                gm.SetText(displayName + " mit " + collision.gameObject.GetComponent<InventoryItem>().displayName + " kombinieren");
                combineObj = collision.gameObject;
            }
        }

        if (collision.gameObject.GetComponent<InventorySlot>())
        {
            if (selected)
            {
                newSlot = collision.gameObject;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        combineObj = null;
    }

    private void OnMouseDown()
    {
        selected = true;
    }

    public void RemoveItem()
    {
        Reset();
        if (!data.reusable)
        {
            Destroy(gameObject);
            gm.save.data.inventoryItems.Remove(data);
        }
    }

    private void setName(string val)
    {
        displayName = val;
    }
}

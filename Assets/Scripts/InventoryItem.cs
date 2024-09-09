using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class InventoryItem : MonoBehaviour
{
    public ItemData data;
    public Inventory inventory;
    public bool selected = false;
    private Vector2 pos;
    private GameObject player;
    private Text displayText;
    private GameObject usableObj;
    private GameObject cursorHandler;
    public void Start()
    {
        GetComponent<SpriteRenderer>().sprite = data.sprite;
        GetComponent<SpriteRenderer>().size = new Vector2(20, 20);
        pos = transform.position;
        player = GameObject.FindWithTag("Player");
        displayText = GameObject.FindWithTag("Text").GetComponent<Text>();
        cursorHandler = GameObject.FindWithTag("Cursor");
    }

    public void Update()
    {
        if (usableObj)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (usableObj.GetComponent<UsableObject>().usableItemID == data.id)
                {
                    selected = false;
                    if (data.reusable)
                    {
                        transform.position = pos;
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                    player.GetComponent<MoveCharacter>().canMove = true;
                    usableObj.GetComponent<UsableObject>().Action();
                }

                else
                {
                    displayText.text = ("Ich kann " + data.displayName + " nicht mit " + usableObj.name + " benutzen.");
                    //add english option
                }
            }
        }

        if (selected)
        {
            player.GetComponent<MoveCharacter>().canMove = false;
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePosition;

            if(Input.GetMouseButtonDown(1)) {
                selected = false;
                transform.position = pos;
                player.GetComponent<MoveCharacter>().canMove = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<UsableObject>())
        {
            cursorHandler.GetComponent<CursorIcon>().SetUse();
            displayText.text = ("Use " + data.name + " on " + collision.name);
            usableObj = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(displayText.text.Contains(collision.name))
        {
            displayText.text = "";
            usableObj = null;
        }

    }
}

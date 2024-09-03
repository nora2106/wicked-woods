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

    public void Start()
    {
        GetComponent<SpriteRenderer>().sprite = data.sprite;
        pos = transform.position;
        player = GameObject.Find("Character");
        displayText = GameObject.Find("ActionText").GetComponent<Text>();
    }

    public void Update()
    {
        if(usableObj)
        {
            if(Input.GetMouseButtonDown(0))
            {
                if(usableObj.GetComponent<UsableObject>().usableItemID == data.id)
                {
                    usableObj.GetComponent<UsableObject>().Action();
                    if(data.reusable)
                    {
                        transform.position = pos;
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                    selected = false;
                    player.GetComponent<MoveCharacter>().canMove = true;
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
        if (collision.gameObject.tag == "Usable")
        {
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

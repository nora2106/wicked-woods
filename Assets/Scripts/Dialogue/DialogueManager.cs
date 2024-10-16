using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour
{
    private Typewriter typewriter;
    private Button skipBtn;
    private bool isPlaying;
    private Story currDialogue;
    private GameObject canvas;
    private GameManager gm;
    private List<GameObject> choices = new List<GameObject>();
    private TextMeshProUGUI[] choicesText;
    private GameObject choiceGrid;

    void Start()
    {
        gm = gameObject.GetComponent<GameManager>();
        canvas = gm.dialoguePanel;
        typewriter = canvas.GetComponentInChildren<Typewriter>();
        skipBtn = canvas.GetComponentInChildren<Button>();
        skipBtn.onClick.AddListener(ContinueDialogue);
        isPlaying = false;

        choiceGrid = canvas.GetComponentInChildren<GridLayoutGroup>().gameObject;
        foreach(Transform t in choiceGrid.transform)
        {
            if(t.GetComponent<Button>())
            {
                choices.Add(t.gameObject);
            }
        }

        choicesText = new TextMeshProUGUI[choices.Count];
        for(int index = 0; index < choices.Count; index++)
        {
            int t = index;
            choices[index].GetComponent<Button>().onClick.AddListener(delegate { MakeChoice(t); });
            choicesText[index] = choices[index].GetComponentInChildren<TextMeshProUGUI>();
            choices[index].SetActive(false);
        }
    }

    private void Update()
    {
        if(!isPlaying)
        {
            return;
        }
    }

    public void StartDialogue(TextAsset inkJSON)
    {
        currDialogue = new Story(inkJSON.text);
        isPlaying = true;
        ContinueDialogue();
    }

    private void ExitDialogue()
    {
        isPlaying = false;
        typewriter.Reset();
        ResetChoices();
        gm.CloseDialogue();
    }

    public void ContinueDialogue()
    {
        if (typewriter.active)
        {
            typewriter.Skip();
        }
        else if (currDialogue.canContinue)
        {
            typewriter.Write(currDialogue.Continue());
            StartCoroutine("WaitForChoices");
        }

        else
        {
            ExitDialogue();
        }
    }

    public IEnumerator WaitForChoices()
    {
        ResetChoices();
        yield return new WaitUntil(() => typewriter.active == false);
        DisplayChoices();
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currDialogue.currentChoices;
        if(currentChoices.Count <= choices.Count)
        {
            int index = 0;
            foreach (Choice choice in currentChoices)
            {
                choices[index].gameObject.SetActive(true);
                choicesText[index].text = choice.text;
                index++;
            }
        }
    }

    private void ResetChoices()
    {
        foreach (GameObject choice in choices)
        {
            choice.SetActive(false);
        }
    }

    public void MakeChoice(int index)
    {
        currDialogue.ChooseChoiceIndex(index);
        ContinueDialogue();
    }
}

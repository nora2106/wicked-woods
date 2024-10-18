using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;
using System.Reflection;

public class DialogueManager : MonoBehaviour
{
    private Typewriter typewriter;
    private Button skipBtn;
    private Story currentStory;
    private GameObject canvas;
    private GameManager gm;
    private List<GameObject> choices = new List<GameObject>();
    private TextMeshProUGUI[] choicesText;
    private GameObject choiceGrid;
    Dictionary<string, object> storyVariables = new Dictionary<string, object>();
    public List<string> storyProgress = new List<string>();
    public string saveName;

    void Start()
    {
        gm = gameObject.GetComponent<GameManager>();
        canvas = gm.dialoguePanel;
        typewriter = canvas.GetComponentInChildren<Typewriter>();
        skipBtn = canvas.GetComponentInChildren<Button>();
        skipBtn.onClick.AddListener(ContinueDialogue);

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

    public void StartDialogue(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);

        //get progress save data for current story
        saveName = "dialogue_" + currentStory.variablesState["story_name"];
        GameData data = gm.save.data;
        FieldInfo fieldInfo = data.GetType().GetField(saveName);
        if (fieldInfo != null)
        {
            object fieldValue = fieldInfo.GetValue(data);

            List<string> valueList = fieldValue as List<string>;
            storyProgress = valueList;

        }
        else
        {
            storyProgress = new List<string>();
        }

        //apply progress save data to ink variables
        SetStoryProgress();
        ContinueDialogue();
    }

    private void ExitDialogue()
    {
        GetAllVars();
        
        //send progress data to savemanager
        GameData data = gm.save.data;
        FieldInfo fieldInfo = data.GetType().GetField(saveName);
        if (fieldInfo != null)
        {
            object fieldValue = fieldInfo.GetValue(data);
            List<string> valueList = fieldValue as List<string>;
            foreach (var variable in storyProgress)
            {
                if (!valueList.Contains(variable)) {
                    fieldInfo.SetValue(variable, fieldInfo);
                }
            }
        }
        
        typewriter.Reset();
        ResetChoices();
        gm.CloseDialogue();
        saveName = "";
    }

    public void GetAllVars()
    {
        var variables = currentStory.variablesState;

        foreach (var variable in variables)
        {
            //check if ink variable is bool and true (for story progress) 
            if (currentStory.variablesState[variable] is bool && (bool)currentStory.variablesState[variable] == true)
            {
                storyProgress.Add(variable);
            }
            // or another type(for future use)
            else if(!storyVariables.ContainsKey(variable))
            {
               // storyVariables.Add(variable, currentStory.variablesState[variable]);
            }
        }
    }

    public void SetStoryProgress()
    {
        if (storyProgress.Count > 0)
        {
            foreach (var variable in storyProgress)
            {
                currentStory.variablesState[variable] = true;
            }
        }
    }

    public void ContinueDialogue()
    {
        if (typewriter.active)
        {
            typewriter.Skip();
        }

        else if (currentStory.canContinue)
        {
            typewriter.Write(currentStory.Continue());
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
        List<Choice> currentChoices = currentStory.currentChoices;
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
        currentStory.ChooseChoiceIndex(index);
        ContinueDialogue();
    }

    //use when needed in future
    public void SetStoryVars()
    {
        if (storyVariables.Count > 0)
        {
            foreach (var variable in storyVariables)
            {
                currentStory.variablesState[variable.Key] = variable.Value;
            }
        }
    }
}

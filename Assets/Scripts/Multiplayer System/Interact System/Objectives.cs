using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine.SceneManagement;


public class Objectives : MonoBehaviour
{

    private GameObject objectiveUI;
    private GameObject[] objectives;

    private void Start()
    {
        objectiveUI = transform.GetChild(0).GetChild(0).gameObject;
        GetObjectives();
        if (SceneManager.GetActiveScene().name == "LEVELONE")
        {
            AddObjective("Find the key to the exit.");
            AddObjective("Turn on the radio station.");
            AddObjective("Listen to the radio broadcast.");
        }
        else if (SceneManager.GetActiveScene().name == "LEVELTWO")
        {
            AddObjective("Find out how to open the exit tunnel.");
        }
        else if (SceneManager.GetActiveScene().name == "LEVELTHREE")
        {
            AddObjective("Find a key to the building.");
            AddObjective("Get something to break the fence with.");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void GetObjectives()
    {
        objectives = GameObject.FindGameObjectsWithTag("Objective");
        for (int i = 0; i < (objectives.Length); i++)
        {
            if (i == 0)
            {
                objectiveUI.transform.GetChild(1).GetComponent<TMP_Text>().text = $"- Get {objectives[i].name.ToUpper()}";
            }
            else
            {
                GameObject lastObj = objectiveUI.transform.GetChild(objectiveUI.transform.childCount - 1).gameObject;
                float yPos = lastObj.transform.position.y - lastObj.GetComponent<RectTransform>().sizeDelta.y;
                GameObject currentObj = Instantiate(lastObj, new Vector3(lastObj.transform.position.x, yPos, lastObj.transform.position.z), lastObj.transform.rotation, objectiveUI.transform);
                currentObj.GetComponent<TMP_Text>().text = $"- Get {objectives[i].name.ToUpper()}";
            }
        }
    }

    public void UpdateObjective(string name)
    {
        foreach (Transform item in objectiveUI.transform)
        {
            GameObject obj = item.gameObject;
            string text = obj.GetComponent<TMP_Text>().text;
            Regex re = new Regex(@$"{name}", RegexOptions.IgnoreCase);
            Regex completed = new Regex(@"<s>.+</s>"); // Checks that the objective is not already completed.
            if (re.IsMatch(text) && !completed.IsMatch(text))
            {
                obj.GetComponent<TMP_Text>().text = $"<s>{text}</s>"; // Puts strike-through on the objective text.
                break; // Breaks the foreach loop to prevent doubles of an objective being crossed out.
            }
        }
        CheckCompletion();
    }

    void CheckCompletion()
    {
        bool complete = true;
        foreach (Transform item in objectiveUI.transform)
        {
            GameObject obj = item.gameObject;
            string text = obj.GetComponent<TMP_Text>().text;
            Regex completed = new Regex(@"<s>.+</s>"); // Checks if the objective is completed.
            if (!completed.IsMatch(text) && obj.name != "TaskListTitle") { complete = false; break; }
        }
        if (complete) { AddObjective("Find the exit."); }
    }

    void AddObjective(string text)
    {
        GameObject lastObj = objectiveUI.transform.GetChild(objectiveUI.transform.childCount - 1).gameObject;
        float yPos = lastObj.transform.position.y - lastObj.GetComponent<RectTransform>().sizeDelta.y;
        GameObject currentObj = Instantiate(lastObj, new Vector3(lastObj.transform.position.x, yPos, lastObj.transform.position.z), lastObj.transform.rotation, objectiveUI.transform);
        currentObj.GetComponent<TMP_Text>().text = $"- {text}";
    }
}

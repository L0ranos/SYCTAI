using System.Collections;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActionHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text upperText;
    public TMP_Text lowerText;
    public List<Button> buttons;
    private Dictionary<string, GameActor> targets = new Dictionary<string, GameActor>();
    public Sprite Image;
    public List<Sprite> imagesList;
    private string spriteName; //przechowuje zmienn¹ z nazw¹ pliku grafiki celu misji (a przynajmniej powinno)
    private Dictionary<string, string> unshownInformation = new Dictionary<string, string>();

    public static event Action<string> OnMissionEvaluationChanged;

    void Awake()
    {
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    }

    void onDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }

    private void GameManagerOnGameStateChanged(string state)
    {
        if (state == "briefing")
        {
            unshownInformation = new Dictionary<string, string>();
            targets = new Dictionary<string, GameActor>();
            imagesList = new List<Sprite>();
            SetUpGameplay();
            prepTextGeneration();
            upperText.text = GenerateBriefingText("upper");
            lowerText.text = GenerateBriefingText("lower");
        }
    }

    void Start()
    {
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => EvaluateTargetStatus(button.name));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EvaluateTargetStatus(string clickedTarget)
    {
        Debug.Log("JESTEM TU");
        Debug.Log(targets[clickedTarget]);
        if (targets[clickedTarget].isTarget) 
        {
            OnMissionEvaluationChanged?.Invoke("victory");
        }
        else
        {
            OnMissionEvaluationChanged?.Invoke("failure");
        }
    }

    void SetUpGameplay()
    {
        
        List<string> images = new List<string>() {"agent_black_female_green",
                                                "agent_black_female_red",
                                                "agent_black_male_blue",
                                                "agent_black_male_white",
                                                "agent_blond_female_green",
                                                "agent_blond_female_red",
                                                "agent_blond_male_blue",
                                                "agent_blond_male_white",
                                                "agent_pink_female_green",
                                                "agent_pink_female_red",
                                                "agent_red_male_blue",
                                                "agent_red_male_white" };
        int n = images.Count;
        int licznik = 0;
        List<int> wylosowane = new List<int>();
        System.Random rand = new System.Random();

        for (int i = 0; i < images.Count; i++)
        {
            imagesList.Add(Resources.Load<Sprite>(images[i]));
        }

        do
        {
            int r = rand.Next(n);

            if (wylosowane.Contains(r) == false)
            {
                wylosowane.Add(r);
                buttons[licznik].GetComponent<Image>().sprite = imagesList[r];
                licznik++;
            }
        }
        while (licznik < 5);

        foreach (Button currentObject in buttons)
        {
            targets.Add(currentObject.name, new GameActor());
        }
        //Change random actor to a target
        
        List<string> keyList = new List<string>(targets.Keys);
        string randomkey = keyList[rand.Next(keyList.Count)];
        Debug.Log(randomkey);

        targets[randomkey].SetAsTarget();

        int buttonNumber = 0;
        for (int i = 0; i < buttons.Count; i++) //z racji tego, ¿e nie umiem siê odwo³aæ do klucza ze s³ownika, to tu szukam który przycisk jest targetem
        {
            if (buttons[i].name == randomkey)
            {
                buttonNumber = i;
            }
        }

        for (int i = 0; i < imagesList.Count; i++) //a tu szukam, który sprite jest przy buttonie, który jest targetem
        {
            if(buttons[buttonNumber].GetComponent<Image>().sprite == imagesList[i])
            {
                spriteName = images[i]; //tu zapisujê nazwê obrazka
            }
        }
    }

    public void MoreInfoHandler()
    {
        lowerText.text = GenerateBriefingText("moreinfo");
    }

    void prepTextGeneration()
    {
        unshownInformation = new Dictionary<string, string>();
        unshownInformation.Add("hair color", spriteName.Split("_")[1]);
        unshownInformation.Add("gender", spriteName.Split("_")[2]);
        unshownInformation.Add("clothing color", spriteName.Split("_")[3]);
    }

    string GenerateBriefingText(string location) //nazwy plików sk³adaj¹ siê ze schematu agent_kolorWlosow_plec_kolorUbrania, zmienna spriteName przechowuje nazwe pliku grafiki targetu
    {

        if (unshownInformation.Keys.Count == 0)
        {
            return "And as always should you be caught or compromised the organisation will disavow any knowledge of your actions. Good luck agent.";
        }
        string beginningText = "Good evening agent. Your mission, should you choose to accept it, is to ";

  
        List<string> middleSentencesUpper = new List<string>(){
            "infiltrate the enemy's secure facility and obtain critical intelligence on their upcoming attack plans. This mission will require you to use all of your skills in covert operations and deception. ",
            "to retrieve sensitive documents that have been stolen from a high-security facility. This mission will require you to use all of your skills in covert operations and deception.",
            "track down and capture a notorious international criminal who has evaded authorities for years. You will need to gather intelligence on the target's whereabouts and plan a surgical strike to take them into custody.",
            "sabotage the production of a new weapon being developed by a rival organization. You will need to use all of your skills in deception and infiltration to accomplish this mission without being detected. ",
            "infiltrate a high-security facility and disable a weapon of mass destruction before it can be used against innocent civilians. You will need to use all of your skills in stealth and deception to accomplish this mission without being detected.",
        };

        string beginningTextLower = "Your first target is a very dangerous individual.";

        List<string> middleSentencesFlavour = new List<string>(){
            "Your first target is a wealthy oil baron who has caused great trouble for the organisation.",
            "The target can often be found at his mansion in the city. Your mission is to deal with this individual swiftly and efficiently. ",
            "Your first target is a rogue agent, ex British intelligence. They posess information releveant to the mission.",
            "Your mission is to deal with this individual swiftly and efficiently.",
            "The target is a seasoned operative with a reputation for being cool under pressure and able to accomplish the most impossible of missions.",
            "With a network of loyal operatives and a seemingly endless supply of resources at their disposal, the target has proven to be a formidable foe. ",
            "The target is a masterr of deception and manipulation, they have pulled off some of the most audacious heists and acts of sabotage in history.",
        };

        System.Random rand = new System.Random();
        int maxWords = rand.Next(0, 100);

        List<string> keyList = new List<string>(unshownInformation.Keys);
        string randomKey = keyList[rand.Next(keyList.Count)];

        switch (location) {
        case "upper":
                return beginningText + middleSentencesUpper[rand.Next(middleSentencesUpper.Count)];
        case "lower":
                string characteristicStr = "The target is characterised by their " + " " + randomKey + " which is " + unshownInformation[randomKey] + ".";
                unshownInformation.Remove(randomKey);
                return beginningTextLower + characteristicStr + middleSentencesFlavour[rand.Next(middleSentencesFlavour.Count)];
            case "moreinfo":         
                string moreinfoStr = "The target is also characterised by their " + " " + randomKey + " which is " + unshownInformation[randomKey] + ".";
                unshownInformation.Remove(randomKey);
                return beginningTextLower + moreinfoStr + middleSentencesFlavour[rand.Next(middleSentencesFlavour.Count)];                    
        default:
             return "Error";
        }
    }
}

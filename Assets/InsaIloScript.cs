using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;

public class InsaIloScript : MonoBehaviour {

    public KMBombInfo Bomb;
    public KMAudio Audio;

    public KMSelectable[] Buttons;
    public GameObject[] Screens;
    public Material[] Mats;
    public TextMesh[] Texts;

    private int chosenIx, anIx, answer;
    private string[] words = { "who", "where", "when", "three", "four", "six", "seven", "ten", "child", "worm", "forest", "root", "flower", "bone", "egg", "horn", "tail", "feather", "tooth", "tongue", "knee", "wing", "belly", "neck", "rain", "river", "sea", "ice", "smoke", "ash", "night", "day", "year", "old", "smooth", "dry", "near", "module", "bomb", "pink", "maroon", "orange", "green", "purple", "alcohol", "blind", "deaf", "love", "gray", "rich", "poor", "glitch", "saliva", "mother", "father", "soda", "second", "year", "eternity", "baby", "basement", "twin", "dinosaur", "banana", "dumb", "button", "welcome", "sorry", "please", "friend", "enemy", "soldier", "vehicle", "city", "hunter", "speaker", "leader", "sentence", "calculator", "movie", "core", "others", "nothing", "snow", "first", "fifth" };
    private const float row = 0.125f;
    private const float col = 0.0625f;
    private float Lrow, Lcol, Rrow, Rcol;
    private string chosenWord, forLogging;
    private List<string> onButtons = new List<string> {};
    private bool chegg = false;

    //Logging
    static int moduleIdCounter = 1;
    int moduleId;
    private bool moduleSolved;

    void Awake () {
        moduleId = moduleIdCounter++;
    
        foreach (KMSelectable button in Buttons) {
            button.OnInteract += delegate () { buttonPress(button); return false; };
        }
    }

    // Use this for initialization
    void Start () {
        chosenIx = UnityEngine.Random.Range(0, 87);
        chosenWord = words[chosenIx];
        switch (chosenIx) {
            case 0: Lrow=row*6; Lcol=col*1; Rrow=row*2; Rcol=col*8; forLogging = "jan seme"; break;
            case 1: Lrow=row*5; Lcol=col*15; Rrow=row*2; Rcol=col*8; forLogging = "ma seme"; break;
            case 2: Lrow=row*1; Lcol=col*10; Rrow=row*2; Rcol=col*8; forLogging = "tenpo seme"; break;
            case 3: Lrow=row*1; Lcol=col*13; Rrow=row*0; Rcol=col*2; forLogging = "tu wan"; break;
            case 4: Lrow=row*1; Lcol=col*13; Rrow=row*1; Rcol=col*13; forLogging = "tu tu"; break;
            case 5: Lrow=row*5; Lcol=col*12; Rrow=row*0; Rcol=col*2; forLogging = "luka wan"; break;
            case 6: Lrow=row*5; Lcol=col*12; Rrow=row*1; Rcol=col*13; forLogging = "luka tu"; break;
            case 7: Lrow=row*5; Lcol=col*12; Rrow=row*5; Rcol=col*12; forLogging = "luka luka"; break;
            case 8: Lrow=row*6; Lcol=col*1; Rrow=row*5; Rcol=col*7; forLogging = "jan lili"; break;
            case 9: Lrow=row*2; Lcol=col*0; Rrow=row*5; Rcol=col*8; forLogging = "pipi linja"; break;
            case 10: Lrow=row*5; Lcol=col*15; Rrow=row*6; Rcol=col*7; forLogging = "ma kasi"; break;
            case 11: Lrow=row*3; Lcol=col*2; Rrow=row*6; Rcol=col*7; forLogging = "noka kasi"; break;
            case 12: Lrow=row*6; Lcol=col*7; Rrow=row*6; Rcol=col*13; forLogging = "kasi kule"; break;
            case 13: Lrow=row*6; Lcol=col*10; Rrow=row*2; Rcol=col*10; forLogging = "kiwen sijelo"; break;
            case 14: Lrow=row*6; Lcol=col*10; Rrow=row*0; Rcol=col*3; forLogging = "kiwen waso"; break;
            case 15: Lrow=row*6; Lcol=col*10; Rrow=row*5; Rcol=col*3; forLogging = "kiwen lawa"; break;
            case 16: Lrow=row*5; Lcol=col*8; Rrow=row*4; Rcol=col*7; forLogging = "linja monsi"; break;
            case 17: Lrow=row*5; Lcol=col*9; Rrow=row*0; Rcol=col*3; forLogging = "lipu waso"; break;
            case 18: Lrow=row*6; Lcol=col*10; Rrow=row*1; Rcol=col*15; forLogging = "kiwen uta"; break;
            case 19: Lrow=row*4; Lcol=col*15; Rrow=row*1; Rcol=col*15; forLogging = "nena uta"; break;
            case 20: Lrow=row*4; Lcol=col*15; Rrow=row*3; Rcol=col*2; forLogging = "nena noka"; break;
            case 21: Lrow=row*5; Lcol=col*12; Rrow=row*0; Rcol=col*3; forLogging = "luka waso"; break;
            case 22: Lrow=row*2; Lcol=col*14; Rrow=row*2; Rcol=col*10; forLogging = "sinpin sijelo"; break;
            case 23: Lrow=row*3; Lcol=col*2; Rrow=row*5; Rcol=col*3; forLogging = "noka lawa"; break;
            case 24: Lrow=row*1; Lcol=col*7; Rrow=row*2; Rcol=col*9; forLogging = "telo sewi"; break;
            case 25: Lrow=row*1; Lcol=col*7; Rrow=row*1; Rcol=col*8; forLogging = "telo tawa"; break;
            case 26: Lrow=row*1; Lcol=col*7; Rrow=row*1; Rcol=col*2; forLogging = "telo suli"; break;
            case 27: Lrow=row*6; Lcol=col*10; Rrow=row*1; Rcol=col*7; forLogging = "kiwen telo"; break;
            case 28: Lrow=row*6; Lcol=col*12; Rrow=row*2; Rcol=col*6; forLogging = "kon seli"; break;
            case 29: Lrow=row*6; Lcol=col*11; Rrow=row*2; Rcol=col*6; forLogging = "ko seli"; break;
            case 30: Lrow=row*1; Lcol=col*10; Rrow=row*3; Rcol=col*14; forLogging = "tenpo pimeja"; break;
            case 31: Lrow=row*1; Lcol=col*10; Rrow=row*1; Rcol=col*3; forLogging = "tenpo suno"; break;
            case 32: Lrow=row*2; Lcol=col*11; Rrow=row*1; Rcol=col*3; forLogging = "sike suno"; break;
            case 33: Lrow=row*2; Lcol=col*12; Rrow=row*7; Rcol=col*2; forLogging = "sin ala"; break;
            case 34: Lrow=row*4; Lcol=col*15; Rrow=row*7; Rcol=col*2; forLogging = "nena ala"; break;
            case 35: Lrow=row*1; Lcol=col*7; Rrow=row*7; Rcol=col*2; forLogging = "telo ala"; break;
            case 36: Lrow=row*5; Lcol=col*11; Rrow=row*2; Rcol=col*1; forLogging = "lon poka"; break;
            case 37: Lrow=row*7; Lcol=col*15; Rrow=row*7; Rcol=col*14; forLogging = "insa ilo"; break;
            case 38: Lrow=row*4; Lcol=col*6; Rrow=row*7; Rcol=col*14; forLogging = "moli ilo"; break;
            case 39: Lrow=row*5; Lcol=col*10; Rrow=row*0; Rcol=col*1; forLogging = "loje walo"; break;
            case 40: Lrow=row*5; Lcol=col*10; Rrow=row*3; Rcol=col*14; forLogging = "loje pimeja"; break;
            case 41: Lrow=row*5; Lcol=col*10; Rrow=row*6; Rcol=col*2; forLogging = "loje jelo"; break;
            case 42: Lrow=row*6; Lcol=col*2; Rrow=row*5; Rcol=col*2; forLogging = "jelo laso"; break;
            case 43: Lrow=row*5; Lcol=col*10; Rrow=row*5; Rcol=col*2; forLogging = "loje laso"; break;
            case 44: Lrow=row*1; Lcol=col*7; Rrow=row*4; Rcol=col*13; forLogging = "telo nasa"; break;
            case 45: Lrow=row*5; Lcol=col*13; Rrow=row*7; Rcol=col*2; forLogging = "oko ala"; break;
            case 46: Lrow=row*6; Lcol=col*15; Rrow=row*7; Rcol=col*2; forLogging = "kute ala"; break;
            case 47: Lrow=row*3; Lcol=col*13; Rrow=row*6; Rcol=col*1; forLogging = "pilin jan"; break;
            case 48: Lrow=row*0; Lcol=col*1; Rrow=row*3; Rcol=col*14; forLogging = "walo pimeja"; break;
            case 49: Lrow=row*4; Lcol=col*1; Rrow=row*7; Rcol=col*4; forLogging = "mani ale"; break;
            case 50: Lrow=row*4; Lcol=col*1; Rrow=row*7; Rcol=col*2; forLogging = "mani ala"; break;
            case 51: Lrow=row*2; Lcol=col*0; Rrow=row*7; Rcol=col*14; forLogging = "pipi ilo"; break;
            case 52: Lrow=row*1; Lcol=col*7; Rrow=row*1; Rcol=col*15; forLogging = "telo uta"; break;
            case 53: Lrow=row*4; Lcol=col*2; Rrow=row*4; Rcol=col*0; forLogging = "meli mama"; break;
            case 54: Lrow=row*4; Lcol=col*4; Rrow=row*4; Rcol=col*0; forLogging = "mije mama"; break;
            case 55: Lrow=row*1; Lcol=col*7; Rrow=row*3; Rcol=col*14; forLogging = "telo pimeja"; break;
            case 56: Lrow=row*1; Lcol=col*10; Rrow=row*5; Rcol=col*7; forLogging = "tenpo lili"; break;
            case 57: Lrow=row*1; Lcol=col*10; Rrow=row*1; Rcol=col*2; forLogging = "tenpo suli"; break;
            case 58: Lrow=row*1; Lcol=col*10; Rrow=row*7; Rcol=col*4; forLogging = "tenpo ale"; break;
            case 59: Lrow=row*6; Lcol=col*1; Rrow=row*5; Rcol=col*7; forLogging = "jan lili"; break;
            case 60: Lrow=row*1; Lcol=col*12; Rrow=row*3; Rcol=col*2; forLogging = "tomo noka"; break;
            case 61: Lrow=row*6; Lcol=col*1; Rrow=row*1; Rcol=col*13; forLogging = "jan tu"; break;
            case 62: Lrow=row*7; Lcol=col*1; Rrow=row*1; Rcol=col*2; forLogging = "akesi suli"; break;
            case 63: Lrow=row*6; Lcol=col*9; Rrow=row*6; Rcol=col*2; forLogging = "kili jelo"; break;
            case 64: Lrow=row*5; Lcol=col*3; Rrow=row*7; Rcol=col*13; forLogging = "lawa ike"; break;
            case 65: Lrow=row*7; Lcol=col*14; Rrow=row*4; Rcol=col*15; forLogging = "ilo nena"; break;
            case 66: Lrow=row*6; Lcol=col*6; Rrow=row*2; Rcol=col*3; forLogging = "kama pona"; break;
            case 67: Lrow=row*4; Lcol=col*3; Rrow=row*3; Rcol=col*7; forLogging = "mi pakala"; break;
            case 68: Lrow=row*4; Lcol=col*3; Rrow=row*0; Rcol=col*6; forLogging = "mi wile"; break;
            case 69: Lrow=row*6; Lcol=col*1; Rrow=row*2; Rcol=col*3; forLogging = "jan pona"; break;
            case 70: Lrow=row*6; Lcol=col*1; Rrow=row*7; Rcol=col*13; forLogging = "jan ike"; break;
            case 71: Lrow=row*6; Lcol=col*1; Rrow=row*0; Rcol=col*0; forLogging = "jan utala"; break;
            case 72: Lrow=row*1; Lcol=col*12; Rrow=row*1; Rcol=col*8; forLogging = "tomo tawa"; break;
            case 73: Lrow=row*5; Lcol=col*15; Rrow=row*1; Rcol=col*12; forLogging = "ma tomo"; break;
            case 74: Lrow=row*6; Lcol=col*1; Rrow=row*7; Rcol=col*3; forLogging = "jan alasa"; break;
            case 75: Lrow=row*6; Lcol=col*1; Rrow=row*6; Rcol=col*5; forLogging = "jan kalama"; break;
            case 76: Lrow=row*6; Lcol=col*1; Rrow=row*5; Rcol=col*3; forLogging = "jan lawa"; break;
            case 77: Lrow=row*3; Lcol=col*1; Rrow=row*4; Rcol=col*11; forLogging = "nimi mute"; break;
            case 78: Lrow=row*4; Lcol=col*12; Rrow=row*7; Rcol=col*14; forLogging = "nanpa ilo"; break;
            case 79: Lrow=row*2; Lcol=col*15; Rrow=row*1; Rcol=col*8; forLogging = "sitelen tawa"; break;
            case 80: Lrow=row*1; Lcol=col*4; Rrow=row*3; Rcol=col*2; forLogging = "supa noka"; break;
            case 81: Lrow=row*2; Lcol=col*13; Rrow=row*7; Rcol=col*2; forLogging = "sina ala"; break;
            case 82: Lrow=row*7; Lcol=col*4; Rrow=row*7; Rcol=col*2; forLogging = "ale ala"; break;
            case 83: Lrow=row*1; Lcol=col*7; Rrow=row*5; Rcol=col*5; forLogging = "telo lete"; break;
            case 84: Lrow=row*0; Lcol=col*2; Rrow=row*4; Rcol=col*12; forLogging = "wan nanpa"; break;
            case 85: Lrow=row*5; Lcol=col*12; Rrow=row*4; Rcol=col*12; forLogging = "luka nanpa"; break;
            
        }
        Screens[0].GetComponent<MeshRenderer>().material.mainTextureOffset = new Vector2(Lcol, Lrow);
        Screens[1].GetComponent<MeshRenderer>().material.mainTextureOffset = new Vector2(Rcol, Rrow);
        onButtons.Add(chosenWord);
        while (onButtons.Count() <= 5) {
            chegg = false;
            anIx = UnityEngine.Random.Range(0, 87);
            for (int w = 0; w < onButtons.Count(); w++) {
                if (words[anIx] == onButtons[w]) {
                    chegg = true;
                }
            }
            if (!chegg) {
                onButtons.Add(words[anIx]);
            }
        }
        onButtons.Shuffle();
        for (int m = 0; m < 6; m++) {
            Texts[m].text = onButtons[m];
            switch (onButtons[m]) {
                case "dinosaur": case "speaker":
                    Texts[m].transform.localScale = new Vector3(0.065f, 0.075f, 0.1f);
                break;
                case "basement": case "calculator": case "sentence": case "welcome":
                    Texts[m].transform.localScale = new Vector3(0.055f, 0.075f, 0.1f);
                break;
                default: break;
            }
            if (onButtons[m] == chosenWord) {
                answer = m;
            }
        }
        Debug.LogFormat("[Insa Ilo #{0}] Words: {1}, {2}, {3}, {4}, {5}, {6}", moduleId, onButtons[0], onButtons[1], onButtons[2], onButtons[3], onButtons[4], onButtons[5]);
        Debug.LogFormat("[Insa Ilo #{0}] On displays: {1}", moduleId, forLogging);
        Debug.LogFormat("[Insa Ilo #{0}] Correct word: {1}", moduleId, chosenWord);
    }

    void buttonPress(KMSelectable button) {
        button.AddInteractionPunch();
        GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
        for (int b = 0; b < 6; b++) {
            if (!moduleSolved && button == Buttons[b]) {
                if (onButtons[b] == chosenWord) {
                    Debug.LogFormat("[Insa Ilo #{0}] You pressed {1}, that is correct. Module solved.", moduleId, chosenWord);
                    GetComponent<KMBombModule>().HandlePass();
                    Screens[0].GetComponent<MeshRenderer>().material.mainTextureOffset = new Vector2(0.6875f, 0.125f);
                    Screens[1].GetComponent<MeshRenderer>().material.mainTextureOffset = new Vector2(0.1875f, 0.25f);
                    moduleSolved = true;
                    GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.CorrectChime, transform);
                } else {
                    Debug.LogFormat("[Insa Ilo #{0}] You pressed {1}, that is incorrect, you should've pressed {2}. Strike!", moduleId, onButtons[b], chosenWord);
                    GetComponent<KMBombModule>().HandleStrike();
                }

                //The below code was used to check which words would normally be too big on the buttons
                /*x++;
                for (int m = 0; m < 6; m++) {
                    if (m < 3) {
                        Texts[m].text = words[x];
                    } else if (x == 0) {
                            switch (m) {
                            case 3: Texts[m].text = "0.075"; break;
                            case 4: Texts[m].text = "0.065"; break;
                            case 3: Texts[m].text = "0.055"; break;
                            }
                        }
                    }
                }*/
            }
        }
    }

    //twitch plays
    private bool inputIsValid(string cmd)
    {
        string[] validstuff = { "1", "2", "3", "4", "5", "6" };
        if (validstuff.Contains(cmd))
        {
            return true;
        }
        return false;
    }

    #pragma warning disable 414
    private readonly string TwitchHelpMessage = @"!{0} press <#> [Presses that button in reading order]";
    #pragma warning restore 414
    IEnumerator ProcessTwitchCommand(string command)
    {
        string[] parameters = command.Split(' ');
        if (Regex.IsMatch(parameters[0], @"^\s*press\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant) || Regex.IsMatch(parameters[0], @"^\s*button\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant) || Regex.IsMatch(parameters[0], @"^\s*pos\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            if (parameters.Length == 2)
            {
                if (inputIsValid(parameters[1]))
                {
                    yield return null;
                    if (parameters[1].Equals("1"))
                    {
                        Buttons[0].OnInteract();
                    }
                    else if (parameters[1].Equals("2"))
                    {
                        Buttons[1].OnInteract();
                    }
                    else if (parameters[1].Equals("3"))
                    {
                        Buttons[2].OnInteract();
                    }
                    else if (parameters[1].Equals("4"))
                    {
                        Buttons[3].OnInteract();
                    }
                    else if (parameters[1].Equals("5"))
                    {
                        Buttons[4].OnInteract();
                    }
                    else if (parameters[1].Equals("6"))
                    {
                        Buttons[5].OnInteract();
                    }
                }
            }
            yield break;
        }
    }

    IEnumerator TwitchHandleForcedSolve()
    {
        Buttons[answer].OnInteract();
        yield return new WaitForSeconds(0.1f);
    }
    
}

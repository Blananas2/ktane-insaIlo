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
    private string[] words = { "who", "where", "when", "three", "four", "six", "seven", "ten", "worm", "forest", "root", "flower", "bone", "egg", "horn", "tail", "feather", "tooth", "tongue", "knee", "wing", "belly", "neck", "rain", "river", "sea", "ice", "smoke", "ash", "night", "day", "year", "old", "smooth", "dry", "near", "module", "bomb", "pink", "maroon", "orange", "green", "purple", "alcohol", "blind", "deaf", "love", "gray", "rich", "poor", "glitch", "saliva", "mother", "father", "soda", "second", "year", "eternity", "baby", "basement", "twin", "dinosaur", "banana", "dumb", "button", "welcome", "sorry", "please", "friend", "enemy", "soldier", "vehicle", "city", "hunter", "speaker", "leader", "sentence", "calculator", "movie", "others", "nothing", "first", "fifth" };
    private const float row = 0.125f;
    private const float col = 0.0625f;
    private float Lrow, Lcol, Rrow, Rcol;
    private string chosenWord, forLogging;
    private List<string> onButtons = new List<string> {};
    private bool chegg = false;
    private int x = -1;

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
        chosenIx = UnityEngine.Random.Range(0, words.Count());
        chosenWord = words[chosenIx];
        
        SetSymbols(chosenIx);
        
        Screens[0].GetComponent<MeshRenderer>().material.mainTextureOffset = new Vector2(Lcol, Lrow);
        Screens[1].GetComponent<MeshRenderer>().material.mainTextureOffset = new Vector2(Rcol, Rrow);
        onButtons.Add(chosenWord);
        while (onButtons.Count() <= 5) {
            chegg = false;
            anIx = UnityEngine.Random.Range(0, words.Count());
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
            }
        }
    }

    void SetSymbols (int n) {
        switch (n) {
            case 0: Lrow=row*6; Lcol=col*1; Rrow=row*2; Rcol=col*8; forLogging = "jan seme"; break; //who
            case 1: Lrow=row*5; Lcol=col*15; Rrow=row*2; Rcol=col*8; forLogging = "ma seme"; break; //where
            case 2: Lrow=row*1; Lcol=col*10; Rrow=row*2; Rcol=col*8; forLogging = "tenpo seme"; break; //when
            case 3: Lrow=row*1; Lcol=col*13; Rrow=row*0; Rcol=col*2; forLogging = "tu wan"; break; //three
            case 4: Lrow=row*1; Lcol=col*13; Rrow=row*1; Rcol=col*13; forLogging = "tu tu"; break; //four
            case 5: Lrow=row*5; Lcol=col*12; Rrow=row*0; Rcol=col*2; forLogging = "luka wan"; break; //six
            case 6: Lrow=row*5; Lcol=col*12; Rrow=row*1; Rcol=col*13; forLogging = "luka tu"; break; //seven
            case 7: Lrow=row*5; Lcol=col*12; Rrow=row*5; Rcol=col*12; forLogging = "luka luka"; break; //ten
            case 8: Lrow=row*2; Lcol=col*0; Rrow=row*5; Rcol=col*8; forLogging = "pipi linja"; break; //worm
            case 9: Lrow=row*5; Lcol=col*15; Rrow=row*6; Rcol=col*7; forLogging = "ma kasi"; break; //forest
            case 10: Lrow=row*3; Lcol=col*2; Rrow=row*6; Rcol=col*7; forLogging = "noka kasi"; break; //root
            case 11: Lrow=row*6; Lcol=col*7; Rrow=row*6; Rcol=col*13; forLogging = "kasi kule"; break; //flower
            case 12: Lrow=row*6; Lcol=col*10; Rrow=row*2; Rcol=col*10; forLogging = "kiwen sijelo"; break; //bone
            case 13: Lrow=row*6; Lcol=col*10; Rrow=row*0; Rcol=col*3; forLogging = "kiwen waso"; break; //egg
            case 14: Lrow=row*6; Lcol=col*10; Rrow=row*5; Rcol=col*3; forLogging = "kiwen lawa"; break; //horn
            case 15: Lrow=row*5; Lcol=col*8; Rrow=row*4; Rcol=col*7; forLogging = "linja monsi"; break; //tail
            case 16: Lrow=row*5; Lcol=col*9; Rrow=row*0; Rcol=col*3; forLogging = "lipu waso"; break; //feather
            case 17: Lrow=row*6; Lcol=col*10; Rrow=row*1; Rcol=col*15; forLogging = "kiwen uta"; break; //tooth
            case 18: Lrow=row*4; Lcol=col*15; Rrow=row*1; Rcol=col*15; forLogging = "nena uta"; break; //tongue
            case 19: Lrow=row*4; Lcol=col*15; Rrow=row*3; Rcol=col*2; forLogging = "nena noka"; break; //knee
            case 20: Lrow=row*5; Lcol=col*12; Rrow=row*0; Rcol=col*3; forLogging = "luka waso"; break; //wing
            case 21: Lrow=row*2; Lcol=col*14; Rrow=row*2; Rcol=col*10; forLogging = "sinpin sijelo"; break; //belly
            case 22: Lrow=row*3; Lcol=col*2; Rrow=row*5; Rcol=col*3; forLogging = "noka lawa"; break; //neck
            case 23: Lrow=row*1; Lcol=col*9; Rrow=row*2; Rcol=col*9; forLogging = "telo sewi"; break; //rain
            case 24: Lrow=row*1; Lcol=col*9; Rrow=row*1; Rcol=col*8; forLogging = "telo tawa"; break; //river
            case 25: Lrow=row*1; Lcol=col*9; Rrow=row*1; Rcol=col*2; forLogging = "telo suli"; break; //sea
            case 26: Lrow=row*6; Lcol=col*10; Rrow=row*1; Rcol=col*9; forLogging = "kiwen telo"; break; //ice
            case 27: Lrow=row*6; Lcol=col*12; Rrow=row*2; Rcol=col*6; forLogging = "kon seli"; break; //smoke
            case 28: Lrow=row*6; Lcol=col*11; Rrow=row*2; Rcol=col*6; forLogging = "ko seli"; break; //ash
            case 29: Lrow=row*1; Lcol=col*10; Rrow=row*3; Rcol=col*14; forLogging = "tenpo pimeja"; break; //night
            case 30: Lrow=row*1; Lcol=col*10; Rrow=row*1; Rcol=col*3; forLogging = "tenpo suno"; break; //day
            case 31: Lrow=row*2; Lcol=col*11; Rrow=row*1; Rcol=col*3; forLogging = "sike suno"; break; //year
            case 32: Lrow=row*2; Lcol=col*12; Rrow=row*7; Rcol=col*2; forLogging = "sin ala"; break; //old
            case 33: Lrow=row*4; Lcol=col*15; Rrow=row*7; Rcol=col*2; forLogging = "nena ala"; break; //smooth
            case 34: Lrow=row*1; Lcol=col*9; Rrow=row*7; Rcol=col*2; forLogging = "telo ala"; break; //dry
            case 35: Lrow=row*5; Lcol=col*11; Rrow=row*2; Rcol=col*1; forLogging = "lon poka"; break; //near
            case 36: Lrow=row*7; Lcol=col*15; Rrow=row*7; Rcol=col*14; forLogging = "insa ilo"; break; //module
            case 37: Lrow=row*4; Lcol=col*6; Rrow=row*7; Rcol=col*14; forLogging = "moli ilo"; break; //bomb
            case 38: Lrow=row*5; Lcol=col*10; Rrow=row*0; Rcol=col*1; forLogging = "loje walo"; break; //pink
            case 39: Lrow=row*5; Lcol=col*10; Rrow=row*3; Rcol=col*14; forLogging = "loje pimeja"; break; //maroon
            case 40: Lrow=row*5; Lcol=col*10; Rrow=row*6; Rcol=col*2; forLogging = "loje jelo"; break; //orange
            case 41: Lrow=row*6; Lcol=col*2; Rrow=row*5; Rcol=col*2; forLogging = "jelo laso"; break; //green
            case 42: Lrow=row*5; Lcol=col*10; Rrow=row*5; Rcol=col*2; forLogging = "loje laso"; break; //purple
            case 43: Lrow=row*1; Lcol=col*9; Rrow=row*4; Rcol=col*13; forLogging = "telo nasa"; break; //alcohol
            case 44: Lrow=row*5; Lcol=col*13; Rrow=row*7; Rcol=col*2; forLogging = "lukin ala"; break; //blind
            case 45: Lrow=row*6; Lcol=col*15; Rrow=row*7; Rcol=col*2; forLogging = "kute ala"; break; //deaf
            case 46: Lrow=row*3; Lcol=col*13; Rrow=row*6; Rcol=col*1; forLogging = "pilin jan"; break; //love
            case 47: Lrow=row*0; Lcol=col*1; Rrow=row*3; Rcol=col*14; forLogging = "walo pimeja"; break; //gray
            case 48: Lrow=row*4; Lcol=col*1; Rrow=row*7; Rcol=col*4; forLogging = "mani ale"; break; //rich
            case 49: Lrow=row*4; Lcol=col*1; Rrow=row*7; Rcol=col*2; forLogging = "mani ala"; break; //poor
            case 50: Lrow=row*2; Lcol=col*0; Rrow=row*7; Rcol=col*14; forLogging = "pipi ilo"; break; //glitch
            case 51: Lrow=row*1; Lcol=col*9; Rrow=row*1; Rcol=col*15; forLogging = "telo uta"; break; //saliva
            case 52: Lrow=row*4; Lcol=col*2; Rrow=row*4; Rcol=col*0; forLogging = "meli mama"; break; //mother
            case 53: Lrow=row*4; Lcol=col*4; Rrow=row*4; Rcol=col*0; forLogging = "mije mama"; break; //father
            case 54: Lrow=row*1; Lcol=col*9; Rrow=row*3; Rcol=col*14; forLogging = "telo pimeja"; break; //soda
            case 55: Lrow=row*1; Lcol=col*10; Rrow=row*5; Rcol=col*7; forLogging = "tenpo lili"; break; //second
            case 56: Lrow=row*1; Lcol=col*10; Rrow=row*1; Rcol=col*2; forLogging = "tenpo suli"; break; //year
            case 57: Lrow=row*1; Lcol=col*10; Rrow=row*7; Rcol=col*4; forLogging = "tenpo ale"; break; //eternity
            case 58: Lrow=row*6; Lcol=col*1; Rrow=row*5; Rcol=col*7; forLogging = "jan lili"; break; //baby
            case 59: Lrow=row*1; Lcol=col*12; Rrow=row*3; Rcol=col*2; forLogging = "tomo noka"; break; //basement
            case 60: Lrow=row*6; Lcol=col*1; Rrow=row*1; Rcol=col*13; forLogging = "jan tu"; break; //twin
            case 61: Lrow=row*7; Lcol=col*1; Rrow=row*1; Rcol=col*2; forLogging = "akesi suli"; break; //dinosaur
            case 62: Lrow=row*6; Lcol=col*9; Rrow=row*6; Rcol=col*2; forLogging = "kili jelo"; break; //banana
            case 63: Lrow=row*5; Lcol=col*3; Rrow=row*7; Rcol=col*13; forLogging = "lawa ike"; break; //dumb
            case 64: Lrow=row*7; Lcol=col*14; Rrow=row*4; Rcol=col*15; forLogging = "ilo nena"; break; //button
            case 65: Lrow=row*6; Lcol=col*6; Rrow=row*2; Rcol=col*3; forLogging = "kama pona"; break; //welcome
            case 66: Lrow=row*4; Lcol=col*3; Rrow=row*3; Rcol=col*7; forLogging = "mi pakala"; break; //sorry
            case 67: Lrow=row*4; Lcol=col*3; Rrow=row*0; Rcol=col*6; forLogging = "mi wile"; break; //please
            case 68: Lrow=row*6; Lcol=col*1; Rrow=row*2; Rcol=col*3; forLogging = "jan pona"; break; //friend
            case 69: Lrow=row*6; Lcol=col*1; Rrow=row*7; Rcol=col*13; forLogging = "jan ike"; break; //enemy
            case 70: Lrow=row*6; Lcol=col*1; Rrow=row*0; Rcol=col*0; forLogging = "jan utala"; break; //soldier
            case 71: Lrow=row*1; Lcol=col*12; Rrow=row*1; Rcol=col*8; forLogging = "tomo tawa"; break; //vehicle
            case 72: Lrow=row*5; Lcol=col*15; Rrow=row*1; Rcol=col*12; forLogging = "ma tomo"; break; //city
            case 73: Lrow=row*6; Lcol=col*1; Rrow=row*7; Rcol=col*3; forLogging = "jan alasa"; break; //hunter
            case 74: Lrow=row*6; Lcol=col*1; Rrow=row*6; Rcol=col*5; forLogging = "jan kalama"; break; //speaker
            case 75: Lrow=row*6; Lcol=col*1; Rrow=row*5; Rcol=col*3; forLogging = "jan lawa"; break; //leader
            case 76: Lrow=row*3; Lcol=col*1; Rrow=row*4; Rcol=col*11; forLogging = "nimi mute"; break; //sentence
            case 77: Lrow=row*4; Lcol=col*12; Rrow=row*7; Rcol=col*14; forLogging = "nanpa ilo"; break; //calculator
            case 78: Lrow=row*2; Lcol=col*15; Rrow=row*1; Rcol=col*8; forLogging = "sitelen tawa"; break; //movie
            case 79: Lrow=row*2; Lcol=col*13; Rrow=row*7; Rcol=col*2; forLogging = "sina ala"; break; //others
            case 80: Lrow=row*7; Lcol=col*4; Rrow=row*7; Rcol=col*2; forLogging = "ale ala"; break; //nothing
            case 81: Lrow=row*0; Lcol=col*2; Rrow=row*4; Rcol=col*12; forLogging = "wan nanpa"; break; //first
            case 82: Lrow=row*5; Lcol=col*12; Rrow=row*4; Rcol=col*12; forLogging = "luka nanpa"; break; //fifth
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

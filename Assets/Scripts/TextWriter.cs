using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TextWriter : MonoBehaviour
{
    private static TextWriter instance = null;

    private TextMeshProUGUI uiText;
    private Image animatedArrow;
    private int characterIndex;
    private float timePerCharacter;
    private float varyingTimePerCharacter;
    private float timer;
    private string[] stringsToWrite;
    private int[] linesPerString;
    private int stringIdx;
    private bool instantiated = false;

    private void Awake()
    {
        instance = this;
    }

    public static void AddWriter_Static(TextMeshProUGUI text, float timePerCharacter, string[] strings, Image animatedArrow)
    {
        instance.AddWriter(text, timePerCharacter, strings, animatedArrow);
    }

    private void AddWriter(TextMeshProUGUI text, float timePerCharacter, string[] strings, Image animatedArrow)
    {
        this.uiText = text;
        this.timePerCharacter = timePerCharacter;
        this.characterIndex = 0;
        this.linesPerString = new int[strings.Length];
        this.stringsToWrite = ParseNewLines(strings);
        this.stringIdx = 0;
        this.instantiated = true;
        this.animatedArrow = animatedArrow;
    }

    private void Update()
    {
        if (!instantiated)
        {
            return;
        }

        if(IsNotLastStringComplete())
        {
            AnimateArrow();
            CheckNextStringInput();
        }
        else if (uiText != null)
        {
            if (Input.GetKey(KeyCode.Z))
            {
                varyingTimePerCharacter = timePerCharacter / 4;
            }
            else
            {
                varyingTimePerCharacter = timePerCharacter;
            }
            uiText.gameObject.SetActive(true);
            timer -= Time.deltaTime;
            while (timer <= 0.0f)
            {
                // Display next character
                timer += varyingTimePerCharacter;
                characterIndex++;
                uiText.text = stringsToWrite[stringIdx].Substring(0, characterIndex);
                if (IsLastStringComplete())
                {
                    // Entire string displayed
                    uiText = null;
                    return;
                }
            }
        }
    }

    private bool IsLastStringComplete()
    {
        return stringIdx >= stringsToWrite.Length - 1 && characterIndex >= stringsToWrite[stringIdx].Length;
    }

    private bool IsNotLastStringComplete()
    {
        return stringIdx < stringsToWrite.Length - 1 && characterIndex >= stringsToWrite[stringIdx].Length;
    }

    private void CheckNextStringInput()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            stringIdx++;
            characterIndex = 0;
            animatedArrow.gameObject.SetActive(false);
        }
    }

    private void AnimateArrow()
    {
        // Debug.Log("Activating arrow");
        animatedArrow.gameObject.SetActive(true);
        float xArrowPos = 100f + GetCharsAfterLastNewline(stringsToWrite[stringIdx]) * 28;
        float yArrowPos = 120 - 50 * (linesPerString[stringIdx] - 1);
        animatedArrow.gameObject.transform.position = new Vector3(xArrowPos, yArrowPos, animatedArrow.gameObject.transform.position.z);
    }

    private string[] ParseNewLines(string[] strings)
    {
        for (int i = 0; i < strings.Length; i++)
        {
            strings[i] = strings[i].Replace("\\n", "\n");
            this.linesPerString[i] = CountLines(strings[i]);
        }
        return strings;
    }

    public int GetCharsAfterLastNewline(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            Debug.LogError("Input string is null or empty.");
            return 0;
        }

        int lastNewlineIndex = input.LastIndexOf('\n');
        if (lastNewlineIndex == -1)
        {
            return input.Length;
        }
        
        int charsAfterLastNewline = input.Length - lastNewlineIndex - 1;
        return charsAfterLastNewline;
    }

    private int CountLines(string input)
    {
        int count = 1;
        foreach (char c in input)
        {
            if (c == '\n')
            {
                count++;
            }
        }
        // Debug.Log(count);
        return count;
    }

    public static bool IsWriterComplete_Static()
    {
        return instance.IsLastStringComplete();
    }

    public static void DeactivateAndResetWriter_Static()
    {
        if (instance.instantiated == true)
        {
            instance.instantiated = false;
            instance.animatedArrow.gameObject.SetActive(false);
        }
    }

}

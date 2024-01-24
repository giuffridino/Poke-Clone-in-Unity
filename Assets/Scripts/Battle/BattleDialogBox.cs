using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BattleDialogBox : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI dialogText;
    [SerializeField] int lettersPerSecond;

    [SerializeField] GameObject actionDialog;
    [SerializeField] GameObject moveDialogBox;
    [SerializeField] GameObject moveDetails;
    [SerializeField] GameObject actionSelector;
    [SerializeField] GameObject moveSelector;
    [SerializeField] List<TextMeshProUGUI> actionTexts;
    [SerializeField] List<TextMeshProUGUI> moveTexts;
    [SerializeField] GameObject ppSlash;
    [SerializeField] TextMeshProUGUI currentPpText;
    [SerializeField] TextMeshProUGUI maxPpText;
    [SerializeField] TextMeshProUGUI typeText;

    private bool speedUp;
    private GameObject actionArrow;
    private GameObject moveArrow;

    // Don't use Start function
    // private void Start()
    // {
        
    // }

    private void Update()
    {
        speedUp = false;
        if (Input.GetKey(KeyCode.Z))
        {
            speedUp = true;
        }
    }

    public void InitialDialogSetup()
    {
        actionDialog.SetActive(false);
        moveDialogBox.SetActive(false);
        moveDetails.SetActive(false);
        actionArrow = actionDialog.transform.Find("ActionSelector").Find("ActionArrow").GameObject();
        actionArrow.GetComponent<RectTransform>().localPosition = new Vector2(-161f, 29f);
        moveArrow = moveDialogBox.transform.Find("MoveArrow").GameObject();
        moveArrow.GetComponent<RectTransform>().localPosition = new Vector2(-216f, 27f);
    }

    public void SetDialog(string dialog)
    {
        dialogText.text = dialog;
    }

    public IEnumerator TypeDialog(string dialog)
    {
        dialogText.text = "";
        foreach (var letter in dialog.ToCharArray())
        {
            dialogText.text += letter;
            int letterSpeed = speedUp ? lettersPerSecond * 4 : lettersPerSecond;
            yield return new WaitForSeconds(1f/letterSpeed);
        }
        yield return new WaitForSeconds(1.0f);
        // for (int i = 0; i < 5; i++)
        // {
        //     yield return new WaitForSeconds(0.2f);
        //     if (speedUp)
        //     {
        //         yield break;
        //     }
        // }
    }

    public void EnableDialogText(bool enabled)
    {
        dialogText.enabled = enabled;
    }

    public void EnableActionDialog(bool enabled)
    {
        actionDialog.SetActive(enabled);
    }

    public void EnableActionSelector(bool enabled)
    {
        actionSelector.SetActive(enabled);
    }

    public void EnableMoveSelector(bool enabled)
    {
        moveSelector.SetActive(enabled);
        moveDetails.SetActive(enabled);
        moveDialogBox.SetActive(enabled);
    }

    public void UpdateActionSelection(int selectedAction)
    {
        switch (selectedAction)
        {
            case 0:
                actionArrow.GetComponent<RectTransform>().localPosition = new Vector2(-161f, 29f);
                break;
            case 1:
                actionArrow.GetComponent<RectTransform>().localPosition = new Vector2(24f, 29f);
                break;
            case 2:
                actionArrow.GetComponent<RectTransform>().localPosition = new Vector2(-161f, -23f);
                break;
            case 3:
                actionArrow.GetComponent<RectTransform>().localPosition = new Vector2(24f, -23f);
                break;
            default:
                break;
        }
    }

    public void UpdateMoveSelection(int selectedMove, List<Move> moves)
    {
        typeText.text = "";
        EnablePP(true);
        switch (selectedMove)
        {
            case 0:
                moveArrow.GetComponent<RectTransform>().localPosition = new Vector2(-216f, 27f);
                if (moves.Count >= 1)
                {
                    typeText.text = moves[0].Base.Type.ToString();
                    maxPpText.text = moves[0].Base.MaxPp.ToString();
                    currentPpText.text = moves[0].PP.ToString();
                }
                else
                    EnablePP(false);
                break;
            case 1:
                moveArrow.GetComponent<RectTransform>().localPosition = new Vector2(20f, 27f);
                if (moves.Count >= 2)
                {
                    typeText.text = moves[1].Base.Type.ToString();
                    maxPpText.text = moves[1].Base.MaxPp.ToString();
                    currentPpText.text = moves[1].PP.ToString();
                }
                else
                    EnablePP(false);
                break;
            case 2:
                moveArrow.GetComponent<RectTransform>().localPosition = new Vector2(-216f, -26f);
                if (moves.Count >= 3)
                {
                    typeText.text = moves[2].Base.Type.ToString();
                    maxPpText.text = moves[2].Base.MaxPp.ToString();
                    currentPpText.text = moves[2].PP.ToString();
                }
                else
                    EnablePP(false);
                break;
            case 3:
                moveArrow.GetComponent<RectTransform>().localPosition = new Vector2(20f, -26f);
                if (moves.Count >= 4)
                {
                    typeText.text = moves[3].Base.Type.ToString();
                    maxPpText.text = moves[3].Base.MaxPp.ToString();
                    currentPpText.text = moves[3].PP.ToString();
                }
                else
                    EnablePP(false);
                break;
            default:
                break;
        }
    }

    public void SetupPokemonMoveNames(List<Move> moves)
    {
        for (int i = 0; i < 4; i++)
        {
            if (i < moves.Count)
            {
                moveTexts[i].text = moves[i].Base.Name;
            }
            else
            {
                moveTexts[i].text = "-";
            }
        }
    }

    private void EnablePP(bool enabled)
    {
        currentPpText.enabled = enabled;
        maxPpText.enabled = enabled;
        ppSlash.SetActive(enabled);
    }
}

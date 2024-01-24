using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class InteractableSign : MonoBehaviour
{
    [SerializeField] public string[] stringsArray;
    public GameObject dialogBox;
    public TextMeshProUGUI dialogText;
    public Image animatedArrow;
    private bool playerInRange;
    private bool playerClosedDialog = false;
    PlayerController playerController = null;

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!dialogBox.activeSelf && playerInRange && playerController && playerController.GetFacingDirection() == PlayerController.Direction.NORTH)
        {
            if (!playerClosedDialog)
            {
                StartDialog(dialogText, 0.05f, stringsArray, animatedArrow);
            }
            else if (playerClosedDialog && Input.GetKeyDown(KeyCode.Z))
            {
                playerClosedDialog = false;
                StartDialog(dialogText, 0.05f, stringsArray, animatedArrow);
            }
        }
        else if (dialogBox.activeSelf && playerInRange && playerController && playerController.GetFacingDirection() == PlayerController.Direction.NORTH && TextWriter.IsWriterComplete_Static() && Input.GetKeyDown(KeyCode.Z))
        {
            playerClosedDialog = true;
            dialogBox.SetActive(false);
            TextWriter.DeactivateAndResetWriter_Static();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        playerController = other.gameObject.GetComponent<PlayerController>();
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            dialogBox.SetActive(false);
            TextWriter.DeactivateAndResetWriter_Static();
            playerClosedDialog = false;
        }
    }

    private void StartDialog(TextMeshProUGUI text, float timePerCharacter, string[] stringsArray, Image animatedArrow)
    {
        dialogBox.SetActive(true);
        TextWriter.AddWriter_Static(text, timePerCharacter, stringsArray, animatedArrow);
        dialogText.text = "";
    }
}

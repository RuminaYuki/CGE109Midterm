using Gamekit3D;
using System.Collections;
using UnityEngine;

public class UIwhenDontHaveKey : MonoBehaviour
{
    PlayerMovement playerMovement;
    DialogueCanvasController dialogueCanvasController;

    private void Start()
    {
        dialogueCanvasController = GetComponent<DialogueCanvasController>();
        if (!playerMovement)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            playerMovement = player.GetComponent<PlayerMovement>();
        }
    }

    public void ShowUIDontHaveKey()
    {
        if (!playerMovement.KeyCard)
        {
            dialogueCanvasController.ActivateCanvasWithText("need KeyCard1 for pass");
            dialogueCanvasController.DeactivateCanvasWithDelay(2f);
        }
    }

    public void ShowUIDontHaveKey2()
    {
        if (!playerMovement.KeyCard2)
        {
            dialogueCanvasController.ActivateCanvasWithText("need KeyCard2 for pass");
            dialogueCanvasController.DeactivateCanvasWithDelay(2f);
        }
    }
}

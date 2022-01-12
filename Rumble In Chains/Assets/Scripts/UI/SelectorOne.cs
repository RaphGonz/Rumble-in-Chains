using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorOne : MonoBehaviour
{
    [SerializeField]
    Commander[] CommanderList;
    LinkedList<Commander> CommanderLinkedList;
    LinkedListNode<Commander> selected;
    bool selectionIsDone1 = false;
    bool selectionIsDone2 = false;

    private void Start()
    {
        CommanderLinkedList = new LinkedList<Commander>(CommanderList);
        selected = CommanderLinkedList.First;
        ChangeSelected(selected.Value);

    }
    private void Update()
    {
        if ((Input.GetAxis("Horizontal1") > .5 || Input.GetAxis("Vertical1") < -.5) && !selectionIsDone1)
        {
            ChangeSelected(selected.Value);
            if (selected.Next == null)
            {
                selected = CommanderLinkedList.First;
            }
            else
            {
                selected = selected.Next;
            }
            selectionIsDone1 = true;
            ChangeSelected(selected.Value);
        }
        else if ((Input.GetAxis("Horizontal1") < -.5 || Input.GetAxis("Vertical1") > .5) && !selectionIsDone1)
        {
            ChangeSelected(selected.Value);
            if (selected.Previous == null)
            {
                selected = CommanderLinkedList.Last;
            }
            else
            {
                selected = selected.Previous;
            }
            ChangeSelected(selected.Value);
            selectionIsDone1 = true;
        }
        else if (selectionIsDone1 && (Input.GetAxis("Horizontal1") < .5 && Input.GetAxis("Vertical1") < .5 && Input.GetAxis("Horizontal1") > -.5 && Input.GetAxis("Vertical1") > -.5))
        {
            selectionIsDone1 = false;
        }
        if ((Input.GetAxis("Horizontal2") > .5 || Input.GetAxis("Vertical2") < -.5) && !selectionIsDone2)
        {
            ChangeSelected(selected.Value);
            if (selected.Next == null)
            {
                selected = CommanderLinkedList.First;
            }
            else
            {
                selected = selected.Next;
            }
            ChangeSelected(selected.Value);
            selectionIsDone2 = true;
        }        
        else if ((Input.GetAxis("Horizontal2") < -.5 || Input.GetAxis("Vertical2") > .5) && !selectionIsDone2)
        {
            ChangeSelected(selected.Value);
            if (selected.Previous == null)
            {
                selected = CommanderLinkedList.Last;
            }
            else
            {
                selected = selected.Previous;
            }
            ChangeSelected(selected.Value);
            selectionIsDone2 = true;
        }
        else if (selectionIsDone2 && (Input.GetAxis("Horizontal2") < .5)  && Input.GetAxis("Vertical2") < .5 && Input.GetAxis("Horizontal2") > -.5  && Input.GetAxis("Vertical2") > -.5)
        {
            selectionIsDone2 = false;
        }
        if (Input.GetButtonDown("A1"))
        {
            selected.Value.execute();
            
        }
        if (Input.GetButtonDown("A2"))
        {
            selected.Value.execute();
        }


    }
    void ChangeSelected(Commander commander, int blueOrRed = 0) // 0 = blue, 1 = red
    {
        GameObject go = commander.transform.GetChild(blueOrRed).gameObject;
        go.SetActive(!go.activeSelf);
    }

}

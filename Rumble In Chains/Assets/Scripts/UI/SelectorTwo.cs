using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorTwo : MonoBehaviour
{
    [SerializeField]
    Commander[] CommanderList;
    LinkedList<Commander> CommanderLinkedList;
    LinkedListNode<Commander> selected1;
    LinkedListNode<Commander> selected2;
    bool selectionIsDone = false;
    bool selectionIsFinished1 = false;
    bool selectionIsFinished2 = false;
    float bTimer1 = 0;
    float bTimer2 = 0;
    private void Start()
    {
        CommanderLinkedList = new LinkedList<Commander>(CommanderList);
        selected1 = CommanderLinkedList.First;
        selected2 = CommanderLinkedList.First;
        ChangeSelected(selected1.Value, 0);
        ChangeSelected(selected2.Value, 1);
        //Faire en sorte que le selected de base soit visible ;
    }
    private void Update()
    {
        if ((Input.GetAxis("Horizontal1") > .5 || Input.GetAxis("Vertical1") < -.5) && !selectionIsDone && !selectionIsFinished1)
        {
            ChangeSelected(selected1.Value, 0);
            if (selected1.Next == null)
            {
                selected1 = CommanderLinkedList.First;
            }
            else
            {
                selected1 = selected1.Next;
            }
            ChangeSelected(selected1.Value, 0);
            selectionIsDone = true;
        }
        else if ((Input.GetAxis("Horizontal2") > .5 || Input.GetAxis("Vertical2") < -.5) && !selectionIsDone && !selectionIsFinished2)
        {
            ChangeSelected(selected2.Value, 1);
            if (selected2.Next == null)
            {
                selected2 = CommanderLinkedList.First;
            }
            else
            {
                selected2 = selected2.Next;
            }
            ChangeSelected(selected2.Value,1);
            selectionIsDone = true;
        }
        else if ((Input.GetAxis("Horizontal1") < -.5 || Input.GetAxis("Vertical1") > .5) && !selectionIsDone && !selectionIsFinished1)
        {
            ChangeSelected(selected1.Value, 0);
            if (selected1.Previous == null)
            {
                selected1 = CommanderLinkedList.Last;
            }
            else
            {
                selected1 = selected1.Previous;
            }
            ChangeSelected(selected1.Value, 0);
            selectionIsDone = true;

        }
        else if ((Input.GetAxis("Horizontal2") < -.5 || Input.GetAxis("Vertical2") > .5) && !selectionIsDone && !selectionIsFinished2)
        {
            ChangeSelected(selected2.Value, 1);
            if (selected2.Previous == null)
            {
                selected2 = CommanderLinkedList.Last;
            }
            else
            {
                selected2 = selected2.Previous;
            }
            ChangeSelected(selected2.Value, 1);
            selectionIsDone = true;
        }
        else if (selectionIsDone && (Input.GetAxis("Horizontal1") < .5 && Input.GetAxis("Horizontal2") < .5) && Input.GetAxis("Vertical1") < .5 && Input.GetAxis("Vertical2") < .5 && Input.GetAxis("Horizontal1") > -.5 && Input.GetAxis("Horizontal2") > -.5 && Input.GetAxis("Vertical1") > -.5 && Input.GetAxis("Vertical2") > -.5)
        {
            selectionIsDone = false;
        }

        if (Input.GetButtonDown("A1"))
        {
            selected1.Value.execute();
            selectionIsFinished1 = true;
            Validate(selected1.Value);
        }
        if (Input.GetButtonDown("A2"))
        {
            selected2.Value.execute();
            selectionIsFinished2 = true;
            Validate(selected2.Value);
        }
        if (Input.GetButtonDown("B1"))
        {
            selectionIsFinished1 = false;
            Validate(selected1.Value);
        }
        if (Input.GetButtonDown("B2"))
        {
            selectionIsFinished2 = false;
            Validate(selected1.Value);
        }
        if (Input.GetButton("B1"))
        {
            bTimer1 += Time.deltaTime;
            if(bTimer1 > 3)
            {
                GameManager.Instance.LoadScene("MenuScene");
            }
        }
        if (Input.GetButton("B2"))
        {
            bTimer2 += Time.deltaTime;
            if (bTimer2 > 3)
            {
                GameManager.Instance.LoadScene("MenuScene");
            }
        }
        if (Input.GetButtonUp("B1"))
        {
            bTimer1 = 0;
        }
        if (Input.GetButtonUp("B2"))
        {
            bTimer2 = 0;
        }
        if (selectionIsFinished1 && selectionIsFinished2)
        {
            GameManager.Instance.LoadScene("FightScene");
        }

    }

    void ChangeSelected(Commander commander, int blueOrRed) // 0 = blue, 1 = red
    {
        GameObject go = commander.transform.GetChild(blueOrRed).gameObject ;
        go.SetActive(!go.activeSelf);
    }

    private void Validate(Commander commander)
    {
        GameObject go = commander.transform.GetChild(2).gameObject;
        go.SetActive(!go.activeSelf);
    }
}

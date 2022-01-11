using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CommandExecuter : MonoBehaviour
{
    public abstract void Execute(InputManager inputManager);
}

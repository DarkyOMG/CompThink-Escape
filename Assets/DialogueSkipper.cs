using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSkipper : MonoBehaviour
{

    public void SkipDialogue(){
        EventCollector.instance.OnDialogueTimeRanOut.Invoke();
    }
}

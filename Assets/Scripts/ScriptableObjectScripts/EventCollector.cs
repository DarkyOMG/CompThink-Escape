using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Manager/EventCollector")]
public class EventCollector : SingletonScriptableObject<EventCollector>
{

    public Action OnRoomStateChanged;
    public Action OnDialogueTimeRanOut;
    public Action OnActionExecuted;
    public Action OnMouseStatusChanged;
    public Action OnDecompRoundEnded;
    public Action OnAnimalReachedEnd;
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestClass : MonoBehaviour
{

    [SerializeField] private ClipType clipType;
    public Action OnTestAction;
    public GameObject DialogueBox;
    public ActionListSO al;
    public AnimalMover m_Mouse;
    public HamsterController hamster;

    public void PlayTestSFX(AudioClip clip)
    {
        AudioManager.instance.PlaySound(clip, ClipType.SFX);
    }
    public void PlayTestDialogue(AudioClip clip)
    {
        AudioManager.instance.PlaySound(clip, ClipType.Dialogue);
    }
    public void PlayTestMusic(AudioClip clip)
    {
        AudioManager.instance.PlaySound(clip, ClipType.Music);
    }
    public void StopSound(int type)
    {
        AudioManager.instance.StopSound((ClipType)type);
    }
    public void TestTimeTrigger()
    {
        DialogueBox.SetActive(true);
        OnTestAction += () =>
        {
            DialogueBox.SetActive(false);
        };
        TimeManager.Instance.GetTimer(4, OnTestAction);
    }
    public void TestDialogue()
    {
        DialogueTextPrinter.instance.ShowDialogueText("Dies ist ein Test", 2.5f,true);
    }

    public void TestActionList()
    {
        ActionListEnumerator.instance.SetActionList(al);
        ActionListEnumerator.instance.StartActionList();
    }
    public void TestHeap()
    {
        TimeManager.Heap<TimeManager.TimeTrigger> testheap = new TimeManager.Heap<TimeManager.TimeTrigger>();

        TimeManager.TimeTrigger a = new TimeManager.TimeTrigger(2.5f, null);
        TimeManager.TimeTrigger b = new TimeManager.TimeTrigger(1.5f, null);
        TimeManager.TimeTrigger c = new TimeManager.TimeTrigger(0.5f, null);
        TimeManager.TimeTrigger d = new TimeManager.TimeTrigger(4.5f, null);
        TimeManager.TimeTrigger e = new TimeManager.TimeTrigger(5.5f, null);
        TimeManager.TimeTrigger f = new TimeManager.TimeTrigger(6.5f, null);

        testheap.Insert(a);
        testheap.Insert(b);
        testheap.Insert(c);
        testheap.Insert(d);
        testheap.Insert(e);
        testheap.Insert(f);

        Debug.Log(testheap.ToString());
        testheap.Delete(b);
        Debug.Log(testheap.ToString());

    }
    public void TestMouseMove()
    {
        m_Mouse.SetTarget(new Vector3(0, 0, 0));
    }
    public void TestHamsterActionChange()
    {
        hamster.SetAction(1);
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class TimeManager : MonoBehaviour
{

    #region SINGLETON PATTERN
    public static TimeManager _instance;
    public static TimeManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<TimeManager>();

                if (_instance == null)
                {
                    GameObject container = new GameObject("TimeManager");
                    _instance = container.AddComponent<TimeManager>();
                }
            }

            return _instance;
        }
    }
    #endregion
    public Heap<TimeTrigger> triggers;


    public void Awake()
    {
        triggers = new Heap<TimeTrigger>();
    }

    public TimeTrigger GetTimer(float time, Action evnt)
    {
        TimeTrigger timtrig = new TimeTrigger(Time.time + time, evnt);
        if (triggers == null)
        {
            triggers = new Heap<TimeTrigger>();
        }
        triggers.Insert(timtrig);
        return timtrig;
    }

    public void RemoveTimeTrigger(TimeTrigger trigger)
    {
        triggers.Delete(trigger);
    }

    // Update is called once per frame
    public void Update()
    {
        TimeTrigger currentTrigger;
        while (triggers.Root != null && Time.time >= triggers.Root.endtime)
        {
            currentTrigger = triggers.PopRoot();
            currentTrigger.evnt.Invoke();
        }
    }

    public class TimeTrigger : IEquatable<TimeTrigger>, IComparable<TimeTrigger>
    {
        public float endtime;
        public Action evnt;

        public TimeTrigger(float time, Action evnt)
        {
            endtime = time;
            this.evnt = evnt;
        }


        public int CompareTo(TimeTrigger other)
        {

            float end = (other).endtime;
            if (this.endtime < end)
            {
                return -1;
            }
            else if (this.endtime == end)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        public bool Equals(TimeTrigger timtrig)
        => timtrig.endtime == endtime && timtrig.evnt == evnt;
    }


    public class Heap<T> where T : IComparable<T>
    {
        List<T> items;


        public override string ToString() => string.Join("\n", items.Select(x => (x as TimeTrigger).endtime.ToString()));
        

        public T Root
        {
            get { return items.Count > 0 ? items[0] : default; }
        }

        public Heap()
        {
            items = new List<T>();
        }

        public void Insert(T item)
        {
            items.Add(item);

            int i = items.Count - 1;


            while (i > 0)
            {
                if ((items[i].CompareTo(items[(i - 1) / 2]) > 0) ^ true)
                {
                    T temp = items[i];
                    items[i] = items[(i - 1) / 2];
                    items[(i - 1) / 2] = temp;
                    i = (i - 1) / 2;
                }
                else
                    break;
            }
        }

        public void Delete(T toDelete)
        {
            int i = items.IndexOf(toDelete);
            if (i == -1) return;
            items[i] = items[^1];
            items.RemoveAt(items.Count-1);

            


            while (true)
            {
                int leftInd = 2 * i + 1;
                int rightInd = 2 * i + 2;
                int largest = i;

                if (leftInd < items.Count)
                {
                    if (!(items[leftInd].CompareTo(items[largest]) > 0))
                        largest = leftInd;
                }

                if (rightInd < items.Count)
                {
                    if (!(items[rightInd].CompareTo(items[largest]) > 0))
                        largest = rightInd;
                }

                if (largest != i)
                {
                    T temp = items[largest];
                    items[largest] = items[i];
                    items[i] = temp;
                    i = largest;
                }
                else
                    break;
            }
        }

        public T PopRoot()
        {
            T result = items[0];

            Delete(result);

            return result;
        }
    }

}

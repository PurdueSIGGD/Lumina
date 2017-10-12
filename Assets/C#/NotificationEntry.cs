using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationEntry : MonoBehaviour {
    public NotificationEntryData myData;
   
    public void copy(NotificationEntryData entry) {
        myData = new NotificationEntryData();
        myData.message = entry.message;
        myData.count = entry.count;
        myData.sprite = entry.sprite;
        myData.text = entry.text;
        myData.animator = entry.animator;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationStackController : MonoBehaviour {

    
    private Hashtable notificationTable;
    private VerticalLayoutGroup group;
    public GameObject listEntity;
    public Sprite defaultSprite;

    public static void PostNotification(string message) {
        GameObject.FindObjectOfType<NotificationStackController>().AddItem(message);
    }
    public static void PostNotification(string message, Sprite sprite) {
        GameObject.FindObjectOfType<NotificationStackController>().AddItem(message, sprite);
    }
    public static void PostNotification(string message, Sprite sprite, double count) {
        GameObject.FindObjectOfType<NotificationStackController>().AddItem(message, sprite, count);
    }
    public static void Remove(string key) {
        GameObject.FindObjectOfType<NotificationStackController>().RemoveItem(key);
    }

    public void Start() {
        group = this.GetComponent<VerticalLayoutGroup>();
        notificationTable = new Hashtable();
    }

    void AddItem(string message) {
        AddItem(message, defaultSprite);
    }
    void AddItem(string message, Sprite sprite) {
        AddItem(message, sprite, -1);
    }
    void AddItem(string message, Sprite sprite, double count) {
        NotificationEntryData newEntry = new NotificationEntryData();
        newEntry.message = message;
        newEntry.sprite = sprite;
        newEntry.count = count;
        AddItem(newEntry);
    }
    void AddItem(NotificationEntryData entry) {
        if (notificationTable.ContainsKey(entry.message)) {
            NotificationEntryData foundEntry = (NotificationEntryData)notificationTable[entry.message];
            if (foundEntry.message == entry.message && foundEntry.count >= 0) {
                foundEntry.count+= entry.count;
                foundEntry.text.text = string.Format(foundEntry.message, System.Math.Round(foundEntry.count, foundEntry.count % 1 == 0 ? 0 : 1));
                foundEntry.animator.SetTrigger("Reset");
                foundEntry.animator.transform.SetSiblingIndex(0);
                return;
            }
        } else {
            string message = entry.message;
            if (entry.count != -1) {
                // Format it like "the number is {0}
                message = string.Format(message, System.Math.Round(entry.count, entry.count % 1 == 0 ? 0 : 1));
                //print(message);
            }
            GameObject spawn = GameObject.Instantiate(listEntity, group.transform);
            spawn.GetComponent<NotificationEntry>().copy(entry);
            spawn.transform.SetSiblingIndex(0);
            spawn.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = entry.sprite;
            (entry.text = spawn.transform.GetComponentInChildren<Text>()).text = message;
            entry.animator = spawn.GetComponent<Animator>();
            notificationTable.Add(entry.message, entry);
        }


    }
    void RemoveItem(string key) {
        notificationTable.Remove(key);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationStackController : MonoBehaviour {
    private VerticalLayoutGroup group;
    public GameObject listEntity;
    public Sprite defaultSprite;
    public static void PostNotification(string message) {
        GameObject.FindObjectOfType<NotificationStackController>().AddItem(message);
    }
    public static void PostNotification(string message, Sprite sprite) {
        GameObject.FindObjectOfType<NotificationStackController>().AddItem(message, sprite);
    }

    public void Start() {
        group = this.GetComponent<VerticalLayoutGroup>();
    }

    void AddItem(string message) {
        AddItem(message, defaultSprite);
    }
    void AddItem(string message, Sprite sprite) {
        GameObject spawn = GameObject.Instantiate(listEntity, group.transform);
        spawn.transform.SetSiblingIndex(0);
        spawn.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = sprite;
        spawn.transform.GetComponentInChildren<Text>().text = message;
    }
}

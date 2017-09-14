using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MessageListControl : MonoBehaviour {

    public List<NotificationMessage> Messages;
    public Action<NotificationMessage> cbMessageAdded;
    public Action<NotificationMessage> cbMessageRemoved;

    public GameObject NotificationPrefab;

    public List<GameObject> NotificationList;

    void Start()
    {
        NotificationList = new List<GameObject>();
        Messages = new List<NotificationMessage>();
        Rebuild();
    }

    void Update()
    {
        foreach (var message in Messages.ToArray())
        {
            message.Update(Time.deltaTime);
        }
    }

    private void Rebuild()
    {
        NotificationList.Clear();

        var list = new List<GameObject>();

        for (int x = 0; x < transform.childCount; x++){
            var child = transform.GetChild(x);
            if(child.GetComponent<MessageModel>() != null)
            {
                list.Add(child.gameObject);
            }
        }
        transform.DetachChildren();

        foreach(var item in list.ToArray())
        {
            Destroy(item);
        }
       
        foreach (var message in Messages)
        {
            var notification = (GameObject)Instantiate(NotificationPrefab);
            var model = notification.GetComponent<MessageModel>();
            model.notification = message;

            NotificationList.Add(notification);
        }

        foreach(var notification in NotificationList)
        {
            notification.transform.SetParent(this.transform);
            notification.SetActive(true);
        }
    }

    public void AddMessage(NotificationMessage message)
    {
        if( Messages.Any(x=>x.Message == message.Message))
        {
            var result = Messages.First(x => x.Message == message.Message);
            result.Count++;
            result.Cooldown = result.CooldownMax;
        }
        else
        {
            Messages.Add(message);
        }
        Rebuild();
    }
    public void RemoveMessage(NotificationMessage message)
    {
        if (Messages.Contains(message)) {
            Messages.Remove(message);
            Rebuild();
        }
    }
}

public class NotificationMessage 
{
    public string Message { get; set; }
    public float Cooldown { get; set; }
    public float CooldownMax { get; set; }
    public int Count { get; set; }
    public MessageListControl Parent { get; set; }



    public void Update(float timeDeltaTime)
    {
        Cooldown -= timeDeltaTime;
 
        if (Cooldown <= 0.00f)
        {
            Parent.RemoveMessage(this);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageModel : MonoBehaviour {


    public Text Message;
    public string text;

    public NotificationMessage notification;
	// Use this for initialization
    void Start()
    {
        Message.text = notification.Message + notification.Count;
    }

    void Update()
    {
        if(notification.Cooldown < 2f)
        {
            var image = (Image) this.transform.GetComponent<Image>();
            image.color = new Color(image.color.r, image.color.g, image.color.b, .25f);
        }

    }
	
}

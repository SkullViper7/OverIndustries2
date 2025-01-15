using System.Collections.Generic;
using UnityEngine;

public class RoomNotificationManager : MonoBehaviour
{
    // Singleton
    private static RoomNotificationManager _instance = null;
    public static RoomNotificationManager Instance => _instance;

    /// <summary>
    /// Prefab of a notification
    /// </summary>
    [SerializeField]
    private GameObject _notificationPrefab;

    [SerializeField]
    private Transform _notificationsContainer;

    /// <summary>
    /// Id of the pool where notifications are stocked.
    /// </summary>
    private int _notificationsPoolID;

    /// <summary>
    /// A list to stock notifications used.
    /// </summary>
    private List<GameObject> _notifications = new();

    private void Awake()
    {
        // Singleton
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            _instance = this;
        }
    }

    public void Init()
    {
        // Create the pool for notifications
        _notificationsPoolID = ObjectPoolManager.Instance.NewObjectPool(_notificationPrefab);
    }

    /// <summary>
    /// Called to add a notification to a room and get it.
    /// </summary>
    /// <param name="room"> Main component of the room who nee a notification. </param>
    public RoomNotifiction NewNotification(Room room)
    {
        // Get a notification
        GameObject notification = ObjectPoolManager.Instance.GetObjectInPool(_notificationsPoolID);
        _notifications.Add(notification);
        notification.GetComponent<RectTransform>().SetParent(_notificationsContainer);
        notification.SetActive(true);
        notification.GetComponent<RoomNotifiction>().InitNotification(room);

        return notification.GetComponent<RoomNotifiction>();
    }

    /// <summary>
    /// Called by a notification when it has end its job to return in the pool.
    /// </summary>
    /// <param name="notification"> Notification to return in the pool. </param>
    public void RemoveNotification(GameObject notification)
    {
        ObjectPoolManager.Instance.ReturnObjectToThePool(_notificationsPoolID, notification);
        notification.SetActive(false);
        _notifications.Remove(notification);
    }
}

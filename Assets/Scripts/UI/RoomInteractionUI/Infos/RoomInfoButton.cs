using UnityEngine;
using UnityEngine.UI;

public class RoomInfoButton : MonoBehaviour
{
    /// <summary>
    /// The pop up which displays room's infos.
    /// </summary>
    [SerializeField]
    private GameObject _popUp;

    /// <summary>
    /// A reference to the script which manage the pop up.
    /// </summary>
    private RoomInfoPopUp _roomInfoPopUp;

    private void Start()
    {
        _roomInfoPopUp = _popUp.GetComponent<RoomInfoPopUp>();
        GetComponent<Button>().onClick.AddListener(OnButtonClicked);
    }

    /// <summary>
    /// Called to desactivate the button.
    /// </summary>
    public void DesactivateButton()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Called when the button is clicked.
    /// </summary>
    private void OnButtonClicked()
    {
        GameManager.Instance.OpenUI();
        _popUp.SetActive(true);
        _roomInfoPopUp.DisplayDatas();
    }
}

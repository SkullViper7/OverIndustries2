using UnityEngine;
using UnityEngine.UI;

public class RoomProductionButton : MonoBehaviour
{
    /// <summary>
    /// The pop up where player can launch a production.
    /// </summary>
    [SerializeField]
    private GameObject _popUp;

    /// <summary>
    /// A reference to the script which manage the pop up.
    /// </summary>
    private RoomProductionPopUp _roomProductionPopUp;

    private void Start()
    {
        _roomProductionPopUp = _popUp.GetComponent<RoomProductionPopUp>();
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
        _roomProductionPopUp.DisplayDatas();
    }
}

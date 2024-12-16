using UnityEngine;
using UnityEngine.UI;

public class StopProductionButton : MonoBehaviour
{
    /// <summary>
    /// Button component.
    /// </summary>
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnButtonClicked);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveAllListeners();
    }

    private void OnButtonClicked()
    {
        UIManager.Instance.InfoRoomButton.SetActive(false);

        Room currentRoomSelected = InteractionManager.Instance.CurrentRoomSelected;

        switch (currentRoomSelected.RoomData.RoomType)
        {
            case RoomType.Machining:
                MachiningRoom machiningRoom = (MachiningRoom)currentRoomSelected.RoomBehaviour;

                if (machiningRoom.CurrentAmountInInternalStorage > 0)
                {
                    UIManager.Instance.OpenUI();
                    UIManager.Instance.StopProductionPopUp.SetActive(true);
                }
                else
                {
                    machiningRoom.StopProduction();
                }
                break;

            case RoomType.Assembly:
                AssemblyRoom assemblyRoom = (AssemblyRoom)currentRoomSelected.RoomBehaviour;

                if (assemblyRoom.CurrentAmountInInternalStorage > 0)
                {
                    UIManager.Instance.OpenUI();
                    UIManager.Instance.StopProductionPopUp.SetActive(true);
                }
                else
                {
                    assemblyRoom.StopProduction();
                }

                break;

            case RoomType.Research:
                ResearchRoom researchRoom = (ResearchRoom)currentRoomSelected.RoomBehaviour;

                UIManager.Instance.OpenUI();
                UIManager.Instance.StopProductionPopUp.SetActive(true);

                break;
        }

        gameObject.SetActive(false);
    }
}

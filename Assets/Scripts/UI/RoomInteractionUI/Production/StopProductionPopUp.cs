using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StopProductionPopUp : MonoBehaviour
{
    /// <summary>
    /// Button to validate.
    /// </summary>
    [SerializeField]
    private Button _validateButton;

    /// <summary>
    /// Text which warns the player.
    /// </summary>
    [SerializeField]
    private TMP_Text _warningText;

    private void OnEnable()
    {
        Room currentRoomSelected = InteractionManager.Instance.CurrentRoomSelected;

        switch (currentRoomSelected.RoomData.RoomType)
        {
            case RoomType.Machining:
                _warningText.text = "Attention ! Toutes les ressources non-r�cup�r�es seront perdues. �tes-vous s�rs de vouloir continuer ?";

                MachiningRoom machiningRoom = (MachiningRoom)currentRoomSelected.RoomBehaviour;
                _validateButton.onClick.AddListener(() =>
                {
                    machiningRoom.StopProduction();
                    gameObject.SetActive(false);
                    UIManager.Instance.CloseUI();
                });

                break;

            case RoomType.Assembly:
                _warningText.text = "Attention ! Toutes les ressources non-r�cup�r�es seront perdues. �tes-vous s�rs de vouloir continuer ?";

                AssemblyRoom assemblyRoom = (AssemblyRoom)currentRoomSelected.RoomBehaviour;
                _validateButton.onClick.AddListener(() =>
                {
                    assemblyRoom.StopProduction();
                    gameObject.SetActive(false);
                    UIManager.Instance.CloseUI();
                });

                break;

            case RoomType.Research:
                _warningText.text = "Attention ! Toutes les ressoucres engag�es pour la recherche seront perdues. �tes-vous s�rs de vouloir continuer ?";

                ResearchRoom researchRoom = (ResearchRoom)currentRoomSelected.RoomBehaviour;
                _validateButton.onClick.AddListener(() =>
                {
                    researchRoom.StopResearch();
                    gameObject.SetActive(false);
                    UIManager.Instance.CloseUI();
                });

                break;
        }
    }

    private void OnDisable()
    {
        _validateButton.onClick.RemoveAllListeners();
    }
}

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
                _warningText.text = "Attention ! Toutes les pièces détachées non-récupérées seront perdues. Êtes-vous sûr de vouloir continuer ?";

                MachiningRoom machiningRoom = (MachiningRoom)currentRoomSelected.RoomBehaviour;
                _validateButton.onClick.AddListener(() =>
                {
                    machiningRoom.StopProduction();
                    UIManager.Instance.CloseUI();
                    UIManager.Instance.CloseSFX();
                    gameObject.SetActive(false);
                });

                break;

            case RoomType.Assembly:
                _warningText.text = "Attention ! Tous les objets non-récupérés seront perdus. Êtes-vous sûr de vouloir continuer ?";

                AssemblyRoom assemblyRoom = (AssemblyRoom)currentRoomSelected.RoomBehaviour;
                _validateButton.onClick.AddListener(() =>
                {
                    assemblyRoom.StopProduction();
                    UIManager.Instance.CloseUI();
                    UIManager.Instance.CloseSFX();
                    gameObject.SetActive(false);
                });

                break;

            case RoomType.Research:
                _warningText.text = "Attention ! Toutes les ressources engagées pour la recherche seront perdues. Êtes-vous sûr de vouloir continuer ?";

                ResearchRoom researchRoom = (ResearchRoom)currentRoomSelected.RoomBehaviour;
                _validateButton.onClick.AddListener(() =>
                {
                    researchRoom.StopResearch();
                    UIManager.Instance.CloseUI();
                    UIManager.Instance.CloseSFX();
                    gameObject.SetActive(false);
                });

                break;
        }
    }

    private void OnDisable()
    {
        _validateButton.onClick.RemoveAllListeners();
    }
}

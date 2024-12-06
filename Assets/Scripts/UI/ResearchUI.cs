//using TMPro;
//using UnityEngine;
//using UnityEngine.UI;

//public class ResearchUI : MonoBehaviour
//{
//    [SerializeField] private ResearchRoom _researchRoom;

//    [Space, Header("Pop Up"), SerializeField]
//    private GameObject _researchUnlockPopUp;
//    [SerializeField] private GameObject _cantUnlockResearchPopUp;

//    [Space, Header("Upgrade Research Room"), SerializeField]
//    private GameObject _objetForResearchLevel2;
//    [SerializeField] private GameObject _objetForResearchLevel3;

//    [Space, Header("Research Object Info"), SerializeField, Tooltip("Information of the research selected.")]
//    private TextMeshProUGUI _objectNameText;
//    [SerializeField] private TextMeshProUGUI _objectCostText;
//    [SerializeField] private Image _objectIcon;
//    [SerializeField] private Button _researchButton;

//    private ObjectData _currentObjectResearch;

//    void Start()
//    {

//    }

//    public void UnlockObjectUI()
//    {
//        Debug.Log("*pop up* Recherche terminée");
//        _researchButton.GetComponentInChildren<TextMeshProUGUI>().text = "Terminée";
//    }

//    public void CantUnlockThisObjectUI()
//    {
//        Debug.Log("*pop up* Tu ne peut pas rechercher cette objet pour le moment");
//    }

//    public void ResearchRoomUpgrade(int _currentLevel)
//    {
//        //rajouter l'amélioration de la salle --> _objetForResearchLevel2 et _objetForResearchLevel3 pour débloquer les objets suivant
//    }
    
//    public void ResearchObject()
//    {
//        _researchRoom.StartNewResearch(_currentObjectResearch);
//    }

//    /// <summary>
//    /// Change research info
//    /// </summary>
//    /// <param name="_object"></param>
//    public void ShowObjectInfo(ObjectData _object)
//    {
//        _currentObjectResearch = _object;

//        _objectNameText.text = _object.Name;
//        _objectCostText.text = $"Coût : {_object.ResearchCost} \nTemps : {_object.ResearchTime}";
//        _objectIcon.sprite = _object.ObjectPicto;

//        for (int i = 0; i < _researchRoom.ObjectToUnlockList.Count; i++)
//        {
//            if (_object.Name == _researchRoom.ObjectToUnlockList[i].Name)
//            {
//                _researchButton.GetComponentInChildren<TextMeshProUGUI>().text = "Rechercher";
//                break;
//            }
//            else
//            {
//                _researchButton.GetComponentInChildren<TextMeshProUGUI>().text = "Terminée";
//            }
//        }
//    }
//}

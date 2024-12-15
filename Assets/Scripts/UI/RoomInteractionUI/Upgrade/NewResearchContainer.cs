using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewResearchContainer : MonoBehaviour
{
    [SerializeField]
    private Image _newResearchPicto;

    [SerializeField]
    private TMP_Text _newResearchName;

    public void InitData(Sprite picto, string name)
    {
        _newResearchPicto.sprite = picto;
        _newResearchName.text = name;
    }
}

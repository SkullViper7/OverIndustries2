using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmployeeButton : MonoBehaviour
{
    public Button Button { get; private set; }

    public Employee CurrentEmployee { get; private set; }

    [SerializeField]
    private Image _employeePicto;

    [SerializeField]
    private TMP_Text _employeeName;

    [SerializeField]
    private TMP_Text _employeeJob;

    private Image _image;

    public void InitButton(Employee employee, int index)
    {
        Button = GetComponent<Button>();
        _image = GetComponent<Image>();

        CurrentEmployee = employee;

        _employeePicto.sprite = CurrentEmployee.EmployeeJob[0].JobPicto;
        _employeeName.text = CurrentEmployee.EmployeeName;
        _employeeJob.text = CurrentEmployee.EmployeeJob[0].JobName;

        if (index % 2 == 0)
        {
            if (ColorUtility.TryParseHtmlString("#9EFFE5", out Color newColor))
            {
                _image.color = newColor;
            }
        }
        else
        {
            if (ColorUtility.TryParseHtmlString("#4AD8AF", out Color newColor))
            {
                _image.color = newColor;
            }
        }
    }
}

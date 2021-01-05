using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropDownSelection : MonoBehaviour
{
    public Dropdown classNameDropdown;

    private void Start()
    {
        classNameDropdown.ClearOptions();
        List<Dropdown.OptionData> optionList = new List<Dropdown.OptionData>();
        foreach (DrawingManager.LineType lineType in (DrawingManager.LineType[])System.Enum.GetValues(typeof(DrawingManager.LineType)))
        {
            optionList.Add(new Dropdown.OptionData(lineType.ToString()));
        }
        classNameDropdown.AddOptions(optionList);
    }

    public void OnValueChange(int option)
    {
        Debug.Log(((DrawingManager.LineType)option).ToString());
    }

}

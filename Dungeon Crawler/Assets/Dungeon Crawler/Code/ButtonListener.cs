using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Events;
public class ButtonListener : MonoBehaviour
{

    protected Button _button;
    protected UnityAction _onClickAction;
    // Start is called before the first frame update
    void Start()
    {
        _button = GetComponent<Button>();
        _onClickAction += OnClickNumber1;
        _onClickAction += OnClickNumber2;
        _button.onClick.AddListener(_onClickAction);
    }
    private void OnEnable()
    {
        _onClickAction += OnClickNumber1;
        _onClickAction += OnClickNumber2;
    }
    private void OnDisable()
    {
        _onClickAction -= OnClickNumber1;
        _onClickAction -= OnClickNumber2;
    }
    // Update is called once per frame
    void Update()
    {

    }
    protected void OnClickNumber1()
    {
        Debug.Log("Holiwis ");
    }
    protected void OnClickNumber2()
    {
        Debug.Log("Holiwis ");
    }
}

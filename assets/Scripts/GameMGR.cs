using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.UI;
using System;
public class GameMGR : MonoBehaviour 
{
	
	public Text _questionText;
	public Text[] _choicesList;
	public Text[] _voteList;
	public Button _refreshBTN;

	public delegate void RefreshClicked();
	public static event RefreshClicked Click;

	void OnEnable()
	{
		DataMGR.DataBack += ProcessData;
		DataMGR.RBStatus += RefreshBTNStatus;
	}

	//After BTN Clicked 
	public void LoadData()
	{
		Click ();
		_refreshBTN.interactable = false;
	}

	//After GettingData
	public void ProcessData(string question, string publishTime, string[] choiceList, int[] voteList)
	{
		_questionText.text = question;
		for(int i=0; i<choiceList.Length; i++)
		{
			_choicesList [i].text = choiceList [i];
			_voteList [i].text = voteList [i].ToString ();
			_voteList [i].transform.parent.gameObject.SetActive (true);
		}
	}

	//Status Of BTN
	void RefreshBTNStatus()
	{
		_refreshBTN.interactable = true;
	}

	void OnDisable()
	{
		DataMGR.DataBack -= ProcessData;
		DataMGR.RBStatus -= RefreshBTNStatus;
	}

}

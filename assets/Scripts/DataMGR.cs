using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System;
public class DataMGR : MonoBehaviour 
{
	public delegate void DataGetFromServer(string question, string publishTime, string[] choiceList, int[] voteList);
	public static event DataGetFromServer DataBack;

	public delegate void RefreshButtonStatus();
	public static event RefreshButtonStatus RBStatus;
	//public void ProcessData(string question, string publishTime, string[] choiceList, int[] voteList)

	 public string URL="https://private-5b1d8-sampleapi187.apiary-mock.com/questions";
	[HideInInspector]
	public string _Question;
	[HideInInspector]
	public string _PubishedAt;
	[HideInInspector]
	public List<string>  _ChoiceList_PL;
	[HideInInspector]
	public List<int>  _VoteList_PV; 

	void OnEnable()
	{
		GameMGR.Click +=StartLoad;
	}

	public void StartLoad()
	{
		StartCoroutine ("LoadStart");
	}

	IEnumerator LoadStart()
	{
		WWW www = new WWW(URL);
		yield return www;
		if (www.error == null)
		{
			SaveAndProcessData(www.text);

		}
		else
		{
			Debug.Log("ERROR: " + www.error);
			RBStatus ();
		}        
	}    

	void SaveAndProcessData(string str)
	{
		str = str.Substring (1,str.Length-2);
		var N = JSON.Parse(str);
		_Question = N["question"].Value;      
		_PubishedAt = N["published_at"].Value; 
		var l = N ["choices"].Count;
		_ChoiceList_PL.Clear ();
		_VoteList_PV.Clear ();
		for(int i=0; i<l; i++)
		{
			_ChoiceList_PL.Add(N ["choices"] [i] ["choice"].Value);
			_VoteList_PV.Add( N ["choices"] [i] ["votes"].AsInt);
		}

		DataBack (_Question, _PubishedAt, _ChoiceList_PL.ToArray (), _VoteList_PV.ToArray ());
		RBStatus ();
	}

	void OnDisable()
	{
		GameMGR.Click -=StartLoad;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace unitykafka
{
	public class GameLogic : MonoBehaviour
	{
		public Text messageDisplay;
		public static List<string> msgList;
		int lastCount = -1;
		// Start is called before the first frame update
		void Start()
		{
			msgList = new List<string>();
			lastCount = msgList.Count;
			print("0Initialized");
		}

		// Update is called once per frame
		void Update()
		{
			if (msgList.Count != lastCount)
			{
				print("new msg");
				int idx = msgList.Count - 1;
				string output = string.Format("New entry -{0}_ at index {1}", msgList[idx], idx);
				print(output);
				lastCount = msgList.Count;
				messageDisplay.text += "\n" + output;
			}
			else
			{
				print("4+ -");
			}

		}

		void print(string msg)
		{
			Debug.Log(msg);
		}
	}
}
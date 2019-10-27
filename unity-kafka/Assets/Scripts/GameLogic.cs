﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace unitykafka
{
	public class GameLogic : MonoBehaviour
	{
		public static List<string> msgList;
		int lastCount = -1;
		// Start is called before the first frame update
		void Start()
		{
			msgList = new List<string>();
			lastCount = msgList.Count;
			print("Initialized");
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
			}
			else
			{
				print("-");
			}

		}

		void print(string msg)
		{
			Debug.Log(msg);
		}
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.Azure.Kinect.Sensor;
using Microsoft.Azure.Kinect.Sensor.BodyTracking;
using System;

namespace unitykafka
{
	public class GameLogic : MonoBehaviour
	{
		public Text messageDisplay;
		public static List<string> msgList;
		int lastCount = -1;
		
		[SerializeField] GameObject[] blockmanArray;
		public GameObject blockPrefab;

		// Start is called before the first frame update
		void Start()
		{
			msgList = new List<string>();
			lastCount = msgList.Count;

			MakeBlockMan();
			foreach (GameObject go in blockmanArray)
			{
				go.SetActive(true);
			}
			print("0Initialized");
		}

		void MakeBlockMan()
		{
			int numberOfJoints = 32; //  (int)JointId.Count;

			blockmanArray = new GameObject[numberOfJoints];

			for (var i = 0; i < numberOfJoints; i++)
			{
				GameObject jointCube = Instantiate(blockPrefab, transform);
				//deactivate it - (its Start() or OnEnable() won't be called)
				jointCube.SetActive(false);
				jointCube.name = Enum.GetName(typeof(JointId), i);
				//why do we multiply by .4?  idk
				jointCube.transform.localScale = Vector3.one * 0.4f;
				blockmanArray[i] = jointCube;
			}
		}


		// Update is called once per frame
		void Update()
		{
			if (msgList.Count != lastCount)
			{
				print("new msg");
				int idx = msgList.Count - 1;
				//string output = string.Format("New entry -{0}_ at index {1}", msgList[idx], idx);
				//print(output);
				lastCount = msgList.Count;
				//messageDisplay.text += "\n" + output;

				updateBlockman(msgList[idx]);
			}
		}

		void updateBlockman(string jointData)
		{
			string[] parts = jointData.Split('!');
			string utcDateNow = parts[0];
			string skeleton = parts[1];
			// Debug.Log("skeleton: " + skeleton);                     // 0th index is empty string
			string[] jointArray = skeleton.Substring(1).Split('@'); // string is @joints@joints@joints
			int index = -1;
			foreach (string joint in jointArray)
			{
				index++;
				string[] jointPositionQuat = joint.Split('#');

				try
				{
					float x = float.Parse(jointPositionQuat[0].Replace("@", string.Empty));
					float y = float.Parse(jointPositionQuat[1]);
					float z = float.Parse(jointPositionQuat[2]);

					float qw = float.Parse(jointPositionQuat[3]);
					float qx = float.Parse(jointPositionQuat[4]);
					float qy = float.Parse(jointPositionQuat[5]);
					float qz = float.Parse(jointPositionQuat[6]);

					Vector3 v = new Vector3(x, -y, z) * 0.004f;
					Quaternion r = new Quaternion(qx, qy, qz, qw);

					GameObject obj = blockmanArray[index];
					obj.transform.SetPositionAndRotation(v, r);
				}
				catch (FormatException e)
				{
					Debug.Log("jointPositionQuat: " + jointPositionQuat);
					Debug.LogError(e.GetBaseException().ToString());
					Debug.LogError(e.ToString());
					Debug.LogError(e.Message);
				}
			}

		}

		void updateBlockmanSimple(string jointData)
		{
			//pos: head 4.902545 -37.4362 701.1378
			string[] parts = jointData.Split(' ');
			print("parts: ");
			print(parts.ToString());

			string jointName = parts[1]; // i typed this when sending
			float x = float.Parse(parts[2]);
			float y = float.Parse(parts[3]);
			float z = float.Parse(parts[4]);

			float qw = float.Parse(parts[5]);
			float qx = float.Parse(parts[6]);
			float qy = float.Parse(parts[7]);
			float qz = float.Parse(parts[8]);

			print(String.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8}", parts));

			Vector3 v = new Vector3(x, -y, z) * 0.004f;
			Quaternion r = new Quaternion(qx, qy, qz, qw);

			GameObject obj = blockmanArray[20];
			obj.transform.SetPositionAndRotation(v, r);
		}

		void print(string msg)
		{
			Debug.Log(msg);
		}
	}
}
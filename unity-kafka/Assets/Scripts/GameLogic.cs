﻿using System.Collections;
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
        public Transform jointRoot;
		public static List<string> msgList;
		int lastCount = -1;
        public string[] values;
		public GameObject[] jointArray;
		public GameObject jointPrefab;

		// Start is called before the first frame update
		void Start()
		{
			msgList = new List<string>();
			lastCount = msgList.Count;

			CreateJoints();

			print("Initialized");
		}

		void CreateJoints()
		{
            //Debug.Log((int)JointId.Count);
            int numberOfJoints = (int)JointId.Count;

			jointArray = new GameObject[numberOfJoints];

			for (var i = 0; i < numberOfJoints; i++)
			{
				GameObject joint = Instantiate(jointPrefab, transform);
                
                joint.name = Enum.GetName(typeof(JointId), i);

                if (joint.name.Contains("Right"))
                {
                    joint.GetComponent<Renderer>().material.color = Color.red;
                }

                if (joint.name.Contains("Left"))
                {
                    joint.GetComponent<Renderer>().material.color = Color.blue;
                }

                if (joint.name.Contains("Nose"))
                {
                    joint.GetComponent<Renderer>().material.color = Color.green;
                }
                joint.transform.localScale = Vector3.one * 0.25f;
				jointArray[i] = joint;
                joint.transform.parent = jointRoot;
            }

            jointRoot.transform.localScale = Vector3.one * .25f;
		}


		// Update is called once per frame
		void Update()
		{
			if (msgList.Count != lastCount)
			{
				int idx = msgList.Count - 1;
				lastCount = msgList.Count;

				updateJoints(msgList[idx]);
			}
		}

		void updateJoints(string jointData)
		{
			string[] parts = jointData.Split('!');
			string utcDateNow = parts[0];
			string skeleton = parts[1];
            // 0th index is empty string
			string[] jointArrayString = skeleton.Substring(1).Split('@'); // string is @joints@joints@joints
            values = jointArrayString;
			int index = -1;

            #region foreach variation
            foreach (string joint in jointArrayString)
            {
                index++;
                string[] jointPositionQuat = joint.Split('#');

                try
                {
                    float x = float.Parse(jointPositionQuat[0].Replace("@", string.Empty));//negative to flip values for better mirroring
                    float y = float.Parse(jointPositionQuat[1]);
                    float z = float.Parse(jointPositionQuat[2]);

                    //Rotational data that is not going to be used
                    //float qw = float.Parse(jointPositionQuat[3]);
                    //float qx = float.Parse(jointPositionQuat[4]);
                    //float qy = float.Parse(jointPositionQuat[5]);
                    //float qz = float.Parse(jointPositionQuat[6]);

                    Vector3 v = new Vector3(x, -y, z) * 0.004f;
                    //Quaternion r = new Quaternion(qx, qy, qz, qw);

                    //GameObject obj = jointArray[index];
                    //obj.transform.SetPositionAndRotation(v, r);

                    jointArray[index].transform.localPosition = v;
                }
                catch (FormatException e)
                {
                    Debug.Log("jointPositionQuat: " + jointPositionQuat);
                    Debug.LogError(e.GetBaseException().ToString());
                    Debug.LogError(e.ToString());
                    Debug.LogError(e.Message);
                }
            }
            #endregion

        }
    }
}
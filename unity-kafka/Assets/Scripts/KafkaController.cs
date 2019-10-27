using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Confluent.Kafka;

namespace unitykafka
{

	public class KafkaController : MonoBehaviour
	{
		[SerializeField] public string ipAddr = "";
		[SerializeField] public string ipPort = "";
		string kafkaServerAddr = "";
		ConsumerConfig config;
		IConsumer<Ignore, string> c;
		CancellationTokenSource cts;
		bool consuming = false; // initially, start consuming


		// Start is called before the first frame update
		void Start()
		{
			print("1KafkaController start");
			c = new ConsumerBuilder<Ignore, string>(getConfig()).Build();
			c.Subscribe("testTopicName");
			cts = new CancellationTokenSource();
			if (ipAddr.Length < 1) { ipAddr = "localhost"; } // 192.168.2.155
			if (ipPort.Length < 1) { ipPort = "9092"; } // 9092
			kafkaServerAddr = ipAddr + ":" + ipPort;
			print("Initialized " + kafkaServerAddr);
		}

		ConsumerConfig getConfig()
		{
			if (config == null)
			{
				print("2Building ConsumerConfig");
				config = new ConsumerConfig
				{
					GroupId = "test-consumer-group",
					BootstrapServers = ipAddr + ipPort,
					AutoOffsetReset = AutoOffsetReset.Earliest
				};
			}
			print("3returned ConsumerConfig");

			return config;
		}
		
		public void btn_connect()
		{
			print("button works");
			consuming = !consuming;
			InvokeRepeating("doConsume", 1.0f, 1.0f);
		}
		
		void doConsume()
		{
			print("doConsume");
			try
			{
				print("try");

				var cr = c.Consume(cts.Token);
				print("cr");

				print($"Consumed message '{cr.Value}' at: '{cr.TopicPartitionOffset}'.");

				GameLogic.msgList.Add(cr.Value);
				print("GameLogic.msgList.Add(cr.Value)");
			}
			catch (ConsumeException e)
			{
				print("ConsumeException:");
				print($"Error occured: {e.Error.Reason}");
			}
			catch (OperationCanceledException e)
			{
				print("OperationCanceledException:");
				print(e.ToString());
				// Ensure the consumer leaves the group cleanly and final offsets are committed.
				c.Close();
			}
			finally
			{
				print("finally");
			}
			print("didConsume");
		}
		
		void print(string msg)
		{
			Debug.Log(msg);
		}
	}

}
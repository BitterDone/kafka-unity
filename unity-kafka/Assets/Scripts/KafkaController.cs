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
		ConsumerConfig config;
		bool consuming = false; // initially, start consuming
		int counter = 0;

		// Start is called before the first frame update
		void Start()
		{
			print("KafkaController start");

		}

		ConsumerConfig getConfig()
		{
			if (config == null)
			{
				print("Building ConsumerConfig");
				config = new ConsumerConfig
				{
					GroupId = "test-consumer-group",
					BootstrapServers = "localhost:9092",
					AutoOffsetReset = AutoOffsetReset.Earliest
				};
			}
			print("returned ConsumerConfig");

			return config;
		}

		void consumeFromKafka()
		{
			//var conf = new ConsumerConfig
			//{
			//	GroupId = "test-consumer-group",
			//	BootstrapServers = "localhost:9092",
			//	AutoOffsetReset = AutoOffsetReset.Earliest
			//};
			using (var c = new ConsumerBuilder<Ignore, string>(getConfig()).Build())
			{
				c.Subscribe("testTopicName");

				CancellationTokenSource cts = new CancellationTokenSource();

				try
				{
					print("while (true)");
					while (true)
					{
						try
						{
							var cr = c.Consume(cts.Token);
							print($"Consumed message '{cr.Value}' at: '{cr.TopicPartitionOffset}'.");
							GameLogic.msgList.Add(cr.Value);
						}
						catch (ConsumeException e)
						{
							print("ConsumeException:");
							print($"Error occured: {e.Error.Reason}");
						}
					}
				}
				catch (OperationCanceledException e)
				{
					print("OperationCanceledException:");
					print(e.ToString());
					// Ensure the consumer leaves the group cleanly and final offsets are committed.
					c.Close();
				}
				print("while (true) closed");
			}
			print("using() closed");
			consuming = false; //signal that consuming ended for some reason
		}

		public void btn_connect()
		{
			print("button works");
			consuming = true;
		}

		void Update1()
		{
			print("new msg");
		}

		void Update()
		{
			print(". - .");
			if (consuming)
			{
				//int counter = 0;
				//while (consuming)
				//{
				if (counter > 0) { /*print("counter " + counter);*/ }
				else { print("counter 0"); }

				counter += 1;
				GameLogic.msgList.Add("counter" + counter);
				if (counter > 10000) { print("loop done"); counter = 0; consuming = false; }
				//consumeFromKafka();
				//if (consuming == false) // only if loop errors
				//{
				//	print("Rec'd false consuming bool, resetting");
				//	consuming = true;
				//}
				//}
				//print("consuming done");
			}
			else
			{
				print(".");
			}
		}

		void print(string msg)
		{
			Debug.Log(msg);
		}
	}

}
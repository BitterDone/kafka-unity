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
		IConsumer<Ignore, string> c;
		CancellationTokenSource cts;

		// Start is called before the first frame update
		void Start()
		{
			print("1KafkaController start");
			c = new ConsumerBuilder<Ignore, string>(getConfig()).Build();
			c.Subscribe("testTopicName");
			cts = new CancellationTokenSource();

		}

		ConsumerConfig getConfig()
		{
			if (config == null)
			{
				print("2Building ConsumerConfig");
				config = new ConsumerConfig
				{
					GroupId = "test-consumer-group",
					BootstrapServers = "localhost:9092",
					AutoOffsetReset = AutoOffsetReset.Earliest
				};
			}
			print("3returned ConsumerConfig");

			return config;
		}

		void consumeFromKafka()
		{
			////using (var c = new ConsumerBuilder<Ignore, string>(getConfig()).Build())
			////{

			//	try
			//	{
			//		print("while (true)");
			//		while (true)
			//		{
			//			try
			//			{
			//				var cr = c.Consume(cts.Token);
			//				print($"Consumed message '{cr.Value}' at: '{cr.TopicPartitionOffset}'.");
			//				GameLogic.msgList.Add(cr.Value);
			//			}
			//			catch (ConsumeException e)
			//			{
			//				print("ConsumeException:");
			//				print($"Error occured: {e.Error.Reason}");
			//			}
			//		}
			//	}
			//	catch (OperationCanceledException e)
			//	{
			//		print("OperationCanceledException:");
			//		print(e.ToString());
			//		// Ensure the consumer leaves the group cleanly and final offsets are committed.
			//		c.Close();
			//	}
			//	print("while (true) closed");
			////}
			//print("using() closed");
			//consuming = false; //signal that consuming ended for some reason
		}

		public void btn_connect()
		{
			print("button works");
			consuming = !consuming;
			InvokeRepeating("doConsume", 1.0f, 1.0f);
		}

		void Update()
		{
			//if (consuming)
			//{
			//	print("consuming");
			//	doConsume();
			//}
		}

		void doConsume()
		{
			print("doConsume");
			try
			{
				print("try");

				//var cr = c.Consume(cts.Token);
				print("cr");

				//print($"Consumed message '{cr.Value}' at: '{cr.TopicPartitionOffset}'.");

				//GameLogic.msgList.Add(cr.Value);
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

		void Update2()
		{
			//print(". - .");
			//if (consuming)
			//{
			//	//int counter = 0;
			//	//while (consuming)
			//	//{
			//	if (counter > 0) { /*print("counter " + counter);*/ }
			//	else { print("counter 0"); }

			//	counter += 1;
			//	GameLogic.msgList.Add("counter" + counter);
			//	if (counter > 10000) { print("loop done"); counter = 0; consuming = false; }
			//	//consumeFromKafka();
			//	//if (consuming == false) // only if loop errors
			//	//{
			//	//	print("Rec'd false consuming bool, resetting");
			//	//	consuming = true;
			//	//}
			//	//}
			//	//print("consuming done");
			//}
			//else
			//{
			//	print(".");
			//}
		}

		void print(string msg)
		{
			Debug.Log(msg);
		}
	}

}
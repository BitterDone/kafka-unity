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
		// CancellationTokenSource cts;
		bool consuming = false; // initially, start consuming


		// Start is called before the first frame update
		void Start()
		{
			print("1KafkaController start");

			c = new ConsumerBuilder<Ignore, string>(getConfig()).Build();
			c.Subscribe("testTopicName");
			// cts = new CancellationTokenSource();
			int count = c.Subscription.Count; c.Subscription.ForEach(sub => print("sub: " + sub + ", type: " + sub.GetType()));
			print("Subscription count: " + count);
		}

		ConsumerConfig getConfig()
		{
			if (config == null)
			{
				if (ipAddr.Length < 1) { ipAddr = "localhost"; } // 192.168.2.155
				if (ipPort.Length < 1) { ipPort = "9092"; } // 9092
				kafkaServerAddr = ipAddr + ":" + ipPort;
				print("1.5 Initialized " + kafkaServerAddr);

				print("2Building ConsumerConfig");
				config = new ConsumerConfig
				{
					GroupId = "test-consumer-group",
					BootstrapServers = kafkaServerAddr,
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

				// var cr = c.Consume(cts.Token);
				var cr = c.Consume(new TimeSpan(0));

				if (cr != null)
				{
					print("cr");
					print(cr.Value);
					print(cr.TopicPartitionOffset);

					print($"Consumed message '{cr.Value}' at: '{cr.TopicPartitionOffset}'.");

					GameLogic.msgList.Add(cr.Value);
					print("GameLogic.msgList.Add(cr.Value)");
				}
				else
				{
					print("cr == null");
				}
			}
			catch (ConsumeException e)
			{
				print($"ConsumeException occured:");
				print(e.ToString());
				// Ensure the consumer leaves the group cleanly and final offsets are committed.
				c.Close();
			}
			catch (OperationCanceledException e)
			{
				print("OperationCanceledException occured:");
				print(e.ToString());
			}
			catch (Exception e)
			{
				print($"Exception occured:");
				print(e.ToString());
			}
			finally
			{
				print("finally");
				// Ensure the consumer leaves the group cleanly and final offsets are committed.
				// c.Close();
			}
			print("didConsume");
		}
		
		void print(string msg)
		{
			Debug.Log(msg);
		}
	}

}
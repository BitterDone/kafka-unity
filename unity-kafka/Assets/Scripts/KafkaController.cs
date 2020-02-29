using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using Confluent.Kafka;

namespace unitykafka
{

	public class KafkaController : MonoBehaviour
	{
        public Button startConsumeButton;

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
			p("1KafkaController start");

			c = new ConsumerBuilder<Ignore, string>(getConfig()).Build();
			c.Subscribe("testTopicName");
			// cts = new CancellationTokenSource();
			int count = c.Subscription.Count; c.Subscription.ForEach(sub => p("sub: " + sub + ", type: " + sub.GetType()));
			p("Subscription count: " + count);
		}

		ConsumerConfig getConfig()
		{
			if (config == null)
			{
				if (ipAddr.Length < 1) { ipAddr = "localhost"; } // 192.168.2.155
				if (ipPort.Length < 1) { ipPort = "9092"; } // 9092
				kafkaServerAddr = ipAddr + ":" + ipPort;
				p("1.5 Initialized " + kafkaServerAddr);

				p("2Building ConsumerConfig");
                config = new ConsumerConfig
                {
                    GroupId = "test-consumer-group",
                    BootstrapServers = kafkaServerAddr,
                    AutoOffsetReset = AutoOffsetReset.Latest,
                    EnableAutoCommit = false

				};
			}
			p("3returned ConsumerConfig");

			return config;
		}
		
		public void btn_connect()
		{
			print("button works. !consuming: " + !consuming);
			consuming = !consuming;
			InvokeRepeating("doConsume", 0.01f, 0.001f);
            startConsumeButton.gameObject.SetActive(false);
		}
		
		void doConsume()
		{
			p("doConsume");
			try
			{
				p("try");

				// var cr = c.Consume(cts.Token);
				var cr = c.Consume(new TimeSpan(0));

				if (cr != null)
				{
					p("cr");
					p(cr.Value);
					p(cr.TopicPartitionOffset.ToString());

					p($"Consumed message '{cr.Value}' at: '{cr.TopicPartitionOffset}'.");

					GameLogic.msgList.Add(cr.Value);
					p("GameLogic.msgList.Add(cr.Value)");
				}
				else
				{
					p("cr == null");
				}
			}
			catch (ConsumeException e)
			{
				p($"ConsumeException occured:");
				p(e.ToString());
				// Ensure the consumer leaves the group cleanly and final offsets are committed.
				c.Close();
			}
			catch (OperationCanceledException e)
			{
				p("OperationCanceledException occured:");
				p(e.ToString());
			}
			catch (Exception e)
			{
				p($"Exception occured:");
				p(e.ToString());
			}
			finally
			{
				p("finally");
				// Ensure the consumer leaves the group cleanly and final offsets are committed.
				// c.Close();
			}
			p("didConsume");
		}
		
		void p(string msg)
		{
			// Debug.Log(msg);
		}
	}

}
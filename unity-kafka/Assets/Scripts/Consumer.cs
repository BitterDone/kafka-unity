using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Confluent.Kafka;

namespace kafka_client
{
	class Consumer
	{
		public static void consume()
		{
			var conf = new ConsumerConfig
			{
				GroupId = "test-consumer-group",
				BootstrapServers = "localhost:9092",
				// Note: The AutoOffsetReset property determines the start offset in the event
				// there are not yet any committed offsets for the consumer group for the
				// topic/partitions of interest. By default, offsets are committed
				// automatically, so in this example, consumption will only start from the
				// earliest message in the topic 'my-topic' the first time you run the program.
				//AutoOffsetReset = AutoOffsetReset.Earliest
			};

			using (var c = new ConsumerBuilder<Ignore, string>(conf).Build())
			{
				c.Subscribe("testTopicName");

				CancellationTokenSource cts = new CancellationTokenSource();
				Console.CancelKeyPress += (_, e) => {
					e.Cancel = true; // prevent the process from terminating.
					cts.Cancel();
				};

				try
				{
					while (true)
					{
						try
						{
                            //var cr = c.Consume(cts.Token);
                            var cr = c.Consume(new TimeSpan(0));
                            Console.WriteLine($"Consumed message '{cr.Value}' at: '{cr.TopicPartitionOffset}'.");
						}
						catch (ConsumeException e)
						{
							Console.WriteLine($"Error occured: {e.Error.Reason}");
						}
					}
				}
				catch (OperationCanceledException)
				{
					Console.WriteLine("cancelled");
					// Ensure the consumer leaves the group cleanly and final offsets are committed.
					c.Close();
				}
			}
		}
	}
}


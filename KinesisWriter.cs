using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Amazon;
using Amazon.Kinesis;
using Amazon.Kinesis.Model;
using log4net;
using Newtonsoft.Json;

namespace PocUnifiedLogWorker
{
    public class KinesisWriter
    {
        public void ProcessDirectory(string dir)
        {
            var log = LogManager.GetLogger(typeof (KinesisWriter));

            var files = Directory.GetFiles(dir, "*.*", SearchOption.TopDirectoryOnly);

            var events = new List<EventData>();
            var processedFiles = new List<string>();

            log.DebugFormat("Found {0} files, dir {1}", files.Length, dir);

            foreach (var file in files)
            {
                try
                {
                    events.Add(JsonConvert.DeserializeObject<EventData>(File.ReadAllText(file)));
                    processedFiles.Add(file);   
                }
                catch (Exception ex)
                {
                    log.WarnFormat("File {0} could not be deserialized, error is [{1}], dir is {2}", file, ex, dir);
                }
                if (processedFiles.Count >= Properties.Settings.Default.BlockSize)
                {
                    break;
                }
            }

            if (events.Count == 0)
            {
                return;
            }

            var putRecords = events.Select(evt => new PutRecordsRequestEntry
            {
                PartitionKey = evt.KinesisInfo.PartitionKey,
                Data =
                    new MemoryStream(System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(evt.UnifiedLogEvent)))
            }).ToList();

            var streamName = events[0].KinesisInfo.StreamName;

            log.DebugFormat("Sending {0} events, stream {1}", putRecords.Count, streamName);
            using (var client = new AmazonKinesisClient(events[0].KinesisInfo.AccessKey, events[0].KinesisInfo.SecretKey, RegionEndpoint.GetBySystemName(events[0].KinesisInfo.Endpoint)))
            {
                try
                {
                    var response = client.PutRecords(new PutRecordsRequest
                    {
                        Records = putRecords,
                        StreamName = streamName
                    });

                    log.DebugFormat("Files send, stream {0}, response was {1}", streamName, response.HttpStatusCode);

                    if (response.HttpStatusCode == HttpStatusCode.OK)
                    {
                        foreach (var file in processedFiles)
                        {
                            File.Delete(file);
                        }
                    }
                    else
                    {
                        log.WarnFormat("Kinesis returned {0}, stream was {1}", response.HttpStatusCode, streamName);
                    }
                }
                catch (Exception ex)
                {
                    log.Error(string.Format("Error putting events to Kinesis, stream was {0}", streamName), ex);
                }
            }
        }
    }
}

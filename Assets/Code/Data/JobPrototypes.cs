
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

namespace Assets.Code.Data
{
    public class JobPrototypes
    {
        public Dictionary<string, Job> JobPrototypeDictionary;

        public JobPrototypes()
        {
            JobPrototypeDictionary = new Dictionary<string, Job>();
        }


        public void RegisterJobPrototype(Job job)
        {
            JobPrototypeDictionary.Add(job.JobType, job);
        }

        public void Init()
        {
            //loads furn xml data
            var filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "Data");
            filePath = System.IO.Path.Combine(filePath, "Jobs.xml");
            var jobXmlText = File.ReadAllText(filePath);

            var reader = XmlTextReader.Create(new StringReader(jobXmlText));

            int jobPrototypesCount = 0;

            if (reader.ReadToDescendant("Jobs"))
            {
                if (reader.ReadToDescendant("Job"))
                {
                    jobPrototypesCount++;

                    var job = new Job();
                    job.ReadXmlPrototype(reader);

                    RegisterJobPrototype(job);
                }
                else
                {
                    Debug.Log("Could not find a Job in job.xml");
                }

            }
            else
            {
                Debug.Log("Could not find Jobs in job.xml");
            }

            Debug.Log("Job prototypes created "  + jobPrototypesCount);
        }


        public Job Get(string jobName)
        {
            return JobPrototypeDictionary[jobName];
        }

    }
}

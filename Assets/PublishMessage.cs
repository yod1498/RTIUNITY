using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTI.Connext.Connector;
using System;
using System.Threading;

public class PublishMessage : MonoBehaviour
{
    public bool publishMode = true;
    public int count = 10;
    string configPath = "/ShapeExample.xml";

    // Start is called before the first frame update
    void Start()
    {
        configPath = Application.dataPath + configPath;
        Debug.Log("config path = " + configPath);

        string configName = "MyParticipantLibrary::Zero";

        Connector connector = new Connector(configName, configPath);

        if (publishMode)
            Publish(connector, count);
        else
            Subscribe(connector, count);

        Debug.Log("Finalizing RTI Connector");

    }

    void Publish(Connector connector, int count)
    {
        string outputName = "MyPublisher::MySquareWriter";
        Output output = connector.GetOutput(outputName);

        Instance instance = output.Instance;
        for (int i = 0; i < count || count == 0; i++)
        {
            Debug.Log("Writing sample {" + i + "}");

            // Optionally, clear the instance field from previous iterations
            output.ClearValues();
            instance.SetValue("x", i);
            instance.SetValue("y", i * 2);
            instance.SetValue("shapesize", 30);
            instance.SetValue("color", "BLUE");

            output.Write();
            Thread.Sleep(100);
        }
    }

    void Subscribe(Connector connector, int count)
    {
        string inputName = "MySubscriber::MySquareReader";
        RTI.Connext.Connector.Input input = connector.GetInput(inputName);

        for (int i = 0; i < count || count == 0; i++)
        {
            // Poll for samples every second
            Debug.Log("Waiting 1 second...");
            Thread.Sleep(1000);

            // Take samples. Accesible from Input.Samples
            input.Take();
            Debug.Log("Received {" + input.Samples.Count + "} samples");
            foreach (Sample sample in input.Samples)
            {
                if (sample.Info.IsValid)
                {
                    Debug.LogFormat(
                        "Received [x={0}, y={1}, size={2}, color={3}]",
                        sample.Data.GetInt32Value("x"),
                        sample.Data.GetInt32Value("y"),
                        sample.Data.GetInt32Value("shapesize"),
                        sample.Data.GetStringValue("color"));
                }
                else
                {
                    Debug.Log("Received metadata");
                }
            }
        }
    }

    //private IEnumerator Delay(float waitTime)
    //{
    //    yield return new WaitForSeconds(waitTime);
    //    print("Coroutine ended: " + Time.time + " seconds");
    //}

    //void SubscribeCoroutine(Connector connector, int count)
    //{
    //    string inputName = "MySubscriber::MySquareReader";
    //    RTI.Connext.Connector.Input input = connector.GetInput(inputName);

    //    for (int i = 0; i < count || count == 0; i++)
    //    {
    //        // Poll for samples every second
    //        Debug.Log("Waiting 1 second...");
    //        //yield return new WaitForSeconds(1);
    //        //WaitAndPrint(input);
    //    }
    //}

    //void WaitAndPrint(RTI.Connext.Connector.Input input)
    //{
    //    // Take samples. Accesible from Input.Samples
    //    input.Take();
    //    Debug.Log("Received {" + input.Samples.Count + "} samples");
    //    foreach (Sample sample in input.Samples)
    //    {
    //        if (sample.Info.IsValid)
    //        {
    //            Debug.LogFormat(
    //                "Received [x={0}, y={1}, size={2}, color={3}]",
    //                sample.Data.GetInt32Value("x"),
    //                sample.Data.GetInt32Value("y"),
    //                sample.Data.GetInt32Value("shapesize"),
    //                sample.Data.GetStringValue("color"));
    //        }
    //        else
    //        {
    //            Debug.Log("Received metadata");
    //        }
    //    }
    //}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTI.Connext.Connector;
using System;

public class Subscriber : MonoBehaviour
{
    string configPath = "/ShapeExample.xml";
    Connector connector;

    // Start is called before the first frame update
    void Start()
    {
        configPath = Application.dataPath + configPath;
        Debug.Log("config path = " + configPath);

        string configName = "MyParticipantLibrary::Zero";

        connector = new Connector(configName, configPath);

        // Poll for samples every second
        InvokeRepeating("ReceiveData", 1, 1);
    }

    void ReceiveData()
    {
        string inputName = "MySubscriber::MySquareReader";
        RTI.Connext.Connector.Input input = connector.GetInput(inputName);
    
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

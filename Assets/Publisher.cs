using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTI.Connext.Connector;
using System;

public class Publisher : MonoBehaviour
{
    string configPath = "/ShapeExample.xml";
    Connector connector;
    int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        configPath = Application.dataPath + configPath;
        Debug.Log("config path = " + configPath);

        string configName = "MyParticipantLibrary::Zero";

        connector = new Connector(configName, configPath);

        InvokeRepeating("SendData", 1, 0.5f);
    }

    void SendData()
    {
        string outputName = "MyPublisher::MySquareWriter";
        Output output = connector.GetOutput(outputName);

        Instance instance = output.Instance;
        Debug.Log("Writing sample {" + count + "}");

        // Optionally, clear the instance field from previous iterations
        output.ClearValues();
        instance.SetValue("x", count);
        instance.SetValue("y", count * 2);
        instance.SetValue("shapesize", 30);
        instance.SetValue("color", "BLUE");

        output.Write();

        count++;
    }
}

using System;
using System.Collections.Generic;
using FullSerializer;
using UnityEngine;

struct SerializedStruct
{
    public int Field;
    public Dictionary<string, string> DictionaryAutoProperty { get; set; }
    [SerializeField]
    private int PrivateField;
}
public static class FullSerializerAPI
{
    private static readonly fsSerializer _serializer = new();

    public static string Serialize(Type type, object value, bool isPretty = false, bool isEncryption = true)
    {
        // serialize the data
        fsData data;
        _serializer.TrySerialize(type, value, out data).AssertSuccessWithoutWarnings();

        // emit the data via JSON
        if (isPretty)
        {
            string jsonStr = fsJsonPrinter.PrettyJson(data);
            //if (isEncryption)
            //    jsonStr = StringEncryption.EncryptDES(jsonStr);   //����
            return jsonStr;
        }
        else
        {
            string jsonStr = fsJsonPrinter.CompressedJson(data);
            //if (isEncryption)
            //    jsonStr = StringEncryption.EncryptDES(jsonStr);   //����
            return jsonStr;
            //return fsJsonPrinter.CompressedJson(data);
        }
    }

    public static object Deserialize(Type type, string serializedState)
    {
        //serializedState = StringEncryption.DecryptDES(serializedState); //////����
        // step 1: parse the JSON data
        fsData data = fsJsonParser.Parse(serializedState);

        // step 2: deserialize the data
        object deserialized = null;
        _serializer.TryDeserialize(data, type, ref deserialized).AssertSuccessWithoutWarnings();

        return deserialized;
    }

    public static T Deserialize<T>(string jsonStr) where T : new()
    {
        fsData data = fsJsonParser.Parse(jsonStr);
        T deserialized = new();
        _serializer.TryDeserialize(data, ref deserialized);
        return deserialized;
    }
}
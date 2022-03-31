using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MyMmo.Commons.Scripts;
using MyMmo.Commons.Snapshots;
using ProtoBuf;
using ProtoBuf.Meta;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class ProtobufSerializerCheck : MonoBehaviour
{
    
    private void Awake()
    {
        TypeModel model = (TypeModel)Activator.CreateInstance(Type.GetType("MyProtoModel, MyProtoModel"));//new MyProtoModel();
        ScriptsDataProtocol.DeserializeTypeModel = model;
        SnapshotsDataProtocol.DeserializeTypeModel = model;
    }

#if UNITY_EDITOR
    [MenuItem("Protobuf/To Test")]
    private static void ToTest() {
        AppDomain.CurrentDomain.Load("MyMmo.Commons");
    }
    
    [MenuItem("Protobuf/Build model")]
    private static void BuildMyProtoModel()
    {
        RuntimeTypeModel typeModel = GetModel();
        typeModel.Compile("MyProtoModel", "MyProtoModel.dll");

        if (!Directory.Exists("Assets/Protobuf"))
        {
            Directory.CreateDirectory("Assets/Protobuf");
        }
        
        File.Copy("MyProtoModel.dll", "Assets/Protobuf/MyProtoModel.dll", true);
        
        AssetDatabase.Refresh();
    }
    
    [MenuItem("Protobuf/Create proto file")]
    private static void CreateProtoFile()
    {
        RuntimeTypeModel typeModel = GetModel();
        using (FileStream stream = File.Open("model.proto", FileMode.Create))
        {
            byte[] protoBytes = Encoding.UTF8.GetBytes(typeModel.GetSchema(null));
            stream.Write(protoBytes, 0, protoBytes.Length);
        }
    }

    private static RuntimeTypeModel GetModel()
    {
        RuntimeTypeModel typeModel = TypeModel.Create();

        foreach (var t in GetTypes())
        {
            var contract = t.GetCustomAttributes(typeof(ProtoContractAttribute), false);
            
            if (t.FullName?.Contains("MyMmo") == true) {
                Debug.Log($"Found contracts[{contract.Length}] in MyMmo type: {t}, ");
            }

            if (contract.Length > 0)
            {
                typeModel.Add(t, true);

                //add unity types
                //typeModel.Add(typeof(Vector2), false).Add("x", "y");
                //typeModel.Add(typeof(Vector3), false).Add("x", "y", "z");
            }
        }

        return typeModel;
    }

    private static IEnumerable<Type> GetTypes() {
        var assemnlyNames = new[] {
            "MyMmo.Commons"
        };
        
        foreach (var precompiledAssemblyName in assemnlyNames) {
            foreach (Type type in AppDomain.CurrentDomain.Load(precompiledAssemblyName).GetTypes())
            {
                yield return type;
            }
        }
    }
#endif
}
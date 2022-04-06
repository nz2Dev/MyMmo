using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MyMmo.Commons.Scripts;
using MyMmo.Commons.Snapshots;
using ProtoBuf;
using ProtoBuf.Meta;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ProtobufAOTCompilationHelper : MonoBehaviour {

    private void Awake() {
        TypeModel model =
            (TypeModel) Activator.CreateInstance(Type.GetType("MyMmoCommonsProtoModel, MyMmoCommonsProtoModel"));
        ScriptsDataProtocol.DeserializeTypeModel = model;
        SnapshotsDataProtocol.DeserializeTypeModel = model;
    }

#if UNITY_EDITOR
    [MenuItem("Protobuf/Build MyMmo.Commons Model")]
    private static void BuildMyMmoCommonsModel() {
        RuntimeTypeModel typeModel = GetMyMmoCommonsModel();
        typeModel.Compile("MyMmoCommonsProtoModel", "MyMmoCommonsProtoModel.dll");

        if (!Directory.Exists("Assets/Photon/Protobuf")) {
            Directory.CreateDirectory("Assets/Photon/Protobuf");
        }

        File.Copy("MyMmoCommonsProtoModel.dll", "Assets/Photon/Protobuf/MyMmoCommonsProtoModel.dll", true);
        File.Delete("MyMmoCommonsProtoModel.dll");

        AssetDatabase.Refresh();
    }

    [MenuItem("Protobuf/Create proto file")]
    private static void CreateProtoFile() {
        RuntimeTypeModel typeModel = GetMyMmoCommonsModel();
        using (FileStream stream = File.Open("model.proto", FileMode.Create)) {
            byte[] protoBytes = Encoding.UTF8.GetBytes(typeModel.GetSchema(null));
            stream.Write(protoBytes, 0, protoBytes.Length);
        }
    }

    private static RuntimeTypeModel GetMyMmoCommonsModel() {
        RuntimeTypeModel typeModel = TypeModel.Create();

        foreach (var t in GetTypes(new[] {"MyMmo.Commons"})) {
            var contract = t.GetCustomAttributes(typeof(ProtoContractAttribute), false);

            if (t.FullName?.Contains("MyMmo") == true) {
                Debug.Log($"Found contracts[{contract.Length}] in MyMmo type: {t}, ");
            }

            if (contract.Length > 0) {
                typeModel.Add(t, true);
            }
        }

        return typeModel;
    }

    private static IEnumerable<Type> GetTypes(string[] assemblyNames) {
        foreach (var precompiledAssemblyName in assemblyNames) {
            foreach (Type type in AppDomain.CurrentDomain.Load(precompiledAssemblyName).GetTypes()) {
                yield return type;
            }
        }
    }
#endif

}
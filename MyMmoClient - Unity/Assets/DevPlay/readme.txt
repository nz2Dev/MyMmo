MyMmo.Processing.dll targets .Net Framework 4.X, but because it references only system assemblies from there, it's compatible with Unity, e.g all the namespaces exist in Unity
MyMmo.Processing.dll reference System.Numerics namespace from separate .dll that was taken from netstandart2.0 subset that the Unity uses
(e.g somewhere from folded C:/.../Editor/netstandart/2.0/extensions/...) when there is compatibility level set to netstandard2.0 in Unity. 
so that's why it works probably...
            
so to work properly in Unity with compatibility level set to netstandaed out of the box, without referencing manually Unity's dll
the project that is build, should have target framework set to netstandard
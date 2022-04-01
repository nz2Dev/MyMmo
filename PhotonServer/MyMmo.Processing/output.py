# Python file one module
import sys
import os
import shutil

rootPath = os.path.abspath(__file__ + '/../../../')
srcPath = os.path.abspath(rootPath + "/PhotonServer/MyMmo.Processing/bin/Debug/")
dstPath = os.path.abspath(rootPath + '/MyMmoClient - Unity/Assets/Dev/Processing/')
print("srcPath: " + srcPath)
print("dstPath: " + dstPath)

if not os.path.exists(dstPath):
	os.mkdir(dstPath)

shutil.copy(os.path.join(srcPath, "MyMmo.Processing.dll"), os.path.join(dstPath, "MyMmo.Processing.dll"))
shutil.copy(os.path.join(srcPath, "Artemis.dll"), os.path.join(dstPath, "Artemis.dll"))
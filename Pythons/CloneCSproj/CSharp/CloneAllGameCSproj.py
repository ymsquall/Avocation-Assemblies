#!/usr/bin/python
#encoding:utf8

import sys
import os

# 设置默认编码为utf8
reload(sys)
sys.setdefaultencoding("utf-8")

cmd = "python CSharp/CloneGameCSproj.py"
os.system(cmd)

cmd = "python CSharp/CloneLuaCSproj.py"
os.system(cmd)

cmd = "python CSharp/CloneNGUIEngineCSproj.py"
os.system(cmd)

cmd = "python CSharp/CloneUniqueCSproj.py"
os.system(cmd)

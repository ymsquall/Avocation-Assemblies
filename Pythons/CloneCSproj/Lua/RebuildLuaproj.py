#!/usr/bin/python
# -*- coding:utf-8 -*-
"""
a new python module
"""

# still running on Python 2.7
from __future__ import unicode_literals # 3.x后字符串默认是unicode,ascii编码表示为 b''
from __future__ import division # 3.x后整数除法返回浮点数,要使用2.x的整数除法需要使用 //

__author__ = "yueming"

import sys
import os
sys.path.append("../Common/")
sys.path.append("../../Common/")
import FileUtils as Files
sys.path.append("./")
sys.path.append("../")
import CSprojFileUtils as CSFileUtils

# 设置默认编码为utf8
reload(sys)
sys.setdefaultencoding("utf-8")

# 检查参数合法性
if len(sys.argv) < 5:
    print "参数数量错误"
    os._exit(0)
    
LuaProjectTemplateFile = sys.argv[1]
LuaProjectFile = sys.argv[2]
LuaSourceParentPath = []
LuaPathName = []
NowProcessSourcePath = ""
for i in range(3, len(sys.argv), 2):
    LuaSourceParentPath.append(sys.argv[i])
    LuaPathName.append(sys.argv[i + 1])

print LuaSourceParentPath
print LuaPathName

if len(LuaSourceParentPath) != len(LuaPathName):
    print "参数非法"
    os._exit(0);


def BuildLuaProject():
    index = 0
    for index in range(0, len(LuaSourceParentPath)):
        path = LuaSourceParentPath[index] + LuaPathName[index]
        if not CSFileUtils.EnumAllFiles(path, LuaSourceParentPath[index], ".lua"):
           return
    if not CSFileUtils.Backup(LuaProjectFile):
        print "备份项目文件 %s 失败！！！" % LuaProjectFile
    reader = Files.TextFileReader(LuaProjectTemplateFile)
    newText = reader.buffer
    groups = CSFileUtils.BuildFileAndPathGroup()
    pathGroup = groups[0]
    fileGroup = groups[1]
    newText = newText + pathGroup
    newText = newText + fileGroup
    newText = newText + "\n</Project>"
    realPath = os.path.dirname(LuaProjectFile)
    realPath = os.path.realpath(realPath)
    if not os.path.exists(realPath):
        os.makedirs(realPath)
    writer = Files.TextFileWriter(LuaProjectFile)
    writer.write(newText)
    writer.close();
    

BuildLuaProject()
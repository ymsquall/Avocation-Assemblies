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
SOURCE_NAME = sys.argv[1]
SOURCE_PATH = sys.argv[2] + SOURCE_NAME + "/"
TARGET_NAME = sys.argv[3]
TARGET_PATH = sys.argv[4] + TARGET_NAME + "/"
TARGET_PARENT_PATH = sys.argv[4]
#SOURCE_NAME = "client_prg.CSharp"
#SOURCE_PATH = "D:/project/arpg_2/unity5ver/arpg_prg/client_prg/"
#TARGET_NAME = "client_prg"
#TARGET_PATH = "D:/works/Work/Lua/Game/" + TARGET_NAME + "/"

if len(sys.argv) > 5:
    IS_UNITY_5 = sys.argv[5] == "unity5"
else:
    IS_UNITY_5 = True

def DoClone(csProjFile, targetName, targetExt, notInclude, include, platform):
    """LoadCsprojs"""
    srcProjFile = SOURCE_PATH + csProjFile
    if not os.path.exists(srcProjFile) or not os.path.isfile(srcProjFile):
        defaultProjFile = csProjFile
        if not os.path.exists(defaultProjFile):
            defaultProjFile = "CSharp/" + csProjFile
        print "未生成项目 %s 的vs项目，使用默认文件 %s 替代" % (srcProjFile, defaultProjFile)
        srcProjFile = defaultProjFile
    reader = Files.TextFileReader(srcProjFile)
    srcText = reader.buffer

    fileGroup = ""
    pathGroup = ""

    otherGroups = ""
    startIdx = srcText.find("<ItemGroup>")
    endIdx = srcText.find("</ItemGroup>", startIdx) + len("</ItemGroup>") + 3
    while(startIdx >= 0):
        group = srcText[startIdx:endIdx]
        groupText1 = ""
        groupText2 = ""
        groupText3 = ""
        if group.find("<Compile Include=") != -1:
            groupText1 = group
        elif group.find("<Folder Include=") != -1:
            groupText2 = group
        elif group.find("<None Include=") != -1:
            groupText3 = group
        else:
            otherGroups = otherGroups + group
        text1 = srcText[0:startIdx]
        text2 = srcText[endIdx:]
        srcText = text1 + text2
        startIdx = srcText.find("<ItemGroup>")
        endIdx = srcText.find("</ItemGroup>", startIdx) + len("</ItemGroup>") + 3
        
    groups = CSFileUtils.BuildFileAndPathGroup(notInclude, include)
    otherGroups = otherGroups + groups[0]
    otherGroups = otherGroups + groups[1]
    startIdx = srcText.find("<Import Project=")
    text1 = srcText[0:startIdx-1]
    text2 = srcText[startIdx-1:]
    srcText = text1 + otherGroups + text2
    srcText = srcText.replace("<HintPath>", "<HintPath>%s" % SOURCE_PATH.replace("/", "\\"))
    srcText = srcText.replace("<AssemblyName>Assembly-CSharp</AssemblyName>", "<AssemblyName>%s</AssemblyName>" % TARGET_NAME)
    srcText = srcText.replace("<AssemblyName>Assembly-CSharp-Editor</AssemblyName>", "<AssemblyName>%s.Editor</AssemblyName>" % TARGET_NAME)
    cmdstr = "  <PropertyGroup>\n"
    cmdstr += "    <PreBuildEvent>\n"
    cmdstr += "      del $(SolutionDir)..\\Unity3D\\Plugins\\%s$(TargetFileName)\n" % platform.replace("/", "\\")
    cmdstr += "      del $(SolutionDir)..\\Unity3D\\Plugins\\%s$(TargetName).pdb\n" % platform.replace("/", "\\")
    cmdstr += "    </PreBuildEvent>\n"
    cmdstr += "  </PropertyGroup>\n"
    cmdstr += "  <PropertyGroup>\n"
    cmdstr += "    <PostBuildEvent>\n"
    cmdstr += "      xcopy $(TargetPath) $(SolutionDir)..\\Unity3D\\Plugins\\%s /s\n" % platform.replace("/", "\\")
    cmdstr += "      xcopy $(ProjectDir)$(OutDir)$(TargetName).pdb $(SolutionDir)..\\Unity3D\\Plugins\\%s /s\n" % platform.replace("/", "\\")
    cmdstr += "    </PostBuildEvent>\n"
    cmdstr += "  </PropertyGroup>\n"
    cmdstr += "</Project>\n"
    srcText = srcText[0:-11]
    srcText = srcText + cmdstr

    writer = Files.TextFileWriter(TARGET_PATH + targetName + targetExt + ".csproj")
    writer.write(srcText)
    writer.close();

if not CSFileUtils.EnumAllFiles(SOURCE_PATH + "Assets/", SOURCE_PATH, ".cs"):
    os._exit(0)
    
realPath = os.path.realpath(TARGET_PATH)
if not os.path.exists(realPath):
    os.makedirs(realPath)

if IS_UNITY_5:
    csprojName = "%s.csproj" % TARGET_NAME
    csprojEditorName = "%s.Editor.csproj" % TARGET_NAME
    if not CSFileUtils.Backup(TARGET_PATH + csprojName):
        print "备份项目文件 %s%s 失败！！！" % (TARGET_PATH, csprojName)
    if not CSFileUtils.Backup(TARGET_PATH + csprojEditorName):
        print "备份项目文件 %s%s 失败！！！" % (TARGET_PATH, csprojEditorName)
    DoClone("%s.csproj" % SOURCE_NAME, TARGET_NAME, "", "/Editor/", "", "x86_64/")
    DoClone("%s.Editor.csproj" % SOURCE_NAME, TARGET_NAME, ".Editor", "", "/Editor/", "x86_64/Editor/")
else:
    csprojName = "Assembly-CSharp-vs.csproj"
    csprojEditorName = "Assembly-CSharp-Editor-vs.csproj"
    if not CSFileUtils.Backup(TARGET_PATH + csprojName):
        print "备份项目文件 %s 失败！！！" % TARGET_PATH + csprojName
    if not CSFileUtils.Backup(TARGET_PATH + csprojEditorName):
        print "备份项目文件 %s 失败！！！" % TARGET_PATH + csprojEditorName
    DoClone(csprojName, "", "/Editor/", "")
    DoClone(csprojEditorName, "-Editor", "", "/Editor/")
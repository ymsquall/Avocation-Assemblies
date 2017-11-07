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
import FileUtils as Files
#import StringUtils as Str

# 设置默认编码为utf8
reload(sys)
sys.setdefaultencoding("utf-8")

SILTEN = False
FileMaps = {}

def FindFileOrDir(fullPath, fn, params):
    nowSourcePath = params[0].replace('/', '\\')
    fullPath = fullPath.replace(fn, '')
    fullPath = fullPath.replace('/', '\\')
    fullPath = fullPath.replace(nowSourcePath, '')
    if fullPath in FileMaps:
        result = FileMaps[fullPath]
        if fn in result:
            raise StandardError("已存在重复项: %s 在资源 %s 中" % path % value)
        result[fn] = nowSourcePath
        if not SILTEN:
            print "找到文件 %s" % fn
    else:
        FileMaps[fullPath] = {fn:nowSourcePath}
        if not SILTEN:
            print "找到目录 %s" % fullPath

def EnumAllFiles(path, nowSourcePath, extName):
    if not os.path.exists(path) or not os.path.isdir(path):
        if not SILTEN:
            print "找不到目录 %s" % path
        return False
    Files.ListFile_DoSomething(path, "", extName, True, FindFileOrDir, (nowSourcePath,))
    return True

def BuildFileAndPathGroup(notInclude = "", include = ""):
    notInclude = notInclude.replace("/", "\\")
    include = include.replace("/", "\\")
    pathGroup = "\n\t<ItemGroup>"
    fileGroup = "\n\t<ItemGroup>"
    for path in FileMaps:
        if include != "" and path.find(include) == -1:
            continue
        if notInclude != "" and path.find(notInclude) != -1:
            continue
        pathGroup = pathGroup + "\n\t\t<Folder Include=\"%s\" />" % path
        files = FileMaps[path]
        for file in files:
            nowSourcePath = files[file]
            fileGroup = fileGroup + "\n\t\t<Compile Include=\"%s%s%s\">" % (nowSourcePath, path, file)
            fileGroup = fileGroup + "\n\t\t\t<Link>%s%s</Link>" % (path, file)
            fileGroup = fileGroup + "\n\t\t</Compile>"
    pathGroup = pathGroup + "\n\t</ItemGroup>"
    fileGroup = fileGroup + "\n\t</ItemGroup>"
    return (pathGroup, fileGroup)

def Backup(projFile):
    if not os.path.exists(projFile) or not os.path.isfile(projFile):
        return False
    bakFileName = projFile + ".bak"
    reader = Files.TextFileReader(projFile)
    newText = reader.buffer
    writer = Files.TextFileWriter(bakFileName)
    writer.write(newText)
    writer.close();
    return True
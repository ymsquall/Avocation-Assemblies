#!/usr/bin/python
#encoding:utf8

import sys
import os

# 设置默认编码为utf8
reload(sys)
sys.setdefaultencoding("utf-8")

LuaProjectTemplateFile = "../../../Lua/LuaScripts/LuaScripts.template"
LuaProjectFile = "../../../Lua/LuaScripts/LuaScripts.luaproj"
LuaSourceParentPath = ("e:/torchlight/master/arpg_prg/client_prg/Assets/", "e:/torchlight/master/arpg_res/")
LuaPathName = ("LuaScripts/", "resource/lua/")

cmd = "python Lua/RebuildLuaproj.py %s %s %s %s %s %s" % (LuaProjectTemplateFile, LuaProjectFile,
                                                    LuaSourceParentPath[0], LuaPathName[0],
                                                    LuaSourceParentPath[1], LuaPathName[1])
os.system(cmd)

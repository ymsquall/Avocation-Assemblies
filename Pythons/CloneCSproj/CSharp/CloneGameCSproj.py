#!/usr/bin/python
#encoding:utf8

import sys
import os

# 设置默认编码为utf8
reload(sys)
sys.setdefaultencoding("utf-8")

SOURCE_NAME = "client_prg"
SOURCE_PATH = "e:/torchlight/master/arpg_prg/"
TARGET_NAME = "client_prg"
TARGET_PATH = "e:/works/Work/Lua/Game/"

cmd = "python CSharp/CloneCSproj.py %s %s %s %s" % (SOURCE_NAME, SOURCE_PATH, TARGET_NAME, TARGET_PATH)
os.system(cmd)

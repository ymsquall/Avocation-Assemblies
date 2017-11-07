#!/usr/bin/python
#encoding:utf8

import sys
import os

# 设置默认编码为utf8
reload(sys)
sys.setdefaultencoding("utf-8")

sys.path.append("./")

SOURCE_NAME = "NGUIEngine"
SOURCE_PATH = "E:/works/Jobs/Unity3D/"
TARGET_NAME = "NGUIEngine"
TARGET_PATH = "E:/works/Jobs/Code/Views/"

cmd = "python CloneCSproj.py %s %s %s %s" % (SOURCE_NAME, SOURCE_PATH, TARGET_NAME, TARGET_PATH)
os.system(cmd)

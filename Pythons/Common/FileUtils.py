# -*- coding:utf-8 -*-
"""
file system operation utilitys
"""

from __future__ import print_function

__author__ = "yueming"

import os

class FileReader(object):
    """basic file reader"""
    def __init__(self, fn, mode):
        with open(fn, mode) as _file:
            self.__buffer = _file.read()

    @property
    def buffer(self):
        """返回文件内容"""
        return self.__buffer

class FileWriter(object):
    """basic file writer"""
    def __init__(self, fn, mode):
        self.__file = open(fn, mode)

    def write_line(self, line):
        if line != None and line != '':
            self.__file.write(line)
        self.__file.write('\n')

    def write(self, text):
        self.__file.write(text)

    def close(self):
        self.__file.close()

class TextFileReader(FileReader):
    """text file reader"""
    def __init__(self, fn):
        super(TextFileReader, self).__init__(fn, "r")

class TextFileWriter(FileWriter):
    """text file writer"""
    def __init__(self, fn):
        super(TextFileWriter, self).__init__(fn, "w")

    def write_md_table_header(self, tabs):
        line = ''
        count = 0
        for i, t in enumerate(tabs):
            if i > 0:
                line += ' | '
            line += t
            i=i+1
            count = count + 1
        self.write_line(line)
        tableFlag = ''
        if count > 0:
            tableFlag += '----'
        while(count > 1):
            tableFlag += '|----'
            count = count - 1
        self.write_line(tableFlag)

def ListFile_DoSomething(path, filename, ext, subs, something, params=()):
    path = os.path.join(path, filename)
    if not os.path.isdir(path):
        if os.path.splitext(path)[1]==ext:
            something(path, filename, params)
    elif subs:
        [ListFile_DoSomething(path, x, ext, subs, something, params) for x in os.listdir(path)]

def ListFileAndDir_DoSomething(path, filename, ext, subs, something, params=()):
    path = os.path.join(path, filename)
    if not os.path.isdir(path):
        if os.path.splitext(path)[1]==ext:
            something(path, filename, params)
    elif subs:
        something(path, filename)
        [ListFile_DoSomething(path, x, ext, subs, something, params) for x in os.listdir(path)]

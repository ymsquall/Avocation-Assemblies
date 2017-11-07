# -*- coding:utf-8 -*-
"xml parser handlers with sax mode"

from __future__ import print_function

__author__ = "yueming"

from xml.parsers.expat import ParserCreate as XmlParser
from XmlNode import XmlNode
import StringUtils as Str

class XmlSaxHandler(object):
    """xml parser handlers with sax mode"""
    def __init__(self, return_unicode, tags, ignore, xml, valueHandler = None):
        self.__caredTags = tags
        self.__ignore = ignore
        self.__rootNode = None
        self.__node = None
        self.__valueHandler = valueHandler
        self.__var_xml_parser = XmlParser()
        self.__var_xml_parser.returns_unicode = return_unicode
        self.__var_xml_parser.StartElementHandler = self.start_element
        self.__var_xml_parser.EndElementHandler = self.end_element
        self.__var_xml_parser.CharacterDataHandler = self.char_data
        self.__var_xml_parser.Parse(xml)

    #@property
    def CaredTags(self):
        return ()

    #@classmethod
    def start_element(self, name, attrs):
        """开始标签"""
        # print('sax:start_element: %s, attrs: %s' % (name, str(attrs)))
        if name in self.__caredTags:
            if not self.__node:
                self.__node = XmlNode(name, attrs)
            else:
                self.__node = self.__node.add_child(name, attrs)
            if not self.__rootNode:
                self.__rootNode = self.__node

    #@classmethod
    def end_element(self, name):
        """结束标签"""
        # print('sax:end_element: %s' % name)
        if self.__node:
            if self.__node.parent:
                self.__node = self.__node.parent

    #@classmethod
    def char_data(self, text):
        """标签内容"""
        # print('sax:char_data: %s' % text)
        if self.__node:
            for ignore in self.__ignore:
                text = Str.replacestr(text, ignore, '')
            if self.__valueHandler:
                text = self.__valueHandler(text)
            if text != '':
                self.__node.add_value(text)

    def node_value_enumerator(self, handler, params):
        if self.__node:
            [handler(value, params) for value in self.__node.values]
            
    def node_childen_enumerator(self, handler, params):
        if self.__node:
            [handler(value, params) for value in self.__rootNode.childen]
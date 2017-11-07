# -*- coding:utf-8 -*-
"""
xml node
"""

class XmlNode(object):
    """表示xml内容中的某一节点"""
    @property
    def parent(self):
        return self.__parent;

    @property
    def values(self):
        return self.__values;

    @property
    def attribute(self):
        return self.__attribute;
    
    @property
    def childen(self):
        return self.__childen;

    def __init__(self, tag, attr):
        self.__tag = tag
        self.__attribute = attr
        self.__values = []
        self.__parent = None
        self.__childen = []

    def add_child(self, tag, attr):
        node = XmlNode(tag, attr)
        node.__parent = self
        self.__childen.append(node)
        return node

    def add_value(self, value):
        self.__values.append(value)




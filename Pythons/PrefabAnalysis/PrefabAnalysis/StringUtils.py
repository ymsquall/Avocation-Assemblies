# -*- coding:utf-8 -*-
"""
some string operation utilitys
"""

def chsstr(text):
    """
    将字符串转为可视的中文文本
    """
    return text.encode("gb18030")

def replacestr(text, old, new):
    text = text.replace(old, new)
    return text
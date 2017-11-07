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
from XmlSaxHandler import XmlSaxHandler as Handler
import StringUtils as Str
import TypeDefines as TypeDef
import FileUtils as Files
import AnalysisResult
# 设置默认编码为utf8
reload(sys)
sys.setdefaultencoding("utf-8")
ARGS_HELPER ="""
请输入正确的参数:
python PrefabAnalysis.py (@path)[@accuracy][@savetofiles].
@path:要扫描的目录,必填参数.
@accuracy:扫描精度（百分比[0.0-100.0]）,可选参数，默认为0最低精度.
@savetofiles:是否将结果保存到文件,可选参数，默认为0不开启.
"""
# 检查参数合法性
if len(sys.argv) < 2:
    print ARGS_HELPER
    os._exit(0)

ROOT_PATH = sys.argv[1]
PERCENT = 0.0
SAVE_TO_FILES = False
if len(sys.argv) > 2:
    PERCENT =  float(sys.argv[2])
if len(sys.argv) > 3:
    SAVE_TO_FILES = bool(sys.argv[3])

if not os.path.isdir(ROOT_PATH):
    ERROR_STR = Str.chsstr("只能输入目录作为第一个参数,您输入的是 %s" % ROOT_PATH)
    raise StandardError(ERROR_STR)

AnalysisResult1 = {}
AnalysisResult2 = {}
AnalysisFinalResult = {}

def DoAnalysis1(value, params):
    """resource => prefab"""
    path = params[0].replace(ROOT_PATH + '\\', '')
    filename = params[1]
    if value in AnalysisResult1:
        result = AnalysisResult1[value]
        result[0] = result[0] + 1
        if path in result[1]:
            raise StandardError("已存在重复项: %s 在资源 %s 中" % path % value)
        result[1].append(path)
    else:
        AnalysisResult1[value] = [1,[path]]
    # pre proc AnalysisResult2
    if path in AnalysisResult2:
        result = AnalysisResult2[path]
        if result.has_key(value):
            raise StandardError("已存在重复项: %s 在资源 %s 中" % path % value)
        result[value] = ['','']
    else:
        AnalysisResult2[path] = { value:['',''] }

def DoAnalysis2(k, v):
    """prefab => resource %"""
    count = AnalysisResult1[v][0]
    total = len(AnalysisResult2)
    percent = '%.02f' % ((count / total) * 100.0)
    return percent, str(count) + "/" + str(total)

def DoReadXml(fullPath, fn):
    reader = Files.TextFileReader(fullPath)
    handler = Handler(True, ("partText",), ('\n', '\t', ' '), reader.buffer, lambda v: v[1:]) # lambda用来去掉首个字符1
    handler.node_value_enumerator(DoAnalysis1, (fullPath, fn))

Files.ListFile_DoSomething(ROOT_PATH, "", ".didi", True, DoReadXml)
#[DoAnalysis2(k, v) for k in AnalysisResult2 for v in AnalysisResult2[k]]

# 保存分析临时结果
AllPrefabFile = []
analysis_temp = None
analysis_final = None
if SAVE_TO_FILES:
    if not os.path.exists('_temp'):
        os.mkdir('_temp')
    if not os.path.exists('AnalysisResult'):
        os.mkdir('AnalysisResult')
    analysis_temp = Files.TextFileWriter(os.path.join('AnalysisResult', 'temp.md'))
    analysis_final = Files.TextFileWriter(os.path.join('AnalysisResult', 'result.txt'))

for kp in AnalysisResult2:
    dic = AnalysisResult2[kp]
    if analysis_temp:
        analysis_temp.write_line('##' + kp)
        analysis_temp.write_md_table_header(('prefab', 'percent', 'count'))

    for k in dic:
        dic[k] = DoAnalysis2(kp, k)
        if analysis_temp:
            analysis_temp.write_line('' + k + ' | ' + dic[k][0] + '% | ' + dic[k][1])
    AllPrefabFile.append(kp)
    
if analysis_temp:
    analysis_temp.close()

TotalCount = len(AllPrefabFile)

# 计算文件相似度
for i, src in enumerate(AllPrefabFile):
    #print Str.chsstr('正在处理文件: %s\t\t' %src + '(' + str(i + 1) + '/' + str(TotalCount) + ')')
    finalResult = []
    filename = src.replace('/', '.')
    filename = filename.replace('\\', '.')
    filename = filename.replace('.didi', '')
    # 写入文件
    analysis_temp = None
    if SAVE_TO_FILES:
        analysis_temp = Files.TextFileWriter(os.path.join('_temp', filename) + '.md')
    #for dst in AllPrefabFile[i + 1:]:
    for dst in AllPrefabFile:
        if src == dst:
            continue
        result = AnalysisResult.AnalysisResult(src, dst)
        srcDict = AnalysisResult2[src]
        dstDict = AnalysisResult2[dst]
        for res in srcDict:
            result.compare_form_src(res, dstDict.has_key(res))
        for res in dstDict:
            result.compare_form_dst(res, srcDict.has_key(res))
        finalResult.append(result)
        if analysis_temp:
            analysis_temp.write_line('##' + src.replace('\\', '/') + "<<==>>" + dst.replace('\\', '/'))
            analysis_temp.write_md_table_header(('src', 'dest'))
        # 资源文件
        for res in result.resdict:
            status = result.resdict[res]
            line = ''
            if status[0]:
                line += '<font color=#00ff00>%s</font> | ' % res
            else:
                line += '<font color=#ff3333>>%s</font> | ' % res
            if status[1]:
                line += '<font color=#00ff00>%s</font>' % res
            else:
                line += '<font color=#ff3333>%s</font>' % res
            if analysis_temp:
                analysis_temp.write_line(line)
        line = ''
        percent_src, count = result.src_percent
        line += str(percent_src) + '% (' + count + ')' + ' | '
        percent_dst, count = result.dst_percent
        line += str(percent_dst) + '% (' + count + ')'
        if analysis_temp:
            analysis_temp.write_line(line)
            analysis_temp.write_line('####----')
        
        src_simple_text = os.path.split(src)
        src_simple_text = src_simple_text[len(src_simple_text) - 1]
        src_simple_text = src_simple_text.replace('.didi', '')
        dst_simple_text = os.path.split(dst)
        dst_simple_text = dst_simple_text[len(dst_simple_text) - 1]
        dst_simple_text = dst_simple_text.replace('.didi', '')

        result_text = ""
        if percent_src >= PERCENT or percent_dst >= PERCENT:
            result_text = "%-30s %-30s %-10s %s"%(src_simple_text, dst_simple_text, percent_src, percent_dst)
        if result_text != '':
            if result_text != '':
                print(result_text)
                if analysis_final:
                    analysis_final.write_line(result_text)

    AnalysisFinalResult[src] = finalResult
    # 写入统计结果
    if analysis_temp:
        analysis_temp.close()
    
if analysis_final:
    analysis_final.close()


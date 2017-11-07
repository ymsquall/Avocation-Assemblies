# -*- coding:utf-8 -*-

class AnalysisResult(object):
    """description of class"""
    def __init__(self, src, dst):
        self.__src = src
        self.__dst = dst
        self.__res = {}

    def __get_count(self, index):
        count = 0
        for k in self.__res:
            v = self.__res[k]
            if v[index]:
                count = count + 1
        return count

    @property
    def resdict(self):
        return self.__res
    @property
    def src_count(self):
        return self.__get_count(0)
    @property
    def dst_count(self):
        return self.__get_count(1)
    @property
    def src_percent(self):
        total = len(self.__res)
        count = self.__get_count(0)
        percent = float('%.02f' % ((float(count) / total) * 100.0))
        return percent, str(count) + '/' + str(total)
    @property
    def dst_percent(self):
        total = len(self.__res)
        count = self.__get_count(1)
        percent = float('%.02f' % ((float(count) / total) * 100.0))
        return percent, str(count) + '/' + str(total)

    def compare_form_src(self, res, exist):
        if self.__res.has_key(res):
            self.__res[res][1] = exist
        else:
            self.__res[res] = [True, exist]

    def compare_form_dst(self, res, exist):
        if self.__res.has_key(res):
            self.__res[res][0] = exist
        else:
            self.__res[res] = [exist, True]

    def __str__(self):
        return super(AnalysisResult, self).__str__()

EasyBinaryFile
==============
[![Build status](https://ci.appveyor.com/api/github/webhook?id=hu87xq6bdcxuvvtr)](https://ci.appveyor.com/project/chinaboard/easybinaryfile)


字符串压缩率测试
String.Length = 1280000
-------zip--------
SmartGzip : True
raw = read : True
zip Time : 32ms
zip Size : 19KB
-------zip--------

------unzip-------
SmartGzip : False
raw = read : True
unzip Time : 5ms
unzip Size : 1250KB
------unzip-------
Test1  End

int型读取写入测试
SmartGzip : False
Count : 100000
write : 368ms
write : 271 IOPS
read : 17ms
read : 5882 IOPS

write = read : True

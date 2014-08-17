EasyBinaryFile
==============

[![Build status](https://ci.appveyor.com/api/projects/status/hu87xq6bdcxuvvtr)](https://ci.appveyor.com/project/chinaboard/easybinaryfile)

本项目不适用于生产环境，但是我自己的项目都在使用该库。
该项目将长期处于alpha状态，并且API部分可能随时会发生变动。


字符串压缩率测试
String.Length = 1280000

zip Time : 32ms

zip Size : 19KB

unzip Time : 5ms

unzip Size : 1250KB


int型读取写入测试

Count : 100000

write : 368ms

write : 271 IOPS

read : 17ms

read : 5882 IOPS


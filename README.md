Project Setup
=============

On Linux:
=========
rhel-like 

'sudo yum install leveldb-devel`

or debian-like:

`sudo apt-get install leveldb-devel`


On source code:
=========
'git clone https://github.com/google/leveldb.git'
'cd leveldb/'
'make'
'sudo scp out-static/lib* out-shared/lib* /usr/local/lib/'
'cd include/'
'sudo scp -r leveldb /usr/local/include/'
'sudo ldconfig'


On Windows:
===========

To build and run locally, you need to clone and build https://github.com/neo-project/leveldb first, 
then copy `libleveldb.dll` to the working directory (i.e. /bin/Debug, /bin/Release)

Note: When building, the project file settings must be changed from static library (lib) to dynamic linked library (dll).

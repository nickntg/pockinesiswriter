# pockinesiswriter
A proof-of-concept kinesis writer written in C#

Runs under Windows and Mono 3.x, the writer reads unified log events from the disk and sends them to Kinesis in batch mode.

The initial version leaves a lot to be desired:
* Debian init.d scripts are included, but the app must be daemonized/turned to a service by using service base (and utilizing mono-service2).
* No throttled disk monitoring implemented.
* No throttled backoff implemented.
* No moving of bad files to another directory.

However:
* It uses the task library to improve throughput and can send thousands of events per second to Kinesis.
* Can accomodate new streams without reconfiguration.
* Compile in Release mode in VS 2013 and xcopy-deploy to Linux. Note the included trustcertificates file.
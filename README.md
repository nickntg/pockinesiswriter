# pockinesiswriter
A proof-of-concept kinesis writer written in C#

Runs under Windows and Mono 3.x, the writer reads unified log events from the disk and sends them to Kinesis in batch mode.

The initial version leaves a lot to be desired:
* No throttled disk monitoring implemented.
* No throttled backoff implemented.

However:
* It uses the task library to improve throughput and can send thousands of events per second to Kinesis.
* Can accomodate new streams without reconfiguration.
* Compile in Release mode in VS 2013 and xcopy-deploy to Linux. Note the included trustcertificates file.
* Can use mono-service to run as a system service using 'sudo service pocsvc start/stop'.
* Can gracefully handle file errors and move invalid files to an error location for troubleshooting.
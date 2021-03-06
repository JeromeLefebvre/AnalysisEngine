# AnalysisEngine

This is a command line application to demonstrate how to calculate AF analyses programmatically.

Please keep in mind that this code was create to illustrate the concept and achieve a specific recalculation task where timestamps can be calculated in the program, not for an extended production use case.

It may require further testings, especially for calculations dependencies, which is something that is not addressed by this program.

#Limitations
Currently, timestamps used to trigger calculations at specific times are generated by the program internally.
To be more accurate, especially for natural calculation, one should get timestamps from input trigger tags.
See this example for details : https://pisquare.osisoft.com/message/28628#28628


#Usage example

    AnalysesEngineCommandLine.exe -s TST-SRV-AF -d AFDB1 -e elementsToRecalc.txt --st 2014-08-15 --et 2015-07-14 --interval hourly --threadsCount 10 --AnalysesThreadsCount 10 -w 45

To enable writing calculation results in the PI Data Archive, use the **--EnableWrite** option.

Instead of writing into PI, you could write into csv files, use the **-f** option to do so.


#Options
  Analyses Calculation Engine Command Line 1.0.5708.31658
  
    -s, --server                  Required. AF Server to connect to
  
    -d, --database                Required. AF Database to use
  
    -e, --elements                Required. full path of the file that contains
                                  the paths of the elements to calculate. The
                                  elements are separated by CRLF.
  
    --st                          Required. Calculation StartTime. i.e 2015-01-01
  
    --et                          Required. Calculation EndTime
  
    --interval                    (Default: hourly) Calculation interval.
                                  options: daily, hourly. default daily
  
    -t, --threadsCount            (Default: 1) Threads Used for elements
                                  collections
  
    -a, --AnalysesThreadsCount    (Default: 2) Threads Used to calculate Analyses
                                  for each element
  
    -w, --DataWriterDelay         (Default: 5) Sets the writing interval for the
                                  DataWriter thread
  
    -f, --outputFile              (Default: ) Name of the file to output data
                                  into.  If specified, data is not written into
                                  PI Data Archive. i.e c:\temp\datafile  or
                                  datafile, .csv extension will be added by the
                                  program
  
    --EnableWrite                 (Default: False) Enables writing calculation
                                  results into PI Data Archive.  if -f option is
                                  specified, writes into text files.  ex:
                                  --EnableWrite
  
    --help                        Display this help screen.


#Elements to recalculate - File format

    #
    #This file contains Elements names which contains the analyses to be (re)calculated
    #
    AnalysesElements\STC_1
    AnalysesElements\STC_2
    

#License

 
    Copyright 2015 Patrice Thivierge Fortin
 
    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at
 
    http://www.apache.org/licenses/LICENSE-2.0
 
    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.
  


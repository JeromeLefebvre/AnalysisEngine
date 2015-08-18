﻿#region Copyright
// /*
// 
//    Copyright 2015 Patrice Thivierge Fortin
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//    http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//  
//  */
#endregion
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AnalysesEngine.Core.Helpers;
using log4net;
using OSIsoft.AF.Asset;
using OSIsoft.AF.Data;

namespace AnalysesEngine.Core.AnalysesLogic
{

    /// <summary>
    /// This class exposes a ConcurrentQueue to make sure information can be gathered smotthly form other threads.
    /// This class is dedicated to write the data generated by calculations into PI.
    /// So Calculation threads does not have to wait for writes to complete before continuing.
    /// </summary>
    public class DataWriter
    {

        private static readonly ILog _logger = LogManager.GetLogger(typeof(DataWriter));
        public static readonly ConcurrentQueue<List<AFValue>> DataQueue = new ConcurrentQueue<List<AFValue>>();

        private CancellationTokenSource _cancellationToken = new CancellationTokenSource();

        public void Run(int writingDelay, string resultsToFile = null)
        {
            Task.Run(() => WriteData(_cancellationToken.Token, writingDelay, resultsToFile));
        }

        public void Stop()
        {
            _cancellationToken.Cancel();
        }

        private void WriteData(CancellationToken cancelToken, int writingDelay, string resultsToFile)
        {
            List<AFValue> values;
            var allValues = new List<AFValue>();

            while (true)
            {
                if (cancelToken.IsCancellationRequested) { return; }

                // gets currently available values from the queue
                while (DataQueue.TryDequeue(out values))
                {
                    allValues.AddRange(values);
                }

                // we go into writing data only if there is data
                if (allValues.Count != 0)
                {

                    _logger.InfoFormat("Data Writer is sorting {0} values to be written", allValues.Count);
                    

                    _logger.InfoFormat("Data Writer values sorting completed. Now Writing...");

                    if (string.IsNullOrEmpty(resultsToFile))
                    {
                        // writing into PI : we sort all the values per timestamp, that will make life easier for the PI Server
                        allValues.Sort();
                        AFListData.UpdateValues(allValues, AFUpdateOption.Replace, AFBufferOption.BufferIfPossible);
                    }
                    else
                    {
                        // writing into a text file: we sort by tagname, then by timestamp.  For readability.
                        // This is a way to check calculation results
                        var sortedvalues=allValues.OrderBy(a => a.PIPoint.Name).ThenBy(a => a.Timestamp).ToList();
                        File.WriteAllLines(resultsToFile + "_" + DateTime.Now.ToIsoReadable() + ".csv", sortedvalues.Select((v) => v.Timestamp.LocalTime.ToIsoReadable() + "," + v.Value + "," + v.Attribute.PIPoint.Name + "," + v.Attribute.Name));
                    }


                    // after values are written, we dont need them anymore
                    allValues.Clear();

                    _logger.InfoFormat("Writing completed, sleeping for now...");
                }
                
                Thread.Sleep(TimeSpan.FromSeconds(writingDelay));

            }

        }


    }
}
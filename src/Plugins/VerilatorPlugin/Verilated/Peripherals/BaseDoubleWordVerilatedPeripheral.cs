﻿//
// Copyright (c) 2010-2022 Antmicro
//
// This file is licensed under the MIT License.
// Full license text is available in 'licenses/MIT.txt'.
//
using System;
using System.Threading;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Antmicro.Renode.Core;
using Antmicro.Renode.Exceptions;
using Antmicro.Renode.Logging;
using Antmicro.Renode.Peripherals.Bus;
using Antmicro.Renode.Peripherals.CPU;
using Antmicro.Renode.Peripherals.Timers;
using Antmicro.Renode.Plugins.VerilatorPlugin.Connection;
using Antmicro.Renode.Plugins.VerilatorPlugin.Connection.Protocols;

namespace Antmicro.Renode.Peripherals.Verilated
{
    public class BaseDoubleWordVerilatedPeripheral : VerilatedPeripheral, IDoubleWordPeripheral
    {
        public BaseDoubleWordVerilatedPeripheral(Machine machine, long frequency, string simulationFilePathLinux = null, string simulationFilePathWindows = null, string simulationFilePathMacOS = null, ulong limitBuffer = LimitBuffer, int timeout = DefaultTimeout, string address = null, int numberOfInterrupts = 0)
            : base(machine, frequency, 32, simulationFilePathLinux, simulationFilePathWindows, simulationFilePathMacOS, limitBuffer, timeout, address, numberOfInterrupts)
        {
        }

        public override uint ReadDoubleWord(long offset)
        {
            if(String.IsNullOrWhiteSpace(simulationFilePath))
            {
                this.Log(LogLevel.Warning, "Cannot read from peripheral. Set SimulationFilePath first!");
                return 0;
            }
            Send(ActionType.ReadFromBus, (ulong)offset, 0);
            var result = Receive();
            CheckValidation(result);

            return (uint)result.Data;
        }

        public override void WriteDoubleWord(long offset, uint value)
        {
            if(String.IsNullOrWhiteSpace(simulationFilePath))
            {
                this.Log(LogLevel.Warning, "Cannot write to peripheral. Set SimulationFilePath first!");
                return;
            }
            Send(ActionType.WriteToBus, (ulong)offset, value);
            CheckValidation(Receive());
        }
    }
}

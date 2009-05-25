// $Id$
using System;
using System.Collections.Generic;
using System.Text;

namespace MacFace
{
    public class MachineStatus
    {
        protected static MachineStatus sharedMachineStatus;

        protected Byte processorCount;

        public static MachineStatus LocalMachineStatus()
        {
            if (sharedMachineStatus == null)
            {
                sharedMachineStatus = new MachineStatus();
            }
            return sharedMachineStatus;
        }

        protected MachineStatus()
        {
            NtKernel.SYSTEM_BASIC_INFORMATION info = NtKernel.QuerySystemBasicInformation();
            processorCount = info.NumberProcessors;
        }

        public int ProcessorCount
        {
            get
            {
                return processorCount;
            }
        }

        public NtKernel.SYSTEM_PROCESSOR_PERFORMANCE_INFORMATION[] ProcessorPerformances()
        {
            return NtKernel.QuerySystemProcessorPerformanceInfomation(processorCount);
        }
    }
}

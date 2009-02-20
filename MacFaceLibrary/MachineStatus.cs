using System;
using System.Collections.Generic;
using System.Text;

namespace MacFace
{
    class MachineStatus
    {
        protected static MachineStatus sharedMachineStatus;

        protected Byte numberProcessors;

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
            numberProcessors = info.NumberProcessors;
        }

        public int NumberProcessors
        {
            get
            {
                return numberProcessors;
            }
        }

    }
}

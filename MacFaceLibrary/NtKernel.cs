using System;
using System.Runtime.InteropServices;

class NtKernel {

    unsafe public static SYSTEM_BASIC_INFORMATION QuerySystemBasicInformation()
    {
        SYSTEM_BASIC_INFORMATION info = new SYSTEM_BASIC_INFORMATION();
        UInt32 len = (UInt32)Marshal.SizeOf(typeof(SYSTEM_BASIC_INFORMATION));
        UInt32 outLen;

        NtQuerySystemInformation(
            SYSTEM_INFORMATION_CLASS.SystemBasicInformation, &info, len, out outLen
        );
        return info;
    }

    unsafe public static SYSTEM_PERFORMANCE_INFORMATION QuerySystemPerformanceInformation()
    {
        SYSTEM_PERFORMANCE_INFORMATION info = new SYSTEM_PERFORMANCE_INFORMATION();
        UInt32 len = (UInt32)Marshal.SizeOf(typeof(SYSTEM_PERFORMANCE_INFORMATION));
        UInt32 outLen;

        NtQuerySystemInformation(
            SYSTEM_INFORMATION_CLASS.SystemPerformanceInformation, &info, len, out outLen
        );
        return info;
    }

    unsafe public static SYSTEM_PROCESSOR_PERFORMANCE_INFORMATION QuerySystemProcessorPerformanceInfomation()
    {
        SYSTEM_PROCESSOR_PERFORMANCE_INFORMATION info = new SYSTEM_PROCESSOR_PERFORMANCE_INFORMATION();
        UInt32 len = (UInt32)(Marshal.SizeOf(typeof(SYSTEM_PROCESSOR_PERFORMANCE_INFORMATION)));
        UInt32 outLen;
        NtQuerySystemInformation(
            SYSTEM_INFORMATION_CLASS.SystemProcessorPerformanceInformation, &info, len, out outLen
        );
        return info;
    }

    [DllImport("ntdll.dll")]
    unsafe protected extern static UInt32 NtQuerySystemInformation(
        SYSTEM_INFORMATION_CLASS SystemInformationClass,
        void* SystemInformation,
        UInt32 SystemInformationLength,
        out UInt32 ReturnLength
    );

    protected enum SYSTEM_INFORMATION_CLASS
    {
	    SystemBasicInformation = 0,
	    SystemPerformanceInformation = 2,
	    SystemTimeOfDayInformation = 3,
	    SystemProcessInformation = 5,
	    SystemProcessorPerformanceInformation = 8,
	    SystemInterruptInformation = 23,
	    SystemExceptionInformation = 33,
	    SystemRegistryQuotaInformation = 37,
	    SystemLookasideInformation = 45
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SYSTEM_BASIC_INFORMATION
    {
        public UInt64 Unknown;
        public UInt64 MaximumIncrement;
        public UInt64 PhysicalPageSize;
        public UInt64 NumberOfPhysicalPages;
        public UInt64 LowestPhysicalPage;
        public UInt64 HighestPhysicalPage;
        public UInt64 AllocationGranularity;
        public UInt64 LowestUserAddress;
        public UInt64 HighestUserAddress;
        public UInt64 ActiveProcessors;
        public Byte NumberProcessors;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SYSTEM_PERFORMANCE_INFORMATION 
    {
	    public Int64  IdleTime;
	    public Int64  ReadTransferCount;
	    public Int64  WriteTransferCount;
	    public Int64  OtherTransferCount;
	    public UInt32 ReadOperationCount;
	    public UInt32 WriteOperationCount;
	    public UInt32 OtherOperationCount;
	    public UInt32 AvailablePages;
	    public UInt32 TotalCommittedPages;
	    public UInt32 TotalCommitLimit;
	    public UInt32 PeakCommitment;
	    public UInt32 PageFaults;
	    public UInt32 WriteCopyFaults;
	    public UInt32 TransitionFaults;
	    public UInt32 CacheTransitionFaults;
	    public UInt32 DemandZeroFaults;
	    public UInt32 PagesRead;
	    public UInt32 PageReadIos;
	    public UInt32 CacheReads;
	    public UInt32 CacheIos;
	    public UInt32 PagefilePagesWritten;
	    public UInt32 PagefilePageWriteIos;
	    public UInt32 MappedFilePagesWritten;
	    public UInt32 MappedFilePageWriteIos;
	    public UInt32 PagedPoolUsage;
	    public UInt32 NonPagedPoolUsage;
	    public UInt32 PagedPoolAllocs;
	    public UInt32 PagedPoolFrees;
	    public UInt32 NonPagedPoolAllocs;
	    public UInt32 NonPagedPoolFrees;
	    public UInt32 TotalFreeSystemPtes;
	    public UInt32 SystemCodePage;
	    public UInt32 TotalSystemDriverPages;
	    public UInt32 TotalSystemCodePages;
	    public UInt32 SmallNonPagedLookasideListAllocateHits;
	    public UInt32 SmallPagedLookasideListAllocateHits;
	    public UInt32 Reserved3;
	    public UInt32 MmSystemCachePage;
	    public UInt32 PagedPoolPage;
	    public UInt32 SystemDriverPage;
	    public UInt32 FastReadNoWait;
	    public UInt32 FastReadWait;
	    public UInt32 FastReadResourceMiss;
	    public UInt32 FastReadNotPossible;
	    public UInt32 FastMdlReadNoWait;
	    public UInt32 FastMdlReadWait;
	    public UInt32 FastMdlReadResourceMiss;
	    public UInt32 FastMdlReadNotPossible;
	    public UInt32 MapDataNoWait;
	    public UInt32 MapDataWait;
	    public UInt32 MapDataNoWaitMiss;
	    public UInt32 MapDataWaitMiss;
	    public UInt32 PinMappedDataCount;
	    public UInt32 PinReadNoWait;
	    public UInt32 PinReadWait;
	    public UInt32 PinReadNoWaitMiss;
	    public UInt32 PinReadWaitMiss;
	    public UInt32 CopyReadNoWait;
	    public UInt32 CopyReadWait;
	    public UInt32 CopyReadNoWaitMiss;
	    public UInt32 CopyReadWaitMiss;
	    public UInt32 MdlReadNoWait;
	    public UInt32 MdlReadWait;
	    public UInt32 MdlReadNoWaitMiss;
	    public UInt32 MdlReadWaitMiss;
	    public UInt32 ReadAheadIos;
	    public UInt32 LazyWriteIos;
	    public UInt32 LazyWritePages;
	    public UInt32 DataFlushes;
	    public UInt32 DataPages;
	    public UInt32 ContextSwitches;
	    public UInt32 FirstLevelTbFills;
	    public UInt32 SecondLevelTbFills;
	    public UInt32 SystemCalls;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SYSTEM_PROCESSOR_PERFORMANCE_INFORMATION
    {
        public Int64 IdleTime;
        public Int64 KernelTime;
        public Int64 UserTime;
        public Int64 Reserved1_0;
        public Int64 Reserved1_1;
        public Int32 Reserved2;
    }
}

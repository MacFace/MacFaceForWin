/*
 * $Id$
 */
using System;

namespace MacFace
{
	/// <summary>
	/// HostStatistics の概要の説明です。
	/// </summary>
	public class HostStatistics
	{
		public HostStatistics()
		{
			// 
			// TODO: コンストラクタ ロジックをここに追加してください。
			//
		}
	}
}

/*

typedef struct {
    int freePages;
    int activePages;
    int inactivePages;
    int wirePages;
    int faults;
    int pageins;
    int pageouts;
    unsigned long userTicks;
    unsigned long systemTicks;
    unsigned long idleTicks;
    unsigned long niceTicks;
} HostStatData;

typedef struct {
    int incount;
    int outcount;
} Pageio;

typedef struct {
    float user;
    float system;
    float idle;
    float nice;
} CPUUsage;

typedef struct {
    HostStatData stat;
    Pageio pageio;
    CPUUsage usage;
} StatisticsRecord;

@interface HostStatistics : NSObject
{
    StatisticsRecord *ringBuffer;
    int bufMaxLen;
    int bufHead;
    int bufTail;
    int bufLen;
    int totalPages;
    int minUsedPages;
    int maxUsedPages;
}

+ (unsigned int)vmPageSize;
+ (NSString*)kernelVersion;
+ (void)getStatistics:(HostStatData*)data;

- (id)initWithCapacity:(unsigned)capacity;
- (void)update;
- (int)totalPages;
- (int)minUsedPages;
- (int)maxUsedPages;
- (int)length;
- (const StatisticsRecord*)indexAt:(unsigned)index;
- (const StatisticsRecord*)head;
- (const StatisticsRecord*)tail;
@end

#import <mach/mach.h>
#import <mach/mach_types.h>
#import "HostStatistics.h"

//
//
static void hostStatistics(HostStatData *data)
{
    vm_statistics_data_t vm_stat;
    mach_msg_type_number_t vm_count = HOST_VM_INFO_COUNT;
    host_cpu_load_info_data_t load_info;
    mach_msg_type_number_t load_info_count = HOST_CPU_LOAD_INFO_COUNT;
    host_t host = mach_host_self();

    host_statistics(host,HOST_VM_INFO,(host_info_t)&vm_stat, &vm_count);
    host_statistics(host,HOST_CPU_LOAD_INFO,(host_info_t)&load_info, &load_info_count);

    data->freePages = vm_stat.free_count;
    data->activePages = vm_stat.active_count;
    data->inactivePages = vm_stat.inactive_count;
    data->wirePages = vm_stat.wire_count;
    data->faults = vm_stat.faults;
    data->pageins = vm_stat.pageins;
    data->pageouts = vm_stat.pageouts;

    data->userTicks = load_info.cpu_ticks[CPU_STATE_USER];
    data->systemTicks = load_info.cpu_ticks[CPU_STATE_SYSTEM];
    data->idleTicks = load_info.cpu_ticks[CPU_STATE_IDLE];
    data->niceTicks = load_info.cpu_ticks[CPU_STATE_NICE];
}

@implementation HostStatistics

//
// 仮想記憶のページサイズを返す
//
+ (unsigned int)vmPageSize
{
    vm_size_t page_size;
    host_page_size(mach_host_self(),&page_size);
    return page_size;
}

//
// カーネルのバージョン文字列を返す
//
+ (NSString*)kernelVersion
{
    kernel_version_t kver;
    host_kernel_version(mach_host_self(),kver);
    return [NSString stringWithUTF8String:kver];
}

//
// 現在の統計情報を設定する
//
+ (void)getStatistics:(HostStatData*)data
{
    hostStatistics(data);
}

//
// 初期化
//   capacity: リングバッファの容量
//
- (id)initWithCapacity:(unsigned)capacity
{
    StatisticsRecord *rec;

    ringBuffer = calloc(sizeof(StatisticsRecord),capacity);
    rec = &ringBuffer[0];
    hostStatistics(&rec->stat);
    rec->usage.user = 0;
    rec->usage.system = 0;
    rec->usage.idle = 100.0;
    rec->usage.nice = 0;

    bufMaxLen = capacity;
    bufHead = 0;
    bufTail = 0;
    bufLen = 1;

    totalPages = rec->stat.wirePages + rec->stat.activePages + rec->stat.inactivePages + rec->stat.freePages;
    minUsedPages = rec->stat.wirePages + rec->stat.activePages;
    maxUsedPages = minUsedPages;

    return self;
}

//
// 後始末
//
- (void)dealloc
{
    free(ringBuffer);
    [super dealloc];
}

//
// 履歴の更新
//
- (void)update
{
    StatisticsRecord *rec, *lastRec;
    int user,sys,idle,nice,total;
    int usedPages;

    lastRec = &ringBuffer[bufHead];
    if (bufLen < bufMaxLen) bufLen++;
    if (++bufHead >= bufLen) bufHead = 0;
    if (bufHead == bufTail)
        if (++bufTail >= bufLen) bufTail = 0;
    rec = &ringBuffer[bufHead];
    hostStatistics(&rec->stat);

    rec->pageio.incount = rec->stat.pageins - lastRec->stat.pageins;
    rec->pageio.outcount = rec->stat.pageouts - lastRec->stat.pageouts;

    user = rec->stat.userTicks - lastRec->stat.userTicks;
    sys = rec->stat.systemTicks - lastRec->stat.systemTicks;
    idle = rec->stat.idleTicks - lastRec->stat.idleTicks;
    nice = rec->stat.niceTicks - lastRec->stat.niceTicks;
    total = user + sys + idle + nice;

    if (total > 0) {
        rec->usage.user = user * 100.0 / total;
        rec->usage.system = sys * 100.0 / total;
        rec->usage.idle = idle * 100.0 / total;
        rec->usage.nice = nice * 100.0 / total;
    } else {
        rec->usage = lastRec->usage;
    }

    usedPages = rec->stat.wirePages + rec->stat.activePages;
    if( minUsedPages < usedPages) minUsedPages = usedPages;
    if( maxUsedPages > usedPages) maxUsedPages = usedPages;
}

- (int)totalPages { return totalPages; }
- (int)minUsedPages { return minUsedPages; }
- (int)maxUsedPages { return maxUsedPages; }
- (int)length { return bufLen; }

//
// 指定した位置の統計情報を得る
//   index: 位置(最新が0)
//
- (const StatisticsRecord*)indexAt:(unsigned)index;
{
    if (index >= bufLen) return nil;
    index = (index <= bufHead) ? bufHead - index : bufLen + bufHead - index;
    return &ringBuffer[index];
}

//
// もっとも新しい統計情報を返す
//
- (const StatisticsRecord*)head
{
    return &ringBuffer[bufHead];
}

//
// もっとも古い統計情報を返す
//
- (const StatisticsRecord*)tail;
{
    return &ringBuffer[bufTail];
}

@end

*/
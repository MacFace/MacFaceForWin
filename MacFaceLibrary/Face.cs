using System;

namespace MacFace
{
	/// <summary>
	/// Face の概要の説明です。
	/// </summary>
	public class Face
	{
		public Face()
		{
			// 
			// TODO: コンストラクタ ロジックをここに追加してください。
			//
		}
	}
}

/*
#import <Cocoa/Cocoa.h>
#import "HostStatistics.h"
#import "FaceDef.h"

@interface Face : NSObject
{
    FaceDef *faceDef;
    NSImage *image;
    int health;
    int activity;
    int pageout;
    unsigned marker;
    NSDate *pageoutDate;
}

- (id)initWithDefinition:(FaceDef*)def;

- (int)activity;
- (int)health;
- (NSImage*)image;
- (FaceDef*)definition;
- (void)setDefinition:(FaceDef*)definition;

- (void)update:(const StatisticsRecord*)record;

@end

@implementation Face

- (id)initWithDefinition:(FaceDef*)def
{
    [super init];
    faceDef = [def retain];
    activity = 0;
    health = 0;
    image = [[NSImage alloc] initWithSize:NSMakeSize(128,128)];
    return self;
}

- (void)dealloc
{
    [faceDef release];
    [image release];
    if (pageoutDate != nil) [pageoutDate release];
    [super dealloc];
}

- (int)activity { return activity; }
- (int)health { return health; }
- (NSImage*)image { return image; }

- (FaceDef*)definition
{
    return faceDef;
}

- (void)setDefinition:(FaceDef*)definition
{
    [faceDef autorelease];
    faceDef = [definition retain];

    [image lockFocus];
    [[NSColor clearColor] set];
    NSRectFill(NSMakeRect(0,0,128,128));
    [faceDef drawImageOfRow:health col:activity marker:marker atPoint:NSMakePoint(0,0)];
    [image unlockFocus];
}

- (void)update:(const StatisticsRecord*)record
{
    activity = (int)(100 - record->usage.idle) / 10;
    pageout = pageout*0.97 + record->pageio.outcount;

    if (record->pageio.outcount > 0) {
        if (pageoutDate != nil) [pageoutDate release];
        pageoutDate = [[NSDate alloc] init];
    }

    if (pageout > 40) {
        health = 2;
    } else if (pageoutDate && [pageoutDate timeIntervalSinceNow] > -15.0*60){
        health = 1;
    } else {
        health = 0;
    }

    marker = 0;
    if (record->pageio.outcount) {
        marker |= FDMARKER_PAGEOUT;
    }
    if (record->pageio.incount) {
        marker |= FDMARKER_PAGEIN;
    }

    [image lockFocus];
    [[NSColor clearColor] set];
    NSRectFill(NSMakeRect(0,0,128,128));
    [faceDef drawImageOfRow:health col:activity marker:marker atPoint:NSMakePoint(0,0)];
    [image unlockFocus];
}

@end

*/
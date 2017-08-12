#include "all.h"

#define COMMON_ID "{8cf92c5e-c4f7-4867-9e1a-5371bb53aa63}" // shared_uuid@g

// 対岸では SEND_IDENT, RECV_IDENT 逆になるよ。

// app >

#define SEND_IDENT "{4155c94a-b0aa-4738-bf59-b444b755ed81}" // shared_uuid
#define RECV_IDENT "{a86e1bc6-907e-4c08-9633-4bf6f61c2b4f}" // shared_uuid

// < app

#define SEND_SIZE_MAX 2000 // 2 KB
#define SEND_NUM_MAX 1000
#define RECV_SIZE_MAX 2000 // 2 KB
#define RECV_NUM_MAX 1000

// delimiter == 0x00 固定！

enum
{
	E_SEND,
	E_RECV,
	E_BIT_0,
	E_BIT_1,
	E_BIT_2,
	E_BIT_3,
	E_BIT_4,
	E_BIT_5,
	E_BIT_6,
	E_BIT_7,

	E_MAX, // == num of E_*
};

static int SendEvs[E_MAX];
static int RecvEvs[E_MAX];
static autoQueue<char *> *SendQueue;
static autoQueue<char *> *RecvQueue;
static char *SendBuff;
static char *RecvBuff;
static int SendIndex;
static int RecvIndex;
static int SendWaitCount;

static void FNLZ(void)
{
	for(int index = 0; index < E_MAX; index++)
	{
		handleClose(SendEvs[index]);
		handleClose(RecvEvs[index]);
	}
}
void Nectar2_INIT(void)
{
	for(int index = 0; index < E_MAX; index++)
	{
		char *sendName = xcout("Nectar2_" COMMON_ID "_" SEND_IDENT "_%d", index);
		char *recvName = xcout("Nectar2_" COMMON_ID "_" RECV_IDENT "_%d", index);

		SendEvs[index] = eventOpen(sendName);
		RecvEvs[index] = eventOpen(recvName);

		memFree(sendName);
		memFree(recvName);
	}
	SendQueue = new autoQueue<char *>();
	RecvQueue = new autoQueue<char *>();
	SendBuff = (char *)memAlloc(SEND_SIZE_MAX + 1);
	RecvBuff = (char *)memAlloc(RECV_SIZE_MAX + 1);
	SendIndex = -1;
//	RecvIndex = 0;
//	SendWaitCount = 0;

	GetFinalizers()->AddFunc(FNLZ);
}
static int Get(int ev)
{
	return waitForMillis(ev, 0);
}
static void Set(int ev)
{
	eventSet(ev);
}
void Nectar2EachFrame(void)
{
	if(m_countDown(SendWaitCount))
	{
		if(!Get(SendEvs[E_RECV]))
			goto BEGIN_RECV;

		SendWaitCount = 0;
	}
	if(SendIndex == -1)
	{
		char *message = SendQueue->Dequeue(NULL);

		if(message)
		{
			strcpy(SendBuff, message);
			memFree(message);
			SendIndex = 0;
			goto DO_SEND;
		}
	}
	else
	{
DO_SEND:
		int chr = SendBuff[SendIndex]; // 終端の '\0' も送る。

		for(int bit = 0; bit < 8; bit++)
			if(chr & (1 << bit))
				Set(SendEvs[E_BIT_0 + bit]);

		Get(SendEvs[E_RECV]); // clear
		Set(SendEvs[E_SEND]);
		SendWaitCount = 900; // 15 sec

		if(chr)
			SendIndex++;
		else
			SendIndex = -1;
	}
BEGIN_RECV:
	if(Get(RecvEvs[E_SEND]))
	{
		int chr = 0;

		for(int bit = 0; bit < 8; bit++)
			if(Get(RecvEvs[E_BIT_0 + bit]))
				chr |= 1 << bit;

		Set(RecvEvs[E_RECV]);

		if(chr)
		{
			if(RecvIndex < RECV_SIZE_MAX)
			{
				RecvBuff[RecvIndex] = chr;
				RecvIndex++;
			}
		}
		else
		{
			RecvBuff[RecvIndex] = '\0';

			if(RecvQueue->GetCount() < RECV_NUM_MAX)
			{
				RecvQueue->Enqueue(strx(RecvBuff));
			}
			RecvIndex = 0;
		}
	}
}
void Nectar2Send_x(char *message)
{
	errorCase(!message);
	errorCase(SEND_SIZE_MAX < strlen(message));

	Pub_AddInstantMessage_x(xcout("<%s", message));

	if(SendQueue->GetCount() < SEND_NUM_MAX)
	{
		SendQueue->Enqueue(message);
	}
}
void Nectar2Send(char *message)
{
	errorCase(!message);

	Nectar2Send_x(strx(message));
}
char *Nectar2Recv(void) // ret: NULL == no data
{
	return RecvQueue->Dequeue(NULL);
}
char *c_Nectar2Recv(void) // ret: NULL == no data
{
	char *message = Nectar2Recv();

	if(message)
	{
		static char *stock;
		memFree(stock);
		stock = message;
	}
	return message;
}
int Nectar2IsRecvJam(void)
{
	return RECV_NUM_MAX / 2 < RecvQueue->GetCount();
}

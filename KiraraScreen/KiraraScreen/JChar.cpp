#include "all.h"

/*
	JChar.bin
		CP932
		但し、半角スペースを含まない。(ASCII・半角カナを含む)
*/
static bitList *GetBitList(void)
{
	static bitList *list;

	if(!list)
	{
		autoList<uchar> *fileData = GetEtcRes()->GetHandle(ETC_JCHAR);
		autoList<uint> *listData = new autoList<uint>(fileData->GetCount() / 4);

		for(int index = 0; index < fileData->GetCount(); index += 4)
		{
			listData->AddElement(
				(uint)fileData->GetElement(index + 0) <<  0 |
				(uint)fileData->GetElement(index + 1) <<  8 |
				(uint)fileData->GetElement(index + 2) << 16 |
				(uint)fileData->GetElement(index + 3) << 24
				);
		}
		list = new bitList();
#if 1
		list->SetList_DIRECT(listData);
#else // old
		list->Overwrite_DIRECT(listData);
		delete listData;
#endif

		GetEtcRes()->UnloadAllHandle(); // 粗大ゴミ開放
	}
	return list;
}
int isJChar(int chr)
{
	return m_isRange(chr, 1, 0xffff) && GetBitList()->RefBit(chr);
}
int isJChar(char *pChr) // ret: 0 == not JChar, 1-2 == バイト数
{
	if(isJChar(*pChr))
		return 1;

	if(*pChr && isJChar(*pChr << 8 | pChr[1]))
		return 2;

	return 0;
}

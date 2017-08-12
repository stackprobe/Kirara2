#include "all.h"

static autoList<char *> *GetDatStrings(void)
{
	static autoList<char *> *datStrings;

	if(!datStrings)
	{
		autoList<uchar> *fileData = GetEtcRes()->GetHandle(ETC_DATSTRINGS);
		int rIndex = 0;

		datStrings = new autoList<char *>();

		for(int index = 0; index < DATSTR_MAX; index++)
		{
			char *datString = readLine(fileData, rIndex);
			errorCase(!datString); // ? �s�����Ȃ��B

			datStrings->AddElement(datString);
		}
		errorCase(readLine(fileData, rIndex)); // ? �s�������B

		GetEtcRes()->UnloadAllHandle(); // �e��S�~�J��
	}
	return datStrings;
}
char *GetDatString(int datStrId)
{
	errorCase(datStrId < 0 || DATSTR_MAX <= datStrId);

	return GetDatStrings()->GetElement(datStrId);
}

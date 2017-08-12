#include "all.h"

char *xcout(char *format, ...)
{
	char *buffer;
	va_list marker;

	va_start(marker, format);

	for(int size = strlen(format) + 128; ; size *= 2)
	{
		errorCase(IMAX < size);

		buffer = (char *)memAlloc(size + 20);
		int retval = _vsnprintf(buffer, size + 10, format, marker);
		buffer[size + 10] = '\0';

		if(0 <= retval && retval <= size)
			break;

		memFree(buffer);
	}
	va_end(marker);

	return buffer;
}
char *strx(char *line)
{
	return (char *)memClone(line, strlen(line) + 1);
}
void strz(char *&buffer, char *line)
{
	memFree(buffer);
	buffer = strx(line);
}
void strz_x(char *&buffer, char *line)
{
	memFree(buffer);
	buffer = line;
}
int atoi_x(char *line)
{
	int value = atoi(line);
	memFree(line);
	return value;
}
__int64 atoi64_x(char *line)
{
	__int64 value = _atoi64(line);
	memFree(line);
	return value;
}

char *mbs_strrchr(char *str, int chr)
{
	char *ret = NULL;

	for(char *p = str; *p; p = mbsNext(p))
		if(*p == chr)
			ret = p;

	return ret;
}

static int ReplacedFlag;

void replaceChar(char *str, int srcChr, int destChr) // mbs_
{
	for(char *p = str; *p; p = mbsNext(p))
		if(*p == srcChr)
			*p = destChr;
}
char *replace(char *str, char *srcPtn, char *destPtn) // ret: strr()
{
	autoList<char> *buff = new autoList<char>();
	int srcPtnLen = strlen(srcPtn);
	int destPtnLen = strlen(destPtn);

	errorCase(srcPtnLen < 1);
	ReplacedFlag = 0;

	for(char *p = str; *p; )
	{
		if(!strncmp(p, srcPtn, srcPtnLen))
		{
			buff->AddElements(destPtn, destPtnLen);
			ReplacedFlag = 1;
			p += srcPtnLen;
		}
		else
		{
			buff->AddElement(*p);
			p++;
		}
	}
	memFree(str);

	buff->AddElement('\0');
	char *ret = buff->UnbindBuffer();
	delete buff;
	return ret;
}
char *replaceLoop(char *str, char *srcPtn, char *destPtn, int max) // ret: strr()
{
	for(int c = 0; c < max; c++)
	{
		str = replace(str, srcPtn, destPtn);

		if(!ReplacedFlag)
			break;
	}
	return str;
}

char *combine(char *path1, char *path2)
{
	char *path = xcout("%s\\%s", path1, path2);

	replaceChar(path, '\\', '/');
	path = replaceLoop(path, "//", "/", 10);
	replaceChar(path, '/', '\\');

	return path;
}

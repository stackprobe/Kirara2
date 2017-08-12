char *xcout(char *format, ...);
char *strx(char *line);
void strz(char *&buffer, char *line);
void strz_x(char *&buffer, char *line);
int atoi_x(char *line);
__int64 atoi64_x(char *line);

#define isMbc(p) \
	(_ismbblead((p)[0]) && (p)[1])
//	(_ismbblead((p)[0]) && _ismbbtrail((p)[1]))

#define mbsNext(p) \
	(p + (isMbc(p) ? 2 : 1))

char *mbs_strrchr(char *str, int chr);

void replaceChar(char *str, int srcChr, int destChr);
char *replace(char *str, char *srcPtn, char *destPtn);
char *replaceLoop(char *str, char *srcPtn, char *destPtn, int max);

char *combine(char *path1, char *path2);

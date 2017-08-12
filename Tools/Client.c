#include "C:\Factory\Common\all.h"
#include "C:\Factory\SubTools\libs\Nectar2.h"

#define MEDIA_DIR_ID "{80b5a52a-7cc7-4875-9980-452b0aa95fac}" // shared_uuid

static char *MediaDir;
static char *SaveDataFile;

// �Ί݂ł� SEND_IDENT, RECV_IDENT �t�ɂȂ��B

#define SEND_IDENT "{a86e1bc6-907e-4c08-9633-4bf6f61c2b4f}" // shared_uuid
#define RECV_IDENT "{4155c94a-b0aa-4738-bf59-b444b755ed81}" // shared_uuid

static Nectar2_t *Sender;
static Nectar2_t *Recver;

#define FFPROBE_EXE_FILE "C:\\app\\ffmpeg-3.2.4-win64-shared\\bin\\ffprobe.exe"

typedef struct MediaInfo_st
{
	uint FileId;
	int Kind; // "AV"
	uint Time; // �~���b
	uint W;
	uint H;
}
MediaInfo_t;

static autoList_t *MediaInfos; // { MediaInfo_t * } ...

#define SCREEN_W 800
#define SCREEN_H 600

typedef struct Rect_st
{
	uint L;
	uint T;
	uint R;
	uint B;
	uint W;
	uint H;
}
Rect_t;

static Rect_t *MkRect(uint l, uint t, uint w, uint h)
{
	Rect_t rect;

	rect.L = l;
	rect.T = t;
	rect.R = l + w;
	rect.B = t + h;
	rect.W = w;
	rect.H = h;

	return (Rect_t *)memClone(&rect, sizeof(Rect_t));
}
static autoList_t *GetMonitors(void)
{
	static autoList_t *list;

	if(!list)
	{
		list = newList();
		addElement(list, (uint)MkRect(0, 0, 1280, 1024));
		addElement(list, (uint)MkRect(1280, 0, 1280, 1024));
	}
	return list;
}
static char *GetTokenLX(char *line, char *format)
{
	autoList_t *tokens = tokenize(line, ' ');
	char *token;
	uint index;
	char *ret = NULL;

	foreach(tokens, token, index)
	{
		if(lineExp(format, token))
		{
			ret = strx(token);
			break;
		}
	}
	errorCase(!ret);
	releaseDim(tokens, 1);
	return ret;
}
static MediaInfo_t *MkMedia(char *file, uint index)
{
	MediaInfo_t mi;
	char *redFile = makeTempPath(NULL);

	coExecute_x(xcout(FFPROBE_EXE_FILE " \"%s\" 2> \"%s\"", file, redFile));

	{
		autoList_t *lines = readLines(redFile);
		char *line;
		uint line_index;

		mi.FileId = index + 1;
		mi.Kind = 'A';
		mi.W = 1;
		mi.H = 1;

		foreach(lines, line, line_index)
		{
			if(strstr(line, "Duration:"))
			{
				char *token = GetTokenLX(line, "<2,09>:<2,09>:<2,09>.<2,09>,");
				uint h, m, s, i;

				token[2] = '\0';
				token[5] = '\0';
				token[8] = '\0';
				token[11] = '\0';

				h = toValue(token);
				m = toValue(token + 3);
				s = toValue(token + 6);
				i = toValue(token + 9);

				mi.Time =
					h * 3600000 +
					m * 60000 +
					s * 1000 +
					i * 10;

				memFree(token);
			}
			if(strstr(line, "Stream #") && strstr(line, "Video:"))
			{
				char *token = GetTokenLX(line, "<09>x<09>");
				char *p;

				p = ne_strchr(token, 'x');
				*p = '\0';
				p++;

				mi.Kind = 'V';
				mi.W = toValue(token);
				mi.H = toValue(p);

				memFree(token);
			}
		}
		releaseDim(lines, 1);
	}

	removeFile_x(redFile);

	cout("FileId: %u\n", mi.FileId);
	cout("Kind: %c\n", mi.Kind);
	cout("Time: %u\n", mi.Time);
	cout("W: %u\n", mi.W);
	cout("H: %u\n", mi.H);

	errorCase(!m_isRange(mi.Time, 1, IMAX));
	errorCase(!m_isRange(mi.W, 1, IMAX));
	errorCase(!m_isRange(mi.H, 1, IMAX));

	{
		char *destFile = combine_cx(MediaDir, xcout("%010u.og%c", mi.FileId, mi.Kind == 'A' ? 'g' : 'v'));

		copyFile(file, destFile);

		memFree(destFile);
	}

	return (MediaInfo_t *)memClone(&mi, sizeof(MediaInfo_t));
}
static void LoadMediaFiles(char *listFile)
{
	autoList_t *files = readLines(listFile);
	char *file;
	uint index;

	MediaInfos = newList();

	foreach(files, file, index)
	{
		addElement(MediaInfos, (uint)MkMedia(file, index));
	}
	releaseDim(files, 1);
}

static int NoBootScreen;

static void BootScreen(void)
{
	if(NoBootScreen)
		return;

	addCwd(getSelfDir());
	addCwd("..\\KiraraScreen\\Release");
	execute("START /HIGH KiraraScreen.exe AMANOGAWA");
	unaddCwd();
	unaddCwd();
}
static void DoSend(char *line)
{
	BootScreen();

	Nectar2SendLine(Sender, line);
	Nectar2SendChar(Sender, 0x00);
}
static void DoSend_x(char *line)
{
	BootScreen();

	Nectar2SendLine_x(Sender, line);
	Nectar2SendChar(Sender, 0x00);
}

static uint PlayingIndex = IMAX; // IMAX == ���Đ�
static uint LastPlayedIndex;

static MediaInfo_t *Maximize_Mi;

static double SeekRate;

/*
	�����l�K��
*/
static uint Screen_L = 100;
static uint Screen_T = 100;
static uint Screen_W = 800;
static uint Screen_H = 600;
static uint Volume = IMAX; // 0 �` IMAX == vol_min �` vol_max

static void DoPlay(uint index)
{
	MediaInfo_t mi;

	errorCase(!m_isRange(index, 0, getCount(MediaInfos) - 1));

	PlayingIndex = index;
	LastPlayedIndex = index;

	mi = *(MediaInfo_t *)getElement(MediaInfos, PlayingIndex);

	if(mi.Kind == 'A')
	{
		DoSend("F");
		DoSend("+");
		DoSend_x(xcout("I%u", mi.FileId));
		DoSend_x(xcout("t%u", mi.Time));
		DoSend("B");
	}
	else // 'V'
	{
		DoSend("-");
		DoSend("F");
		DoSend_x(xcout("I%u", mi.FileId));
		DoSend_x(xcout("W%u", mi.W));
		DoSend_x(xcout("H%u", mi.H));
		DoSend("T0");
		DoSend_x(xcout("t%u", mi.Time));
		DoSend("P");
	}
}
static void RecvedEvent(char *line)
{
	// ---- �ő剻 ----

	if(!strcmp(line, "M"))
	{
		int moviePlaying = 0;

		if(PlayingIndex != IMAX)
		{
			MediaInfo_t *mi = (MediaInfo_t *)getElement(MediaInfos, PlayingIndex);

			if(mi->Kind == 'V')
			{
				moviePlaying = 1;
				Maximize_Mi = mi;
			}
		}
		if(moviePlaying)
		{
			DoSend("C");
			DoSend("D");
		}
		DoSend("R");
		DoSend("EM-2");

		if(moviePlaying)
			DoSend("EM-3");
		else
			DoSend("EM-3.2");

		return;
	}
	if(startsWith(line, "Curr="))
	{
		SeekRate = (double)toValue(line + 5) / IMAX;
		return;
	}
	if(startsWith(line, "Rect="))
	{
		autoList_t *tokens = tokenize(line + 5, ',');

		Screen_L = toValue(getLine(tokens, 0));
		Screen_T = toValue(getLine(tokens, 1));
		Screen_W = toValue(getLine(tokens, 2));
		Screen_H = toValue(getLine(tokens, 3));

		releaseDim(tokens, 1);
		return;
	}
	if(!strcmp(line, "M-2"))
	{
		Rect_t *rect;
		uint index;

		foreach(GetMonitors(), rect, index)
		{
			if(
				Screen_L == rect->L &&
				Screen_T == rect->T &&
				Screen_W == rect->W &&
				Screen_H == rect->H
				)
			{
				uint l = rect->L + (rect->W - SCREEN_W) / 2;
				uint t = rect->T + (rect->H - SCREEN_H) / 2;
				uint w = SCREEN_W;
				uint h = SCREEN_H;

				DoSend_x(xcout("L%u", l));
				DoSend_x(xcout("Y%u", t));
				DoSend_x(xcout("W%u", w));
				DoSend_x(xcout("H%u", h));
				DoSend("M");
				return;
			}
		}
		foreach(GetMonitors(), rect, index)
		{
			if(
				Screen_L < rect->R &&
				Screen_T < rect->B &&
				rect->L < (Screen_L + Screen_W) &&
				rect->T < (Screen_T + Screen_H)
				)
				break;
		}
		if(!rect)
			rect = (Rect_t *)getElement(GetMonitors(), 0);

		DoSend_x(xcout("L%u", rect->L));
		DoSend_x(xcout("Y%u", rect->T));
		DoSend_x(xcout("W%u", rect->W));
		DoSend_x(xcout("H%u", rect->H));
		DoSend("M");
		return;
	}
	if(!strcmp(line, "M-3"))
	{
		MediaInfo_t *mi = Maximize_Mi;
		uint startTime;

		errorCase(!mi);
		errorCase(mi->Kind != 'V');

		startTime = d2i(SeekRate * mi->Time);
		m_range(startTime, 0, mi->Time - 2000); // �Q�b�̗]�T <- ����̒�����蒷���ƕs����ɂȂ�B

		DoSend_x(xcout("I%u", mi->FileId));
		DoSend_x(xcout("W%u", mi->W));
		DoSend_x(xcout("H%u", mi->H));
		DoSend_x(xcout("T%u", startTime));
		DoSend_x(xcout("t%u", mi->Time));
		DoSend("P");
		return;
	}
	if(!strcmp(line, "M-3.2"))
	{
		DoSend("+");
		return;
	}

	// ---- �A���Đ� ----

	if(PlayingIndex != IMAX)
	{
		MediaInfo_t *mi = (MediaInfo_t *)getElement(MediaInfos, PlayingIndex);

		if(mi->Kind == 'A' ? !strcmp(line, "B") : !strcmp(line, "R"))
		{
			DoPlay((PlayingIndex + 1) % getCount(MediaInfos));
			return;
		}
	}

	// ---- �Đ��{�^�� ----

	if(!strcmp(line, "S"))
	{
		if(PlayingIndex != IMAX) // ? �Đ���
		{
			PlayingIndex = IMAX;

			DoSend("F");
			DoSend("+");
		}
		else // ? ��~��
		{
			DoPlay(LastPlayedIndex);
		}
		return;
	}

	// ---- �V�[�N�o�[���� ----

	if(startsWith(line, "Seek="))
	{
		if(PlayingIndex != IMAX)
		{
			MediaInfo_t *mi = (MediaInfo_t *)getElement(MediaInfos, PlayingIndex);

			if(mi->Kind == 'V')
			{
				double rate = (double)toValue(line + 5) / IMAX;
				uint startTime;

				startTime = d2i(rate * mi->Time);
				m_range(startTime, 0, mi->Time - 2000);

				DoSend_x(xcout("I%u", mi->FileId));
				DoSend_x(xcout("W%u", mi->W));
				DoSend_x(xcout("H%u", mi->H));
				DoSend_x(xcout("T%u", startTime));
				DoSend_x(xcout("t%u", mi->Time));
				DoSend("P");
				return;
			}
		}
	}

	// ---- ��񃌃X�|���X ----

	if(startsWith(line, "Volume="))
	{
		Volume = toValue(line + 7);
		return;
	}

	// ----

	if(!strcmp(line, "Booting"))
	{
		DoSend_x(xcout("L%u", Screen_L));
		DoSend_x(xcout("Y%u", Screen_T));
		DoSend_x(xcout("W%u", Screen_W));
		DoSend_x(xcout("H%u", Screen_H));
		DoSend("M");

		DoSend_x(xcout("v%u", Volume));

		DoSend("+"); // �ǎ��\��
		return;
	}
	if(!strcmp(line, "XP")) // �I��
	{
		DoSend("V");
		DoSend("R");
		DoSend("EX");
		return;
	}
}
int main(int argc, char **argv)
{
	MediaDir = combine(getEnvLine("TMP"), MEDIA_DIR_ID);
	SaveDataFile = changeExt(getSelfFile(), "dat");

	Sender = CreateNectar2(SEND_IDENT);
	Recver = CreateNectar2(RECV_IDENT);

	recurRemoveDirIfExist(MediaDir);
	createDir(MediaDir);

	LoadMediaFiles(hasArgs(1) ? nextArg() : "media_files.txt");

	// ���ɋN�����Ă��邩�`�F�b�N
	{
		char *line = Nectar2RecvLine(Recver, '\0'); // �Q�b�Ԏ�M�҂�

		if(line) // ? ��������M�����B== ���ɋN�����Ă���B-> ��~����B
		{
			DoSend("X"); // ���M�����܂Ńu���b�N�����̂ŁA(����̎�M��)�҂K�v�����B
			coSleep(2000); // ���S�ɏI������܂ő҂B
			memFree(line);
		}
	}

	BootScreen();

	if(existFile(SaveDataFile)) // Load-Data
	{
		FILE *fp = fileOpen(SaveDataFile, "rt");

		Screen_L = toValue_x(readLine(fp));
		Screen_T = toValue_x(readLine(fp));
		Screen_W = toValue_x(readLine(fp));
		Screen_H = toValue_x(readLine(fp));
		Volume   = toValue_x(readLine(fp));

		fileClose(fp);

		// �ǂݍ��񂾃f�[�^�𔽉f����B-->

		// moved -> recved 'Booting'
		/*
		DoSend_x(xcout("L%u", Screen_L));
		DoSend_x(xcout("Y%u", Screen_T));
		DoSend_x(xcout("W%u", Screen_W));
		DoSend_x(xcout("H%u", Screen_H));
		DoSend("M");

		DoSend_x(xcout("v%u", Volume));
		*/
	}

//	DoSend("+"); // �ǎ��\�� // moved -> recved 'Booting'

	cout("Client_Ready\n");

	for(; ; )
	{
		char *line = Nectar2RecvLine(Recver, '\0');
		int key;

		if(line)
		{
			if(!strcmp(line, "X")) // ? �I��
			{
				memFree(line);
				break;
			}
			RecvedEvent(line);
			memFree(line);
		}

		switch(key = waitKey(0))
		{
		case 0x1b:
			LOGPOS();
			DoSend("EXP"); // �I�����N�G�X�g
			break;

		case 0x20: // ��~
			{
				PlayingIndex = IMAX;

				DoSend("F");
				DoSend("+");
			}
			break;

		case '1':
		case '2':
		case '3':
		case '4':
		case '5':
		case '6':
		case '7':
		case '8':
			DoPlay(key - '1');
			break;
		}
	}

	// Save-Data
	{
		FILE *fp = fileOpen(SaveDataFile, "wt");

		writeLine_x(fp, xcout("%u", Screen_L));
		writeLine_x(fp, xcout("%u", Screen_T));
		writeLine_x(fp, xcout("%u", Screen_W));
		writeLine_x(fp, xcout("%u", Screen_H));
		writeLine_x(fp, xcout("%u", Volume));

		fileClose(fp);
	}

	DoSend("F"); // ����E���y�t�F�[�h�A�E�g
	DoSend("-"); // �ǎ���\��
	coSleep(2000);

	NoBootScreen = 1; // ��~(X)�𑗐M����ۂǂ��^�C�~���O�� BootScreen �����Ȃ��B

	DoSend("X"); // ���M�����܂Ńu���b�N�����̂ŁA(����̎�M��)�҂K�v�����B
	coSleep(2000); // ���S�ɏI������܂ő҂B

	recurRemoveDir(MediaDir);

	memFree(MediaDir);
	memFree(SaveDataFile);

	ReleaseNectar2(Sender);
	ReleaseNectar2(Recver);
}

#include "C:\Factory\Common\all.h"
#include "C:\Factory\SubTools\libs\Nectar2.h"

// �Ί݂ł� SEND_IDENT, RECV_IDENT �t�ɂȂ��B

#define MEDIA_DIR_ID "{80b5a52a-7cc7-4875-9980-452b0aa95fac}" // shared_uuid
#define SEND_IDENT "{a86e1bc6-907e-4c08-9633-4bf6f61c2b4f}" // shared_uuid

static char *MediaDir;
static Nectar2_t *Sender;

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
static void DoSend(char *line)
{
	Nectar2SendLine(Sender, line);
	Nectar2SendChar(Sender, 0x00);
}
static void DoSend_x(char *line)
{
	Nectar2SendLine_x(Sender, line);
	Nectar2SendChar(Sender, 0x00);
}
static void Main2(void)
{
	uint videoStartTime = 0;

	MediaDir = combine(getEnvLine("TMP"), MEDIA_DIR_ID);
	Sender = CreateNectar2(SEND_IDENT);

	recurRemoveDirIfExist(MediaDir);
	createDir(MediaDir);

	LoadMediaFiles(hasArgs(1) ? nextArg() : "media_files.txt");

	cout("Ready\n");

	for(; ; )
	{
		int key = getKey();

		cout("[%s] %02x:%c\n", c_makeJStamp(NULL, 0), key, toHalf(key));

		switch(key)
		{
		case 0x1b:
			goto endLoop;

		case '1':
		case '2':
		case '3':
		case '4':
		case '5':
		case '6':
		case '7':
		case '8':
			{
				MediaInfo_t mi = *(MediaInfo_t *)getElement(MediaInfos, key - '1');

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
					cout("videoStartTime: %u\n", videoStartTime);

					if(!videoStartTime)
					{
						DoSend("-");
						DoSend("F");
					}
					DoSend_x(xcout("I%u", mi.FileId));
					DoSend_x(xcout("W%u", mi.W));
					DoSend_x(xcout("H%u", mi.H));
					DoSend_x(xcout("T%u", videoStartTime));
					DoSend_x(xcout("t%u", mi.Time));
					DoSend("P");
				}
			}
			break;

		case '9': // ���݂��Ȃ�_V (�e�X�g�p)
			{
				DoSend("-");
				DoSend("F");
				DoSend("I9");
				DoSend("W800");
				DoSend("H600");
				DoSend("T0");
				DoSend("t25000");
				DoSend("P");
			}
			break;

		/*
			�����r������Đ��̃e�X�g
			�r������ɂ���ƁA���Đ����̓���E���y���~�߂Ȃ����Ƃɒ��ӁI

			ff3b == F1
		*/
		case 0xff3b: videoStartTime = 0; break;
		case 0xff3c: videoStartTime = 15000; break;
		case 0xff3d: videoStartTime = 30000; break;
		case 0xff3e: videoStartTime = 45000; break;

		case 0x20: // ��~
			DoSend("F");
			DoSend("+");
			break;

		case 'D': // ����~
			DoSend("D");
			break;

		case 'X': // �I��
			DoSend("X");
			break;

		case 'x': // �N��
			addCwd(getSelfDir());
			addCwd("..\\KiraraScreen\\Release");
			coExecute("START /HIGH KiraraScreen.exe");
			unaddCwd();
			unaddCwd();
			break;

		case 'M': // �S��ʕ\��_�v���C�}�����j�^=(0,0,1280,1024)
			DoSend("L0");
			DoSend("Y0");
			DoSend("W1280");
			DoSend("H1024");
			DoSend("M");
			break;

		case ',': // �S��ʕ\��_�Z�J���_�����j�^=(1280,0,1280,1024)
			DoSend("L1280");
			DoSend("Y0");
			DoSend("W1280");
			DoSend("H1024");
			DoSend("M");
			break;

		case 'm': // �S��ʕ\���u�����v�v���C�}�����j�^��
			DoSend_x(xcout("L%u", (1280 - 800) / 2));
			DoSend_x(xcout("Y%u", (1024 - 600) / 2));
			DoSend("W800");
			DoSend("H600");
			DoSend("M");
			break;

		case '<': // �S��ʕ\���u�����v�Z�J���_�����j�^��
			DoSend_x(xcout("L%u", (1280 - 800) / 2 + 1280));
			DoSend_x(xcout("Y%u", (1024 - 600) / 2));
			DoSend("W800");
			DoSend("H600");
			DoSend("M");
			break;

		case 'C': // ���݂̃V�[�N�ʒu�����N�G�X�g
			DoSend("C");
			break;

		case 'V': // ���݂̉��ʂ����N�G�X�g
			DoSend("V");
			break;

		case 'v': // ���ʐݒ� 80 PCT
			DoSend_x(xcout("v%d", d2i(IMAX * 0.8)));
			break;

		case 'R': // ���݂̃E�B���h�E�̈ʒu�E�T�C�Y�����N�G�X�g
			DoSend("R");
			break;
		}
	}
endLoop:
	ReleaseNectar2(Sender);
	memFree(MediaDir);
}
int main(int argc, char **argv)
{
	uint mtxProc = mutexTryProcLock("{22bd753f-1574-477e-9b1b-754d573dea7e}");
	Main2();
	mutexUnlock(mtxProc);
}

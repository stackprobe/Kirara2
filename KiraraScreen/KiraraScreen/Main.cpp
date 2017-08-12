#include "all.h"

static char *GetVersionString(void)
{
	const char *CONCERT_PTN = "{a9a54906-791d-4e1a-8a71-a4c69359cf68}:0.00"; // shared_uuid@g
	return (char *)strchr(CONCERT_PTN, ':') + 1;
}

int ProcMtxHdl;
int DxLibInited;

static void ReleaseProcMtxHdl(void)
{
	mutexRelease(ProcMtxHdl);
	handleClose(ProcMtxHdl);
}
void EndProc(void)
{
	GetEndProcFinalizers()->Flush();

	ExportSaveData();
	Gnd_FNLZ();

	if(DxLibInited)
	{
		DxLibInited = 0;
		errorCase(DxLib_End()); // ? ���s
	}
	termination();
}
int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
{
	memAlloc_INIT();

	{
		ProcMtxHdl = mutexOpen("{e22f7d0b-a088-4cb9-b65b-6aba80370a0c}"); // �ŗL��id

		if(!waitForMillis(ProcMtxHdl, 0))
		{
			handleClose(ProcMtxHdl);
			return 0;
		}
		GetFinalizers()->AddFunc(ReleaseProcMtxHdl);
	}

	initRnd((int)time(NULL));

	if(argIs("/L"))
	{
		termination(LOG_ENABLED ? 1 : 0);
	}

#define ARG_SAFETY_WORD "AMANOGAWA"

#if LOG_ENABLED
	argIs(ARG_SAFETY_WORD);
#else
	errorCase(!argIs(ARG_SAFETY_WORD)); // ����s�h�~
#endif

	Gnd_INIT();
	ImportSaveData();
	Nectar2_INIT();

	// DxLib >

#if LOG_ENABLED
	SetApplicationLogSaveDirectory("C:\\temp");
#endif
	SetOutApplicationLogValidFlag(LOG_ENABLED); // DxLib �̃��O���o�� 1: ���� 0: ���Ȃ�

	// app >

	Gnd.RealScreen_W = SCREEN_W;
	Gnd.RealScreen_H = SCREEN_H;

	// < app

	SetAlwaysRunFlag(1); // ? ��A�N�e�B�u���� 1: ���� 0: �~�܂�

	SetMainWindowText(xcout(
#if LOG_ENABLED
		"(LOG_ENABLED, DEBUGGING_MODE) %s %s"
#else
		GetDatString(DATSTR_PCT_S_SPC_PCT_S)//"%s %s"
#endif
		,GetDatString(DATSTR_APPNAME)
		,GetVersionString()
		));

//	SetGraphMode(SCREEN_W, SCREEN_H, 32);
	SetGraphMode(Gnd.RealScreen_W, Gnd.RealScreen_H, 32); // orig
	ChangeWindowMode(1); // 1: �E�B���h�E 0: �t���X�N���[��

//	SetFullSceneAntiAliasingMode(4, 2); // �K���Ȓl���������B�t���X�N���[���p�~�����̂ŕs�v

	SetWindowIconID(101); // �E�B���h�E����̃A�C�R��

	errorCase(DxLib_Init()); // ? ���s

	DxLibInited = 1;

	SetMouseDispMode(Gnd.RO_MouseDispMode); // ? �}�E�X��\�� 1: ���� 0: ���Ȃ�
	SetWindowSizeChangeEnableFlag(1); // �E�B���h�E�̉E�����h���b�O�ŐL�k 1: ���� 0: ���Ȃ�

	SetDrawScreen(DX_SCREEN_BACK);
	SetDrawMode(DX_DRAWMODE_BILINEAR); // ������f�t�H���g�Ƃ���B

	// < DxLib

	// *_Reset
	{
		DPE_Reset();
		CEE_Reset();
		PE_Reset();
	}

#if 1 // [X]�{�^��������
	{
		HWND hdl = GetMainWindowHandle();
		EnableMenuItem(GetSystemMenu(hdl, NULL), SC_CLOSE, MF_GRAYED);
	}
#endif

#if LOG_ENABLED // ���̊m�F
	{
		char *b = na(char, 32);
		int s = 32;
		aes128_decrypt_extend(b, s, 1);
		memFree(b);
	}
#endif

	LOGPOS();
	ProcMain();
	LOGPOS();

	EndProc();
	return 0; // dummy
}

// DxPrv_ >

static int DxPrv_GetMouseDispMode(void)
{
	return GetMouseDispFlag() ? 1 : 0;
}
static void DxPrv_SetMouseDispMode(int mode)
{
	SetMouseDispFlag(mode ? 1 : 0);
}
static void UnloadGraph(int &hdl)
{
	if(hdl != -1)
	{
		errorCase(DeleteGraph(hdl)); // ? ���s
		hdl = -1;
	}
}
static void DxPrv_SetScreenSize(int w, int h)
{
	int mdm = GetMouseDispMode();

	UnloadAllPicResHandle();
	UnloadAllSubScreenHandle();

	errorCase(SetGraphMode(w, h, 32) != DX_CHANGESCREEN_OK); // ? ���s

	SetDrawScreen(DX_SCREEN_BACK);
	SetDrawMode(DX_DRAWMODE_BILINEAR);

	SetMouseDispMode(mdm);
}

// < DxPrv_

int GetMouseDispMode(void)
{
	return DxPrv_GetMouseDispMode();
}
void SetMouseDispMode(int mode)
{
	DxPrv_SetMouseDispMode(mode);
}
void ApplyScreenSize(void)
{
	DxPrv_SetScreenSize(Gnd.RealScreen_W, Gnd.RealScreen_H);
}
void SetScreenSize(int w, int h)
{
	m_range(w, SCREEN_W, SCREEN_W_MAX);
	m_range(h, SCREEN_H, SCREEN_H_MAX);

	if(Gnd.RealScreen_W != w || Gnd.RealScreen_H != h)
	{
		Gnd.RealScreen_W = w;
		Gnd.RealScreen_H = h;

		ApplyScreenSize();
	}
}

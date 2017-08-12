#include "all.h"

int KariMenu_Color = -1;
int KariMenu_BorderColor = -1;
int KariMenu_WallPicId = -1;
double KariMenu_WallCurtain = 0.0;
int KariMenu_X = 16;
int KariMenu_Y = 16;
int KariMenu_YStep = 32;

int KariMenu(char *menuTitle, char **menuItems, int selectMax, int selectIndex)
{
	SetCurtain();
	FreezeInput();
	ActFrame = 0;

	for(; ; )
	{
		DrawCurtain();

		if(KariMenu_WallPicId != -1)
		{
			DrawRect(KariMenu_WallPicId, 0, 0, SCREEN_W, SCREEN_H);
//			DrawCenter(KariMenu_WallPicId, SCREEN_W / 2.0, SCREEN_H / 2.0); // old
			DrawCurtain(KariMenu_WallCurtain);
		}
		if(KariMenu_Color != -1)
			PE.Color = KariMenu_Color;

		if(KariMenu_BorderColor != -1)
			PE_Border(KariMenu_BorderColor);

		SetPrint(KariMenu_X, KariMenu_Y, KariMenu_YStep);
//		SetPrint(16, 16, 32); // old
		Print(menuTitle);
		PrintRet();

		for(int c = 0; c < selectMax; c++)
		{
			Print_x(xcout("[%c]�@%s\n", selectIndex == c ? '>' : ' ', menuItems[c]));
			PrintRet();
		}
		PE_Reset();

		if(GetPound(INP_A))
		{
			break;
		}
		if(GetPound(INP_B))
		{
			if(selectIndex == selectMax - 1)
				break;

			selectIndex = selectMax - 1;
		}
		if(GetPound(INP_DIR_8))
		{
			selectIndex--;
		}
		if(GetPound(INP_DIR_2))
		{
			selectIndex++;
		}

		selectIndex += selectMax;
		selectIndex %= selectMax;

		EachFrame();
	}
	FreezeInput();
	DrawCurtain();

	return selectIndex;
}

// ---- �{�^���ݒ� ----

static void *PadBtnIdBkup;

void RestorePadBtnId(void)
{
	memcpy(&Gnd.PadBtnId, PadBtnIdBkup, sizeof(Gnd.PadBtnId));
}
void KariPadConfig(void)
{
	int *BtnPList[INP_MAX] =
	{
		&Gnd.PadBtnId.Dir_2,
		&Gnd.PadBtnId.Dir_4,
		&Gnd.PadBtnId.Dir_6,
		&Gnd.PadBtnId.Dir_8,
		&Gnd.PadBtnId.A,
		&Gnd.PadBtnId.B,
		&Gnd.PadBtnId.C,
		&Gnd.PadBtnId.D,
		&Gnd.PadBtnId.E,
		&Gnd.PadBtnId.F,
		&Gnd.PadBtnId.L,
		&Gnd.PadBtnId.R,
		&Gnd.PadBtnId.Pause,
		&Gnd.PadBtnId.Start,
	};

	/*
		NULL == �g�p���Ă��Ȃ��B
	*/
	char *BTN_LIST[INP_MAX] =
	{
		// app >

		"��", // INP_DIR_2
		"��", // INP_DIR_4
		"�E", // INP_DIR_6
		"��", // INP_DIR_8
		"�V���b�g�{�^��", // INP_A
		"�ᑬ�{�^��", // INP_B
		"�{���{�^��", // INP_C
		NULL, // INP_D
		NULL, // INP_E
		NULL, // INP_F
		NULL, // INP_L
		NULL, // INP_R
		"�|�[�Y�{�^��", // INP_PAUSE
		NULL, // INP_START

		// < app
	};

	PadBtnIdBkup = memClone(&Gnd.PadBtnId, sizeof(Gnd.PadBtnId));
	GetEndProcFinalizers()->AddFunc(RestorePadBtnId);

	for(int c = 0; c < INP_MAX; c++)
		*BtnPList[c] = -1;

	SetCurtain();
	FreezeInput();
	ActFrame = 0;

	int currBtnIndex = 0;

	while(currBtnIndex < INP_MAX)
	{
		if(!BTN_LIST[currBtnIndex])
		{
			currBtnIndex++;
			continue;
		}

		DrawCurtain();

		if(KariMenu_WallPicId != -1)
		{
			DrawRect(KariMenu_WallPicId, 0, 0, SCREEN_W, SCREEN_H);
//			DrawCenter(KariMenu_WallPicId, SCREEN_W / 2.0, SCREEN_H / 2.0); // old
			DrawCurtain(KariMenu_WallCurtain);
		}
		if(KariMenu_Color != -1)
			PE.Color = KariMenu_Color;

		if(KariMenu_BorderColor != -1)
			PE_Border(KariMenu_BorderColor);

		SetPrint(KariMenu_X, KariMenu_Y, KariMenu_YStep);
//		SetPrint(16, 16, 32); // old
		Print("�Q�[���p�b�h�̃{�^���ݒ�");
		PrintRet();

		for(int c = 0; c < INP_MAX; c++)
		{
			if(!BTN_LIST[c])
				continue;

			Print_x(xcout("[%c]�@%s", currBtnIndex == c ? '>' : ' ', BTN_LIST[c]));

			if(c < currBtnIndex)
			{
				int btnId = *BtnPList[c];

				Print("�@->�@");

				if(btnId == -1)
					Print("���蓖�ăi�V");
				else
					Print_x(xcout("%d", btnId));
			}
			PrintRet();
		}
		Print("���@�J�[�\���̋@�\�Ɋ��蓖�Ă�{�^���������ĉ������B");
		PrintRet();
		Print("���@�X�y�[�X�������ƃL�����Z�����܂��B");
		PrintRet();
		Print("���@[Z]�������ƃ{�^���̊��蓖�Ă��X�L�b�v���܂��B");
		PrintRet();

		if(GetKeyInput(KEY_INPUT_SPACE) == 1)
		{
			RestorePadBtnId();
			break;
		}
		if(GetKeyInput(KEY_INPUT_Z) == 1)
		{
			currBtnIndex++;
			goto doEachFrame;
		}
		int pressBtnId = -1;

		for(int padId = 0; padId < GetPadCount(); padId++)
		for(int btnId = 0; btnId < PAD_BUTTON_MAX; btnId++)
			if(GetPadInput(padId, btnId) == 1)
				pressBtnId = btnId;

		for(int c = 0; c < currBtnIndex; c++)
			if(*BtnPList[c] == pressBtnId)
				pressBtnId = -1;

		if(pressBtnId != -1)
		{
			*BtnPList[currBtnIndex] = pressBtnId;
			currBtnIndex++;
		}
doEachFrame:
		EachFrame();
	}

	GetEndProcFinalizers()->RemoveFunc(RestorePadBtnId);
	memFree(PadBtnIdBkup);

	FreezeInput();
	DrawCurtain();
}

// ---- ��ʃT�C�Y ----

void KariWindowSizeConfig(void)
{
	char *MENU_ITEMS[] =
	{
		"800 x 600 (�f�t�H���g)",
		"900 x 675",
		"1000 x 750",
		"1100 x 825",
		"1200 x 900",
		"1300 x 975",
		"1400 x 1050",
		"1500 x 1125",
		"1600 x 1200",
		"�t���X�N���[��",
		"�t���X�N���[�� (�c������ێ�����)",
		"�߂�",
	};

	int selectIndex = 0;

	for(; ; )
	{
		selectIndex = KariMenu("�E�B���h�E�T�C�Y�ݒ�", MENU_ITEMS, lengthof(MENU_ITEMS), selectIndex);

		switch(selectIndex)
		{
		case 0: SetScreenSize(800, 600); break;
		case 1: SetScreenSize(900, 675); break;
		case 2: SetScreenSize(1000, 750); break;
		case 3: SetScreenSize(1100, 825); break;
		case 4: SetScreenSize(1200, 900); break;
		case 5: SetScreenSize(1300, 975); break;
		case 6: SetScreenSize(1400, 1050); break;
		case 7: SetScreenSize(1500, 1125); break;
		case 8: SetScreenSize(1600, 1200); break;

		case 9:
			SetScreenSize(
				GetSystemMetrics(SM_CXSCREEN),
				GetSystemMetrics(SM_CYSCREEN)
				);
			break;

		case 10:
			{
				int w = GetSystemMetrics(SM_CXSCREEN);
				int h = GetSystemMetrics(SM_CYSCREEN);

				if(w * SCREEN_H < h * SCREEN_W) // �c�����j�^ -> �����ɍ��킹��
				{
					h = d2i(((double)w * SCREEN_H) / SCREEN_W);
				}
				else // �������j�^ -> �c���ɍ��킹��
				{
					w = d2i(((double)h * SCREEN_W) / SCREEN_H);
				}
				SetScreenSize(w, h);
			}
			break;

		case 11:
			goto endLoop;

		default:
			error();
		}
	}
endLoop:;
}

// ---- �{�����[�� ----

static double KVC_ValueToRate(double value, double minval, double valRange)
{
	return (double)(value - minval) / valRange;
}

/*
	(ret, rate): 0.0 - 1.0
	pulseFrm: 0 == ����
*/
double KariVolumeConfig(char *menuTitle, double rate, int minval, int maxval, int valStep, int valFastStep, void (*valChanged)(double), int pulseFrm)
{
	int valRange = maxval - minval;
	int value = minval + d2i(rate * valRange);
	int origval = value;

	SetCurtain();
	FreezeInput();
	ActFrame = 0;

	for(; ; )
	{
		int chgval = 0;

		DrawCurtain();

		if(KariMenu_WallPicId != -1)
		{
			DrawRect(KariMenu_WallPicId, 0, 0, SCREEN_W, SCREEN_H);
			DrawCurtain(KariMenu_WallCurtain);
		}
		if(KariMenu_Color != -1)
			PE.Color = KariMenu_Color;

		if(KariMenu_BorderColor != -1)
			PE_Border(KariMenu_BorderColor);

		if(GetPound(INP_A))
		{
			break;
		}
		if(GetPound(INP_B))
		{
			if(value == origval)
				break;

			value = origval;
			chgval = 1;
		}
		if(GetPound(INP_DIR_8))
		{
			value += valFastStep;
			chgval = 1;
		}
		if(GetPound(INP_DIR_6))
		{
			value += valStep;
			chgval = 1;
		}
		if(GetPound(INP_DIR_4))
		{
			value -= valStep;
			chgval = 1;
		}
		if(GetPound(INP_DIR_2))
		{
			value -= valFastStep;
			chgval = 1;
		}
		if(chgval || pulseFrm && ActFrame % pulseFrm == 0)
		{
			m_range(value, minval, maxval);
			valChanged(KVC_ValueToRate(value, minval, valRange));
		}

		SetPrint(KariMenu_X, KariMenu_Y, KariMenu_YStep);
		Print(menuTitle);
		PrintRet();

		Print_x(xcout("[ %d ]�@�ŏ�=%d�@�ő�=%d", value, minval, maxval));
		PrintRet();

		Print("���@����������");
		PrintRet();
		Print("���@�E���グ��");
		PrintRet();
		Print("���@��������������");
		PrintRet();
		Print("���@�ぁ�����グ��");
		PrintRet();
		Print("���@�������I������猈��{�^���������ĉ������B");
		PrintRet();

		EachFrame();
	}
	FreezeInput();
	DrawCurtain();

	return KVC_ValueToRate(value, minval, valRange);
}

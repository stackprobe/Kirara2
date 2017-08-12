typedef struct Gnd_st
{
	taskList *EL; // EffectList
	int PrimaryPadId; // -1 == 未設定
	SubScreen_t *MainScreen; // NULL == 不使用

	// app >

	// < app

	// SaveData {

	int RealScreen_W;
	int RealScreen_H;

	/*
		音量
		0.0 - 1.0
		def: DEFAULT_VOLUME
	*/
	double MusicVolume;
	double SEVolume;

	/*
		-1 == 割り当てナシ
		0 - (PAD_BUTTON_MAX - 1) == 割り当てボタンID
		def: SNWPB_*
	*/
	struct
	{
		int Dir_2;
		int Dir_4;
		int Dir_6;
		int Dir_8;
		int A;
		int B;
		int C;
		int D;
		int E;
		int F;
		int L;
		int R;
		int Pause;
		int Start;
	}
	PadBtnId;

	int RO_MouseDispMode;

	// app >

	// < app

	// }
}
Gnd_t;

extern Gnd_t Gnd;

void Gnd_INIT(void);
void Gnd_FNLZ(void);

void ImportSaveData(void);
void ExportSaveData(void);

void UnassignAllPadBtnId(void);

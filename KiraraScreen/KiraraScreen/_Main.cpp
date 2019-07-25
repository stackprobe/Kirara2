#include "all.h"

#define MEDIA_DIR_ID "{80b5a52a-7cc7-4875-9980-452b0aa95fac}" // shared_uuid

#define MOVIE_W_MIN 10
#define MOVIE_H_MIN 10

static int Param_FileId;
static int Param_Width;
static int Param_Height;
static int Param_StartTime;
static int Param_MediaTime;
static int Param_Left;
static int Param_Top;
static int Param_DoubleMovie;
static int Param_DoubleMovie_DarknessPct = 30;
static int Param_DoubleMovie_BokashiLevel = 30;

// ---- movie ----

static int FileId;
static int Width  = MOVIE_W_MIN;
static int Height = MOVIE_H_MIN;
static int StartTime; // ミリ秒
static int MovieTime; // ミリ秒
static int Movie_L;
static int Movie_T;
static int Movie_W = MOVIE_W_MIN;
static int Movie_H = MOVIE_H_MIN;
static int BackMovie_L;
static int BackMovie_T;
static int BackMovie_W = MOVIE_W_MIN;
static int BackMovie_H = MOVIE_H_MIN;
static char *File; // null ng
static int MovieHdl = -1; // -1 == 未再生

static int MoviePlaying;
static int MovieStoppedSendFrame; // cd zd
static int MoviePlayingFrame;

// ---- bgm ----

static int BgmFileId;
static int BgmTime; // ミリ秒
static char *BgmFile;
static autoList<uchar> *BgmFileImage;
static int BgmHdl = -1; // -1 == 未再生

static int BgmPlaying;
static int BgmStoppedSendFrame; // cd zd
static int BgmPlayingFrame;

// ----

static char *ErrorMessage_1 = NULL;
static char *ErrorMessage_2 = NULL;
static char *ErrorMessage_3 = NULL;

static double MaxButtonShowRate = 0.0;
static double SeekBarShowRate = 0.0;
static double LastSeekRate = 0.0;

static int MaxButtonKeepFrame; // cd zd
static int SeekBarKeepFrame; // cd zd
static int FadeoutRushFrame; // cd zd

static double KrrVolume = 1.0; // 0.0 - 1.0

static int SeekPosChanging;
static int SeekPosChangingFrame; // cd zd
static double SeekPosChangingRate;
static int VolumeChanging;

static autoList<char *> *InstantMessages;
static int InstantMessagesTopLineShownFrame; // cu
static int InstantMessagesDisabled;

// ====

void Krr_EachFrame(void);

static void AddInstantMessage_x(char *message)
{
	InstantMessages->AddElement(message);
}
static void AddInstantMessage(char *message)
{
	AddInstantMessage_x(strx(message));
}
void Pub_AddInstantMessage_x(char *message)
{
	AddInstantMessage_x(message);
}
static double GetKrrRealVolume(void)
{
	double ret = KrrVolume;

	m_range(ret, 0.0, 1.0); // 2bs

	ret = 1.0 - ret;
	ret *= ret;
	ret = 1.0 - ret;

	m_range(ret, 0.0, 1.0); // 2bs???

	return ret;
}

// ---- movie ----

static void DeleteMovie(void)
{
	if(MovieHdl != -1)
	{
		DeleteGraph(MovieHdl);
		MovieHdl = -1;
	}
}
static void SetMovie_LTWH(void)
{
	Movie_W = Gnd.RealScreen_W;
	Movie_H = (Height * Gnd.RealScreen_W) / Width;

	if(Gnd.RealScreen_H < Movie_H)
	{
		Movie_W = (Width * Gnd.RealScreen_H) / Height;
		Movie_H = Gnd.RealScreen_H;

		errorCase(Gnd.RealScreen_W < Movie_W); // 有り得ない。
	}
	m_maxim(Movie_W, MOVIE_W_MIN);
	m_maxim(Movie_H, MOVIE_H_MIN);

	Movie_L = (Gnd.RealScreen_W - Movie_W) / 2;
	Movie_T = (Gnd.RealScreen_H - Movie_H) / 2;

	// ----

	BackMovie_W = Gnd.RealScreen_W;
	BackMovie_H = (Height * Gnd.RealScreen_W) / Width;

	if(BackMovie_H < Gnd.RealScreen_H)
	{
		BackMovie_W = (Width * Gnd.RealScreen_H) / Height;
		BackMovie_H = Gnd.RealScreen_H;

		errorCase(BackMovie_W < Gnd.RealScreen_W); // 有り得ない。
	}
	m_maxim(BackMovie_W, MOVIE_W_MIN);
	m_maxim(BackMovie_H, MOVIE_H_MIN);

	BackMovie_L = (Gnd.RealScreen_W - BackMovie_W) / 2;
	BackMovie_T = (Gnd.RealScreen_H - BackMovie_H) / 2;
}
static void MovieDraw(void)
{
	if(MovieHdl != -1)
	{
		if(Param_DoubleMovie)
		{
			static int scr = -1;
			static int scr_w = -1;
			static int scr_h = -1;

			if(scr != -1 && (scr_w != BackMovie_W || scr_h != BackMovie_H))
			{
				DeleteGraph(scr);
				scr = -1;
			}
			if(scr == -1)
			{
				scr_w = BackMovie_W;
				scr_h = BackMovie_H;
				scr = MakeScreen(scr_w, scr_h);
				errorCase(scr == -1);

				// memo:
				// MakeScreenで生成したハンドル(scr)はフルスクリーンからタスク切り替えで無効？になるっぽい。
				// <--- ChangeWindowModeを使っていないので問題無いはず。。。
			}
			SetDrawScreen(scr);

			DrawExtendGraph(0, 0, BackMovie_W, BackMovie_H, MovieHdl, FALSE);
			GraphFilter(scr, DX_GRAPH_FILTER_GAUSS, 16,
//				3000 // old
				100 * Param_DoubleMovie_BokashiLevel
				);

			SetDrawScreen(DX_SCREEN_BACK); // 復元

			DrawExtendGraph(BackMovie_L, BackMovie_T, BackMovie_L + BackMovie_W, BackMovie_T + BackMovie_H, scr, FALSE);
			DrawCurtain(
//				-0.3 // old
				Param_DoubleMovie_DarknessPct / -100.0
				);
		}
		DrawExtendGraph(Movie_L, Movie_T, Movie_L + Movie_W, Movie_T + Movie_H, MovieHdl, FALSE);
	}
}
static void SetMovieVolume(double rate)
{
	rate *= GetKrrRealVolume();

	if(MovieHdl != -1)
	{
		int volume = d2i(rate * 10000.0);

		m_range(volume, 0, 10000);

		SetMovieVolumeToGraph(volume, MovieHdl);
	}
}
static void MovieFadeOut(int quickFlag)
{
	if(MovieHdl == -1)
		return;

	if(quickFlag)
	{
		LOG("MovieFadeOut_quickMode");

		SetCurtain(10, -1.0);

		forscene(10)
		{
			SetMovieVolume(1.0 - sc_rate);

			DrawCurtain();
			MovieDraw();
			Krr_EachFrame();
		}
		sceneLeave();
	}
	else
	{
		forscene(30)
		{
			SetMovieVolume(1.0 - sc_rate);

			DrawCurtain();
			MovieDraw();
			Krr_EachFrame();
		}
		sceneLeave();

		SetCurtain(30, -1.0);

		forscene(30)
		{
			DrawCurtain();
			MovieDraw();
			Krr_EachFrame();
		}
		sceneLeave();
	}
	DeleteMovie();
}

// ---- bgm ----

static void StopBgm(void)
{
	if(BgmHdl != -1)
	{
		StopSoundMem(BgmHdl);
	}
}
static void DeleteBgm(void)
{
	StopBgm();

	if(BgmHdl != -1)
	{
		DeleteSoundMem(BgmHdl);
		BgmHdl = -1;
	}
}
static void SetBgmVolume(double rate)
{
	rate *= GetKrrRealVolume();

	if(BgmHdl != -1)
	{
		int volume = d2i(rate * 255.0);

		m_range(volume, 0, 255);

		ChangeVolumeSoundMem(volume, BgmHdl);
	}
}
static void BgmFadeOut(int quickFlag)
{
	if(BgmHdl == -1)
		return;

	if(quickFlag)
	{
		LOG("BgmFadeOut_quickMode");

		forscene(10)
		{
			SetBgmVolume(1.0 - sc_rate);

			DrawCurtain();
			MovieDraw();
			Krr_EachFrame();
		}
		sceneLeave();
	}
	else
	{
		forscene(30)
		{
			SetBgmVolume(1.0 - sc_rate);

			DrawCurtain();
			MovieDraw();
			Krr_EachFrame();
		}
		sceneLeave();
	}
	DeleteBgm();
}

// ---- wall ----

#define WALL_CHANGE_FRAME (60 * 20)

static double Wall_A;
static double Wall_ADest;
static double Wall_ARate;

static void SetWallVisible(int flag)
{
	if(flag)
	{
		Wall_ADest = 1.0;
		Wall_ARate = 0.997;
	}
	else
	{
		Wall_ADest = 0.0;
		Wall_ARate = 0.93;
	}
}

static struct Wall_st
{
	int PicId;
	
	struct Rect_st
	{
		double L;
		double T;
		double R;
		double B;
	}
	Rect[2]; // [Curr, Dest]

	double A;
}
Walls[2]; // [Curr, Next]

static int WallCount;
static SubScreen_t *WallScreen;

static int WallMakePicId(int currPicId)
{
	for(; ; )
	{
		int picId = bRnd(P_WALL_00, P_WALL_00_END);

		if(picId != currPicId)
			return picId;
	}
}
/*
	picW, picH 内の表示したい領域の Rect -> 画面に描画するための Rect
*/
static void InsideRectToDrawRect(struct Wall_st::Rect_st *rect, int picW, int picH)
{
	double L = rect->L;
	double T = rect->T;
	double W = rect->R - rect->L;
	double H = rect->B - rect->T;
	double PW = picW;
	double PH = picH;

	m_range(PW, 1.0, IMAX);
	m_range(PH, 1.0, IMAX);
	m_range(W, 1.0, PW);
	m_range(H, 1.0, PH);
	m_range(L, 0.0, PW - W);
	m_range(T, 0.0, PH - H);

	double l = L / PW;
	double t = T / PH;
	double w = W / PW;
	double h = H / PH;

	double rw = Gnd.RealScreen_W / w;
	double rh = Gnd.RealScreen_H / h;
	double rl = rw * -l;
	double rt = rh * -t;

	rect->L = rl;
	rect->T = rt;
	rect->R = rl + rw;
	rect->B = rt + rh;
}
static struct Wall_st::Rect_st WallMakeRect(int picId)
{
	struct Wall_st::Rect_st ret;

	int picW = Pic_W(picId);
	int picH = Pic_H(picId);

	int w = Gnd.RealScreen_W;
	int h = Gnd.RealScreen_H;

	adjustInside(w, h, picW, picH);

	{
		double rate = 0.5 + dRnd() * 0.5;

		w = d2i(w * rate);
		h = d2i(h * rate);
	}

	int l = picW - w;
	int t = picH - h;

	l = bRnd(0, l);
	t = bRnd(0, t);

	ret.L = l;
	ret.T = t;
	ret.R = l + w;
	ret.B = t + h;

	InsideRectToDrawRect(&ret, picW, picH);

	return ret;
}
static void WallMake(int index, int currPicId)
{
	Walls[index].PicId = WallMakePicId(currPicId);
	Walls[index].Rect[0] = WallMakeRect(Walls[index].PicId);
	Walls[index].Rect[1] = WallMakeRect(Walls[index].PicId);
	Walls[index].A = 0.0;
}
static void WallInit(void)
{
	Wall_A = 0.0;
	Wall_ADest = 0.0;
	Wall_ARate = 1.0;

	WallMake(0, -1);
	WallCount = 1;
	WallScreen = CreateSubScreen(Gnd.RealScreen_W, Gnd.RealScreen_H);
}
static void WallReinit(void)
{
	ReleaseSubScreen(WallScreen);
	WallInit();
}
static void WallDraw(void)
{
	m_approach(Wall_A, Wall_ADest, Wall_ARate);

	if((ProcFrame + 1) % (60 * 15) == 0)
	{
		WallMake(1, Walls[0].PicId);
		WallCount = 2;
	}
	for(int index = 0; index < WallCount; index++)
	{
		struct Wall_st *wall = Walls + index;

		{
			const double RATE = 0.9997;

			m_approach(wall->Rect[0].L, wall->Rect[1].L, RATE);
			m_approach(wall->Rect[0].T, wall->Rect[1].T, RATE);
			m_approach(wall->Rect[0].R, wall->Rect[1].R, RATE);
			m_approach(wall->Rect[0].B, wall->Rect[1].B, RATE);
		}

		m_approach(wall->A, 1.0, 0.993);
	}
	if(WallCount == 2 && 0.99 < Walls[1].A)
	{
		Walls[0] = Walls[1];
		Walls[0].A = 1.0;
		WallCount = 1;
	}

	// draw -->

	if(Wall_A < 0.01)
		return;

	ChangeDrawScreen(WallScreen);
	DrawCurtain();

	for(int index = 0; index < WallCount; index++)
	{
		struct Wall_st *wall = Walls + index;

		{
			double a = wall->A;
//			double a = wall->A * Wall_A;
			int pal = d2i(a * 255.0);
			m_range(pal, 0, 255);

			SetDrawBlendMode(DX_BLENDMODE_ALPHA, pal);
		}

		DrawExtendGraphF(
			(float)wall->Rect[0].L,
			(float)wall->Rect[0].T,
			(float)wall->Rect[0].R,
			(float)wall->Rect[0].B,
			Pic(wall->PicId),
			TRUE
			);

		SetDrawBlendMode(DX_BLENDMODE_NOBLEND, 0); // reset
	}
	RestoreDrawScreen();

	{
		double a = Wall_A;
		int pal = d2i(a * 255.0);
		m_range(pal, 0, 255);

		SetDrawBlendMode(DX_BLENDMODE_ALPHA, pal);
		DrawExtendGraph(0, 0, Gnd.RealScreen_W, Gnd.RealScreen_H, GetHandle(WallScreen), 0);
		SetDrawBlendMode(DX_BLENDMODE_NOBLEND, 0); // reset
	}
}

// ----

static int LastWindowMaximizeFrame;
static int LastWindowXPressFrame;
static int LastSaiseiTeishiPressFrame;

static int IsActionBusy(void)
{
	return
		LastWindowMaximizeFrame ||
		LastSaiseiTeishiPressFrame;
}
static void WindowMaximizeEvent(void)
{
	if(IsActionBusy())
		return;

	Nectar2Send("M");
	LastWindowMaximizeFrame = 120;
}
static void WindowXPressEvent(void)
{
	if(IsActionBusy())
		return;

	Nectar2Send("XP");
	LastWindowXPressFrame = 120;
}
static void SaiseiTeishiPressEvent(void)
{
	if(IsActionBusy())
		return;

	Nectar2Send("S");
	LastSaiseiTeishiPressFrame = 30;
}
static void VolumeChangedEvent(void)
{
	if(MoviePlaying)
		SetMovieVolume(1.0);

	if(BgmPlaying)
		SetBgmVolume(1.0);
}
void ProcMain(void)
{
	File = strx("Dummy");
	InstantMessages = new autoList<char *>();
	WallInit();
	Nectar2Send("Booting");

	for(; ; )
	{
		char *message = c_Nectar2Recv();

		if(message)
		{
			AddInstantMessage_x(xcout(">%s", message));

			switch(*message)
			{
			case 'I':
				Param_FileId = atoi(message + 1);
				break;

			case 'W':
				Param_Width = atoi(message + 1);
				break;

			case 'H':
				Param_Height = atoi(message + 1);
				break;

			case 'T':
				Param_StartTime = atoi(message + 1);
				break;

			case 't':
				Param_MediaTime = atoi(message + 1);
				break;

			case 'P':
				{
					DeleteMovie();

					FileId    = Param_FileId;
					Width     = Param_Width;
					Height    = Param_Height;
					StartTime = Param_StartTime;
					MovieTime = Param_MediaTime;

					m_range(FileId, 0, IMAX);
					m_range(Width, 1, IMAX);
					m_range(Height, 1, IMAX);
					m_range(StartTime, 0, IMAX);
					m_range(MovieTime, 1, IMAX);

					SetMovie_LTWH();

					memFree(File);

					{
						char *dir = combine(getTempDir(), MEDIA_DIR_ID);
						char *lFile = xcout("%010d.ogv", FileId);

						File = combine(dir, lFile);

						memFree(dir);
						memFree(lFile);
					}

					MovieHdl = LoadGraph(File);

					LOGPOS();

					if(MovieHdl != -1)
					{
						// 注意: StartTime を動画の長さ以上にすると、うまく再生されないどころか、不安定になるっぽい！

						SeekMovieToGraph(MovieHdl, StartTime);
						PlayMovieToGraph(MovieHdl);

						// reset
						{
							SetCurtain(0);
							SetMovieVolume(1.0);
							MoviePlayingFrame = d2i(StartTime * 60.0 / 1000.0);
						}

						ErrorMessage_1 = NULL;
						ErrorMessage_2 = NULL;
						ErrorMessage_3 = NULL;
					}
					else
					{
						ErrorMessage_1 = "+--------------------------+";
						ErrorMessage_2 = "| ERROR LOADING MOVIE FILE |";
						ErrorMessage_3 = "+--------------------------+";
					}
				}
				break;

			case 'B':
				{
					DeleteBgm();

					BgmFileId = Param_FileId;
					BgmTime = Param_MediaTime;

					m_range(BgmFileId, 0, IMAX);
					m_range(BgmTime, 1, IMAX);

					memFree(BgmFile);

					{
						char *dir = combine(getTempDir(), MEDIA_DIR_ID);
						char *lFile = xcout("%010d.ogg", BgmFileId);

						BgmFile = combine(dir, lFile);

						memFree(dir);
						memFree(lFile);
					}

					delete BgmFileImage;
					BgmFileImage = NULL;
					BgmHdl = -1; // 2bs

					if(AUDIO_FILE_SIZE_MAX < getFileSizeByFile(BgmFile))
					{
						ErrorMessage_1 = "+--------------------------+";
						ErrorMessage_2 = "| ERROR SOUND FILE TOO BIG |";
						ErrorMessage_3 = "+--------------------------+";
						break;
					}
					BgmFileImage = readFile(BgmFile);
					BgmHdl = LoadSoundMemByMemImage(BgmFileImage->ElementAt(0), BgmFileImage->GetCount());

					if(BgmHdl != -1)
					{
						// 注意: BgmStartTime を曲の長さ以上にしないこと！！

						//SetSoundCurrentTime(0, BgmHdl); // 途中から再生に対応しない！
						PlaySoundMem(BgmHdl, DX_PLAYTYPE_BACK, FALSE); // mem ver

						// reset
						{
							SetBgmVolume(1.0);
						}

						ErrorMessage_1 = NULL;
						ErrorMessage_2 = NULL;
						ErrorMessage_3 = NULL;
					}
					else
					{
						ErrorMessage_1 = "+--------------------------+";
						ErrorMessage_2 = "| ERROR LOADING SOUND FILE |";
						ErrorMessage_3 = "+--------------------------+";
					}
				}
				break;

			case 'F':
				{
					//int quickFlag = Nectar2IsRecvJam();
					int quickFlag = FadeoutRushFrame ? 1 : 0;

					BgmFadeOut(quickFlag);
					MovieFadeOut(quickFlag);

					FadeoutRushFrame = 30;
				}
				break;

			case 'D': // 即停止
				DeleteBgm();
				DeleteMovie();
				break;

			case '+':
				SetWallVisible(1);
				break;

			case '-':
				SetWallVisible(0);
				break;

			case 'X':
				goto endMainLoop;

#if 0 // 廃止
			case 'D': // 動画・音楽ファイル削除
				{
					char *dir = combine(getTempDir(), MEDIA_DIR_ID);
					char *lFile1 = xcout("%010d.ogv", Param_FileId);
					char *lFile2 = xcout("%010d.ogg", Param_FileId);
					char *file1 = combine(dir, lFile1);
					char *file2 = combine(dir, lFile2);

					remove(file1);
					remove(file2);

					memFree(dir);
					memFree(lFile1);
					memFree(lFile2);
					memFree(file1);
					memFree(file2);
				}
				break;
#endif

			case 'L':
				Param_Left = atoi(message + 1);
				break;

			case 'Y':
				Param_Top = atoi(message + 1);
				break;

			case 'd':
				switch(message[1])
				{
				case 'F':
					Param_DoubleMovie = message[2] - '0';
					break;

				case 'D':
					Param_DoubleMovie_DarknessPct = atoi(message + 2);
					break;

				case 'B':
					Param_DoubleMovie_BokashiLevel = atoi(message + 2);
					break;
				}
				break;

			case 'M': // 全画面表示用
				{
//					errorCase(MoviePlaying); // 動画の再生中に行えない！！(動画が止まる)
					if(MoviePlaying)
					{
						AddInstantMessage("Error:動画の再生中にスクリーンサイズを変更しようとしました。");
						Nectar2Send("!動画の再生中にスクリーンサイズを変更しようとしました。"); // エラーを通知
						break;
					}

					int win_l = Param_Left;
					int win_t = Param_Top;
					int win_w = Param_Width;
					int win_h = Param_Height;

					m_range(win_l, -IMAX, IMAX);
					m_range(win_t, -IMAX, IMAX);
					m_range(win_w, SCREEN_W, SCREEN_W_MAX);
					m_range(win_h, SCREEN_H, SCREEN_H_MAX);

					SetWindowSizeExtendRate(1.0); // ウィンドウの端をドラッグでリサイズすると、これが変わる。
					SetScreenSize(win_w, win_h);
					SetWindowPosition(win_l, win_t);

					POINT p;

					p.x = 0;
					p.y = 0;

					ClientToScreen(GetMainWindowHandle(), &p);

					int targDiff_l = win_l - (int)p.x;
					int targDiff_t = win_t - (int)p.y;

//					LOG("ウィンドウを右に %d px ずらします。\n", targDiff_l);
//					LOG("ウィンドウを下に %d px ずらします。\n", targDiff_t);

					SetWindowPosition(win_l + targDiff_l, win_t + targDiff_t);

					// ウィンドウサイズ変更後にすべきこと
					{
						WallReinit();
					}
				}
				break;

			case 'C': // 現在のシーク位置をレスポンス
				Nectar2Send_x(xcout("Curr=%d", d2i(LastSeekRate * IMAX)));
				break;

			case 'V': // 現在の音量をレスポンス
				Nectar2Send_x(xcout("Volume=%d", d2i(KrrVolume * IMAX)));
				break;

			case 'v': // 音量を設定
				{
					int iVol = atoi(message + 1);
					double vol = (double)iVol / IMAX;

					m_range(vol, 0.0, 1.0);

					KrrVolume = vol;
					VolumeChangedEvent();
				}
				break;

			case 'R': // 現在のウィンドウの位置・サイズをレスポンス
				{
					// 最小化 check
					{
						RECT rect;
						GetClientRect(GetMainWindowHandle(), &rect);

						if(rect.right <= 0 || rect.bottom <= 0) // ? 最小化している。
							break;
					}

					POINT p;

					p.x = 0;
					p.y = 0;

					ClientToScreen(GetMainWindowHandle(), &p);

					Nectar2Send_x(xcout("Rect=%d,%d,%d,%d", p.x, p.y, Gnd.RealScreen_W, Gnd.RealScreen_H));
				}
				break;

			case 'E': // Echo
				Nectar2Send(message + 1);
				break;

			case 'r':
				AddInstantMessage_x(xcout("SetWindowSizeChangeEnableFlag(%c);", message[1]));
				SetWindowSizeChangeEnableFlag(message[1] - '0');
				break;

			case 'i':
				InstantMessagesDisabled = message[1] - '0';
				break;
			}
		}

		MoviePlaying = MovieHdl != -1 && GetMovieStateToGraph(MovieHdl) == 1;

		if(!m_countDown(MovieStoppedSendFrame))
		{
			if(!MoviePlaying)
			{
				if(MovieHdl != -1) // ハンドルが生きているということは自然停止した。
				{
					if(!SeekPosChanging && !SeekPosChangingFrame) // シークバーを「動かしている・動かした直後」ではない。
					{
						Nectar2Send("R"); // Ready
						MovieStoppedSendFrame = 60 * 2;
					}
				}
			}
		}

		BgmPlaying = BgmHdl != -1 && CheckSoundMem(BgmHdl) == 1;

		if(!m_countDown(BgmStoppedSendFrame))
		{
			if(!BgmPlaying)
			{
				if(BgmHdl != -1) // ハンドルが生きているということは自然停止した。
				{
					if(!SeekPosChanging && !SeekPosChangingFrame) // シークバーを「動かしている・動かした直後」ではない。
					{
						Nectar2Send("B"); // Bgm ready
						BgmStoppedSendFrame = 60 * 2;
					}
				}
			}
		}

		int mouseX;
		int mouseY;

		GetMousePoint(&mouseX, &mouseY);

		int mouseLeftDown;
		int mouseLeftUp;
		int mouseLeft = GetMouseInput() & MOUSE_INPUT_LEFT ? 1 : 0;

		{
			static int lastMouseLeft;

			mouseLeftDown =  mouseLeft && !lastMouseLeft ? 1 : 0;
			mouseLeftUp   = !mouseLeft &&  lastMouseLeft ? 1 : 0;

			lastMouseLeft = mouseLeft;
		}

		// 画面中央をダブルクリック -> 全画面 or 解除
		{
			const int GAMEN_AREA_L = 0;
			const int GAMEN_AREA_T = 100;
			const int GAMEN_AREA_R = Gnd.RealScreen_W;
			const int GAMEN_AREA_B = Gnd.RealScreen_H - 100;

			static int gamenAreaClickedFrame;

			if(
				mouseLeftDown &&
				m_isRange(mouseX, GAMEN_AREA_L, GAMEN_AREA_R) &&
				m_isRange(mouseY, GAMEN_AREA_T, GAMEN_AREA_B)
				)
			{
				if(gamenAreaClickedFrame)
				{
					WindowMaximizeEvent();
					gamenAreaClickedFrame = 0;
				}
				else
					gamenAreaClickedFrame = 30;
			}
			m_countDown(gamenAreaClickedFrame);
		}

		// 画面最大化ボタン押下 -> 全画面 or 解除
		{
			const int GAMEN_MAX_L = Gnd.RealScreen_W - 100;
			const int GAMEN_MAX_T = 0;
			const int GAMEN_MAX_R = Gnd.RealScreen_W - 50;
			const int GAMEN_MAX_B = 50;

			if(
				mouseLeftDown &&
				m_isRange(mouseX, GAMEN_MAX_L, GAMEN_MAX_R) &&
				m_isRange(mouseY, GAMEN_MAX_T, GAMEN_MAX_B)
				)
				WindowMaximizeEvent();
		}

		// [X]ボタン押下
		{
			const int XBTN_L = Gnd.RealScreen_W - 95; // 左に最大化ボタンがあるので、マージン入れる。
			const int XBTN_T = 0;
			const int XBTN_R = Gnd.RealScreen_W;
			const int XBTN_B = 50;

			if(
				mouseLeftDown &&
				m_isRange(mouseX, XBTN_L, XBTN_R) &&
				m_isRange(mouseY, XBTN_T, XBTN_B)
				)
				WindowXPressEvent();
		}

		// 再生・停止ボタン押下
		{
			const int SAISEI_BTN_L = 0;
			const int SAISEI_BTN_T = Gnd.RealScreen_H - 50;
			const int SAISEI_BTN_R = 50;
			const int SAISEI_BTN_B = Gnd.RealScreen_H;

			if(
				mouseLeftDown &&
				m_isRange(mouseX, SAISEI_BTN_L, SAISEI_BTN_R) &&
				m_isRange(mouseY, SAISEI_BTN_T, SAISEI_BTN_B)
				)
				SaiseiTeishiPressEvent();
		}

		// シークバーのつまみを掴んでグイッと..
		{
			const int SEEKER_L = 70;
			const int SEEKER_T = Gnd.RealScreen_H - 50;
			const int SEEKER_R = Gnd.RealScreen_W - 120;
			const int SEEKER_B = Gnd.RealScreen_H;

			if(!SeekPosChanging)
			{
				// ? シークバーのつまみを掴んだ
				if(
					mouseLeftDown &&
					m_isRange(mouseX, SEEKER_L, SEEKER_R) &&
					m_isRange(mouseY, SEEKER_T, SEEKER_B)
					)
					SeekPosChanging = 1;
			}
			if(!MoviePlaying)
				SeekPosChanging = 0;

			if(SeekPosChanging)
			{
				SeekPosChangingRate = (mouseX - SEEKER_L) * 1.0 / (SEEKER_R - SEEKER_L);
				m_range(SeekPosChangingRate, 0.0, 1.0);
				SeekPosChangingFrame = 60;

				if(!mouseLeft) // ? シークバーのつまみを放した
				{
					SeekPosChanging = 0;

					if(!IsActionBusy())
					{
						Nectar2Send_x(xcout("Seek=%d", d2i(SeekPosChangingRate * IMAX)));
					}
				}
			}
			else
				m_countDown(SeekPosChangingFrame);
		}

		// 音量のつまみを掴んでグイッと..
		{
			const int VOLUME_L = Gnd.RealScreen_W - 100;
			const int VOLUME_T = Gnd.RealScreen_H - 50;
			const int VOLUME_R = Gnd.RealScreen_W - 5;
			const int VOLUME_B = Gnd.RealScreen_H;

			if(!VolumeChanging)
			{
				// ? 音量のつまみを掴んだ
				if(
					mouseLeftDown &&
					m_isRange(mouseX, VOLUME_L, VOLUME_R) &&
					m_isRange(mouseY, VOLUME_T, VOLUME_B)
					)
					VolumeChanging = 1;
			}
			if(VolumeChanging)
			{
				double volRate = (mouseX - VOLUME_L) * 1.0 / (VOLUME_R - VOLUME_L);
				m_range(volRate, 0.0, 1.0);

				int volChanged = 0.0001 < abs(KrrVolume - volRate);

				KrrVolume = volRate;

				if(!mouseLeft) // ? 音量のつまみを放した
				{
					VolumeChanging = 0;
				}
				if(volChanged)
				{
					VolumeChangedEvent();

					// プレイリストに通知が必要な場合は、ここで通知する。
					// -- 通知がラッシュしないように気をつけること！
				}
			}
		}

		{
			int maxButtonShowFlag;

			if(
				m_isRange(mouseX, Gnd.RealScreen_W - 150, Gnd.RealScreen_W) &&
				m_isRange(mouseY, -50, 100)
				)
				maxButtonShowFlag = 1;
			else
				maxButtonShowFlag = 0;

			if(maxButtonShowFlag)
				MaxButtonKeepFrame = 30;
			else if(m_countDown(MaxButtonKeepFrame))
				maxButtonShowFlag = 1;

			if(maxButtonShowFlag)
				m_approach(MaxButtonShowRate, 1.0, 0.83);
			else
				m_approach(MaxButtonShowRate, 0.0, 0.93);
		}

		{
			int seekBarShowFlag;

			if(
				m_isRange(mouseX, 0, Gnd.RealScreen_W) &&
				m_isRange(mouseY, Gnd.RealScreen_H - 100, Gnd.RealScreen_H)
				)
				seekBarShowFlag = 1;
			else
				seekBarShowFlag = 0;

			if(seekBarShowFlag)
				SeekBarKeepFrame = 30;
			else if(m_countDown(SeekBarKeepFrame))
				seekBarShowFlag = 1;

			if(seekBarShowFlag)
				m_approach(SeekBarShowRate, 1.0, 0.83);
			else
				m_approach(SeekBarShowRate, 0.0, 0.93);
		}

		if(ProcFrame < 300) // ? プロセス起動直後
			goto endWinRectMon;

		// (ウィンドウの端をドラッグしての)ウィンドウサイズ変更(リサイズ)を検知
		{
			RECT rect;
			GetClientRect(GetMainWindowHandle(), &rect);
			int l = rect.left;
			int t = rect.top;
			int r = rect.right;
			int b = rect.bottom;
			int w = r - l;
			int h = b - t;

			// <-- l, t はいつも 0 っぽいよ？

			if(w <= 0 || h <= 0) // ? 最小化された。
				goto endWinRectMon;

			// set l, t
			{
				POINT p;

				p.x = 0;
				p.y = 0;

				ClientToScreen(GetMainWindowHandle(), &p);

				l = p.x;
				t = p.y;
			}

			static int stress = 0;

			if(w != Gnd.RealScreen_W || h != Gnd.RealScreen_H) // ? リサイズされた。
			{
				static int lastSentW = -IMAX;
				static int lastSentH = -IMAX;

				stress++;

				if(
					1200 < stress || // 20 sec < ... 20秒経ったらもう事故だろ。
					2 < stress && (w != lastSentW || h != lastSentH)
					)
				{
					stress = 0;
					lastSentW = w;
					lastSentH = h;

					// リサイズされた reaction
					{
						AddInstantMessage("SCREEN-RESIZED");

						Nectar2Send_x(xcout("Rect=%d,%d,%d,%d", l, t, w, h));
						Nectar2Send("Resized");
					}
				}
			}
			else
				stress = 0;

			// 移動の検知と通知
			{
				static int mvStress = 0;
				static int lastSentL = -IMAX;
				static int lastSentT = -IMAX;

				if(l != lastSentL || t != lastSentT)
				{
					if(stress || InstantMessages->GetCount()) // 移動検知キャンセル
					{
						AddInstantMessage("SCREEN-MOVED-Suppressed!");

						mvStress = 0;
						lastSentL = l;
						lastSentT = t;
					}
					else
					{
						mvStress++;

						if(30 < mvStress)
						{
							mvStress = 0;
							lastSentL = l;
							lastSentT = t;

							// reaction
							{
								AddInstantMessage("SCREEN-MOVED");

								Nectar2Send_x(xcout("Rect=%d,%d,%d,%d", l, t, w, h));
							}
						}
					}
				}
				else
					mvStress = 0;
			}
		}
endWinRectMon:

		// draw -->

		DrawCurtain();
		MovieDraw();
		Krr_EachFrame();

		// カウントダウンなど...

		m_countDown(FadeoutRushFrame);

		int trueFrameAdd = 0;

		{
			static __int64 lastTime = -1L;
			__int64 currTime = FrameStartTime;
			static double trueElapsed = 0.0;

			if(lastTime == -1L)
				lastTime = currTime;

			trueElapsed += currTime - lastTime;
			lastTime = currTime;

			while(0.0 < trueElapsed)
			{
				trueFrameAdd++;
				trueElapsed -= 1000.0 / 60.0;
			}
		}

		if(MoviePlaying)
			MoviePlayingFrame += trueFrameAdd;
		else
			MoviePlayingFrame = 0;

		if(BgmPlaying)
			BgmPlayingFrame += trueFrameAdd;
		else
			BgmPlayingFrame = 0;

		m_countDown(LastWindowMaximizeFrame);
		m_countDown(LastSaiseiTeishiPressFrame);
	}
endMainLoop:
	DeleteMovie();
	DeleteBgm();

	memFree(File);
	File = NULL;

	delete InstantMessages;
	InstantMessages = NULL;
}
static void Krr_EachFrame(void)
{
	CurtainEachFrame();
	WallDraw();

	// 最大化ボタン
	{
		SubScreen_t *ss = CreateSubScreen(100, 50);

		ChangeDrawScreen(ss);

		DPE_SetBright(0.5, 0.5, 0.0);
		DrawRect(P_WHITEBOX, 0, 0, 50, 50);
		DPE_Reset();
		DrawRect(P_WHITEBOX, 5, 5, 40, 5);
		DrawRect(P_WHITEBOX, 5, 15, 40, 5);
		DrawRect(P_WHITEBOX, 5, 5, 5, 40);
		DrawRect(P_WHITEBOX, 5, 40, 40, 5);
		DrawRect(P_WHITEBOX, 40, 5, 5, 40);

		DPE_SetBright(0.5, 0.0, 0.0);
		DrawRect(P_WHITEBOX, 50, 0, 50, 50);
		DPE_Reset();
		DrawSimple(P_BUTTON_X, 50, 0);

		RestoreDrawScreen();

		int t = d2i(MaxButtonShowRate * 50.0 - 50.0);

		DPE_SetAlpha(0.5 * MaxButtonShowRate);
		DPE_SetGraphicSize(GetSubScreenSize(ss));
		DrawSimple(GetHandle(ss), Gnd.RealScreen_W - 100, t);
		DPE_Reset();

		ReleaseSubScreen(ss);
	}

	// 停止ボタン・シークバー
	{
		SubScreen_t *ss = CreateSubScreen(Gnd.RealScreen_W, 50);

		ChangeDrawScreen(ss);

		DPE_SetBright(0.0, 0.0, 0.5);
		DrawRect(P_WHITEBOX, 0, 0, Gnd.RealScreen_W, 50);
		DPE_Reset();
		DrawSimple(MoviePlaying || BgmPlaying ? P_BUTTON_TEISHI : P_BUTTON_SAISEI, 0.0, 0.0);

		const int SEEK_BAR_L = 60;
		const int SEEK_BAR_W = Gnd.RealScreen_W - 170;
		const int SEEK_TSUMAMI_W = 20;

		LastSeekRate = 0.0; // reset_init

		{
			if(!MoviePlaying)
				DPE_SetBright(0.25, 0.25, 0.75);

			DrawRect(P_WHITEBOX, SEEK_BAR_L, 25, SEEK_BAR_W, 2);

			if(MoviePlaying || BgmPlaying) // シークボタンの表示
			{
				double seekRate;

				if(MoviePlaying)
					seekRate = MoviePlayingFrame * 1000.0 / 60.0 / MovieTime;
				else // ? BgmPlaying
					seekRate = BgmPlayingFrame * 1000.0 / 60.0 / BgmTime;

				if(SeekPosChangingFrame)
					seekRate = SeekPosChangingRate;

				m_range(seekRate, 0.0, 1.0);

				DrawRect(P_WHITEBOX, SEEK_BAR_L + d2i((SEEK_BAR_W - SEEK_TSUMAMI_W) * seekRate), 10, SEEK_TSUMAMI_W, 30);

				LastSeekRate = seekRate;
			}
			DPE_Reset();
		}

		const int VOLUME_L = Gnd.RealScreen_W - 100;
		const int VOLUME_T = 10;
		const int VOLUME_W = 95;
		const int VOLUME_H = 30;

		DPE_SetAlpha(0.5);
		DrawRect(P_WHITEBOX, VOLUME_L, VOLUME_T, VOLUME_W, VOLUME_H);
		DPE_Reset();

		int vol_w = d2i(VOLUME_W * KrrVolume);

		if(1 <= vol_w)
			DrawRect(P_WHITEBOX, VOLUME_L, VOLUME_T, vol_w, VOLUME_H);

		DPE_SetBright(0.0, 0.0, 0.5);
		DrawSimple(P_VOLUME_MASK, VOLUME_L, VOLUME_T);
		DPE_Reset();

		RestoreDrawScreen();

		int t = d2i(Gnd.RealScreen_H - 50.0 * SeekBarShowRate);

		DPE_SetAlpha(0.5 * SeekBarShowRate);
		DPE_SetGraphicSize(GetSubScreenSize(ss));
		DrawSimple(GetHandle(ss), 0, t);
		DPE_Reset();

		ReleaseSubScreen(ss);
	}

	SetPrint(0, 0);

	if(
		ErrorMessage_1 &&
		ErrorMessage_2 &&
		ErrorMessage_3
		)
	{
		int rbFlag = (ProcFrame / 30) % 2;

		PE_Border(rbFlag ? GetColor(200, 0, 0) : GetColor(0, 0, 200));
		Print(ErrorMessage_1);
		PrintRet();
		Print(ErrorMessage_2);
		PrintRet();
		Print(ErrorMessage_3);
		PrintRet();
		PE_Reset();
	}
	if(LastWindowMaximizeFrame)
	{
		PE.Color = GetColor(255, 255, 200);
		PE_Border(GetColor(64, 64, 0));
		Print("+---------------------+"); PrintRet();
		Print("| WINDOW RESIZING ... |"); PrintRet();
		Print("+---------------------+"); PrintRet();
		PE_Reset();
	}
	if(LastWindowXPressFrame)
	{
		PE.Color = GetColor(255, 200, 200);
		PE_Border(GetColor(64, 0, 0));
		Print("+--------------------+"); PrintRet();
		Print("| WINDOW CLOSING ... |"); PrintRet();
		Print("+--------------------+"); PrintRet();
		PE_Reset();
	}
#if 0 // すぐ画面的に反応するので、却って鬱陶しい。
	if(LastSaiseiTeishiPressFrame)
	{
		PE.Color = GetColor(200, 200, 255);
		PE_Border(GetColor(0, 0, 64));
		Print("+-------------------------+"); PrintRet();
		Print("| PLAY BUTTON PRESSED ... |"); PrintRet();
		Print("+-------------------------+"); PrintRet();
		PE_Reset();
	}
	if(SeekPosChangingFrame)
	{
		PE.Color = GetColor(200, 255, 200);
		PE_Border(GetColor(0, 64, 0));
		Print("+----------------------------+"); PrintRet();
		Print("| SEEK-POSITION CHANGING ... |"); PrintRet();
		Print("+----------------------------+"); PrintRet();
		PE_Reset();
	}
#endif
	if(InstantMessages->GetCount())
	{
		if(!InstantMessagesDisabled)
		{
			PE.Color = GetColor(200, 200, 255);
			PE_Border(GetColor(0, 0, 64));

			for(int index = 0; index < InstantMessages->GetCount(); index++)
			{
				Print(InstantMessages->GetElement(index));
				PrintRet();
			}
			PE_Reset();
		}
		InstantMessagesTopLineShownFrame++;

		if(90 < InstantMessagesTopLineShownFrame)
		{
			InstantMessagesTopLineShownFrame -= 3;
			memFree(InstantMessages->DesertElement(0));
		}
	}
	else
		InstantMessagesTopLineShownFrame = 0;

	EachFrame();
}

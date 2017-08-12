void AddFontFile(int etcId, char *localFile);

// ---- FontHandle ----

typedef struct FontHandle_st
{
	int Handle;
	char *FontName;
	int FontSize;
	int FontThick;
	int AntiAliasing;
	int EdgeSize;
	int ItalicFlag;
}
FontHandle_t;

FontHandle_t *CreateFontHandle(char *fontName, int fontSize, int fontThick = 6, int antiAliasing = 1, int edgeSize = 0, int italicFlag = 0);
void ReleaseFontHandle(FontHandle_t *fh);

// ---- GetFontHandle ----

FontHandle_t *GetFontHandle(char *fontName, int fontSize, int fontThick, int antiAliasing = 1, int edgeSize = 0, int italicFlag = 0);
void ReleaseAllFontHandle(void);

// ----

void DrawStringByFont(int x, int y, char *str, FontHandle_t *fh, int tategakiFlag = 0, int color = GetColor(255, 255, 255), int edgeColor = GetColor(0, 0, 0));

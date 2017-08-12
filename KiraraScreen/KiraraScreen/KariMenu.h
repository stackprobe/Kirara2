extern int KariMenu_Color;
extern int KariMenu_BorderColor;
extern int KariMenu_WallPicId;
extern double KariMenu_WallCurtain;
extern int KariMenu_X;
extern int KariMenu_Y;
extern int KariMenu_YStep;

int KariMenu(char *menuTitle, char **menuItems, int selectMax, int selectIndex = 0);
double KariVolumeConfig(char *menuTitle, double rate, int minval, int maxval, int valStep, int valFastStep, void (*valChanged)(double), int pulseFlag = 0);
void KariPadConfig(void);
void KariWindowSizeConfig(void);

#include "all.h"

static PicInfo_t *LoadPic(autoList<uchar> *fileData)
{
	return Pic_GraphicHandle2PicInfo(Pic_SoftImage2GraphicHandle(Pic_FileData2SoftImage(fileData)));
}
static void UnloadPic(PicInfo_t *i)
{
	Pic_ReleasePicInfo(i);
}
oneObject(resCluster<PicInfo_t *>, CreatePicRes(LoadPic, UnloadPic), GetStdPicRes);

static PicInfo_t *LoadInvPic(autoList<uchar> *fileData)
{
	int si_h = Pic_FileData2SoftImage(fileData);
	int w;
	int h;

	Pic_GetSoftImageSize(si_h, w, h);

	for(int x = 0; x < w; x++)
	for(int y = 0; y < h; y++)
	{
		Pic_GetSIPixel(si_h, x, y);

		SI_R ^= 0xff;
		SI_G ^= 0xff;
		SI_B ^= 0xff;

		Pic_SetSIPixel(si_h, x, y);
	}
	return Pic_GraphicHandle2PicInfo(Pic_SoftImage2GraphicHandle(si_h));
}
oneObject(resCluster<PicInfo_t *>, CreatePicRes(LoadInvPic, UnloadPic), GetInvPicRes);

static PicInfo_t *LoadMirrorPic(autoList<uchar> *fileData)
{
	int si_h = Pic_FileData2SoftImage(fileData);
	int w;
	int h;

	Pic_GetSoftImageSize(si_h, w, h);

	int new_si_h = Pic_CreateSoftImage(w * 2, h);

	for(int x = 0; x < w; x++)
	for(int y = 0; y < h; y++)
	{
		Pic_GetSIPixel(si_h, x, y);

		Pic_SetSIPixel(new_si_h, x, y);
		Pic_SetSIPixel(new_si_h, w * 2 - 1 - x, y);
	}
	Pic_ReleaseSoftImage(si_h);
	return Pic_GraphicHandle2PicInfo(Pic_SoftImage2GraphicHandle(new_si_h));
}
oneObject(resCluster<PicInfo_t *>, CreatePicRes(LoadMirrorPic, UnloadPic), GetMirrorPicRes);

#include "C:\Factory\Common\all.h"
#include "C:\Factory\SubTools\libs\Nectar2.h"

// ëŒä›Ç≈ÇÕ SEND_IDENT, RECV_IDENT ãtÇ…Ç»ÇÈÇÊÅB

#define RECV_IDENT "{4155c94a-b0aa-4738-bf59-b444b755ed81}" // shared_uuid

static Nectar2_t *Recver;

static void Main2(void)
{
	Recver = CreateNectar2(RECV_IDENT);

	while(!waitKey(0)) // âΩÇ©ÉLÅ[âüÇ≥ÇÍÇÈÇ‹Ç≈
	{
		char *line = Nectar2RecvLine(Recver, '\0');

		if(line)
		{
			cout("[%s] %s\n", c_makeJStamp(NULL, 0), line);
			memFree(line);
		}
	}
	ReleaseNectar2(Recver);
}
int main(int argc, char **argv)
{
	uint mtxProc = mutexTryProcLock("{365e67d1-a700-435e-882a-32bd17cd1e80}");
	Main2();
	mutexUnlock(mtxProc);
}

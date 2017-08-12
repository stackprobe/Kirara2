int d2i(double value);
int s2i(char *line, int minval, int maxval, int defval);
int s2i_x(char *line, int minval, int maxval, int defval);
int isPound(int counter);
autoList<char *> *tokenize(char *line, char *delimiters);
char *untokenize(autoList<char *> *tokens, char *separator);

template <class Var_t>
void t_swap(Var_t &a, Var_t &b)
{
	Var_t tmp = a;
	a = b;
	b = tmp;
}

int getZero(void);
uint getUIZero(void);
__int64 getI64Zero(void);
void noop(void);
void noop_i(int dummy);
void noop_ui(uint dummy);
void noop_i64(__int64 dummy);

i2D_t makeI2D(int x, int y);
d2D_t makeD2D(double x, double y);

bitList *createBitList(void);
void releaseBitList(bitList *bl);

void my_memset(void *block, int fillValue, int size);
int isfilled(void *block, int fillValue, int size);
int isSame(autoList<uchar> *binData1, autoList<uchar> *binData2);

template <class Var_t>
void zeroclear(Var_t *var, int num = 1)
{
	my_memset(var, 0x00, num * sizeof(Var_t));
}

template <class Var_t>
int isallzero(Var_t *var, int num = 1)
{
	return isfilled(var, 0x00, num * sizeof(Var_t));
}

template <class Type_t>
int isPointNull(Type_t **pp)
{
	return !*pp;
}

template <class Deletable_t>
void callDelete(Deletable_t *i)
{
	delete i;
}

template <class Deletable_t>
void deleteList(autoList<Deletable_t> *list)
{
	list->CallAllElement(callDelete);
	delete list;
}

double getAngle(double x, double y);
double getAngle(double x, double y, double originX, double originY);
void rotatePos(double angle, double &x, double &y);
void rotatePos(double angle, double &x, double &y, double originX, double originY);

extern char *decimal;
extern char *binadecimal;
extern char *octodecimal;
extern char *hexadecimal;

void adjustInside(int &w, int &h, int screen_w, int screen_h);
void adjustOutside(int &w, int &h, int screen_w, int screen_h);

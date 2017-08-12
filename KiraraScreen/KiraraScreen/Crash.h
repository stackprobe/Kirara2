double GetDistance(double x, double y);
double GetDistance(double x1, double y1, double x2, double y2);
void MakeXYSpeed(double x, double y, double destX, double destY, double speed, double &speedX, double &speedY);

int IsCrashed_Circle_Circle(
	double x1, double y1, double rCir1,
	double x2, double y2, double rCir2
	);
int IsCrashed_Circle_Podouble(
	double x1, double y1, double rCir,
	double x2, double y2
	);
int IsCrashed_Circle_Rect(
	double x, double y, double rCir,
	double l, double t, double r, double b
	);
int IsCrashed_Rect_Podouble(
	double l, double t, double r, double b,
	double x, double y
	);
int IsCrashed_Rect_Rect(
	double l1, double t1, double r1, double b1,
	double l2, double t2, double r2, double b2
	);
